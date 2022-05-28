using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models
{
    public class PermissionModel
    {
        public int iPermissionId { get; set; }
        public string sPath { get; set; }
        public string sPermissionName { get; set; }
        public List<PermissionModel> childrens { get; set; }
    }
}