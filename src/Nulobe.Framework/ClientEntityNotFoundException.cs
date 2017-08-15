using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public class ClientEntityNotFoundException<TIdentity> : ClientException
    {
        public TIdentity Id { get; private set; }

        public Type EntityType { get; private set; }


        public ClientEntityNotFoundException(Type entityType, TIdentity id)
        {
            Id = id;
            EntityType = entityType;
        }

    }
}
