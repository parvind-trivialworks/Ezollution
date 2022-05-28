using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Utilities;
using EzollutionPro_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EzollutionPro_BAL.Services.MasterServices
{
    public class BondService
    {
        private static BondService instance = null;
        private BondService()
        {
        }
        public static BondService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BondService();
                }
                return instance;
            }
        }

        public List<BondModel> GetBondMasters(int draw, int displayStart, int displayLength, string search, int iShippingId, int iPOD, int iFPOD, string sModeOfTransport, string sCargoMovement, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = db.tblBondMasterMs
                    .Where(z =>
                    (z.iShippingId == iShippingId || iShippingId == 0) &&
                    (z.iPODId == iPOD || iPOD == 0) &&
                    (z.iFPODId == iFPOD || iFPOD == 0) &&
                    (z.sModeOfTransport == sModeOfTransport || sModeOfTransport == "") &&
                    (z.sCargoMovement == sCargoMovement || sCargoMovement == "")
                    );
                var data = query
                    .OrderBy(z => z.nBondNo).Skip(displayStart).Take(displayLength).Select(z => new BondModel
                    {
                        iBondId = z.iBondId,
                        nBondNo = z.nBondNo,
                        sCarrierCode = z.sCarrierCode,
                        sMLOCode = z.sMLOCode,
                        sModeOfTransport = z.sModeOfTransport,
                    }).ToList();
                recordsTotal = query.Count();
                return data;
            }
        }


        public BondModel GetBondById(int iBondId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblBondMasterMs.Where(z => z.iBondId == iBondId)
                        .Select(z => new BondModel
                        {
                            iBondId = z.iBondId,
                            iFPODId = z.iFPODId,
                            iPODId = z.iPODId,
                            iShippingId = z.iShippingId,
                            nBondNo = z.nBondNo,
                            sCarrierCode = z.sCarrierCode,
                            sCFSCode = z.sCFSCode,
                            sCFSName = z.sCFSName,
                            sModeOfTransport = z.sModeOfTransport,
                            sMLOCode = z.tblShippingLine.sMLOCode,
                            sCargoMovement = z.sCargoMovement
                        })
                        .SingleOrDefault();
            }
        }

        public string GetMLOCode(int iShippingId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblShippingLines.Where(z => z.iShippingID == iShippingId).Select(z => z.sMLOCode).SingleOrDefault();
            }
        }

        public ResponseStatus SaveBond(BondModel model, int v)
        {
            try
            {

                using (var db = new EzollutionProEntities())
                {
                    var data = db.tblBondMasterMs.Where(z => z.iBondId == model.iBondId).SingleOrDefault();
                    if (data == null)
                    {
                        data = new tblBondMasterM
                        {
                            dtActionDate = DateTime.Now,
                            iActionBy = v,
                            iFPODId = model.iFPODId,
                            iPODId = model.iPODId,
                            iShippingId = model.iShippingId,
                            nBondNo = model.nBondNo,
                            sCarrierCode = model.sCarrierCode,
                            sCFSCode = model.sCFSCode,
                            sCFSName = model.sCFSName,
                            sModeOfTransport = model.sModeOfTransport,
                            sCargoMovement = model.sCargoMovement,
                            sMLOCode = model.sMLOCode
                        };
                        db.tblBondMasterMs.Add(data);
                    }
                    else
                    {
                        data.dtActionDate = DateTime.Now;
                        data.iActionBy = v;
                        data.iFPODId = model.iFPODId;
                        data.iPODId = model.iPODId;
                        data.iShippingId = model.iShippingId;
                        data.nBondNo = model.nBondNo;
                        data.sCarrierCode = model.sCarrierCode;
                        data.sCFSCode = model.sCFSCode;
                        data.sCFSName = model.sCFSName;
                        data.sModeOfTransport = model.sModeOfTransport;
                        data.sCargoMovement = model.sCargoMovement;
                        data.sMLOCode = model.sMLOCode;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new ResponseStatus { Status = true, Message = "Bond Master saved successfully" };
                }
            }
            catch (Exception)
            {
                return new ResponseStatus { Status = false, Message = "Something went wrong!" };
            }
        }
    }
}
