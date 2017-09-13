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
using Nulobe.Microsoft.WindowsAzure.Storage;

namespace Nulobe.Api.Core.Events
{
    public class FlagEventService : EventService<FlagCreate>, IFlagEventService
    {
        public FlagEventService(IOptions<DocumentDbOptions> documentDbOptions, IRemoteIpAddressAccessor remoteIpAddressAccessor, IDocumentClientFactory documentClientFactory, ICloudStorageClientFactory cloudStorageClientFactory, IMapper mapper) : base(documentDbOptions, remoteIpAddressAccessor, documentClientFactory, cloudStorageClientFactory, mapper)
        {
        }
    }
}
