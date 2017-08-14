using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public interface IAuditableAction
    {
        string ActionName { get; set; }

        DateTime Actioned { get; set; }

        string ActionedById { get; set; }

        string ActionedByRemoteIp { get; set; }
    }

    public class Auditor
    {
        private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;

        public Auditor(
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IRemoteIpAddressAccessor remoteIpAddressAccessor)
        {
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
        }

        public void AuditAction(string actionName, IAuditableAction auditableAction)
        {
            auditableAction.ActionName = actionName;
            auditableAction.Actioned = DateTime.UtcNow;
            auditableAction.ActionedById = _claimsPrincipalAccessor.ClaimsPrincipal.Identities.First().GetId();
            auditableAction.ActionedByRemoteIp = _remoteIpAddressAccessor.RemoteIpAddress;
        }
    }
}
