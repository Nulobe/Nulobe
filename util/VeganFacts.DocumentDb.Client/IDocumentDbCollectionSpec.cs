using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganFacts.DocumentDb.Client
{
    public interface IDocumentDbCollectionSpec : IDocumentDbDatabaseSpec
    {
        string CollectionName { get; set; }
    }
}
