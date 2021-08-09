using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ReportViewerForMvc;
using PassportTracker.Models;
using System.Web.UI;
using System.Data;
using System.IO;
using System.Drawing.Printing;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
//using System.Security.Principal;

namespace PassportTracker.Reports
{

    public class PassportReport
    {

        private PassportTrackerDB db = new PassportTrackerDB();
        private int m_currentPageIndex;
        private IList<System.IO.Stream> m_streams;
        public void getDefendantReceipt(string rec, int? id) //(int? id)
        {
            
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);

            LocalReport report = new LocalReport();
            report.ReportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + @"Reports\" + rec;
            report.ReportEmbeddedResource = rec;
            IEnumerable<DefendantReceipt> data = db.tblPassportForm.Where(r => r.PF_Id == id)
             .Join(db.tblPassportOffice, a => a.PF_Country, b => b.PO_Location_Country, (a, b) => new { PF = a, PO = b })
             .Join(db.tblLawCourts, c => c.PF.PF_Current_Location, o => o.LC_Name, (c, o) => new DefendantReceipt
             {
                 DR_Id = c.PF.PF_Id,
                 DR_Created = c.PF.PF_Created, //User.Identity.Name, 
                 DR_Location_Country = c.PO.PO_Location_Country, //c.PF.PF_Initial_Location,
                 DR_Location_Name = c.PO.PO_Location_Name,
                 DR_Location_Street = c.PO.PO_Location_Street,
                 DR_Location_Suburb = c.PO.PO_Location_Suburb,
                 DR_Location_State = c.PO.PO_Location_State,
                 DR_Location_PostCode = c.PO.PO_Location_PostCode,
                 DR_Name = o.LC_Name,
                 DR_Phone = o.LC_Phone,
                 DR_Street = o.LC_Street,
                 DR_Suburb = o.LC_Suburb,
                 DR_State = o.LC_State,
                 DR_DX = o.LC_DX,
                 DR_PostCode = o.LC_PostCode,
                 DR_Fax = o.LC_Fax,
                 DR_Case_Id = c.PF.PF_Case_Id,
                 DR_Date_Of_Birth = c.PF.PF_Date_Of_Birth,
                 DR_Place_Of_Birth = c.PF.PF_Place_Of_Birth,
                 DR_Name_Def = c.PF.PF_Name,
                 DR_Passport_Expiry_Date = c.PF.PF_Passport_Expiry_Date,
                 DR_Passport_Number = c.PF.PF_Passport_Number,
                 DR_Next_Hearing_Date = c.PF.PF_Next_Hearing_Date,
                 DR_Jurisdiction = o.LC_Jurisdiction
             }).ToList();

           

            ReportParameter[] RptParameters = new ReportParameter[23];//declare the number of parameters 
            RptParameters[0] = new ReportParameter("DR_Created", data.ElementAt(0).DR_Created); // + " acknowledge receipt of the following passport:");
            RptParameters[1] = new ReportParameter("DR_Location_Country", data.ElementAt(0).DR_Location_Country);
            RptParameters[2] = new ReportParameter("DR_Name", data.ElementAt(0).DR_Name);
            RptParameters[3] = new ReportParameter("DR_Phone", data.ElementAt(0).DR_Phone);
            RptParameters[4] = new ReportParameter("DR_Street", data.ElementAt(0).DR_Street);
            RptParameters[5] = new ReportParameter("DR_Suburb", data.ElementAt(0).DR_Suburb);
            RptParameters[6] = new ReportParameter("DR_StatePostcode", data.ElementAt(0).DR_Suburb + ", " + data.ElementAt(0).DR_State + ", " + data.ElementAt(0).DR_PostCode);
            RptParameters[7] = new ReportParameter("DR_DX", data.ElementAt(0).DR_DX);
            RptParameters[8] = new ReportParameter("DR_PostCode", data.ElementAt(0).DR_PostCode.ToString());
            RptParameters[9] = new ReportParameter("DR_Fax", data.ElementAt(0).DR_Fax);
            RptParameters[10] = new ReportParameter("DR_Case_Id", data.ElementAt(0).DR_Case_Id);
            RptParameters[11] = new ReportParameter("DR_Date_Of_Birth", data.ElementAt(0).DR_Date_Of_Birth.ToShortDateString());
            RptParameters[12] = new ReportParameter("DR_Name_Def", data.ElementAt(0).DR_Name_Def);
            RptParameters[13] = new ReportParameter("DR_Passport_Expiry_Date", data.ElementAt(0).DR_Passport_Expiry_Date.ToShortDateString());
            RptParameters[14] = new ReportParameter("DR_Passport_Number", data.ElementAt(0).DR_Passport_Number);
            RptParameters[15] = new ReportParameter("DR_Next_Hearing_Date", data.ElementAt(0).DR_Next_Hearing_Date.ToShortDateString());
            RptParameters[16] = new ReportParameter("DR_Location_Name", data.ElementAt(0).DR_Location_Name);
            RptParameters[17] = new ReportParameter("DR_Location_Street", data.ElementAt(0).DR_Location_Street);
            RptParameters[18] = new ReportParameter("DR_Location_Suburb", data.ElementAt(0).DR_Location_Suburb);
            RptParameters[19] = new ReportParameter("DR_Location_State", data.ElementAt(0).DR_Location_State);
            RptParameters[20] = new ReportParameter("DR_Location_PostCode", data.ElementAt(0).DR_Location_PostCode.ToString());
            RptParameters[21] = new ReportParameter("DR_Place_Of_Birth", data.ElementAt(0).DR_Place_Of_Birth);
            if (data.ElementAt(0).DR_Jurisdiction == "County Court")
                RptParameters[22] = new ReportParameter("ReportImageParam", HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + @"Reports\Images\CountyCourt.PNG");
            else
                if (data.ElementAt(0).DR_Jurisdiction == "Childrens Court")
                    RptParameters[22] = new ReportParameter("ReportImageParam", HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + @"Reports\Images\ChildrensCourtVic.PNG");
                else
                    if (data.ElementAt(0).DR_Jurisdiction == "Magistrates Court")
                        RptParameters[22] = new ReportParameter("ReportImageParam", HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + @"Reports\Images\MelbMagisCourt.PNG");
                    else
                        if (data.ElementAt(0).DR_Jurisdiction == "Supreme Court")
                            RptParameters[22] = new ReportParameter("ReportImageParam", HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + @"Reports\Images\Supreme.PNG");
          //  db.Dispose();
            report.EnableExternalImages = true;
            report.SetParameters(RptParameters);         
            report.DataSources.Add(
               new ReportDataSource("Sales", data));
            string deviceInfo =
             @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            var aa = report.GetParameters();

            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;


            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
                Dispose();
            }
        }


        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                {
                    stream.Close();
                }
                m_streams = null;
            }
        }
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
              Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

     
    }
    
}

