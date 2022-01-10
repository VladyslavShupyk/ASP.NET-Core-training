using CMSys.Core.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class RoleResponse
    {
        public IEnumerable<RoleForResponse> UserRoles { get; set; }
        public IEnumerable<RoleForResponse> Roles {get; set;}
    }
}
