using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Nulobe.Api.Core.Facts;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;

namespace Nulobe.Api.Core.Events
{
    public class VoteEventService : EventService<VoteCreate>, IVoteEventService
    {
        public VoteEventService(IOptions<EventServiceOptions> eventOptions, IOptions<FactServiceOptions> factOptions, IRemoteIpAddressAccessor remoteIpAddressAccessor, IDocumentClientFactory documentClientFactory, IMapper mapper) : base(eventOptions, factOptions, remoteIpAddressAccessor, documentClientFactory, mapper)
        {
        }
    }
}
