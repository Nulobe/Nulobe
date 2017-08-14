using AutoMapper;
using Microsoft.Extensions.Options;
using Nulobe.Api.Core.Facts;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;
using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Events
{
    public class EventService<TEventCreate> : IEventService<TEventCreate> where TEventCreate : IEventCreate
    {
        private readonly DocumentDbOptions _documentDbOptions;
        private readonly EventServiceOptions _eventOptions;
        private readonly FactServiceOptions _factOptions;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IDocumentClientFactory _documentClientFactory;
        private readonly IMapper _mapper;

        public EventService(
            IOptions<DocumentDbOptions> documentDbOptions,
            IOptions<EventServiceOptions> eventOptions,
            IOptions<FactServiceOptions> factOptions,
            IRemoteIpAddressAccessor remoteIpAddressAccessor,
            IDocumentClientFactory documentClientFactory,
            IMapper mapper)
        {
            _documentDbOptions = documentDbOptions.Value;
            _eventOptions = eventOptions.Value;
            _factOptions = factOptions.Value;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            _documentClientFactory = documentClientFactory;
            _mapper = mapper;
        }

        async Task<Event> IEventService<TEventCreate>.CreateEventAsync(TEventCreate create)
        {
            // TODO: Validate TEventCreate
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                var fact = await client.ReadDocumentAsync<Fact>(_documentDbOptions, _factOptions.FactCollectionName, create.FactId);
                if (fact == null)
                {
                    throw new ClientException($"Fact with ID {create.FactId} could not be found");
                }

                var ev = _mapper.Map<TEventCreate, Event>(create);
                ev.Created = DateTime.UtcNow;
                ev.CreatedByIp = _remoteIpAddressAccessor.RemoteIpAddress;

                var result = await client.CreateDocumentAsync(_documentDbOptions, _eventOptions.EventCollectionName, ev);
                ev.Id = result.Resource.Id;

                return ev;
            }
        }
    }
}
