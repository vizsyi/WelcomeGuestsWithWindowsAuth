using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models
{
    public class AntonUserGroup
    {
        public string UserGroup { get; set; }
        public bool isCEO;
        public bool isTopLeader;
        public bool isInf;
    }
}