using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EzollutionPro_BAL.Models
{
    public class SchedulingViewModel
    {
        public int iSchedulingId { get; set; }
        public string sClientName { get; set; }
        [Required(ErrorMessage = "MBL Number is a required field.")]
        [MaxLength(20,ErrorMessage ="MBL Number cannot exceed 20 characters.")]
        public string sMBLNumber { get; set; }
        [Required(ErrorMessage = "Container Number is a required field.")]
        [MaxLength(11,ErrorMessage ="Container Number cannot exceed 11 characters.")]
        public string sContainerNumber { get; set; }
        public string sPOD { get; set; }
        public string sFPOD { get; set; }
        [Required(ErrorMessage ="Vessel Name is a required field.")]
        [MaxLength(50, ErrorMessage = "Vessel Name cannot exceed 50 characters.")]
        public string sVesselName { get; set; }
        [Required(ErrorMessage = "Recieved On is a required field.")]
        public string sRecieveOn { get; set; }
        [MaxLength(80, ErrorMessage = "Remarks cannot exceed 80 characters.")]
        public string sRemarks { get; set; }
        [MaxLength(80, ErrorMessage = "Invoice Remarks cannot exceed 80 characters.")]
        public string sInvoiceRemarks { get; set; }
        public string sShippingLine { get; set; }
        [Required(ErrorMessage ="Estimated Date Of Arrival is a required field.")]
        public string sEDA { get; set; }
        public string sCheckListSent { get; set; }
        public string sCheckListApproved { get; set; }
        public short iSAction { get; set; }
        [Required(ErrorMessage = "Agent is a required field.")]
        public int iClientId { get; set; }
        [Required(ErrorMessage = "Port Of Destination is a required field.")]
        public int iPODId { get; set; }
        [Required(ErrorMessage = "Final Destination is a required field.")]
        public int iFPODId { get; set; }
        [Required(ErrorMessage ="Shipping line is a required field.")]
        public int iShippingId { get; set; }
        public string sIGMNo { get; set; }
        public string sIGMDate { get; set; }
        public string sRecievedOn { get; set; }

        public int? iModifiedBy { get; set; }
        public DateTime dtModifiedOn { get; set; }
        public string sModifiedFromIp { get; set; }

                   
    }


    public class ChecklistData
    {
        public int iSchedulingId { get; set; }
        public List<MBLData> lstMBLData { get; set; }
        public List<HBLData> lstHBLData { get; set; }
        public List<ContainerData> lstContainerData { get; set; }
        public List<BondData> lstBondData { get; set; }
    }

    public class MBLData
    {
        public string sPOSCode { get; set; }
        public string sPODCode { get; set; }
        public string sPOFDCode  { get; set; }
        public string sShippingLineMLOCode { get; set; }
        public string sMBLNumber { get; set; }
        public int iMBLId { get; set; }
        public int iSchedulingId { get; set; }
        [Required(ErrorMessage ="MBL Date is a required field.")]
        public string sMBLDate { get; set; }
        [Required(ErrorMessage ="Port Of Shipment is a required field.")]
        public int iPOSId { get; set; }
        [Required(ErrorMessage = "Cargo Movement is a required field.")]
        public string sCargoMovement { get; set; }
        [Required(ErrorMessage = "Destination Code (CFS Code) is a required field.")]
        [MaxLength(10,ErrorMessage ="Destination Code (CFS Code) should have 10 characters.")]
        [MinLength(10, ErrorMessage = "Destination Code (CFS Code) should have 10 characters.")]
        public string sCFSCode { get; set; }
        [MaxLength(7,ErrorMessage = "IGM Number cannot exceed 7 digits.")]
        [Display(Name ="IGM Number")]
        [RegularExpression("([0-9]+)", ErrorMessage = "IGM Number can only be a number")]
        public string sIGMNo { get; set; }
        public string sIGMDate { get; set; }
        [MaxLength(10,ErrorMessage ="IMO Code cannot exceed 10 characters.")]
        public string sIMOCode { get; set; }
        [MaxLength(10,ErrorMessage = "Vessel Code cannot exceed 10 characters.")]
        public string sVesselCode { get; set; }
        [MaxLength(10,ErrorMessage = "Voyage Number cannot exceed 10 characters.")]
        public string sVoyageNo { get; set; }
        [MaxLength(4,ErrorMessage = "Line Number cannot exceed 4 digits.")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Line Number can only be a number")]
        [Display(Name ="Line Number")]
        public string sLineNo { get; set; }

        public string sPODName { get; set; }
        public string sPOFDNames { get; set; }
    }

    public class HBLData
    {
        public int iMBLId { get; set; }
        [Required(ErrorMessage ="House Bill of Landing Number is a required field.")]
        [MaxLength(20,ErrorMessage ="House Bill of Landing Number cannot exceed 20 characters.")]
        public string sHouseBillofLadingNo { get; set; }
        [Required(ErrorMessage ="House Bill of Landing Date is a required field.")]
        public string sHouseBillofLadingDate { get; set; }
        [MaxLength(35, ErrorMessage = "Importer Name cannot exceed 35 characters.")]
        [Required(ErrorMessage ="Importer Name is a required field.")]
        public string sImporterName { get; set; }
        [MaxLength(35, ErrorMessage = "Importer Address1 cannot exceed 35 characters.")]
        [Required(ErrorMessage = "Importer Address1 is a required field.")]
        public string sImporterAddress1 { get; set; }
        [MaxLength(35, ErrorMessage = "Importer Address2 cannot exceed 35 characters.")]
        [Required(ErrorMessage = "Importer Address2 is a required field.")]
        public string sImporterAddress2 { get; set; }
        [MaxLength(35, ErrorMessage = "Importer Address3 cannot exceed 35 characters.")]
        public string sImporterAddress3 { get; set; }
        [Range(0,99999999, ErrorMessage = "Total Number of Packages cannot exceed 8 digits.")]
        [Required(ErrorMessage ="Total Number of Packages is a required field.")]
        public decimal dTotalNumberofPackages { get; set; }
        public string sPackageCode { get; set; }
        [Range(0, 999999999999.999, ErrorMessage = "Gross Weight cannot exceed 12 digits and 3 decimal characters.")]
        [Required(ErrorMessage ="Gross Weight is a required field.")]
        public decimal dGrossWeight { get; set; }
        [MaxLength(3,ErrorMessage ="Unit of weight cannot exceed 3 characters.")]
        public string sUnitofWeight { get; set; }
        [MaxLength(255,ErrorMessage ="Marks and Numbers cannot exceed 255 characters.")]
        [Required(ErrorMessage ="Marks and Numbers is a required field.")]
        public string sMarksandNumbers { get; set; }
        [MaxLength(200, ErrorMessage = "Goods Description cannot exceed 200 characters.")]
        public string sGoodsDescription { get; set; }
        public string sMBLNumber { get; set; }
        public int iHBLId { get; set; }
        public int iSchedulingId { get; set; }
        public byte iSublineNo { get; set; }
    }

    public class BondData
    {
        public int iSchedulingId { get; set; }
        public int iBondID { get; set; }
        [Required(ErrorMessage ="Bond Number is a required field.")]
        [Range(0,9999999999,ErrorMessage ="Bond Number cannot exceed 10 digits.")]
        public long dBondNumber { get; set; }
        [MaxLength(10,ErrorMessage ="Carrier Code cannot exceed 10 characters.")]
        public string sCarrierCode { get; set; }
        [MaxLength(1, ErrorMessage = "Movement Type cannot exceed 1 characters.")]
        public string sMovementType { get; set; }
        public string sMLOCode { get; set; }
        public string sMBLNumber { get; set; }
    }
    public class ContainerData
    {
        public string sMBLNumber { get; set; }
        public int iContainerId { get; set; }
        public int iHBLId { get; set; }
        [Required(ErrorMessage ="Container Number is a required field.")]
        [MaxLength(11,ErrorMessage ="Container Number cannot exceed 11 characters.")]
        public string sContainerNumber { get; set; }
        [MaxLength(15, ErrorMessage = "Container Seal Number cannot exceed 15 characters.")]
        [Required(ErrorMessage = "Container Seal Number is a required field.")]
        public string sContainerSealNumber { get; set; }
        [Required(ErrorMessage = "Total Packages is a required field.")]
        [Range(0,99999999,ErrorMessage ="Total packages cannot exceed 8 digits.")]
        public decimal? nTotalPackages { get; set; }
        [Range(0, 999999999999.99, ErrorMessage = "Container Weight cannot exceed 12 digits and 2 decimal.")]
        [Required(ErrorMessage ="Container Weight is a required field.")]
        public decimal? nContainerWeight { get; set; }
        [Required(ErrorMessage ="Container Status is a required field.")]
        public string sContainerStatus { get; set; }
        [Required(ErrorMessage = "ISO Code is a required field.")]
        public string sISOCode { get; set; }
        public int iSchedulingId { get; set; }
        public byte iSubLineNo { get; set; }
    }
    public enum Scheduling
    {
        Pending = 1,
        Transmit = 2,
        Cancel = 3,
        Approved=4
    }

    public class BulkUploadHBLModel
    {
        [Required(ErrorMessage ="HBL File is a required field.")]
        public HttpPostedFileBase HBLFile { get; set; }
    }
}