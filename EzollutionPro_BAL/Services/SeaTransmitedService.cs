using EzollutionPro_DAL;
using EzollutionPro_BAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EzollutionPro_BAL.Services
{
    public class SeaTransmitedService
    {
        private static SeaTransmitedService instance = null;
        private SeaTransmitedService()
        {
        }
        public static SeaTransmitedService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SeaTransmitedService();
                }
                return instance;
            }
        }
        public List<SchedulingViewModel> GetScheduling(string minDate, string maxDate, out int recordsTotal)
        {
            using (var db = new EzollutionProEntities())
            {
                DateTime dtMinDate, dtMaxDate;
                if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate))
                {
                    dtMinDate = DateTime.ParseExact(minDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtMaxDate = DateTime.ParseExact(maxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    dtMinDate = DateTime.Now.Date;
                    dtMaxDate = DateTime.Now.Date.AddMonths(2);
                }
                var query = from scheduling in db.tblSeaSchedulings
                            where (scheduling.dtEstimatedDateOfArrival >= dtMinDate && scheduling.dtEstimatedDateOfArrival <= dtMaxDate) && (scheduling.iSAction == 1 && (scheduling.bCheckListSent ?? false) && (scheduling.bCheckListApproved ?? false))
                            select scheduling;
                recordsTotal = query.Count();
                return query.OrderBy(z => z.dtEstimatedDateOfArrival).ThenBy(z => z.tblPODMaster.sPortCode).ThenBy(z => z.sVesselName).ThenBy(z => z.tblClientMaster.sClientName).ToList()
                .Select((z, i) => new SchedulingViewModel
                {
                    sCheckListApproved = (z.bCheckListApproved ?? false) == false ? "N" : "Y",
                    sCheckListSent = (z.bCheckListSent ?? false) == false ? "N" : "Y",
                    sEDA = z.dtEstimatedDateOfArrival.HasValue ? z.dtEstimatedDateOfArrival.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                    iSchedulingId = z.iSchedulingId,
                    sClientName = z.tblClientMaster.sClientName,
                    sContainerNumber = z.sContainerNumber,
                    sFPOD = z.tblPOFDMaster.sPortCode,
                    sVesselName = z.sVesselName,
                    sMBLNumber = z.sMBLNumber,
                    sPOD = z.tblPODMaster.sPortCode,
                    sShippingLine = z.tblShippingLine.sShippingLineName,
                    iSAction = z.iSAction ?? 0,
                    sRecieveOn = z.dtReceivedOn.HasValue ? z.dtReceivedOn.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                }).ToList();

            }
        }

        public string GetTransmitFileData(int iSchedulingId)
        {
            var dataSet = GetDataForTransmition(iSchedulingId);
            return TransformString(dataSet);
        }

        private string TransformString(DataSet dataSet)
        {
            DataTable tblSch = dataSet.Tables[0];
            DataTable tblMH = dataSet.Tables[1];
            DataTable tblMC = dataSet.Tables[2];
            string mysf = Encoding.ASCII.GetString(new byte[] { 29 });
            string Header = string.Empty;
            string MLOCode = string.Empty;
            if (tblSch.Rows.Count > 0)
            {
                string currentDate = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "").Replace("/", "");
                string currentTime = DateTime.Now.ToString("HH:mm").Replace(":", "");
                Header = "HREC" + mysf + "ZZ" + mysf + tblSch.Rows[0]["ICEGateSeaID"] + mysf + "ZZ" + mysf + tblSch.Rows[0]["PortOfDestination"] + mysf + "ICES1_5" + mysf + "T" + mysf + mysf + "CMCHI21" + mysf + tblSch.Rows[0]["SchedulingId"] + mysf + currentDate + mysf + currentTime + '\n' +
                    "<consoligm>" + '\n' +
                    "<conscargo>" + '\n';
            }
            string MBLWithHBL = string.Empty;
            foreach (DataRow row in tblMH.Rows)
            {
                string MessageType = row["MessageType"].ToString() == "" ? mysf : row["MessageType"].ToString() + mysf;
                string PortofDestination = row["PortofDestination"].ToString() == "" ? mysf : row["PortofDestination"].ToString() + mysf;
                string CARNNumber = tblSch.Rows[0]["CARN"].ToString() == "" ? mysf : tblSch.Rows[0]["CARN"].ToString() + mysf;
                string IGMNumber = row["IGMNumber"].ToString() == "" ? mysf : row["IGMNumber"].ToString() + mysf;
                string IGMDate = row["IGMDate"].ToString() == "" ? mysf : row["IGMDate"].ToString().Replace("/", "") + mysf;
                string IMOCodeofVessel = row["IMOCodeofVessel"].ToString() == "" ? mysf : row["IMOCodeofVessel"].ToString() + mysf;
                string VesselCode = row["VesselCode"].ToString() == "" ? mysf : row["VesselCode"].ToString() + mysf;
                string VoyageNumber = row["VoyageNumber"].ToString() == "" ? mysf : row["VoyageNumber"].ToString() + mysf;
                string LineNumber = row["LineNumber"].ToString() == "" ? mysf : row["LineNumber"].ToString() + mysf;
                string SubLineNumber = row["HSubLineNumber"].ToString() == "" ? mysf : row["HSubLineNumber"].ToString() + mysf;
                string MasterBillofLadingNo = row["MasterBillofLadingNo"].ToString() == "" ? mysf : row["MasterBillofLadingNo"].ToString() + mysf;
                string MasterBillofLadingdate = row["MasterBillofLadingdate"].ToString() == "" ? mysf : row["MasterBillofLadingdate"].ToString().Replace("/", "") + mysf;
                string PortofShipment = row["PortofShipment"].ToString() == "" ? mysf : row["PortofShipment"].ToString() + mysf;
                string FinalDestination = row["FinalDestination"].ToString() == "" ? mysf : row["FinalDestination"].ToString() + mysf;
                string HouseBillofLadingNo = row["HouseBillofLadingNo"].ToString() == "" ? mysf : row["HouseBillofLadingNo"].ToString() + mysf;
                string HouseBillofLadingDate = row["HouseBillofLadingDate"].ToString() == "" ? mysf : row["HouseBillofLadingDate"].ToString().Replace("/", "") + mysf;
                string ImporterName = row["ImporterName"].ToString() == "" ? mysf : row["ImporterName"].ToString() + mysf;
                string ImporterAddress1 = row["ImporterAddress1"].ToString() == "" ? mysf : row["ImporterAddress1"].ToString() + mysf;
                string ImporterAddress2 = row["ImporterAddress2"].ToString() == "" ? mysf : row["ImporterAddress2"].ToString() + mysf;
                string ImporterAddress3 = row["ImporterAddress3"].ToString() == "" ? mysf : row["ImporterAddress3"].ToString() + mysf;
                string ConsigneeName = row["ImporterName"].ToString() == "" ? mysf : row["ImporterName"].ToString() + mysf;
                string ConsigneeAddress1 = row["ImporterAddress1"].ToString() == "" ? mysf : row["ImporterAddress1"].ToString() + mysf;
                string ConsigneeAddress2 = row["ImporterAddress2"].ToString() == "" ? mysf : row["ImporterAddress2"].ToString() + mysf;
                string ConsigneeAddress3 = row["ImporterAddress3"].ToString() == "" ? mysf : row["ImporterAddress3"].ToString() + mysf;
                string NatureofCargo = row["NatureofCargo"].ToString() == "" ? mysf : row["NatureofCargo"].ToString() + mysf;
                string ItemType = row["ItemType"].ToString() == "" ? mysf : row["ItemType"].ToString() + mysf;
                string CargoMovement = row["CargoMovement"].ToString() == "" ? mysf : row["CargoMovement"].ToString() + mysf;
                string DestinationCode = row["DestinationCode"].ToString() == "" ? mysf : row["DestinationCode"].ToString() + mysf;
                string TotalNumberofPackages = row["TotalNumberofPackages"].ToString() == "" ? mysf : row["TotalNumberofPackages"].ToString() + mysf;
                string PackageCode = row["PackageCode"].ToString() == "" ? mysf : row["PackageCode"].ToString() + mysf;
                string GrossWeight = row["GrossWeight"].ToString() == "" ? mysf : (Convert.ToDecimal(row["GrossWeight"])).ToString("0.00") + mysf;
                string UnitofWeight = row["UnitofWeight"].ToString() == "" ? mysf : row["UnitofWeight"].ToString() + mysf;
                string GrossVolume = row["GrossVolume"].ToString() == "" ? mysf : row["GrossVolume"].ToString() + mysf;
                string UnitofVolume = row["UnitofVolume"].ToString() == "" ? mysf : row["UnitofVolume"].ToString() + mysf;
                string MarksandNumbers = row["MarksandNumbers"].ToString() == "" ? mysf : row["MarksandNumbers"].ToString() + mysf;
                string GoodsDescription = row["GoodsDescription"].ToString() == "" ? mysf : row["GoodsDescription"].ToString() + mysf;
                string UNOCode = row["UNOCode"].ToString() == "" ? mysf : row["UNOCode"].ToString() + mysf;
                string IMCOCode = row["IMCOCode"].ToString() == "" ? mysf : row["IMCOCode"].ToString() + mysf;
                string BondNumber = row["BondNumber"].ToString() == "" ? mysf : row["BondNumber"].ToString() + mysf;
                string CarrierCode = row["CarrierCode"].ToString() == "" ? mysf : row["CarrierCode"].ToString() + mysf;
                string ModeofTransport = row["ModeofTransport"].ToString() == "" ? mysf : row["ModeofTransport"].ToString() + mysf;
                MLOCode = row["MLOCode"].ToString() == "" ? mysf : row["MLOCode"].ToString();

                string localMBLWithHBL =
                MessageType + PortofDestination + CARNNumber + IGMNumber + IGMDate + IMOCodeofVessel + VesselCode + VoyageNumber + LineNumber + SubLineNumber + MasterBillofLadingNo +
                MasterBillofLadingdate + PortofShipment + FinalDestination + HouseBillofLadingNo + HouseBillofLadingDate + ImporterName + ImporterAddress1 + ImporterAddress2 +
                ImporterAddress3 + ConsigneeName + ConsigneeAddress1 + ConsigneeAddress2 + ConsigneeAddress3 + NatureofCargo + ItemType + CargoMovement + DestinationCode + TotalNumberofPackages +
                PackageCode + GrossWeight + UnitofWeight + GrossVolume + UnitofVolume + MarksandNumbers + GoodsDescription + UNOCode + IMCOCode + BondNumber + CarrierCode +
                ModeofTransport + MLOCode + '\n';

                MBLWithHBL = MBLWithHBL + localMBLWithHBL.ToUpper();

            }

            MBLWithHBL = MBLWithHBL +
                "<END-conscargo>" + '\n' +
                "<conscont>" + '\n';

            string MBLWithContainer = string.Empty;
            foreach (DataRow row in tblMC.Rows)
            {
                string MessageType = row["MessageType"].ToString() == "" ? mysf : row["MessageType"].ToString() + mysf;
                string CustomHouseCode = row["PortofDestination"].ToString() == "" ? mysf : row["PortofDestination"].ToString() + mysf;
                string CARNNumber = tblSch.Rows[0]["CARN"].ToString() == "" ? mysf : tblSch.Rows[0]["CARN"].ToString() + mysf;
                string IGMNumber = row["IGMNumber"].ToString() == "" ? mysf : row["IGMNumber"].ToString() + mysf;
                string IGMDate = row["IGMDate"].ToString() == "" ? mysf : row["IGMDate"].ToString().Replace("/", "") + mysf;
                string IMOCodeofVessel = row["IMOCodeofVessel"].ToString() == "" ? mysf : row["IMOCodeofVessel"].ToString() + mysf;
                string VesselCode = row["VesselCode"].ToString() == "" ? mysf : row["VesselCode"].ToString() + mysf;
                string VoyageNumber = row["VoyageNumber"].ToString() == "" ? mysf : row["VoyageNumber"].ToString() + mysf;
                string LineNumber = row["LineNumber"].ToString() == "" ? mysf : row["LineNumber"].ToString() + mysf;
                string SubLineNumber = row["CSubLineNumber"].ToString() == "" ? mysf : row["CSubLineNumber"].ToString() + mysf;

                string ContainerNumber = row["ContainerNumber"].ToString() == "" ? mysf : row["ContainerNumber"].ToString() + mysf;
                string ContainerSealNo = row["ContainerSealNo"].ToString() == "" ? mysf : row["ContainerSealNo"].ToString() + mysf;
                string ContainerAgentCode = row["ContainerAgentCode"].ToString() == "" ? mysf : row["ContainerAgentCode"].ToString() + mysf;
                string ContainerStatus = row["ContainerStatus"].ToString() == "" ? mysf : row["ContainerStatus"].ToString() + mysf;
                string TotalPackages = row["TotalPackages"].ToString() == "" ? mysf : row["TotalPackages"].ToString() + mysf;
                string ContainerWeight = row["ContainerWeight"].ToString() == "" ? mysf : (Convert.ToDecimal(row["ContainerWeight"]) / 1000).ToString("0.00") + mysf;
                string ISOCode = row["ISOCode"].ToString() == "" ? mysf : row["ISOCode"].ToString() + mysf;
                string SOCFlag = row["SOCFlag"].ToString() == "" ? mysf : row["SOCFlag"].ToString();

                string localMBLWithContainer =
                    MessageType + CustomHouseCode + CARNNumber + IGMNumber + IGMDate + IMOCodeofVessel + VesselCode + VoyageNumber + LineNumber + SubLineNumber +
                    ContainerNumber + ContainerSealNo + MLOCode +mysf+ ContainerStatus + TotalPackages + ContainerWeight + ISOCode + SOCFlag + '\n';
                    
                MBLWithContainer = MBLWithContainer + localMBLWithContainer.ToUpper();
            }

            if (tblSch.Rows.Count > 0)
            {
                MBLWithContainer = MBLWithContainer +
                    "<END-conscont>" + '\n' +
                    "<END-consoligm>" + '\n' +
                    "TREC" + mysf + tblSch.Rows[0]["SchedulingId"] + '\n';
            }

            return (Header + MBLWithHBL + MBLWithContainer);
        }

        public DataSet GetDataForTransmition(int iSchedulingId)
        {
            var db = new EzollutionProEntities();
            using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                db.Dispose();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "uspGetMsgResult";
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm1 = new SqlParameter("@iSchedulingId", iSchedulingId);
                    cmd.Parameters.Add(prm1);

                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    ds.Tables[0].TableName = "Data_Scheduling";
                    ds.Tables[1].TableName = "Data_MBLWithHBL";
                    ds.Tables[2].TableName = "Data_MBLWithContainer";

                    conn.Close();

                    return ds;
                }
            }
        }

    }
}