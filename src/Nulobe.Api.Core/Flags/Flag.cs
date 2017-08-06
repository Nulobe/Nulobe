using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Flags
{
    public class FlagCreate : IRemoteIpAddressAudited
    {
        [Required]
        public string FactId { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
