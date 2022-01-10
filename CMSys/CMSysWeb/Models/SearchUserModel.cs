using CMSys.Common.Paging;
using CMSys.Core.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class SearchUserModel
    {
        public string Search { get; set; }
        public PagedList<User> Users {get;set;}
    }
}
