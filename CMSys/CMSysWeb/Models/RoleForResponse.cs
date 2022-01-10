using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class RoleForResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool Check(List<RoleForResponse> list)
        {
            foreach(var item in list)
            {
                if (item.Id == this.Id)
                    return true;
            }
            return false;
        } 
    }
}
