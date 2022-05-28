using EzollutionPro_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EzollutionPro_BAL.Services
{
    public class MenuService
    {
        private static MenuService instance = null;

        private MenuService()
        {
        }

        public static MenuService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuService();
                }
                return instance;
            }
        }

        public List<string> GetRights(int iRoleId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblRolePermissionMaps.Where(z => z.iRoleId == iRoleId).Select(z => z.tblPermissionsM.sPath).ToList();
            }
        }

        public string GetMenuRights(int iRoleId)
        {
            StringBuilder sb = new StringBuilder();
            using (var db = new EzollutionProEntities())
            {
                var menus = db.tblRolePermissionMaps
                           .Where(z => z.iRoleId == iRoleId && (z.tblPermissionsM.bShownInMenu ?? false) && !(z.tblPermissionsM.bIsMaster ?? false))
                           .Select(z => z.tblPermissionsM).ToList();
                foreach (var parentmenu in menus.Where(z => z.iParentId == null || z.iParentId == 0).OrderBy(z => z.iSort))
                {
                    sb.Append("<li class=\"dropdown\">");
                    sb.Append("<a href = \"" + parentmenu.sPath + "\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"true\">");
                    sb.Append("<i class=\"fa fa-share-square-o\"></i> " + parentmenu.sPermissionName);
                    sb.Append("<span class=\"caret\"></span>");
                    sb.Append("</a>");
                    sb.Append("<ul class=\"dropdown-menu\" role=\"menu\">");
                    foreach (var childMenu in menus.Where(z => z.iParentId == parentmenu.iPermissionId).OrderBy(z => z.iSort))
                    {
                        sb.Append("<li id = \"Users Management\" ><a href=\"" + childMenu.sPath + "\"><i class=\"fa fa-dot-circle-o\"></i>" + childMenu.sPermissionName + "</a></li>");
                    }
                    sb.Append("</ul></li>");
                }
                return sb.ToString();
            }
        }

        public string GetMasters(int iRoleId)
        {
            StringBuilder sb = new StringBuilder();
            using (var db = new EzollutionProEntities())
            {
                var menus = db.tblRolePermissionMaps
                           .Where(z => z.iRoleId == iRoleId && (z.tblPermissionsM.bShownInMenu ?? false) && (z.tblPermissionsM.bIsMaster ?? false))
                           .Select(z => z.tblPermissionsM).ToList();
                foreach (var parentmenu in menus.Where(z => z.iParentId == null || z.iParentId == 0).OrderBy(z => z.iSort))
                {
                    sb.Append("<li class=\"dropdown\">");
                    sb.Append("<a href = \"" + parentmenu.sPath + "\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"true\">");
                    sb.Append("<i class=\"fa fa-share-square-o\"></i> " + parentmenu.sPermissionName);
                    sb.Append("<span class=\"caret\"></span>");
                    sb.Append("</a>");
                    sb.Append("<ul class=\"dropdown-menu\" role=\"menu\">");
                    foreach (var childMenu in menus.Where(z => z.iParentId == parentmenu.iPermissionId).OrderBy(z => z.iSort))
                    {
                        sb.Append("<li id = \"Users Management\" ><a href=\"" + childMenu.sPath + "\"><i class=\"fa fa-dot-circle-o\"></i>" + childMenu.sPermissionName + "</a></li>");
                    }
                    sb.Append("</ul></li>");
                }
                return sb.ToString();
            }
        }
    }
}