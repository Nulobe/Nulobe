using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using Nulobe.Api.Core.Facts;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;
using Nulobe.Microsoft.WindowsAzure.Storage;
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
        private readonly FactServiceOptions _factOptions;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IDocumentClientFactory _documentClientFactory;
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;
        private readonly IMapper _mapper;

        public EventService(
            IOptions<DocumentDbOptions> documentDbOptions,
            IRemoteIpAddressAccessor remoteIpAddressAccessor,
            IDocumentClientFactory documentClientFactory,
            ICloudStorageClientFactory cloudStorageClientFactory,
            IMapper mapper)
        {
            _documentDbOptions = documentDbOptions.Value;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            _documentClientFactory = documentClientFactory;
            _cloudStorageClientFactory = cloudStorageClientFactory;
            _mapper = mapper;
        }

        async Task<Event> IEventService<TEventCreate>.CreateEventAsync(TEventCreate create)
        {
            // TODO: Validate TEventCreate
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                var fact = await client.ReadDocumentAsync<Fact>(_documentDbOptions, "Facts", create.FactId);
                if (fact == null)
                {
                    throw new ClientException($"Fact with ID {create.FactId} could not be found");
                }
            }

            var ev = _mapper.Map<TEventCreate, Event>(create);
            ev.Created = DateTime.UtcNow;
            ev.CreatedByIp = _remoteIpAddressAccessor.RemoteIpAddress;

            var tableClient = _cloudStorageClientFactory.CreateTableClient();
            var table = tableClient.GetTableReference("events");
            await table.CreateIfNotExistsAsync();

            await table.ExecuteAsync(TableOperation.Insert(ev));

            return ev;
        }
    }
}
