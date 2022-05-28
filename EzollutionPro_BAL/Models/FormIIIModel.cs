using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Models
{
    public class FormIIIModel
    {
        public string ShippingLine { get; set; }
        public string IMOCode { get; set; }
        public string CallSign { get; set; }
        public string VoyageNo { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public string PortOfFinalDestination { get; set; }
        public string CARNNo { get; set; }
        public string IGMNo { get; set; }
        public string IGMDate { get; set; }
        public string MBLDate { get; set; }
        public string MBLNumber { get; set; }
        public string AgentName { get; set; }
        public List<ContainerFormIIIData> lstContainerFormIIIData { get; set; }
    }

    public class ContainerFormIIIData
    {
        public string LineNo { get; set; }
        public string HBLNo { get; set; }
        public string HBLDate { get; set; }
        public string NoofPackages { get; set; }
        public string GrossWeight { get; set; }
        public string MarksAndNumber { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string NameOfConsigneeAndAddress { get; set; }
        public string ContainerDetails { get; set; }
        public string CargoMovement { get; set; }
    }
}

