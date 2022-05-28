using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Models
{
    public class CheckListPDFModel
    {
        public string ShippingLine { get; set; }
        public string MBLNumber { get; set; }
        public string MBLDate { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public string PortOfFinalDestination { get; set; }
        public string VesselName { get; set; }
        public string EDA { get; set; }

        public List<HBLPDFData> lstHBLData { get; set; }
    }

    public class HBLPDFData
    {
        public string HBLNumber { get; set; }
        public string HBLDate { get; set; }
        public string SublineNo { get; set; }
        public string CargoMovement { get; set; }
        public string ImporterName { get; set; }
        public string ImporterAddress { get; set; }
        public string NoOfPackages { get; set; }
        public string GrossWeight { get; set; }
        public string GoodsDescription { get; set; }
        public string MarksAndNumbers { get; set; }
        public List<ContainerPDFData> lstContainerData { get; set; }
    }
    public class ContainerPDFData
    {
        public string ContainerNumber { get; set; }
        public string SealNumber { get; set; }
        public string ContainerStatus { get; set; }
        public string TotalPackages { get; set; }
        public string ContainerWeight { get; set; }
        public string ContainerType { get; set; }
    }


    public class CheckListMAWBPDFModel
    {
        public string sDescription { get; set; }
        public string sCustomLocation { get; set; }
        public string sCustomLocationCode { get; set; }
        //MAWB Details
        public string sMAWBNo { get; set; }
        public string sPortOfOrigin { get; set; }
        public string sPortOfDestination { get; set; }
        public string sPackages { get; set; }
        public string sWeight { get; set; }
        public string sTotalWeight { get; set; }
        public string sTotalPackages { get; set; }
        public List<CheckListHAWBPDFModel> lstHAWBData { get; set; }
    }
    public class CheckListHAWBPDFModel
    {
        public int iSno { get; set; }
        public string sHAWBNo { get; set; }
        public string sPortOfOrigin { get; set; }
        public string sPortOfDestination { get; set; }
        public string sPackages { get; set; }
        public string sWeight { get; set; }
        public string sDescription { get; set; }
    }

    public class CheckListIGMCLPDFModel
    {
        public string sFlightNo { get; set; }
        public string sCustomLocation { get; set; }
        public string sCustomLocationCode { get; set; }     
        public string sPortOfOrigin { get; set; }
        public string sPortOfDestination { get; set; }
        public string sDepatureDate { get; set; }
        public string sArrivalDate { get; set; }
        public string sFlightRegNo { get; set; }
        public string sTime { get; set; }
        //MAWB Details
        public List<CheckListAirIGMMAWBPDFModel> lstMAWBData { get; set; }
    }

    public class CheckListAirIGMMAWBPDFModel
    {
        public string sMAWBNo { get; set; }
        public string sPortOfOrigin { get; set; }
        public string sPortOfDestination { get; set; }
        public decimal sPackages { get; set; }
        public decimal sWeight { get; set; }
        public string sDescription { get; set; }
    }
}
