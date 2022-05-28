using EzollutionPro_BAL.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EzollutionPro_BAL.Services
{
    public enum TemplateOrientation
    {
        PORTRAIT,
        LANDSCAPE,
        ON_IMAGE
    }
    public class PDFFonts
    {
        public Font titleFont { get; set; }
        public Font checkListFont { get; set; }
        public Font checkListTableTitleFont { get; set; }
        public Font subTitleFont { get; set; }
        public Font paragraphFont { get; set; }
        public Font subParagarhFont { get; set; }
        public Font priceFont { get; set; }
        public Font headerpriceFont { get; set; }
        public Font headerParagraphFont { get; set; }
        public Font formIIIfont { get; set; }
        public Font formIIITitlefont { get; set; }
        public PDFFonts init()
        {
            titleFont = new Font(BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/cambriab.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED), 16)
            {
                Color = new BaseColor(0, 0, 0)
            };

            checkListFont = new Font(BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/Cambria.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED), 11)
            {
                Color = new BaseColor(0, 0, 0)
            };

            checkListTableTitleFont = new Font(BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/Cambriab.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED), 11)
            {
                Color = new BaseColor(0, 0, 0)
            };

            formIIIfont = new Font(BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/Cambria.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED), 8)
            {
                Color = new BaseColor(0, 0, 0)
            };

            formIIITitlefont = new Font(BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/Cambriab.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED), 8)
            {
                Color = new BaseColor(0, 0, 0)
            };

            subTitleFont = (new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/Cambriab.ttf", iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.EMBEDDED), 14));
            subTitleFont.Color = new iTextSharp.text.BaseColor(0, 0, 0);

            paragraphFont = (new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/Cambria.ttf", iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.EMBEDDED), 7));
            paragraphFont.Color = new iTextSharp.text.BaseColor(0, 0, 0);

            subParagarhFont = (new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/Cambria.ttf", iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.EMBEDDED), 7));
            subParagarhFont.Color = new iTextSharp.text.BaseColor(96, 96, 96);

            priceFont = (new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/Cambria.ttf", iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.EMBEDDED), 24));
            priceFont.Color = new iTextSharp.text.BaseColor(96, 96, 96);

            headerpriceFont = (new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/cambriab.ttf", iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.EMBEDDED), 18));
            headerpriceFont.Color = new iTextSharp.text.BaseColor(255, 255, 255);

            headerParagraphFont = (new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/Content/Fonts") + "/cambriab.ttf", iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.EMBEDDED), 18));
            headerParagraphFont.Color = new iTextSharp.text.BaseColor(255, 255, 255);

            return this;
        }
    }

    public class GeneratePDFService
    {
        private string outputPath;
        private CheckListPDFModel model;
        private CheckListMAWBPDFModel MAWBModel;
        private CheckListIGMCLPDFModel AIRIGMFLIGHTModel;
        private PDFFonts pdfFonts;
        private TemplateOrientation orientation;
        private Document document;
        private PdfWriter pdfWriter;
        private FormIIIModel formIIIModel;
        public GeneratePDFService(string outputPath, CheckListPDFModel model, string OutputPath)
        {
            this.outputPath = outputPath;
            this.model = model;
        }

        public GeneratePDFService(string outputPath, CheckListMAWBPDFModel model, string OutputPath)
        {
            this.outputPath = outputPath;
            this.MAWBModel = model;
        }

        public GeneratePDFService(string outputPath, CheckListIGMCLPDFModel model, string OutputPath)
        {
            this.outputPath = outputPath;
            this.AIRIGMFLIGHTModel = model ;
        }

        public GeneratePDFService(string outputPath, FormIIIModel model, string OutputPath)
        {
            this.outputPath = outputPath;
            this.formIIIModel = model;
        }

        public GeneratePDFService AddFonts(PDFFonts pdfFonts)
        {
            this.pdfFonts = pdfFonts;
            return this;
        }

        public GeneratePDFService SetOrientation(TemplateOrientation orientation)
        {
            this.orientation = orientation;
            return this;
        }

        public void Start()
        {
            try
            {
                document = new Document(PageSize.A4);
                document.SetMargins(20, 20, 20, 20);
                if (File.Exists(outputPath))
                {
                    pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Truncate));
                }
                else
                {
                    pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
                }
                pdfWriter.SetLinearPageMode();
                CheckListTemplate.Instance.setConfig(document, orientation, model, pdfFonts);
                CheckListTemplate.Instance.SetPDFWriter(pdfWriter);
                CheckListTemplate.Instance.setPageSize(pdfWriter.PageSize.Rotate());
                CheckListTemplate.Instance.GetDocument().Open();
                CheckListTemplate.Instance.CreateHeading();
                CheckListTemplate.Instance.CreateMBLTable();
            }
            catch (Exception)
            {
                CheckListTemplate.Instance.GetDocument().Close();
            }
            finally
            {
                CheckListTemplate.Instance.GetDocument().Close();
            }

        }

        public void StartFormIII()
        {
            try
            {
                document = new Document(PageSize.A4.Rotate());
                document.SetMargins(20, 20, 20, 20);
                if (File.Exists(outputPath))
                {
                    pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Truncate));
                }
                else
                {
                    pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
                }
                pdfWriter.SetLinearPageMode();
                FormIIITemplate.Instance.setConfig(document, orientation, formIIIModel, pdfFonts);
                FormIIITemplate.Instance.SetPDFWriter(pdfWriter);
                FormIIITemplate.Instance.setPageSize(pdfWriter.PageSize.Rotate());
                FormIIITemplate.Instance.GetDocument().Open();
                FormIIITemplate.Instance.CreateHeading();
                FormIIITemplate.Instance.CreateMBLPart();
            }
            catch (Exception)
            {
                FormIIITemplate.Instance.GetDocument().Close();
            }
            finally
            {
                FormIIITemplate.Instance.GetDocument().Close();
            }

        }

        public void StartMAWB()
        {
            try
            {
                document = new Document(PageSize.A4.Rotate());
                document.SetMargins(20, 20, 20, 20);
                if (File.Exists(outputPath))
                {
                    pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Truncate));
                }
                else
                {
                    pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
                }
                pdfWriter.SetLinearPageMode();
                CGMCheckListTemplate.Instance.setConfig(document, orientation, MAWBModel, pdfFonts);
                CGMCheckListTemplate.Instance.SetPDFWriter(pdfWriter);
                CGMCheckListTemplate.Instance.setPageSize(pdfWriter.PageSize.Rotate());
                CGMCheckListTemplate.Instance.GetDocument().Open();
                CGMCheckListTemplate.Instance.CreateHeading();
                CGMCheckListTemplate.Instance.CreateMAWBPart();
            }
            catch (Exception)
            {
                CGMCheckListTemplate.Instance.GetDocument().Close();
            }
            finally
            {
                CGMCheckListTemplate.Instance.GetDocument().Close();
            }

        }

        public void StartAIRIGMFlight()
        {
            try
            {
                document = new Document(PageSize.A4.Rotate());
                document.SetMargins(20, 20, 20, 20);
                if (File.Exists(outputPath))
                {
                    pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Truncate));
                }
                else
                {
                    pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
                }
                pdfWriter.SetLinearPageMode();
                IGMAIRCheckListTemplate.Instance.setConfig(document, orientation, AIRIGMFLIGHTModel, pdfFonts);
                IGMAIRCheckListTemplate.Instance.SetPDFWriter(pdfWriter);
                IGMAIRCheckListTemplate.Instance.setPageSize(pdfWriter.PageSize.Rotate());
                IGMAIRCheckListTemplate.Instance.GetDocument().Open();
                IGMAIRCheckListTemplate.Instance.CreateHeading();
                IGMAIRCheckListTemplate.Instance.CreateMAWBPart();
            }
            catch (Exception)
            {
                IGMAIRCheckListTemplate.Instance.GetDocument().Close();
            }
            finally
            {
                IGMAIRCheckListTemplate.Instance.GetDocument().Close();
            }

        }


    }

    public class FormIIITemplate
    {
        private static FormIIITemplate instance = null;

        private FormIIITemplate()
        {
        }

        public static FormIIITemplate Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FormIIITemplate();
                }
                return instance;
            }
        }
        private Document document;
        private TemplateOrientation templateOrientation;
        private FormIIIModel model;
        private PDFFonts pdfFonts;
        private Rectangle pageSize;
        private PdfWriter pdfWriter;
        public void setConfig(Document document, TemplateOrientation templateOrientation, FormIIIModel model, PDFFonts pdfFonts)
        {
            this.document = document;
            this.templateOrientation = templateOrientation;
            this.model = model;
            this.pdfFonts = pdfFonts;
        }

        public void setPageSize(Rectangle pageSize)
        {
            this.pageSize = pageSize;
        }

        public Document GetDocument()
        {
            return document;
        }

        public void SetPDFWriter(PdfWriter writer)
        {
            pdfWriter = writer;
        }

        internal void CreateHeading()
        {
            Paragraph heading = new Paragraph("FORM III", pdfFonts.titleFont)
            {
                Alignment = 1
            };
            document.Add(heading);
            document.Add(new Paragraph("", pdfFonts.checkListFont));
            document.Add(new Paragraph("Cargo Declaration", pdfFonts.titleFont)
            {
                Alignment = 1
            });
            document.Add(Chunk.NEWLINE);
        }
        internal void CreateMBLPart()
        {
            PdfPTable tbl = new PdfPTable(8)
            {
                WidthPercentage = 100,
            };
            tbl.DefaultCell.Border = Rectangle.NO_BORDER;

            tbl.AddCell(new Paragraph("Shipping Line: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.ShippingLine, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("IMO Code: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.IMOCode, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("Call Sign: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.CallSign, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("Voyage No.: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.VoyageNo, pdfFonts.formIIIfont));


            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIIfont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIIfont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIIfont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("Port of Loading: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.PortOfLoading, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("Port of Destination: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.PortOfDestination, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("Final Destination: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.PortOfFinalDestination, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("CARN No.: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.CARNNo, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIIfont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIIfont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIIfont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("\n", pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("IGM No.: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.IGMNo, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("IGM Date: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.IGMDate, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("MBL No: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.MBLNumber, pdfFonts.formIIIfont));

            tbl.AddCell(new Paragraph("MBL Date: ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph(model.MBLDate, pdfFonts.formIIIfont));

            document.Add(tbl);
            document.Add(Chunk.NEWLINE);

            InsertTableData();
        }

        internal void InsertTableData()
        {
            PdfPTable tbl = new PdfPTable(8)
            {
                WidthPercentage = 100,
            };
            int[] firstTablecellwidth = { 7, 10, 5, 5, 15, 10, 38, 10 };
            tbl.SetWidths(firstTablecellwidth);
            tbl.AddCell(new Paragraph("Line No ", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("HBL No. & HBL Date", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("No. & Kind of Packages", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("Gross Weight and Unit", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("Marks and Numbers", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("Description of Goods", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("Name of Consignee & Address", pdfFonts.formIIITitlefont));
            tbl.AddCell(new Paragraph("Container Details", pdfFonts.formIIITitlefont));
            foreach (var item in model.lstContainerFormIIIData)
            {
                tbl.AddCell(new Paragraph(item.CargoMovement + item.LineNo, pdfFonts.formIIIfont));
                tbl.AddCell(new Paragraph(item.HBLNo + "\n" + item.HBLDate, pdfFonts.formIIIfont));
                tbl.AddCell(new Paragraph(item.NoofPackages, pdfFonts.formIIIfont));
                tbl.AddCell(new Paragraph(item.GrossWeight, pdfFonts.formIIIfont));
                tbl.AddCell(new Paragraph(item.MarksAndNumber, pdfFonts.formIIIfont));
                tbl.AddCell(new Paragraph(item.DescriptionOfGoods, pdfFonts.formIIIfont));
                tbl.AddCell(new Paragraph(item.NameOfConsigneeAndAddress, pdfFonts.formIIIfont));
                tbl.AddCell(new Paragraph(item.ContainerDetails, pdfFonts.formIIIfont));
            }
            document.Add(tbl);

            document.Add(Chunk.NEWLINE);
            var lstItem = string.Join(",", model.lstContainerFormIIIData.Select(z => z.LineNo).ToList());
            document.Add(new Paragraph("We certify that all items indicated on this hard copy of IGM have been fully represented in magnetic medium.", pdfFonts.subParagarhFont));
            document.Add(new Paragraph(""));
            document.Add(new Chunk("THIS TO CERTIFY THAT ITEM(S) NO. ", pdfFonts.formIIIfont));
            document.Add(new Chunk(lstItem, pdfFonts.formIIITitlefont));
            document.Add(new Chunk(" ARE ON ACCOUNT OF OUR PRINCIPALS, WE ARE RESPONSIBLE FOR THE FULL OUTTURN OF THE CARGO MANIFESTED UNDER THE ABOVE ITEMS AND WILL BE LIBALE FOR ANY PENALTY WE HEREBY HOLD.", pdfFonts.formIIIfont));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph("AGENTS INDEMNIFED FOR ANY SHORTLANGING SURVEY SHORTAGES FOR THE ABOVE CARGO.", pdfFonts.formIIIfont));
            document.Add(new Chunk("FOR (", pdfFonts.formIIIfont));
            document.Add(new Chunk(model.AgentName, pdfFonts.formIIITitlefont));
            document.Add(new Chunk(" )", pdfFonts.formIIIfont));
            document.Add(Chunk.NEWLINE);
            document.Add(new Paragraph("AS AGENT", pdfFonts.formIIIfont));

        }

    }

    public class CheckListTemplate
    {
        private static CheckListTemplate instance = null;

        private CheckListTemplate()
        {
        }

        public static CheckListTemplate Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CheckListTemplate();
                }
                return instance;
            }
        }

        private Document document;
        private TemplateOrientation templateOrientation;
        private CheckListPDFModel model;
        private PDFFonts pdfFonts;
        private Rectangle pageSize;
        private PdfWriter pdfWriter;

        public void CreateMBLTable()
        {
            Paragraph heading = new Paragraph("MBL Information", pdfFonts.subTitleFont);
            heading.Alignment = Element.ALIGN_LEFT;
            document.Add(heading);
            PdfPTable tblMBL = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            tblMBL.AddCell(new Paragraph("Shipping Line", pdfFonts.checkListTableTitleFont));
            tblMBL.AddCell(new Paragraph(model.ShippingLine, pdfFonts.checkListFont));

            tblMBL.AddCell(new Paragraph("MBL Number", pdfFonts.checkListTableTitleFont));
            tblMBL.AddCell(new Paragraph(model.MBLNumber, pdfFonts.checkListFont));

            tblMBL.AddCell(new Paragraph("MBL Date", pdfFonts.checkListTableTitleFont));
            tblMBL.AddCell(new Paragraph(model.MBLDate, pdfFonts.checkListFont));

            tblMBL.AddCell(new Paragraph("Port of Loading", pdfFonts.checkListTableTitleFont));
            tblMBL.AddCell(new Paragraph(model.PortOfLoading, pdfFonts.checkListFont));

            tblMBL.AddCell(new Paragraph("Port of Destination", pdfFonts.checkListTableTitleFont));
            tblMBL.AddCell(new Paragraph(model.PortOfDestination, pdfFonts.checkListFont));

            tblMBL.AddCell(new Paragraph("Final Destination", pdfFonts.checkListTableTitleFont));
            tblMBL.AddCell(new Paragraph(model.PortOfFinalDestination, pdfFonts.checkListFont));

            tblMBL.AddCell(new Paragraph("Vessel Name", pdfFonts.checkListTableTitleFont));
            tblMBL.AddCell(new Paragraph(model.VesselName, pdfFonts.checkListFont));

            tblMBL.AddCell(new Paragraph("EDA", pdfFonts.checkListTableTitleFont));
            tblMBL.AddCell(new Paragraph(model.EDA, pdfFonts.checkListFont));
            document.Add(tblMBL);

            CreateHBLTable();
        }

        public void CreateHBLTable()
        {
            foreach (var hblData in model.lstHBLData)
            {
                document.Add(new Chunk(Chunk.NEWLINE));
                Paragraph heading = new Paragraph("HBL Information", pdfFonts.subTitleFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                document.Add(heading);
                PdfPTable tblHBL = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                tblHBL.AddCell(new Paragraph("HBL No", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.HBLNumber, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("HBL Date", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.HBLDate, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("Subline No.", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.SublineNo, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("Cargo Movement", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.CargoMovement, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("Importer Name", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.ImporterName, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("Importer Address", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.ImporterAddress, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("No of Packages", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.NoOfPackages, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("Gross Weight", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.GrossWeight, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("Goods Description", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.GoodsDescription, pdfFonts.checkListFont));

                tblHBL.AddCell(new Paragraph("Marks and Numbers", pdfFonts.checkListTableTitleFont));
                tblHBL.AddCell(new Paragraph(hblData.MarksAndNumbers, pdfFonts.checkListFont));
                document.Add(tblHBL);
                foreach (var containerData in hblData.lstContainerData)
                    CreateContainerTable(containerData);
            }
        }

        public void CreateContainerTable(ContainerPDFData containerPDFData)
        {
            document.Add(new Chunk(Chunk.NEWLINE));
            Paragraph heading = new Paragraph("Container Information", pdfFonts.subTitleFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            document.Add(heading);
            PdfPTable tblContainer = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            tblContainer.AddCell(new Paragraph("Container Number", pdfFonts.checkListTableTitleFont));
            tblContainer.AddCell(new Paragraph(containerPDFData.ContainerNumber, pdfFonts.checkListFont));

            tblContainer.AddCell(new Paragraph("Seal Number", pdfFonts.checkListTableTitleFont));
            tblContainer.AddCell(new Paragraph(containerPDFData.SealNumber, pdfFonts.checkListFont));

            tblContainer.AddCell(new Paragraph("Container Status", pdfFonts.checkListTableTitleFont));
            tblContainer.AddCell(new Paragraph(containerPDFData.ContainerStatus, pdfFonts.checkListFont));

            tblContainer.AddCell(new Paragraph("Total Packages", pdfFonts.checkListTableTitleFont));
            tblContainer.AddCell(new Paragraph(containerPDFData.TotalPackages, pdfFonts.checkListFont));

            tblContainer.AddCell(new Paragraph("Container Weight", pdfFonts.checkListTableTitleFont));
            tblContainer.AddCell(new Paragraph(containerPDFData.ContainerWeight, pdfFonts.checkListFont));

            tblContainer.AddCell(new Paragraph("Container Type", pdfFonts.checkListTableTitleFont));
            tblContainer.AddCell(new Paragraph(containerPDFData.ContainerType, pdfFonts.checkListFont));

            document.Add(tblContainer);
        }

        internal void CreateHeading()
        {
            Image image = Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images") + "/logo.png");
            image.Alignment = (Image.ALIGN_CENTER);
            image.ScaleToFit(new Rectangle(200, 80));
            document.Add(image);
            Paragraph heading = new Paragraph("SEA CONSOL MANIFEST CHECKLIST", pdfFonts.titleFont)
            {
                Alignment = 1
            };
            document.Add(heading);
        }

        public void setConfig(Document document, TemplateOrientation templateOrientation, CheckListPDFModel model, PDFFonts pdfFonts)
        {
            this.document = document;
            this.templateOrientation = templateOrientation;
            this.model = model;
            this.pdfFonts = pdfFonts;
        }

        public void setPageSize(Rectangle pageSize)
        {
            this.pageSize = pageSize;
        }

        public Document GetDocument()
        {
            return document;
        }

        public void SetPDFWriter(PdfWriter writer)
        {
            pdfWriter = writer;
        }

        public void ClosePDF()
        {
            throw new NotImplementedException();
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }


    }

    public class IGMCheckListTemplate
    {
        private static IGMCheckListTemplate instance = null;

        private IGMCheckListTemplate()
        {
        }

        public static IGMCheckListTemplate Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IGMCheckListTemplate();
                }
                return instance;
            }
        }

        private Document document;
        private TemplateOrientation templateOrientation;
        private CheckListMAWBPDFModel model;
        private PDFFonts pdfFonts;
        private Rectangle pageSize;
        private PdfWriter pdfWriter;

        public void setConfig(Document document, TemplateOrientation templateOrientation, CheckListMAWBPDFModel model, PDFFonts pdfFonts)
        {
            this.document = document;
            this.templateOrientation = templateOrientation;
            this.model = model;
            this.pdfFonts = pdfFonts;
        }

        public void setPageSize(Rectangle pageSize)
        {
            this.pageSize = pageSize;
        }

        public Document GetDocument()
        {
            return document;
        }

        public void SetPDFWriter(PdfWriter writer)
        {
            pdfWriter = writer;
        }

        internal void CreateHeading()
        {
            Paragraph heading = new Paragraph("AIR CGM CHECKLIST", pdfFonts.titleFont)
            {
                Alignment = 1
            };
            document.Add(heading);
            document.Add(new Paragraph("\n", pdfFonts.checkListFont));
            document.Add(new Paragraph("CUSTOM LOCATION: " + model.sCustomLocation + " (" + model.sCustomLocationCode + ")\n", pdfFonts.subTitleFont)
            {
                Alignment = 0
            });
            document.Add(new Paragraph("\nMAWB Details:", pdfFonts.subTitleFont)
            {
                Alignment = 0
            });
            document.Add(new Chunk(""));
        }

        internal void CreateMAWBPart()
        {
            PdfPTable tbl = new PdfPTable(6)
            {
                WidthPercentage = 100,
            };
            int[] firstTablecellwidth = { 16, 17, 16, 17, 16, 18 };
            tbl.SetWidths(firstTablecellwidth);
            tbl.AddCell(new Paragraph("MAWB NO", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("PORT OF ORIGIN", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("PORT OF DESTINATION", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("PACKAGES", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("WEIGHT", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("Description", pdfFonts.checkListTableTitleFont));

            tbl.AddCell(new Paragraph(model.sMAWBNo, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sPortOfOrigin, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sPortOfDestination, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sPackages, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sWeight, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sDescription, pdfFonts.checkListFont));
            document.Add(tbl);

            document.Add(new Paragraph("\nHAWB Details:", pdfFonts.subTitleFont)
            {
                Alignment = 0
            });
            document.Add(new Chunk(""));

            PdfPTable tbl2 = new PdfPTable(7)
            {
                WidthPercentage = 100,
                PaddingTop = 5
            };
            int[] secondTablecellwidth = { 10, 15, 15, 15, 15, 15, 15 };
            tbl2.SetWidths(secondTablecellwidth);
            tbl2.AddCell(new Paragraph("S. NO", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("HAWB NO", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PORT OF ORIGIN", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PORT OF DESTINATION", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PACKAGES", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("WEIGHT", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("Description", pdfFonts.checkListTableTitleFont));
            foreach (var item in model.lstHAWBData)
            {
                tbl2.AddCell(new Paragraph(item.iSno.ToString(), pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sHAWBNo, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sPortOfOrigin, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sPortOfDestination, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sPackages, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sWeight, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sDescription, pdfFonts.checkListFont));
            }
            document.Add(tbl2);

            Paragraph p = new Paragraph("\n")
            {
                IndentationLeft = 320
            };

            PdfPTable tbl3 = new PdfPTable(3)
            {
                WidthPercentage = 75
            };
            int[] thirdtableWidths = { 34, 33, 33 };
            tbl3.SetWidths(thirdtableWidths);
            tbl3.AddCell(new Paragraph("TOTAL", pdfFonts.checkListTableTitleFont));
            tbl3.AddCell(new Paragraph(model.sTotalPackages, pdfFonts.checkListTableTitleFont));
            tbl3.AddCell(new Paragraph(model.sTotalWeight, pdfFonts.checkListTableTitleFont));
            tbl3.HorizontalAlignment = Element.ALIGN_LEFT;
            p.Add(tbl3);
            document.Add(p);

        }
    }

    public class CGMCheckListTemplate
    {
        private static CGMCheckListTemplate instance = null;

        private CGMCheckListTemplate()
        {
        }

        public static CGMCheckListTemplate Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CGMCheckListTemplate();
                }
                return instance;
            }
        }

        private Document document;
        private TemplateOrientation templateOrientation;
        private CheckListMAWBPDFModel model;
        private PDFFonts pdfFonts;
        private Rectangle pageSize;
        private PdfWriter pdfWriter;

        public void setConfig(Document document, TemplateOrientation templateOrientation, CheckListMAWBPDFModel model, PDFFonts pdfFonts)
        {
            this.document = document;
            this.templateOrientation = templateOrientation;
            this.model = model;
            this.pdfFonts = pdfFonts;
        }

        public void setPageSize(Rectangle pageSize)
        {
            this.pageSize = pageSize;
        }

        public Document GetDocument()
        {
            return document;
        }

        public void SetPDFWriter(PdfWriter writer)
        {
            pdfWriter = writer;
        }

        internal void CreateHeading()
        {
            Paragraph heading = new Paragraph("AIR CGM CHECKLIST", pdfFonts.titleFont)
            {
                Alignment = 1
            };
            document.Add(heading);
            document.Add(new Paragraph("\n", pdfFonts.checkListFont));
            document.Add(new Paragraph("CUSTOM LOCATION: " + model.sCustomLocation + " (" + model.sCustomLocationCode + ")\n", pdfFonts.subTitleFont)
            {
                Alignment = 0
            });
            document.Add(new Paragraph("\nMAWB Details:", pdfFonts.subTitleFont)
            {
                Alignment = 0
            });
            document.Add(new Chunk(""));
        }

        internal void CreateMAWBPart()
        {
            PdfPTable tbl = new PdfPTable(6)
            {
                WidthPercentage = 100,
            };
            int[] firstTablecellwidth = { 16, 17, 16, 17, 16, 18 };
            tbl.SetWidths(firstTablecellwidth);
            tbl.AddCell(new Paragraph("MAWB NO", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("PORT OF ORIGIN", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("PORT OF DESTINATION", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("PACKAGES", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("WEIGHT", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("Description", pdfFonts.checkListTableTitleFont));

            tbl.AddCell(new Paragraph(model.sMAWBNo, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sPortOfOrigin, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sPortOfDestination, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sPackages, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sWeight, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sDescription, pdfFonts.checkListFont));
            document.Add(tbl);

            document.Add(new Paragraph("\nHAWB Details:", pdfFonts.subTitleFont)
            {
                Alignment = 0
            });
            document.Add(new Chunk(""));

            PdfPTable tbl2 = new PdfPTable(7)
            {
                WidthPercentage = 100,
                PaddingTop=5
            };
            int[] secondTablecellwidth = { 10, 15, 15, 15, 15, 15, 15 };
            tbl2.SetWidths(secondTablecellwidth);
            tbl2.AddCell(new Paragraph("S. NO", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("HAWB NO", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PORT OF ORIGIN", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PORT OF DESTINATION", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PACKAGES", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("WEIGHT", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("Description", pdfFonts.checkListTableTitleFont));
            foreach (var item in model.lstHAWBData)
            {
                tbl2.AddCell(new Paragraph(item.iSno.ToString(), pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sHAWBNo, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sPortOfOrigin, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sPortOfDestination, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sPackages, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sWeight, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sDescription, pdfFonts.checkListFont));
            }
            document.Add(tbl2);

            Paragraph p = new Paragraph("\n")
            {
                IndentationLeft = 320
            };

            PdfPTable tbl3 = new PdfPTable(3)
            {
                WidthPercentage = 75
            };
            int[] thirdtableWidths= {  34, 33, 33};
            tbl3.SetWidths(thirdtableWidths);
            tbl3.AddCell(new Paragraph("TOTAL", pdfFonts.checkListTableTitleFont));
            tbl3.AddCell(new Paragraph(model.sTotalPackages, pdfFonts.checkListTableTitleFont));
            tbl3.AddCell(new Paragraph(model.sTotalWeight, pdfFonts.checkListTableTitleFont));
            tbl3.HorizontalAlignment = Element.ALIGN_LEFT;
            p.Add(tbl3);
            document.Add(p);

        }
    }

    public class IGMAIRCheckListTemplate
    {
        private static IGMAIRCheckListTemplate instance = null;

        private IGMAIRCheckListTemplate()
        {
        }

        public static IGMAIRCheckListTemplate Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IGMAIRCheckListTemplate();
                }
                return instance;
            }
        }

        private Document document;
        private TemplateOrientation templateOrientation;
        private CheckListIGMCLPDFModel model;
        private PDFFonts pdfFonts;
        private Rectangle pageSize;
        private PdfWriter pdfWriter;

        public void setConfig(Document document, TemplateOrientation templateOrientation, CheckListIGMCLPDFModel model, PDFFonts pdfFonts)
        {
            this.document = document;
            this.templateOrientation = templateOrientation;
            this.model = model;
            this.pdfFonts = pdfFonts;
        }

        public void setPageSize(Rectangle pageSize)
        {
            this.pageSize = pageSize;
        }

        public Document GetDocument()
        {
            return document;
        }

        public void SetPDFWriter(PdfWriter writer)
        {
            pdfWriter = writer;
        }

        internal void CreateHeading()
        {
            Paragraph heading = new Paragraph("AIR IGM CHECKLIST", pdfFonts.titleFont)
            {
                Alignment = 1
            };
            document.Add(heading);
            document.Add(new Paragraph("\n", pdfFonts.checkListFont));
            document.Add(new Paragraph("FLIGHT Details: " + model.sFlightNo , pdfFonts.subTitleFont)
            {
                Alignment = 0
            });
            //document.Add(new Paragraph("\nMAWB Details:", pdfFonts.subTitleFont)
            //{
            //    Alignment = 0
            //});
            document.Add(new Chunk(""));
        }

        internal void CreateMAWBPart()
        {
            PdfPTable tbl = new PdfPTable(8)
            {
                WidthPercentage = 100,
            };
            int[] firstTablecellwidth = {11,12,14, 10, 14, 14, 13, 12 };
            tbl.SetWidths(firstTablecellwidth);
            tbl.AddCell(new Paragraph("CUSTOM LOCATION", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("PORT OF ORIGIN", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("PORT OF DESTINATION", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("FLIGHT NO", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("DEPARTURE DATE", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("ARRIVAL DATE", pdfFonts.checkListTableTitleFont));
            tbl.AddCell(new Paragraph("FLIGHT REG NO", pdfFonts.checkListTableTitleFont)); 
                tbl.AddCell(new Paragraph("TIME", pdfFonts.checkListTableTitleFont)); 

            tbl.AddCell(new Paragraph(model.sCustomLocation, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sPortOfOrigin, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sPortOfDestination, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sFlightNo, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sDepatureDate, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sArrivalDate, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sFlightRegNo, pdfFonts.checkListFont));
            tbl.AddCell(new Paragraph(model.sTime, pdfFonts.checkListFont));
            document.Add(tbl);

            document.Add(new Paragraph("\nMAWB Details:", pdfFonts.subTitleFont)
            {
                Alignment = 0
            });
            document.Add(new Chunk(""));

            PdfPTable tbl2 = new PdfPTable(6)
            {
                WidthPercentage = 100,
                PaddingTop = 5
            };
            int[] secondTablecellwidth = {  15, 15, 15, 15, 15, 25 };
            tbl2.SetWidths(secondTablecellwidth);
           // tbl2.AddCell(new Paragraph("S. NO", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("MAWB NO", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PORT OF ORIGIN", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PORT OF DESTINATION", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("PACKAGES", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("WEIGHT", pdfFonts.checkListTableTitleFont));
            tbl2.AddCell(new Paragraph("Description", pdfFonts.checkListTableTitleFont));
            foreach (var item in model.lstMAWBData)
            {
                tbl2.AddCell(new Paragraph(item.sMAWBNo, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sPortOfOrigin, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sPortOfDestination, pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(String.Format("{0:0.##}", item.sPackages), pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(String.Format("{0:0.##}", item.sWeight), pdfFonts.checkListFont));
                tbl2.AddCell(new Paragraph(item.sDescription, pdfFonts.checkListFont));
            }
            document.Add(tbl2);

            Paragraph p = new Paragraph("\n")
            {
                IndentationLeft =200
            };

            PdfPTable tbl3 = new PdfPTable(3)
            {
                WidthPercentage = 70
            };
            int[] thirdtableWidths = {20, 16, 16 };
            tbl3.SetWidths(thirdtableWidths);
            tbl3.AddCell(new Paragraph("TOTAL", pdfFonts.checkListTableTitleFont));
            tbl3.AddCell(new Paragraph(String.Format("{0:0.##}", model.lstMAWBData.Sum(s => s.sPackages)), pdfFonts.checkListTableTitleFont));
            tbl3.AddCell(new Paragraph(String.Format("{0:0.##}", model.lstMAWBData.Sum(s => s.sWeight)), pdfFonts.checkListTableTitleFont));
            tbl3.HorizontalAlignment = Element.ALIGN_LEFT;
            p.Add(tbl3);
            document.Add(p);

        }
    }
}
