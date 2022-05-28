using EzollutionPro_BAL.Models.Masters;
using EzollutionPro_BAL.Utilities;
using EzollutionPro_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Services.MasterServices
{
    public class ClientEmailService
    {
        private static ClientEmailService instance = null;
        private ClientEmailService()
        {
        }
        public static ClientEmailService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientEmailService();
                }
                return instance;
            }
        }
        public ResponseStatus SaveClient(ClientEmailModel model)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblClientMultipleEmails.Where(z => z.iClientId == model.iClientId && z.iClientType==model.iClientType).SingleOrDefault();
                if (data == null)
                {
                    if (db.tblClientMultipleEmails.Any(z => z.sEmailId == model.sEmailId))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "This EmailId is already exists"
                        };
                    }
                    data = new tblClientMultipleEmail
                    {
                        iClientType =model.iClientType,
                        iClientId = model.iClientId,
                        sEmailId = model.sEmailId,
                        sEmailPersonName = model.sEmailPersonName,
                        bIsDefault = model.bIsDefault,
                    };
                    db.tblClientMultipleEmails.Add(data);
                }
                else
                {
                    if (db.tblClientMultipleEmails.Any(z => z.sEmailId == model.sEmailId && z.iClientId != model.iClientId && z.iClientType!= model.iClientType))
                    {
                        return new ResponseStatus
                        {
                            Status = false,
                            Message = "EmailId is already exists"
                        };
                    }
                    data.iClientType = model.iClientType;
                    data.iClientId = model.iClientId;
                    data.sEmailId = model.sEmailId;
                    data.sEmailPersonName = model.sEmailPersonName;
                    data.bIsDefault = model.bIsDefault;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return new ResponseStatus
                {
                    Status = true,
                    Message = "Email-Id saved successfully!"
                };
            }
        }
        public List<ClientEmailModel> GetClientMails(int iClientype,int iCLientId)
        {
            using (var db = new EzollutionProEntities())
            {
                var query = (from email in db.tblClientMultipleEmails
                             join client in db.vw_ClientMaster on email.iClientId equals client.iClientId
                             where client.iClientType == iClientype.ToString() &&
                             (iCLientId > 0 ? email.iClientId == iCLientId : 1 == 1) 
                             && email.blsActive==true
                             && client.iClientType==iClientype.ToString()
                             && email.iClientType==iClientype
                             select new { email, client });
                if (iClientype == -1 && iCLientId > 0)
                {
                    query.Where(x => x.email.iClientId == iCLientId);
                }
                else
                {
                    if (iClientype >= 0 && iCLientId == 0)
                    {
                        query.Where(x => x.email.iClientType == iClientype);
                    }
                    else
                    {
                        query.Where(x => x.email.iClientId == iCLientId && x.email.iClientType == iClientype);
                    }

                }
                return query.OrderBy(x=>x.client.ClientName).ToList().Select((z, i) => new ClientEmailModel
                {
                    iMailId = z.email.iMailId,
                    iClientType = z.email.iClientType,
                    iClientId = z.email.iClientId,
                    sEmailId = z.email.sEmailId,
                    sEmailPersonName = z.email.sEmailPersonName,
                    bIsDefault = z.email.bIsDefault.GetValueOrDefault(),
                    sClientName=z.client.ClientName
                }).ToList();
            }
        }

        public ClientEmailModel AddUdpdateEmail(int iMailId)
        {
            using (var db = new EzollutionProEntities())
            {
                return db.tblClientMultipleEmails.Where(x => x.iMailId == iMailId).ToList()
                .Select((z, i) => new ClientEmailModel
                {
                    
                    sEmailId = z.sEmailId,
                    iClientType = z.iClientType,
                    iClientId=z.iClientId,
                    sEmailPersonName = z.sEmailPersonName,
                    iMailId = z.iMailId,
                    bIsDefault = z.bIsDefault.GetValueOrDefault(),
                    blsActive = z.blsActive.GetValueOrDefault()
                }).FirstOrDefault();
            }
        }

        public ResponseStatus SaveEmail(ClientEmailModel model, int iUserId)
        {
            using (var db = new EzollutionProEntities())
            {
                var data = db.tblClientMultipleEmails.Where(z => z.iMailId == model.iMailId).FirstOrDefault();
                if (data != null)
                {
                    db.Usp_UpdateIsDefaultEmail(data.iMailId, model.iClientType, model.iClientId);
                    data.iClientType = model.iClientType;
                    data.iClientId = model.iClientId;
                    data.sEmailId = model.sEmailId;
                    data.sEmailPersonName = model.sEmailPersonName;
                    data.bIsDefault = model.bIsDefault;
                    data.blsActive = model.blsActive;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();         
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "Email account created successfully!"
                    };
                }
                else
                {
                    db.Usp_UpdateIsDefaultEmail(0, model.iClientType, model.iClientId);
                    data = new tblClientMultipleEmail
                    {
                        iClientType = (short)model.iClientType,
                        iClientId = model.iClientId,
                        sEmailId = model.sEmailId,
                        sEmailPersonName = model.sEmailPersonName,
                        bIsDefault = model.bIsDefault,
                        blsActive = true
                    };
                    db.tblClientMultipleEmails.Add(data);
                    db.SaveChanges();
                    return new ResponseStatus
                    {
                        Status = true,
                        Message = "Email account updated successfully!"
                    };
                }
               
            }
        }
    }
}
