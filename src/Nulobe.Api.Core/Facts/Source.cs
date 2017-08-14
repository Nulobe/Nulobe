using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class Source
    {
        [Required]
        public Uri Url { get; set; }

        public string Description { get; set; }
    }
}
