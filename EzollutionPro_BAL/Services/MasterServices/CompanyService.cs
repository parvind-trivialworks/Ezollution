using EzollutionPro_DAL;
using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EzollutionPro_BAL.Services
{
    public class CompanyService
    {
        private static CompanyService instance = null;

        private CompanyService()
        {
        }

        public static CompanyService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CompanyService();
                }
                return instance;
            }
        }

        public List<DropDownData> GetCompaniesForDdl()
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblCompanies.ToList().OrderBy(x=>x.blsIsDefault==true).OrderBy(x=>x.sName).Select(z => new DropDownData
                {
                    Text = z.sName+" (Company-ID: "+z.iCompanyId+")",
                    Value = z.iCompanyId.ToString(),
                    Id = z.iCompanyId
                }).ToList();
            }
        }

    }
}