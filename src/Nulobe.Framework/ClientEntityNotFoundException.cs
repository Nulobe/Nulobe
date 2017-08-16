using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public class ClientEntityNotFoundException : ClientException
    {
        public object Id { get; private set; }

        public Type EntityType { get; private set; }

        public ClientEntityNotFoundException(Type entityType, object id) : this(entityType, id, null) { }

        public ClientEntityNotFoundException(Type entityType, object id, Exception innerException)
            : base($"Could not find entity of type {entityType.FullName} with Id {id}", innerException)
        {
            Id = id;
            EntityType = entityType;
        }
    }
}
