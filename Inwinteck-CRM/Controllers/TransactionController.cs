using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inwinteck_CRM.Models;


namespace Inwinteck_CRM.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Transaction
        string url = "https://fms.inwinteck.com/Upload/";
        string mailer = "Support<support@inwinteck.com>";

       

        //string url = "http://inwinteckcrm.3sptechmind.com/Upload/";
        public ActionResult Ticket(int pageNo = 0, string searchtype = "", string searchtext = "")
        {
            // To check user access right
            string uri1 = "/Transaction/Ticket";

            int bid = utlity.GetBussinessId(uri1);
            string ucheckid = User.Identity.GetUserId();

            checkpermission cm = new checkpermission();

            cm = db.Database.SqlQuery<checkpermission>("Access @id,@bid", new SqlParameter("@id", ucheckid), new SqlParameter("@bid", bid)).FirstOrDefault();
            if (cm == null)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            ViewBag.Permission = cm;
            // End user access right

            int pageSize = 10;
            int pgc = 0;
            int pageCount = 0;
            List<TicketDetails> li = new List<TicketDetails>();
            if (searchtype != "" && searchtext != "")
            {
                if (searchtype == "EU")
                {
                    
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.EU_Name.Contains(searchtext)).OrderByDescending(x => x.Ticket_No).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Ticket")
                {
                   
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Ticket_No.Contains(searchtext)).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Status")
                {
                
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext)).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Status-YTD")
                {
                    string year = DateTime.Now.Year.ToString();
                   
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(year)).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Status-MTD")
                {
                    string date = DateTime.Now.ToString("MMM yyyy");
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(date)).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Status-TD")
                {
                    string today = DateTime.Now.ToString("dd MMM yyyy");
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(today)).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                ViewBag.Search = searchtext;
            }
            else
            {
                if (searchtype == "Status-YTD")
                {
                    string year = DateTime.Now.Year.ToString();
            
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(year)).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = "Status";
                }
                else if (searchtype == "Status-MTD")
                {

                    string date = DateTime.Now.ToString("MMM yyyy");
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(date)).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = "Status";
                }
                else if (searchtype == "Status-TD")
                {

                    string today = DateTime.Now.ToString("dd MMM yyyy");
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(today)).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = "Status";
                }
                else
                {
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").ToList();

                }

               
            }
            return View(li);
        }
        public ActionResult CreateTicket()
        {
           
            // To check user access right
            string uri1 = "/Transaction/Ticket";

            int bid = utlity.GetBussinessId(uri1);
            string ucheckid = User.Identity.GetUserId();

            checkpermission cm = new checkpermission();

            cm = db.Database.SqlQuery<checkpermission>("Access @id,@bid", new SqlParameter("@id", ucheckid), new SqlParameter("@bid", bid)).FirstOrDefault();
            if (cm == null)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            if (cm.record_insert == false)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            // access right ends 
            ViewBag.Country = (from c in db.Country where c.Status == 1 select new SelectListItem { Text = c.CountryName, Value = c.CountryName.Trim() }).ToList();
            ViewBag.Customer = (from s in db.EU_Master where s.Status == 1 select new SelectListItem { Text = s.Customer_Name, Value = SqlFunctions.StringConvert((double)s.Id).TrimStart() }).ToList();
            ViewBag.OEM = (from c in db.HeaderDescription where c.header_id == 4 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.System_Info = (from c in db.HeaderDescription where c.header_id == 19 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Type = (from c in db.HeaderDescription where c.header_id == 11 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Mode = (from c in db.HeaderDescription where c.header_id == 13 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.SLA = (from c in db.HeaderDescription where c.header_id == 14 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Priority = (from c in db.HeaderDescription where c.header_id == 15 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Certification = (from c in db.Certification_Master where c.Status == 1 select new SelectListItem { Text = c.Certification_Name, Value = SqlFunctions.StringConvert((double)c.Id).Trim() }).ToList();

            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTicket(Ticket sa, string Comments,string TSE_Name, string[] email_office, string Email_Subject, List<Ticket_System_Info> IV, List<Ticket_EU_Detail> EUL, string[] Certification_Name)
        {

            try
            {
                string email = "";



                foreach (var i in email_office)
                {
                    email = email + i + ",";
                }


                email = email.Remove(email.Length - 1);

                if (email == "")
                {
                    TempData["message"] = "Kindly add Email ID !! ";
                    return RedirectToAction("CreateTicket", "Transaction");
                }
                sa.Ticket_No = utlity.CheckTicketNo(sa.EU_ID);
                sa.Status = 18;
                sa.CreatedBy = User.Identity.GetUserId();
                sa.CreatedOn = DateTime.Now;
                sa.TSE_Name = TSE_Name;
                if (Certification_Name != null)
                { 
                sa.Certification_Name = string.Join(", ", Certification_Name);
                }
                else { sa.Certification_Name = ""; }
                db.Ticket.Add(sa);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    Ticket_History th = new Ticket_History();
                    th.Ticket_no = sa.Id;
                    th.Comments = Comments;
                    th.WhatsappChat = sa.WhatsappChat;
                    th.FE_ID = sa.FE_ID;
                    th.status = 18;
                    th.CreatedBy = User.Identity.GetUserId();
                    th.CreatedOn = DateTime.Now;
                    db.Ticket_History.Add(th);
                    db.SaveChanges();

                    Ticket_Email TE = new Ticket_Email();
                    TE.Ticket_no = sa.Id;
                    TE.Email = email;
                    if (Email_Subject != "")
                    {
                        TE.Email_Subject = Email_Subject;

                    }
                    else
                    {
                        TE.Email_Subject = "Ticket Generated";
                        Email_Subject = "Ticket Generated";
                    }

                    TE.CreatedBy = User.Identity.GetUserId();
                    TE.CreatedOn = DateTime.Now;
                    db.Ticket_Email.Add(TE);
                    db.SaveChanges();

                    if (IV[0].System_Information != null)
                    {
                        Ticket_System_Info TSI = new Ticket_System_Info();

                        //Loop and insert records.
                        foreach (Ticket_System_Info iv in IV)
                        {
                            TSI.Ticket_no = sa.Id;
                            TSI.System_Information = iv.System_Information;
                            TSI.Make_Model = iv.Make_Model;
                            TSI.Serial_Number = iv.Serial_Number;
                            TSI.Required_Tool = iv.Required_Tool;
                            TSI.CreatedBy = User.Identity.GetUserId();
                            TSI.CreatedOn = DateTime.Now;
                            db.Ticket_System_Info.Add(TSI);
                            db.SaveChanges();
                        }
                    }

                    if (EUL[0].EU_Name != null)
                    {
                        Ticket_EU_Detail TSEU = new Ticket_EU_Detail();

                        //Loop and insert records.
                        foreach (Ticket_EU_Detail eul in EUL)
                        {
                            TSEU.Ticket_no = sa.Id;
                            TSEU.EU_Name = eul.EU_Name;
                            TSEU.EU_Email = eul.EU_Email;
                            TSEU.EU_Contact = eul.EU_Contact;
                            TSEU.CreatedBy = User.Identity.GetUserId();
                            TSEU.CreatedOn = DateTime.Now;
                            db.Ticket_EU_Detail.Add(TSEU);
                            db.SaveChanges();
                        }
                    }
                }

                string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
               string body = utlity.TicketGenerated(eu, sa.Ticket_No, sa.Case_No, sa.Site_Name, sa.Street_Address, sa.Dispatch_Date.ToString(),sa.Job_Description);
               utlity.sendmail(email, Email_Subject, body, mailer);
                TempData["message"] = "Ticket Created : <br>  Number :" + sa.Ticket_No;

            }
            catch (Exception ex)
            {
                TempData["message"] = "Ticket Not Created !! ";
            }
            return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });
        }
        public ActionResult editTicket(int id)
        {
            // To check user access right
            string uri1 = "/Transaction/Ticket";

            int bid = utlity.GetBussinessId(uri1);
            string ucheckid = User.Identity.GetUserId();

            checkpermission cm = new checkpermission();

            cm = db.Database.SqlQuery<checkpermission>("Access @id,@bid", new SqlParameter("@id", ucheckid), new SqlParameter("@bid", bid)).FirstOrDefault();
            if (cm == null)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            if (cm.record_update == false)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            // access right ends 
            Ticket ti = new Ticket();
            ti = (from s in db.Ticket where s.Id == id select s).FirstOrDefault(); 
            ViewBag.Country = (from c in db.Country where c.Status == 1 select new SelectListItem { Text = c.CountryName, Value = c.CountryName.Trim() }).ToList();
            ViewBag.Customer = (from s in db.EU_Master where s.Id == ti.EU_ID select s.Customer_Name).FirstOrDefault();
            ViewBag.OEM = (from c in db.HeaderDescription where c.header_id == 4 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Status = (from c in db.HeaderDescription where c.header_id == 5 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Part_Management = (from c in db.HeaderDescription where c.header_id == 6 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Part_Handover = (from c in db.HeaderDescription where c.header_id == 7 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();


            ViewBag.Ticket_Type = (from c in db.HeaderDescription where c.header_id == 11 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Mode = (from c in db.HeaderDescription where c.header_id == 13 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.SLA = (from c in db.HeaderDescription where c.header_id == 14 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Priority = (from c in db.HeaderDescription where c.header_id == 15 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Certification = (from c in db.Certification_Master where c.Status == 1 select new SelectListItem { Text = c.Certification_Name, Value = SqlFunctions.StringConvert((double)c.Id).Trim() }).ToList();


            ViewBag.Cust_Business = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Business_Hr).FirstOrDefault();
            ViewBag.Cust_Business_Non = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Non_Business_Hr).FirstOrDefault();
            ViewBag.Cust_Minimum = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Minimum_Hrs).FirstOrDefault();
            ViewBag.Cust_Charges_job = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Per_Job).FirstOrDefault();

            ViewBag.FE = (from s in db.FE_Master_Personal where s.Id == ti.FE_ID select s.First_Name).FirstOrDefault();

            ViewBag.Decline_Reason = (from c in db.HeaderDescription where c.header_id == 16 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Cancel_Reason = (from c in db.HeaderDescription where c.header_id == 17 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket = "INWIN000" + ti.Id;
            ViewBag.Currency = (from s in db.Currency_Master where s.Country == ti.Country select s.Currency).FirstOrDefault();

            ViewBag.EU_Office = (from s in db.EU_Master_Branch where s.Id == ti.EU_Office select s.Office).FirstOrDefault();
            ViewBag.Email_Subject = (from s in db.Ticket_Email where s.Ticket_no == ti.Id select s.Email_Subject).FirstOrDefault();
            ViewBag.email_office = (from s in db.Ticket_Email where s.Ticket_no == ti.Id select s.Email).FirstOrDefault();

            ViewBag.Old_Ticket = (from s in db.Ticket where s.Id == ti.Old_Ticket select s.Ticket_No).FirstOrDefault();

            ViewBag.System_info = (from s in db.Ticket_System_Info where s.Ticket_no == ti.Id select s).ToList();

            return View(ti);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult editTicket(Ticket sa, HttpPostedFileBase pht, string[] email_office, string Comments,string WhatsappChat, string Quality_Remark,List<Part_Ticket_Data> IV, string[] Certification_Name)
        {
            string email = "";



            foreach (var i in email_office)
            {
                email = email + i + ",";
            }


            email = email.Remove(email.Length - 1);

            Ticket_Email TEE = new Ticket_Email();
            TEE = (from s in db.Ticket_Email where s.Ticket_no == sa.Id select s).FirstOrDefault();
          
            if (email == TEE.Email)
            {

            }
            else
            {
                TEE.Email = email;
                db.Entry(TEE).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

           

            var email_auto = (from s in db.Ticket_Email where s.Ticket_no == sa.Id select s).FirstOrDefault();
            var fe = (from s in db.FE_Master_Personal where s.Id == sa.FE_ID select s).FirstOrDefault();
            try
            {
                if (sa.Status == 20)
                {
                    if (sa.In_Time == null)
                    {
                        TempData["message"] = "Do enter In Time before Closing the ticket !!";
                        return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });
                    }
                    else if (sa.Out_Time == null)
                    {
                        TempData["message"] = "Do enter Out Time before Closing the ticket !!";
                        return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });
                    }
                    else if (sa.In_Time > sa.Out_Time)
                    {
                        TempData["message"] = "Do Check Out Time is past In Time!!";
                        return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });

                    }

                }
                else if (sa.Status == 1362)
                {
                    sa.Is_Decline = 1;
                }
                else if (sa.Status == 19)
                {
                    if (sa.In_Time == null)
                    {
                        sa.Is_Reschedule = 1;
                    }
                    else
                    {
                        TempData["message"] = "Ticket is already checked in! Can't be Rescheduled !!";
                        return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });
                    }
                    
                }

                string pic = "";
                string pathtest = "";
                if (pht != null)
                {

                    pic = System.IO.Path.GetFileName(pht.FileName);
                    string fileName = sa.Id + DateTime.Now.ToString("yyyyMMddHHmmssFFF") + "_" + pic;
                    pathtest = System.IO.Path.Combine(Server.MapPath("~/Upload/Pregame"), fileName);
                    pht.SaveAs(pathtest);

                    sa.pregame_upload = url + "Pregame/" + fileName;
                }
                if (Certification_Name != null)
                {
                    sa.Certification_Name = string.Join(", ", Certification_Name);
                }
                else { sa.Certification_Name = ""; }
                //sa.Certification_Name = string.Join(", ", Certification_Name);
                sa.ModifiedBy = User.Identity.GetUserId();
                sa.ModifiedOn = DateTime.Now;
                db.Entry(sa).State = System.Data.Entity.EntityState.Modified;
                int res = db.SaveChanges();
                if (res > 0)
                {
                    Ticket_History th = new Ticket_History();
                    th.Ticket_no = sa.Id;
                    th.Comments = Comments;
                    th.WhatsappChat = WhatsappChat;
                    th.Quality_Remark = Quality_Remark;
                    th.FE_ID = sa.FE_ID;
                    th.status = sa.Status;
                    th.CreatedBy = User.Identity.GetUserId();
                    th.CreatedOn = DateTime.Now;
                    db.Ticket_History.Add(th);
                    db.SaveChanges();


                }

                if (IV[0].Serial_No != null)
                {
                    Part_Ticket_Data TSI = new Part_Ticket_Data();

                    //Loop and insert records.
                    foreach (Part_Ticket_Data iv in IV)
                    {
                        TSI.Ticket_No = sa.Id;
                        TSI.Make_Model = iv.Make_Model;
                        TSI.Part_Description = iv.Part_Description;
                        TSI.Part_type = iv.Part_type;
                        TSI.Serial_No = iv.Serial_No;
                        TSI.CreatedBy = User.Identity.GetUserId();
                        TSI.CreatedOn = DateTime.Now;
                        db.Part_Ticket_Data.Add(TSI);
                        db.SaveChanges();
                    }
                }


                TempData["message"] = "Ticket Updated !!";

                if (sa.Status == 20)
                {
                    DateTime start = sa.In_Time.Value;
                    DateTime end = sa.Out_Time.Value;

                    int count = 0;
                    int countnon = 0;

                    Decimal? FETotal;
                    Decimal? CTTotal;


                    for (var i = start; i < end; i = i.AddMinutes(15))
                    {
                        if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                        {
                            if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours < 17)
                            {
                                count++;
                            }
                            else
                            {
                                countnon++;
                            }

                        }
                        else
                        {
                            countnon++;
                        }
                    }


                    TimeSpan bc = TimeSpan.FromMinutes(15 * count);
                    TimeSpan bnc = TimeSpan.FromMinutes(15 * countnon);

                    if (sa.FE_Payment_Mode == "H")
                    {


                        var FE_NB = ((sa.FE_Non_Buss_Hrs / 4) * countnon);
                        var FE_B = ((sa.FE_Buss_Hrs / 4) * count);
                        FETotal = FE_NB + FE_B + sa.FE_Allowance + sa.Other_Amt;


                    }
                    else if (sa.FE_Payment_Mode == "F")
                    {
                        FETotal = sa.FE_Allowance + sa.FE_Fixed + sa.Other_Amt;
                    }
                    else
                    {
                        FETotal = 0;
                    }




                    if (sa.CT_Payment_Mode == "H")
                    {


                        var CT_NB = ((sa.CT_Non_Buss_Hrs / 4) * countnon);
                        var CT_B = ((sa.CT_Buss_Hrs / 4) * count);
                        CTTotal = CT_NB + CT_B + sa.CT_Allowance + sa.Other_Amt;


                    }
                    else if (sa.CT_Payment_Mode == "F")
                    {
                        CTTotal = sa.CT_Allowance + sa.CT_Fixed + sa.Other_Amt;
                    }
                    else
                    {
                        CTTotal = 0;
                    }


                    TimeSpan TRT = sa.Out_Time.Value - sa.In_Time.Value;
                    

                    string sys_info = "  <table style='width: 100 %; height: 40px;padding-left:20px; border:1px;'><tr style='background: rgb(131,194, 51);'><td>System Information</td><td>Make/Model<td>Serial Number</td><td>Required Tool</td></tr>";
                    List<Ticket_System_Info> tsi = new List<Ticket_System_Info>();
                    tsi = (from s in db.Ticket_System_Info where s.Ticket_no == sa.Id select s).ToList();
                    for (int i = 0; i < tsi.Count; i++)
                    {
                        sys_info = sys_info + "<tr><td>" + tsi[i].System_Information + "</td><td>" + tsi[i].Make_Model + "</td><td>" + tsi[i].Serial_Number + "</td><td>" + tsi[i].Required_Tool + "</td></tr>";
                    }
                    sys_info = sys_info + "</table>";

                    TempData["Payment"] = "Number of Business Hour Served: " + count + " and Non Business Hour : " + countnon;

                    TempData["message"] = "Ticket No: " + sa.Ticket_No + " Closed and Email sent to engineer and to customer";

                    string RRT;
                    if (sa.Other_Charge == 1)
                    {
                        RRT = "Yes";
                    }
                    else
                    {
                        RRT = "No";
                    }

                    string Part_Detail = "";
                    if (sa.Part_Management == 22)
                    {
                        if (sa.Return_Label == 1)
                        {
                            if (sa.FE_Part_Storage_Charge == 1)
                            {
                                Part_Detail = "Part Management :Pick up , Return Lable Available . Tracking Number : " + sa.Tracking_number + " Part Storage Charges is applicable. ";

                            }
                            else
                            {
                                Part_Detail = "Part Management :Pick up , Return Lable Available . Tracking Number : " + sa.Tracking_number + " Part Storage Charges is not applicable. ";
                            }

                        }
                        else
                        {
                            if (sa.FE_Part_Storage_Charge == 1)
                            {
                                Part_Detail = "Part Management :Pick up , Return Lable Not Available . Part Storage Charges is applicable. ";

                            }
                            else
                            {
                                Part_Detail = "Part Management :Pick up , Return Lable Not Available . Part Storage Charges is not applicable. ";
                            }
                        }

                    }
                    else if (sa.Part_Management == 23)
                    {
                        Part_Detail = "Part Management : Handover , Part Handover " + (from s in db.HeaderDescription where s.id == sa.Part_Handover select s.header_description).FirstOrDefault() + " .Contact Name : " + sa.ph_Name + " Contact Number : " + sa.ph_contact; 
                    }
                    else if (sa.Part_Management == 1371)
                    {
                        Part_Detail = "Part Management : Not Applicable ";
                    }
                    var callbackUrl = Url.Action("CSAT", "Transaction", new { Id = sa.Id }, protocol: Request.Url.Scheme);
                    string oem = (from s in db.HeaderDescription where s.id == sa.OEM select s.header_description).FirstOrDefault();
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string body = utlity.CustomterClosing(eu, sa.In_Time.Value.ToString("dd/MM/yyyy"), sa.In_Time.Value.ToString("HH:mm:ss"), sa.Out_Time.Value.ToString("dd/MM/yyyy"), sa.Out_Time.Value.ToString("HH:mm:ss"), "", "", Comments, callbackUrl,sa.Case_No,sa.Ticket_No,
                        oem,sys_info, fe.First_Name + " " + fe.Last_Name, TRT.ToString(),RRT, Part_Detail);
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);

                    string invaddress;
                    if (fe.Country == "India")
                    {
                        invaddress = "Inwinteck Private Limited, 10/34, Vijay Garden, Off G.B Road, Thane (W) -400615";
                    }
                    else
                    {
                        invaddress = "Inwinteck Pte Ltd, 23 Kelantan Lane,#04-01 Kim Hoe Centre, Singapore – 208642.";
                    }

                    string bodyde = utlity.FEClosing(fe.First_Name + "" + fe.Last_Name, sa.Ticket_No,oem,sys_info,TRT.ToString(),RRT,Part_Detail, sa.In_Time.Value.ToString("dd/MM/yyyy"), sa.In_Time.Value.ToString("HH:mm:ss"), sa.Out_Time.Value.ToString("dd/MM/yyyy"),
                        sa.Out_Time.Value.ToString("HH:mm:ss"),Comments,invaddress);
                    utlity.sendmail(fe.Email, "Inwinteck Ticket no " + sa.Ticket_No + " is Closed", bodyde, mailer);


                    return RedirectToAction("viewTicket", "Transaction", new { id = sa.Id });
                }
                else if (sa.Status == 21)
                {
                    TempData["message"] = "Ticket No: " + sa.Ticket_No + " Cancel";
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string Cancel_Reason = (from c in db.HeaderDescription where c.id == sa.Cancel_Reason select c.header_description).FirstOrDefault();
                    string body = utlity.CustomerCancel(eu, sa.Ticket_No , sa.Case_No , sa.Site_Name,sa.Street_Address,sa.Job_Description,sa.Dispatch_Date.Value.ToString("dd/MM/yyyy"),sa.Dispatch_Date.Value.ToString("HH:mm:ss"), Cancel_Reason);
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);

                    if (sa.FE_ID != 0)
                    {
                        string body1 = utlity.FECancel(fe.First_Name + " " + fe.Last_Name);
                        utlity.sendmail(fe.Email, "Ticket No : " + sa.Ticket_No + " has been Canceled", body1, mailer);

                    }
                    return RedirectToAction("Ticket", "Transaction");
                }
                else if (sa.Status == 1362)
                {
                    TempData["message"] = "Ticket No: " + sa.Ticket_No + " Declined";
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string body = utlity.CustomerDecline(eu);
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);

                    string body1 = utlity.FEDecline(fe.First_Name + " " + fe.Last_Name);
                    utlity.sendmail(fe.Email, "Ticket No : " + sa.Ticket_No + " has been Declined", body1, mailer);


                    return RedirectToAction("Ticket", "Transaction");
                }
                else if (sa.Status == 1414)
                {
                    TempData["message"] = "Engineer details sent to customer";
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string body = utlity.CustomerSchedule(eu, sa.Case_No, fe.First_Name + " " + fe.Last_Name, fe.Email, fe.Phone_Number_Code + "-" + fe.Phone_Number);
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);
                    return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });
                }
                else if (sa.Status == 1415)
                {
                    TempData["message"] = "FE Arrived on site and Email send to customer";
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string body = utlity.FECheckedIn(eu, sa.Case_No, fe.First_Name + " " + fe.Last_Name, fe.Email, fe.Phone_Number_Code + "-" + fe.Phone_Number, sa.In_Time.Value.ToString("dd/MM/yyyy"), sa.In_Time.Value.ToString("HH:mm:ss"));
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);
                    return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });
                }
                else if (sa.Status == 1372)
                {

                    TempData["message"] = "Ticket has been Scheduled.FE has been informed by email.";
                    string sys_info = "  <table style='width: 100 %; height: 40px;padding-left:20px; border:1px;'><tr style='background: rgb(131,194, 51);'><td>System Information</td><td>Make/Model<td>Serial Number</td><td>Required Tool</td></tr>";
                    List<Ticket_System_Info> tsi = new List<Ticket_System_Info>();
                    tsi = (from s in db.Ticket_System_Info where s.Ticket_no == sa.Id select s).ToList();
                    for (int i = 0; i < tsi.Count; i++)
                    {
                        sys_info = sys_info + "<tr><td>" + tsi[i].System_Information + "</td><td>" + tsi[i].Make_Model + "</td><td>" + tsi[i].Serial_Number + "</td><td>" + tsi[i].Required_Tool + "</td></tr>";
                    }

                    sys_info = sys_info + "</table>";

                    string oem = (from s in db.HeaderDescription where s.id == sa.OEM select s.header_description).FirstOrDefault();
                    if (sa.pregame_upload != null)
                    {
                        string body = utlity.JobConfirm(fe.First_Name + " " + fe.Last_Name, sa.Site_Name, sa.EU_Name, sa.Street_Address, sa.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), sa.Job_Description, sa.pregame_detail, sys_info, oem);
                        utlity.sendmailattach(fe.Email, "Job Confirmation and additional details for Inwinteck Ticket no :" + sa.Ticket_No, body, mailer, pathtest);

                      
                    }
                    else
                    {
                        string body = utlity.JobConfirm(fe.First_Name + " " + fe.Last_Name, sa.Site_Name, sa.EU_Name, sa.Street_Address, sa.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), sa.Job_Description, sa.pregame_detail, sys_info, oem);
                        utlity.sendmail(fe.Email, "Job Confirmation and additional details for Inwinteck Ticket no :" + sa.Ticket_No, body, mailer);
                    }

                   


                    return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });

                }
                else if (sa.Status == 19) 
                {

                    Ticket rti = new Ticket();
                    Ticket_Email rte = new Ticket_Email();
                   List<Ticket_System_Info> reTSI = new List<Ticket_System_Info>();

                    rti = (from s in db.Ticket where s.Id == sa.Id select s).FirstOrDefault();
                    rte = (from s in db.Ticket_Email where s.Ticket_no == sa.Id select s).FirstOrDefault();
                    reTSI = (from s in db.Ticket_System_Info where s.Ticket_no == sa.Id select s).ToList();
                    try
                    {
                        rti.Dispatch_Date = sa.Reschedule_DT;
                        rti.FE_ID = 0;
                        rti.Ticket_No = utlity.CheckTicketNo(rti.EU_ID);
                        rti.Status = 18;
                        rti.Pregame = null;
                        rti.Is_Reschedule = 0;
                        rti.Old_Ticket = sa.Id;
                        rti.CreatedBy = User.Identity.GetUserId();
                        rti.CreatedOn = DateTime.Now;
                        db.Ticket.Add(sa);
                        int reres = db.SaveChanges();
                        if (reres > 0)
                        {
                            Ticket_History th = new Ticket_History();
                            th.Ticket_no = rti.Id;
                            th.Comments = Comments;
                            th.WhatsappChat = sa.WhatsappChat;
                            th.FE_ID = rti.FE_ID;
                            th.status = 18;
                            th.CreatedBy = User.Identity.GetUserId();
                            th.CreatedOn = DateTime.Now;
                            db.Ticket_History.Add(th);
                            db.SaveChanges();

                            Ticket_Email TE = new Ticket_Email();
                            TE.Ticket_no = rti.Id;
                            TE.Email = rte.Email;                         
                            TE.Email_Subject = email_auto.Email_Subject;
                            string resEmail_Subject = email_auto.Email_Subject;
                           

                            TE.CreatedBy = User.Identity.GetUserId();
                            TE.CreatedOn = DateTime.Now;
                            db.Ticket_Email.Add(TE);
                            db.SaveChanges();



                            if (reTSI[0].System_Information != null)
                            {
                                Ticket_System_Info TSI = new Ticket_System_Info();

                                //Loop and insert records.
                                foreach (Ticket_System_Info iv in reTSI)
                                {
                                    TSI.Ticket_no = rti.Id;
                                    TSI.System_Information = iv.System_Information;
                                    TSI.Make_Model = iv.Make_Model;
                                    TSI.Serial_Number = iv.Serial_Number;
                                    TSI.Required_Tool = iv.Required_Tool;
                                    TSI.CreatedBy = User.Identity.GetUserId();
                                    TSI.CreatedOn = DateTime.Now;
                                    db.Ticket_System_Info.Add(TSI);
                                    db.SaveChanges();
                                }
                            }

                            string old_ticket = (from s in db.Ticket where s.Id == rti.Old_Ticket select s.Ticket_No).FirstOrDefault();
                            string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                            string body = utlity.CustomerReschedule(eu, rti.Ticket_No, sa.Case_No, rti.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), old_ticket, sa.Reschedule_Reason);
                            utlity.sendmail(rte.Email, resEmail_Subject, body, mailer);

                            string body1 = utlity.FEReschedule(fe.First_Name + " " + fe.Last_Name, old_ticket, rti.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), sa.Reschedule_Reason);
                            utlity.sendmail(fe.Email, "Ticket Rescheduled for Inwinteck Ticket no :" + old_ticket, body1, mailer);

                            TempData["message"] = "Ticket has been Reschedule New Ticket has been created Ticket No " + rti.Ticket_No;
                        }

                        return RedirectToAction("editTicket", "Transaction", new { id = rti.Id });

                    }
                    catch (Exception ex)
                    {
                        string msg = "/Ticket/Not Rescheduled" + " " + "Ticket No : " + sa.Id + " " + "Error: " + ex.Message.ToString();
                        utlity.createlog(msg);
                        TempData["message"] = "Ticket Not Rescheduled !! ";
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "Ticket Not Updated !! ";
                string msg = "/Ticket/Error" + " " + "Ticket No : " + sa.Id + " " + "Error: " + ex.Message.ToString();
            }
            return RedirectToAction("editTicket", "Transaction", new { id = sa.Id });
        }
        public ActionResult viewTicket(int id)
        {
            // To check user access right
            string uri1 = "/Transaction/Ticket";

            int bid = utlity.GetBussinessId(uri1);
            string ucheckid = User.Identity.GetUserId();

            checkpermission cm = new checkpermission();

            cm = db.Database.SqlQuery<checkpermission>("Access @id,@bid", new SqlParameter("@id", ucheckid), new SqlParameter("@bid", bid)).FirstOrDefault();
            if (cm == null)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            if (cm.record_view == false)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            // access right ends 
            Ticket ti = new Ticket();
            ti = (from s in db.Ticket where s.Id == id select s).FirstOrDefault();
            ViewBag.Country = (from c in db.Country where c.Status == 1 select new SelectListItem { Text = c.CountryName, Value = c.CountryName.Trim() }).ToList();
            ViewBag.Customer = (from s in db.EU_Master where s.Id == ti.EU_ID select s.Customer_Name).FirstOrDefault();
            ViewBag.OEM = (from c in db.HeaderDescription where c.header_id == 4 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Status = (from c in db.HeaderDescription where c.header_id == 5 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Part_Management = (from c in db.HeaderDescription where c.header_id == 6 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Part_Handover = (from c in db.HeaderDescription where c.header_id == 7 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();


            ViewBag.Ticket_Type = (from c in db.HeaderDescription where c.header_id == 11 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Mode = (from c in db.HeaderDescription where c.header_id == 13 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.SLA = (from c in db.HeaderDescription where c.header_id == 14 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Priority = (from c in db.HeaderDescription where c.header_id == 15 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Certification = (from c in db.Certification_Master where c.Status == 1 select new SelectListItem { Text = c.Certification_Name, Value = SqlFunctions.StringConvert((double)c.Id).Trim() }).ToList();


            ViewBag.Cust_Business = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Business_Hr).FirstOrDefault();
            ViewBag.Cust_Business_Non = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Non_Business_Hr).FirstOrDefault();
            ViewBag.Cust_Minimum = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Minimum_Hrs).FirstOrDefault();
            ViewBag.Cust_Charges_job = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Per_Job).FirstOrDefault();

            ViewBag.FE = (from s in db.FE_Master_Personal where s.Id == ti.FE_ID select s.First_Name).FirstOrDefault();

            ViewBag.Decline_Reason = (from c in db.HeaderDescription where c.header_id == 16 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Cancel_Reason = (from c in db.HeaderDescription where c.header_id == 17 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket = "INWIN000" + ti.Id;
            ViewBag.Currency = (from s in db.Currency_Master where s.Country == ti.Country select s.Currency).FirstOrDefault();

            ViewBag.EU_Office = (from s in db.EU_Master_Branch where s.Id == ti.EU_Office select s.Office).FirstOrDefault();
            ViewBag.Email_Subject = (from s in db.Ticket_Email where s.Ticket_no == ti.Id select s.Email_Subject).FirstOrDefault();
            ViewBag.email_office = (from s in db.Ticket_Email where s.Ticket_no == ti.Id select s.Email).FirstOrDefault();

            ViewBag.Old_Ticket = (from s in db.Ticket where s.Id == ti.Old_Ticket select s.Ticket_No).FirstOrDefault();

            ViewBag.New_Ticket = (from s in db.Ticket where s.Old_Ticket == ti.Id select s.Ticket_No).FirstOrDefault();

            return View(ti);
        }
        public ActionResult Tickettest(int pageNo = 0, string searchtype = "", string searchtext = "")
        {
            // To check user access right
            string uri1 = "/Transaction/Tickettest";

            int bid = utlity.GetBussinessId(uri1);
            string ucheckid = User.Identity.GetUserId();
            string username = User.Identity.GetUserName();

            checkpermission cm = new checkpermission();

            cm = db.Database.SqlQuery<checkpermission>("Access @id,@bid", new SqlParameter("@id", ucheckid), new SqlParameter("@bid", bid)).FirstOrDefault();
            if (cm == null)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            ViewBag.Permission = cm;
            // End user access right

            int pageSize = 10;
            int pgc = 0;
            int pageCount = 0;
            List<TicketDetails> li = new List<TicketDetails>();
            if (searchtype != "" && searchtext != "")
            {
                if (searchtype == "EU")
                {

                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.EU_Name.Contains(searchtext) && x.Username == username).OrderByDescending(x => x.Ticket_No).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Ticket")
                {

                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Ticket_No.Contains(searchtext) && x.Username == username).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Status")
                {

                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) && x.Username == username).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Status-YTD")
                {
                    string year = DateTime.Now.Year.ToString();

                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(year) && x.Username == username).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Status-MTD")
                {
                    string date = DateTime.Now.ToString("MMM yyyy");
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(date) && x.Username == username).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                else if (searchtype == "Status-TD")
                {
                    string today = DateTime.Now.ToString("dd MMM yyyy");
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(today) && x.Username == username).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = searchtype;
                }
                ViewBag.Search = searchtext;
            }
            else
            {
                if (searchtype == "Status-YTD")
                {
                    string year = DateTime.Now.Year.ToString();

                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(year) && x.Username == username).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = "Status";
                }
                else if (searchtype == "Status-MTD")
                {

                    string date = DateTime.Now.ToString("MMM yyyy");
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(date) && x.Username == username).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = "Status";
                }
                else if (searchtype == "Status-TD")
                {

                    string today = DateTime.Now.ToString("dd MMM yyyy");
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x => x.Status.Contains(searchtext) &&
                    x.Ticket_Created.Contains(today) && x.Username == username).ToList();
                    ViewBag.searchtext = searchtext;
                    ViewBag.searchtype = "Status";
                }
                else
                {
                    li = db.Database.SqlQuery<TicketDetails>("getTicketDetails").Where(x=>x.Username == username).ToList();

                }


            }
            return View(li);
        }
        public ActionResult CreateTickettest()
        {

            // To check user access right
            string uri1 = "/Transaction/Tickettest";

            int bid = utlity.GetBussinessId(uri1);
            string ucheckid = User.Identity.GetUserId();

            checkpermission cm = new checkpermission();

            cm = db.Database.SqlQuery<checkpermission>("Access @id,@bid", new SqlParameter("@id", ucheckid), new SqlParameter("@bid", bid)).FirstOrDefault();
            if (cm == null)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            if (cm.record_insert == false)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            // access right ends 
            ViewBag.Country = (from c in db.Country where c.Status == 1 select new SelectListItem { Text = c.CountryName, Value = c.CountryName.Trim() }).ToList();
            ViewBag.Customer = (from s in db.EU_Master where s.Status == 1 && s.Id == 1013 select new SelectListItem { Text = s.Customer_Name, Value = SqlFunctions.StringConvert((double)s.Id).TrimStart() }).ToList();
            ViewBag.OEM = (from c in db.HeaderDescription where c.header_id == 4 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.System_Info = (from c in db.HeaderDescription where c.header_id == 19 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Type = (from c in db.HeaderDescription where c.header_id == 11 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Mode = (from c in db.HeaderDescription where c.header_id == 13 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.SLA = (from c in db.HeaderDescription where c.header_id == 14 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Priority = (from c in db.HeaderDescription where c.header_id == 15 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Certification = (from c in db.Certification_Master where c.Status == 1 select new SelectListItem { Text = c.Certification_Name, Value = SqlFunctions.StringConvert((double)c.Id).Trim() }).ToList();


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTickettest(Ticket sa, string Comments, string[] email_office, string Email_Subject, List<Ticket_System_Info> IV, List<Ticket_EU_Detail> EUL)
        {

            try
            {
                string email = "";



                foreach (var i in email_office)
                {
                    email = email + i + ",";
                }


                email = email.Remove(email.Length - 1);

                if (email == "")
                {
                    TempData["message"] = "Kindly add Email ID !! ";
                    return RedirectToAction("CreateTickettest", "Transaction");
                }
                sa.Ticket_No = utlity.CheckTicketNo(sa.EU_ID);
                sa.Status = 18;
                sa.CreatedBy = User.Identity.GetUserId();
                sa.CreatedOn = DateTime.Now;
                db.Ticket.Add(sa);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    Ticket_History th = new Ticket_History();
                    th.Ticket_no = sa.Id;
                    th.Comments = Comments;
                    th.FE_ID = sa.FE_ID;
                    th.status = 18;
                    th.CreatedBy = User.Identity.GetUserId();
                    th.CreatedOn = DateTime.Now;
                    db.Ticket_History.Add(th);
                    db.SaveChanges();

                    Ticket_Email TE = new Ticket_Email();
                    TE.Ticket_no = sa.Id;
                    TE.Email = email;
                    if (Email_Subject != "")
                    {
                        TE.Email_Subject = Email_Subject;

                    }
                    else
                    {
                        TE.Email_Subject = "Ticket Generated";
                        Email_Subject = "Ticket Generated";
                    }

                    TE.CreatedBy = User.Identity.GetUserId();
                    TE.CreatedOn = DateTime.Now;
                    db.Ticket_Email.Add(TE);
                    db.SaveChanges();

                    if (IV[0].System_Information != null)
                    {
                        Ticket_System_Info TSI = new Ticket_System_Info();

                        //Loop and insert records.
                        foreach (Ticket_System_Info iv in IV)
                        {
                            TSI.Ticket_no = sa.Id;
                            TSI.System_Information = iv.System_Information;
                            TSI.Make_Model = iv.Make_Model;
                            TSI.Serial_Number = iv.Serial_Number;
                            TSI.Required_Tool = iv.Required_Tool;
                            TSI.CreatedBy = User.Identity.GetUserId();
                            TSI.CreatedOn = DateTime.Now;
                            db.Ticket_System_Info.Add(TSI);
                            db.SaveChanges();
                        }
                    }

                    if (EUL[0].EU_Name != null)
                    {
                        Ticket_EU_Detail TSEU = new Ticket_EU_Detail();

                        //Loop and insert records.
                        foreach (Ticket_EU_Detail eul in EUL)
                        {
                            TSEU.Ticket_no = sa.Id;
                            TSEU.EU_Name = eul.EU_Name;
                            TSEU.EU_Email = eul.EU_Email;
                            TSEU.EU_Contact = eul.EU_Contact;
                            TSEU.CreatedBy = User.Identity.GetUserId();
                            TSEU.CreatedOn = DateTime.Now;
                            db.Ticket_EU_Detail.Add(TSEU);
                            db.SaveChanges();
                        }
                    }
                }

                string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                string body = utlity.TicketGenerated(eu, sa.Ticket_No, sa.Case_No, sa.Site_Name, sa.Street_Address, sa.Dispatch_Date.ToString(), sa.Job_Description);
                utlity.sendmail(email, Email_Subject, body, mailer);
                TempData["message"] = "Ticket Created : <br>  Number :" + sa.Ticket_No;

            }
            catch (Exception ex)
            {
                TempData["message"] = "Ticket Not Created !! ";
            }
            return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });
        }
        public ActionResult editTickettest(int id)
        {
            // To check user access right
            string uri1 = "/Transaction/Tickettest";

            int bid = utlity.GetBussinessId(uri1);
            string ucheckid = User.Identity.GetUserId();

            checkpermission cm = new checkpermission();

            cm = db.Database.SqlQuery<checkpermission>("Access @id,@bid", new SqlParameter("@id", ucheckid), new SqlParameter("@bid", bid)).FirstOrDefault();
            if (cm == null)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            if (cm.record_update == false)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            // access right ends 
            Ticket ti = new Ticket();
            ti = (from s in db.Ticket where s.Id == id select s).FirstOrDefault();
            ViewBag.Country = (from c in db.Country where c.Status == 1 select new SelectListItem { Text = c.CountryName, Value = c.CountryName.Trim() }).ToList();
            ViewBag.Customer = (from s in db.EU_Master where s.Id == ti.EU_ID select s.Customer_Name).FirstOrDefault();
            ViewBag.OEM = (from c in db.HeaderDescription where c.header_id == 4 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Status = (from c in db.HeaderDescription where c.header_id == 5 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Part_Management = (from c in db.HeaderDescription where c.header_id == 6 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Part_Handover = (from c in db.HeaderDescription where c.header_id == 7 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();


            ViewBag.Ticket_Type = (from c in db.HeaderDescription where c.header_id == 11 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Mode = (from c in db.HeaderDescription where c.header_id == 13 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.SLA = (from c in db.HeaderDescription where c.header_id == 14 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Priority = (from c in db.HeaderDescription where c.header_id == 15 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Certification = (from c in db.Certification_Master where c.Status == 1 select new SelectListItem { Text = c.Certification_Name, Value = SqlFunctions.StringConvert((double)c.Id).Trim() }).ToList();


            ViewBag.Cust_Business = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Business_Hr).FirstOrDefault();
            ViewBag.Cust_Business_Non = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Non_Business_Hr).FirstOrDefault();
            ViewBag.Cust_Minimum = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Minimum_Hrs).FirstOrDefault();
            ViewBag.Cust_Charges_job = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Per_Job).FirstOrDefault();

            ViewBag.FE = (from s in db.FE_Master_Personal where s.Id == ti.FE_ID select s.First_Name).FirstOrDefault();

            ViewBag.Decline_Reason = (from c in db.HeaderDescription where c.header_id == 16 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Cancel_Reason = (from c in db.HeaderDescription where c.header_id == 17 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket = "INWIN000" + ti.Id;
            ViewBag.Currency = (from s in db.Currency_Master where s.Country == ti.Country select s.Currency).FirstOrDefault();

            ViewBag.EU_Office = (from s in db.EU_Master_Branch where s.Id == ti.EU_Office select s.Office).FirstOrDefault();
            ViewBag.Email_Subject = (from s in db.Ticket_Email where s.Ticket_no == ti.Id select s.Email_Subject).FirstOrDefault();
            ViewBag.email_office = (from s in db.Ticket_Email where s.Ticket_no == ti.Id select s.Email).FirstOrDefault();

            ViewBag.Old_Ticket = (from s in db.Ticket where s.Id == ti.Old_Ticket select s.Ticket_No).FirstOrDefault();

            ViewBag.System_info = (from s in db.Ticket_System_Info where s.Ticket_no == ti.Id select s).ToList();

            return View(ti);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult editTickettest(Ticket sa, HttpPostedFileBase pht, string[] email_office, string Comments, List<Part_Ticket_Data> IV)
        {
            string email = "";



            foreach (var i in email_office)
            {
                email = email + i + ",";
            }


            email = email.Remove(email.Length - 1);
            Ticket_Email TEE = new Ticket_Email();
            TEE = (from s in db.Ticket_Email where s.Ticket_no == sa.Id select s).FirstOrDefault();

            if (email == TEE.Email)
            {

            }
            else
            {
                TEE.Email = email;
                db.Entry(TEE).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }



            var email_auto = (from s in db.Ticket_Email where s.Ticket_no == sa.Id select s).FirstOrDefault();
            var fe = (from s in db.FE_Master_Personal where s.Id == sa.FE_ID select s).FirstOrDefault();
            try
            {
                if (sa.Status == 20)
                {
                    if (sa.In_Time == null)
                    {
                        TempData["message"] = "Do enter In Time before Closing the ticket !!";
                        return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });
                    }
                    else if (sa.Out_Time == null)
                    {
                        TempData["message"] = "Do enter Out Time before Closing the ticket !!";
                        return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });
                    }
                    else if (sa.In_Time > sa.Out_Time)
                    {
                        TempData["message"] = "Do Check Out Time is past In Time!!";
                        return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });

                    }

                }
                else if (sa.Status == 1362)
                {
                    sa.Is_Decline = 1;
                }
                else if (sa.Status == 19)
                {
                    if (sa.In_Time == null)
                    {
                        sa.Is_Reschedule = 1;
                    }
                    else
                    {
                        TempData["message"] = "Ticket is already checked in! Can't be Rescheduled !!";
                        return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });
                    }

                }

                string pic = "";
                string pathtest = "";
                if (pht != null)
                {

                    pic = System.IO.Path.GetFileName(pht.FileName);
                    string fileName = sa.Id + DateTime.Now.ToString("yyyyMMddHHmmssFFF") + "_" + pic;
                    pathtest = System.IO.Path.Combine(Server.MapPath("~/Upload/Pregame"), fileName);
                    pht.SaveAs(pathtest);

                    sa.pregame_upload = url + "Pregame/" + fileName;
                }

                sa.ModifiedBy = User.Identity.GetUserId();
                sa.ModifiedOn = DateTime.Now;
                db.Entry(sa).State = System.Data.Entity.EntityState.Modified;
                int res = db.SaveChanges();
                if (res > 0)
                {
                    Ticket_History th = new Ticket_History();
                    th.Ticket_no = sa.Id;
                    th.Comments = Comments;
                    th.FE_ID = sa.FE_ID;
                    th.status = sa.Status;
                    th.CreatedBy = User.Identity.GetUserId();
                    th.CreatedOn = DateTime.Now;
                    db.Ticket_History.Add(th);
                    db.SaveChanges();


                }

                if (IV[0].Serial_No != null)
                {
                    Part_Ticket_Data TSI = new Part_Ticket_Data();

                    //Loop and insert records.
                    foreach (Part_Ticket_Data iv in IV)
                    {
                        TSI.Ticket_No = sa.Id;
                        TSI.Make_Model = iv.Make_Model;
                        TSI.Part_Description = iv.Part_Description;
                        TSI.Part_type = iv.Part_type;
                        TSI.Serial_No = iv.Serial_No;
                        TSI.CreatedBy = User.Identity.GetUserId();
                        TSI.CreatedOn = DateTime.Now;
                        db.Part_Ticket_Data.Add(TSI);
                        db.SaveChanges();
                    }
                }


                TempData["message"] = "Ticket Updated !!";

                if (sa.Status == 20)
                {
                    DateTime start = sa.In_Time.Value;
                    DateTime end = sa.Out_Time.Value;

                    int count = 0;
                    int countnon = 0;

                    Decimal? FETotal;
                    Decimal? CTTotal;


                    for (var i = start; i < end; i = i.AddMinutes(15))
                    {
                        if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                        {
                            if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours < 17)
                            {
                                count++;
                            }
                            else
                            {
                                countnon++;
                            }

                        }
                        else
                        {
                            countnon++;
                        }
                    }


                    TimeSpan bc = TimeSpan.FromMinutes(15 * count);
                    TimeSpan bnc = TimeSpan.FromMinutes(15 * countnon);

                    if (sa.FE_Payment_Mode == "H")
                    {


                        var FE_NB = ((sa.FE_Non_Buss_Hrs / 4) * countnon);
                        var FE_B = ((sa.FE_Buss_Hrs / 4) * count);
                        FETotal = FE_NB + FE_B + sa.FE_Allowance + sa.Other_Amt;


                    }
                    else if (sa.FE_Payment_Mode == "F")
                    {
                        FETotal = sa.FE_Allowance + sa.FE_Fixed + sa.Other_Amt;
                    }
                    else
                    {
                        FETotal = 0;
                    }




                    if (sa.CT_Payment_Mode == "H")
                    {


                        var CT_NB = ((sa.CT_Non_Buss_Hrs / 4) * countnon);
                        var CT_B = ((sa.CT_Buss_Hrs / 4) * count);
                        CTTotal = CT_NB + CT_B + sa.CT_Allowance + sa.Other_Amt;


                    }
                    else if (sa.CT_Payment_Mode == "F")
                    {
                        CTTotal = sa.CT_Allowance + sa.CT_Fixed + sa.Other_Amt;
                    }
                    else
                    {
                        CTTotal = 0;
                    }


                    TimeSpan TRT = sa.Out_Time.Value - sa.In_Time.Value;


                    string sys_info = "  <table style='width: 100 %; height: 40px;padding-left:20px; border:1px;'><tr style='background: rgb(131,194, 51);'><td>System Information</td><td>Make/Model<td>Serial Number</td><td>Required Tool</td></tr>";
                    List<Ticket_System_Info> tsi = new List<Ticket_System_Info>();
                    tsi = (from s in db.Ticket_System_Info where s.Ticket_no == sa.Id select s).ToList();
                    for (int i = 0; i < tsi.Count; i++)
                    {
                        sys_info = sys_info + "<tr><td>" + tsi[i].System_Information + "</td><td>" + tsi[i].Make_Model + "</td><td>" + tsi[i].Serial_Number + "</td><td>" + tsi[i].Required_Tool + "</td></tr>";
                    }
                    sys_info = sys_info + "</table>";

                    TempData["Payment"] = "Number of Business Hour Served: " + count + " and Non Business Hour : " + countnon;

                    TempData["message"] = "Ticket No: " + sa.Ticket_No + " Closed and Email sent to engineer and to customer";

                    string RRT;
                    if (sa.Other_Charge == 1)
                    {
                        RRT = "Yes";
                    }
                    else
                    {
                        RRT = "No";
                    }

                    string Part_Detail = "";
                    if (sa.Part_Management == 22)
                    {
                        if (sa.Return_Label == 1)
                        {
                            if (sa.FE_Part_Storage_Charge == 1)
                            {
                                Part_Detail = "Part Management :Pick up , Return Lable Available . Tracking Number : " + sa.Tracking_number + " Part Storage Charges is applicable. ";

                            }
                            else
                            {
                                Part_Detail = "Part Management :Pick up , Return Lable Available . Tracking Number : " + sa.Tracking_number + " Part Storage Charges is not applicable. ";
                            }

                        }
                        else
                        {
                            if (sa.FE_Part_Storage_Charge == 1)
                            {
                                Part_Detail = "Part Management :Pick up , Return Lable Not Available . Part Storage Charges is applicable. ";

                            }
                            else
                            {
                                Part_Detail = "Part Management :Pick up , Return Lable Not Available . Part Storage Charges is not applicable. ";
                            }
                        }

                    }
                    else if (sa.Part_Management == 23)
                    {
                        Part_Detail = "Part Management : Handover , Part Handover " + (from s in db.HeaderDescription where s.id == sa.Part_Handover select s.header_description).FirstOrDefault() + " .Contact Name : " + sa.ph_Name + " Contact Number : " + sa.ph_contact;
                    }
                    else if (sa.Part_Management == 1371)
                    {
                        Part_Detail = "Part Management : Not Applicable ";
                    }
                    var callbackUrl = Url.Action("CSAT", "Transaction", new { Id = sa.Id }, protocol: Request.Url.Scheme);
                    string oem = (from s in db.HeaderDescription where s.id == sa.OEM select s.header_description).FirstOrDefault();
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string body = utlity.CustomterClosing(eu, sa.In_Time.Value.ToString("dd/MM/yyyy"), sa.In_Time.Value.ToString("HH:mm:ss"), sa.Out_Time.Value.ToString("dd/MM/yyyy"), sa.Out_Time.Value.ToString("HH:mm:ss"), "", "", Comments, callbackUrl, sa.Case_No, sa.Ticket_No,
                        oem, sys_info, fe.First_Name + " " + fe.Last_Name, TRT.ToString(), RRT, Part_Detail);
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);

                    string invaddress;
                    if (fe.Country == "India")
                    {
                        invaddress = "Inwinteck Private Limited, 10/34, Vijay Garden, Off G.B Road, Thane (W) -400615";
                    }
                    else
                    {
                        invaddress = "Inwinteck Pte Ltd, 23 Kelantan Lane,#04-01 Kim Hoe Centre, Singapore – 208642.";
                    }

                    string bodyde = utlity.FEClosing(fe.First_Name + "" + fe.Last_Name, sa.Ticket_No, oem, sys_info, TRT.ToString(), RRT, Part_Detail, sa.In_Time.Value.ToString("dd/MM/yyyy"), sa.In_Time.Value.ToString("HH:mm:ss"), sa.Out_Time.Value.ToString("dd/MM/yyyy"),
                        sa.Out_Time.Value.ToString("HH:mm:ss"), Comments, invaddress);
                    utlity.sendmail(fe.Email, "Inwinteck Ticket no " + sa.Ticket_No + " is Closed", bodyde, mailer);


                    return RedirectToAction("viewTickettest", "Transaction", new { id = sa.Id });
                }
                else if (sa.Status == 21)
                {
                    TempData["message"] = "Ticket No: " + sa.Ticket_No + " Cancel";
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string Cancel_Reason = (from c in db.HeaderDescription where c.id == sa.Cancel_Reason select c.header_description).FirstOrDefault();
                    string body = utlity.CustomerCancel(eu, sa.Ticket_No, sa.Case_No, sa.Site_Name, sa.Street_Address, sa.Job_Description, sa.Dispatch_Date.Value.ToString("dd/MM/yyyy"), sa.Dispatch_Date.Value.ToString("HH:mm:ss"), Cancel_Reason);
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);

                    if (sa.FE_ID != 0)
                    {
                        string body1 = utlity.FECancel(fe.First_Name + " " + fe.Last_Name);
                        utlity.sendmail(fe.Email, "Ticket No : " + sa.Ticket_No + " has been Canceled", body1, mailer);

                    }
                    return RedirectToAction("Tickettest", "Transaction");
                }
                else if (sa.Status == 1362)
                {
                    TempData["message"] = "Ticket No: " + sa.Ticket_No + " Declined";
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string body = utlity.CustomerDecline(eu);
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);

                    string body1 = utlity.FEDecline(fe.First_Name + " " + fe.Last_Name);
                    utlity.sendmail(fe.Email, "Ticket No : " + sa.Ticket_No + " has been Declined", body1, mailer);


                    return RedirectToAction("Tickettest", "Transaction");
                }
                else if (sa.Status == 1414)
                {
                    TempData["message"] = "Engineer details sent to customer";
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string body = utlity.CustomerSchedule(eu, sa.Case_No, fe.First_Name + " " + fe.Last_Name, fe.Email, fe.Phone_Number_Code + "-" + fe.Phone_Number);
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);
                    return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });
                }
                else if (sa.Status == 1415)
                {
                    TempData["message"] = "FE Arrived on site and Email send to customer";
                    string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                    string body = utlity.FECheckedIn(eu, sa.Case_No, fe.First_Name + " " + fe.Last_Name, fe.Email, fe.Phone_Number_Code + "-" + fe.Phone_Number, sa.In_Time.Value.ToString("dd/MM/yyyy"), sa.In_Time.Value.ToString("HH:mm:ss"));
                    utlity.sendmail(email_auto.Email, email_auto.Email_Subject, body, mailer);
                    return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });
                }
                else if (sa.Status == 1372)
                {

                    TempData["message"] = "Ticket has been Scheduled.FE has been informed by email.";
                    string sys_info = "  <table style='width: 100 %; height: 40px;padding-left:20px; border:1px;'><tr style='background: rgb(131,194, 51);'><td>System Information</td><td>Make/Model<td>Serial Number</td><td>Required Tool</td></tr>";
                    List<Ticket_System_Info> tsi = new List<Ticket_System_Info>();
                    tsi = (from s in db.Ticket_System_Info where s.Ticket_no == sa.Id select s).ToList();
                    for (int i = 0; i < tsi.Count; i++)
                    {
                        sys_info = sys_info + "<tr><td>" + tsi[i].System_Information + "</td><td>" + tsi[i].Make_Model + "</td><td>" + tsi[i].Serial_Number + "</td><td>" + tsi[i].Required_Tool + "</td></tr>";
                    }

                    sys_info = sys_info + "</table>";

                    string oem = (from s in db.HeaderDescription where s.id == sa.OEM select s.header_description).FirstOrDefault();
                    if (sa.pregame_upload != null)
                    {
                        string body = utlity.JobConfirm(fe.First_Name + " " + fe.Last_Name, sa.Site_Name, sa.EU_Name, sa.Street_Address, sa.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), sa.Job_Description, sa.pregame_detail, sys_info, oem);
                        utlity.sendmailattach(fe.Email, "Job Confirmation and additional details for Inwinteck Ticket no :" + sa.Ticket_No, body, mailer, pathtest);


                    }
                    else
                    {
                        string body = utlity.JobConfirm(fe.First_Name + " " + fe.Last_Name, sa.Site_Name, sa.EU_Name, sa.Street_Address, sa.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), sa.Job_Description, sa.pregame_detail, sys_info, oem);
                        utlity.sendmail(fe.Email, "Job Confirmation and additional details for Inwinteck Ticket no :" + sa.Ticket_No, body, mailer);
                    }




                    return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });

                }
                else if (sa.Status == 19)
                {

                    Ticket rti = new Ticket();
                    Ticket_Email rte = new Ticket_Email();
                    List<Ticket_System_Info> reTSI = new List<Ticket_System_Info>();

                    rti = (from s in db.Ticket where s.Id == sa.Id select s).FirstOrDefault();
                    rte = (from s in db.Ticket_Email where s.Ticket_no == sa.Id select s).FirstOrDefault();
                    reTSI = (from s in db.Ticket_System_Info where s.Ticket_no == sa.Id select s).ToList();
                    try
                    {
                        rti.Dispatch_Date = sa.Reschedule_DT;
                        rti.FE_ID = 0;
                        rti.Ticket_No = utlity.CheckTicketNo(rti.EU_ID);
                        rti.Status = 18;
                        rti.Pregame = null;
                        rti.Is_Reschedule = 0;
                        rti.Old_Ticket = sa.Id;
                        rti.CreatedBy = User.Identity.GetUserId();
                        rti.CreatedOn = DateTime.Now;
                        db.Ticket.Add(sa);
                        int reres = db.SaveChanges();
                        if (reres > 0)
                        {
                            Ticket_History th = new Ticket_History();
                            th.Ticket_no = rti.Id;
                            th.Comments = Comments;
                            th.FE_ID = rti.FE_ID;
                            th.status = 18;
                            th.CreatedBy = User.Identity.GetUserId();
                            th.CreatedOn = DateTime.Now;
                            db.Ticket_History.Add(th);
                            db.SaveChanges();

                            Ticket_Email TE = new Ticket_Email();
                            TE.Ticket_no = rti.Id;
                            TE.Email = rte.Email;
                            TE.Email_Subject = email_auto.Email_Subject;
                            string resEmail_Subject = email_auto.Email_Subject;


                            TE.CreatedBy = User.Identity.GetUserId();
                            TE.CreatedOn = DateTime.Now;
                            db.Ticket_Email.Add(TE);
                            db.SaveChanges();



                            if (reTSI[0].System_Information != null)
                            {
                                Ticket_System_Info TSI = new Ticket_System_Info();

                                //Loop and insert records.
                                foreach (Ticket_System_Info iv in reTSI)
                                {
                                    TSI.Ticket_no = rti.Id;
                                    TSI.System_Information = iv.System_Information;
                                    TSI.Make_Model = iv.Make_Model;
                                    TSI.Serial_Number = iv.Serial_Number;
                                    TSI.Required_Tool = iv.Required_Tool;
                                    TSI.CreatedBy = User.Identity.GetUserId();
                                    TSI.CreatedOn = DateTime.Now;
                                    db.Ticket_System_Info.Add(TSI);
                                    db.SaveChanges();
                                }
                            }

                            string old_ticket = (from s in db.Ticket where s.Id == rti.Old_Ticket select s.Ticket_No).FirstOrDefault();
                            string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                            string body = utlity.CustomerReschedule(eu, rti.Ticket_No, sa.Case_No, rti.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), old_ticket, sa.Reschedule_Reason);
                            utlity.sendmail(rte.Email, resEmail_Subject, body, mailer);

                            string body1 = utlity.FEReschedule(fe.First_Name + " " + fe.Last_Name, old_ticket, rti.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), sa.Reschedule_Reason);
                            utlity.sendmail(fe.Email, "Ticket Rescheduled for Inwinteck Ticket no :" + old_ticket, body1, mailer);

                            TempData["message"] = "Ticket has been Reschedule New Ticket has been created Ticket No " + rti.Ticket_No;
                        }

                        return RedirectToAction("editTickettest", "Transaction", new { id = rti.Id });

                    }
                    catch (Exception ex)
                    {
                        string msg = "/Ticket/Not Rescheduled" + " " + "Ticket No : " + sa.Id + " " + "Error: " + ex.Message.ToString();
                        utlity.createlog(msg);
                        TempData["message"] = "Ticket Not Rescheduled !! ";
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "Ticket Not Updated !! ";
                string msg = "/Ticket/Error" + " " + "Ticket No : " + sa.Id + " " + "Error: " + ex.Message.ToString();
            }
            return RedirectToAction("editTickettest", "Transaction", new { id = sa.Id });
        }
        public ActionResult viewTickettest(int id)
        {
            // To check user access right
            string uri1 = "/Transaction/Tickettest";

            int bid = utlity.GetBussinessId(uri1);
            string ucheckid = User.Identity.GetUserId();

            checkpermission cm = new checkpermission();

            cm = db.Database.SqlQuery<checkpermission>("Access @id,@bid", new SqlParameter("@id", ucheckid), new SqlParameter("@bid", bid)).FirstOrDefault();
            if (cm == null)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            if (cm.record_view == false)
            {
                return RedirectToAction("UnAuthorisedAccess", "Dashboard");
            }
            // access right ends 
            Ticket ti = new Ticket();
            ti = (from s in db.Ticket where s.Id == id select s).FirstOrDefault();
            ViewBag.Country = (from c in db.Country where c.Status == 1 select new SelectListItem { Text = c.CountryName, Value = c.CountryName.Trim() }).ToList();
            ViewBag.Customer = (from s in db.EU_Master where s.Id == ti.EU_ID select s.Customer_Name).FirstOrDefault();
            ViewBag.OEM = (from c in db.HeaderDescription where c.header_id == 4 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Status = (from c in db.HeaderDescription where c.header_id == 5 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Part_Management = (from c in db.HeaderDescription where c.header_id == 6 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Part_Handover = (from c in db.HeaderDescription where c.header_id == 7 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();


            ViewBag.Ticket_Type = (from c in db.HeaderDescription where c.header_id == 11 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Mode = (from c in db.HeaderDescription where c.header_id == 13 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.SLA = (from c in db.HeaderDescription where c.header_id == 14 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket_Priority = (from c in db.HeaderDescription where c.header_id == 15 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Certification = (from c in db.Certification_Master where c.Status == 1 select new SelectListItem { Text = c.Certification_Name, Value = SqlFunctions.StringConvert((double)c.Id).Trim() }).ToList();


            ViewBag.Cust_Business = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Business_Hr).FirstOrDefault();
            ViewBag.Cust_Business_Non = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Non_Business_Hr).FirstOrDefault();
            ViewBag.Cust_Minimum = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Minimum_Hrs).FirstOrDefault();
            ViewBag.Cust_Charges_job = (from s in db.EU_Rate_Card where s.EU_ID == ti.EU_ID && s.Country == ti.Country select s.Per_Job).FirstOrDefault();

            ViewBag.FE = (from s in db.FE_Master_Personal where s.Id == ti.FE_ID select s.First_Name).FirstOrDefault();

            ViewBag.Decline_Reason = (from c in db.HeaderDescription where c.header_id == 16 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Cancel_Reason = (from c in db.HeaderDescription where c.header_id == 17 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            ViewBag.Ticket = "INWIN000" + ti.Id;
            ViewBag.Currency = (from s in db.Currency_Master where s.Country == ti.Country select s.Currency).FirstOrDefault();

            ViewBag.EU_Office = (from s in db.EU_Master_Branch where s.Id == ti.EU_Office select s.Office).FirstOrDefault();
            ViewBag.Email_Subject = (from s in db.Ticket_Email where s.Ticket_no == ti.Id select s.Email_Subject).FirstOrDefault();
            ViewBag.email_office = (from s in db.Ticket_Email where s.Ticket_no == ti.Id select s.Email).FirstOrDefault();

            ViewBag.Old_Ticket = (from s in db.Ticket where s.Id == ti.Old_Ticket select s.Ticket_No).FirstOrDefault();

            ViewBag.New_Ticket = (from s in db.Ticket where s.Old_Ticket == ti.Id select s.Ticket_No).FirstOrDefault();

            return View(ti);
        }
        

        /*----------------------------------------------------------------------------------------------*/

        public ActionResult FEInvoice(DateTime? from, DateTime? to, int FE_ID = 0)
        {
            List<ManageHeader_FE> MH = new List<ManageHeader_FE>();
            List<FEL> FE = new List<FEL>();
            FE = (from c in db.Ticket
                  join r in db.FE_Master_Personal on c.FE_ID equals r.Id
                  where c.Status == 20
                  select new FEL
                  { id = r.Id, name = r.First_Name + "" + r.Last_Name }).Distinct().ToList();
            ViewBag.FE = FE;


            if (from != null && to != null && FE_ID > 0)
            {

                string frm = from.Value.ToString("yyyyMMdd");
                string to1 = to.Value.ToString("yyyyMMdd");
                MH = db.Database.SqlQuery<ManageHeader_FE>("GetTicketPriceFE @FE,@from,@to",
                    new SqlParameter("@FE", FE_ID), new SqlParameter("@from", frm),
                    new SqlParameter("@to", to1)).ToList();
                ViewBag.Message = 1;
            }
            else
            {
                MH = null;
                ViewBag.Message = 0;
            }

            return View(MH);
        }

        public ActionResult GenerateInvoiceFE(int Ticket)
        {
            Invoice_FE inv = new Invoice_FE();
            inv = db.Database.SqlQuery<Invoice_FE>("GetFEInvoiceDetail @id", new SqlParameter("@id ", Ticket)).FirstOrDefault();

            DateTime start = inv.In_Time;
            DateTime end = inv.Out_Time;

            int count = 0;
            int countnon = 0;

            for (var i = start; i < end; i = i.AddMinutes(15))
            {
                if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours < 17)
                    {
                        count++;
                    }
                    else
                    {
                        countnon++;
                    }

                }
                else
                {
                    countnon++;
                }
            }

            TimeSpan bc = TimeSpan.FromMinutes(15 * count);
            TimeSpan bnc = TimeSpan.FromMinutes(15 * countnon);
            ViewBag.Bussiness = string.Format("{0:00}:{1:00}", (int)bc.TotalHours, bc.Minutes);
            ViewBag.Bussinessnon = string.Format("{0:00}:{1:00}", (int)bnc.TotalHours, bnc.Minutes);
            ViewBag.BussinessnonChg = ((inv.FE_Non_Buss_Hrs / 4) * countnon);
            ViewBag.BussinessChg = ((inv.FE_Buss_Hrs / 4) * count);

            if (inv.FE_Payment_Mode == "F")
            {
                ViewBag.Total = inv.FE_Fixed + inv.FE_Allowance;
            }
            else if (inv.FE_Payment_Mode == "H")
            {
                ViewBag.Total = ViewBag.BussinessnonChg + ViewBag.BussinessChg + inv.FE_Allowance;
            }

            return View(inv);
        }

        [HttpPost]
        public ActionResult GenerateInvoiceFE(Header_Invoice_Detail_FE sa)
        {
            int cnt = (from s in db.Header_Invoice_Detail_FE where s.Ticket_no == sa.Ticket_no select s).Count();
            decimal amt_total = 0;
            if (cnt > 0)
            {
                TempData["message"] = "Invoice allready Generated !! ";
            }
            else
            {
                try
                {
                    if (sa.Fixed_amt > 0)
                    {

                        amt_total = sa.Fixed_amt + sa.Travel_Charge + sa.Part_Handling_Charge;
                    }
                    else
                    {
                        amt_total = sa.Business_hour_amt + sa.Non_Business_hour_amt + sa.Travel_Charge + sa.Part_Handling_Charge;

                    }
                    Header_Invoice_FE hf = new Header_Invoice_FE();
                    hf.FE_ID = sa.FE_ID;
                    hf.Total_Amt = amt_total;
                    hf.Invoice_Date = DateTime.Now.Date;
                    hf.FE_Payment_Status = "Unpaid";
                    hf.CreatedBy = User.Identity.GetUserId();
                    hf.CreatedOn = DateTime.Now;
                    db.Header_Invoice_FE.Add(hf);
                    int res = db.SaveChanges();
                    if (res > 0)
                    {

                        sa.Inv_no = hf.Id;
                        sa.CreatedBy = User.Identity.GetUserId();
                        sa.CreatedOn = DateTime.Now;
                        db.Header_Invoice_Detail_FE.Add(sa);
                        db.SaveChanges();


                    }

                    TempData["message"] = "FE Invoice Generated : Invoice No !!" + " " + hf.Id;
                    TempData["link"] = "Yes";
                    TempData["INV"] = hf.Id;
                }
                catch (Exception ex)
                {
                    TempData["message"] = "Invoice Not Generated !! ";
                }
            }

            return RedirectToAction("AllInvoiceFE", "Transaction");
        }
        public ActionResult PrintInvoiceFE(int InvoiceNo)
        {
            Invoice_FE_Print inv = new Invoice_FE_Print();
            inv = db.Database.SqlQuery<Invoice_FE_Print>("GetFEInvoicePrint @id", new SqlParameter("@id ", InvoiceNo)).FirstOrDefault();

            return View(inv);
        }

        public ActionResult AllInvoiceFE()
        {
            List<Invoice_FE_Print> inv = new List<Invoice_FE_Print>();
            try
            {
                inv = db.Database.SqlQuery<Invoice_FE_Print>("GetFEInvoiceAll").ToList();
                ViewBag.Payment_Mode = (from c in db.HeaderDescription where c.header_id == 10 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();
            }
            catch (Exception ex)
            {

            }
            return View(inv);
        }
        /*----------------------------------------------------------------------------------------------*/
        public ActionResult EUInvoice(DateTime? from, DateTime? to, int EU_ID = 0)
        {
            List<ManageHeader_FE> MH = new List<ManageHeader_FE>();
            List<FEL> EU = new List<FEL>();
            EU = (from c in db.Ticket
                  join r in db.EU_Master on c.EU_ID equals r.Id
                  where c.Status == 20
                  select new FEL
                  { id = r.Id, name = r.Customer_Name }).Distinct().ToList();
            ViewBag.EU = EU;


            if (from != null && to != null && EU_ID > 0)
            {

                string frm = from.Value.ToString("yyyyMMdd");
                string to1 = to.Value.ToString("yyyyMMdd");
                MH = db.Database.SqlQuery<ManageHeader_FE>("GetTicketPriceEU @EU,@from,@to",
                    new SqlParameter("@EU", EU_ID), new SqlParameter("@from", frm),
                    new SqlParameter("@to", to1)).ToList();
                ViewBag.Message = 1;
            }
            else
            {
                MH = null;
                ViewBag.Message = 0;
            }

            return View(MH);
        }

        public ActionResult GenerateInvoiceEU(int Ticket)
        {
            Invoice_EU inv = new Invoice_EU();
            inv = db.Database.SqlQuery<Invoice_EU>("GetEUInvoiceDetail @id", new SqlParameter("@id ", Ticket)).FirstOrDefault();



            int count = 0;
            int countnon = 0;
            if (inv.Cancel_Charge > 0)
            {
                ViewBag.Total = inv.Cancel_Charge;
            }
            else
            {
                DateTime start = inv.In_Time;
                DateTime end = inv.Out_Time;
                for (var i = start; i < end; i = i.AddMinutes(15))
                {
                    if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                    {
                        if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours < 17)
                        {
                            count++;
                        }
                        else
                        {
                            countnon++;
                        }

                    }
                    else
                    {
                        countnon++;
                    }
                }
                TimeSpan bc = TimeSpan.FromMinutes(15 * count);
                TimeSpan bnc = TimeSpan.FromMinutes(15 * countnon);
                ViewBag.Bussiness = string.Format("{0:00}:{1:00}", (int)bc.TotalHours, bc.Minutes);
                ViewBag.Bussinessnon = string.Format("{0:00}:{1:00}", (int)bnc.TotalHours, bnc.Minutes);
                ViewBag.BussinessnonChg = ((inv.CT_Non_Buss_Hrs / 4) * countnon);
                ViewBag.BussinessChg = ((inv.CT_Buss_Hrs / 4) * count);
                if (inv.CT_Payment_Mode == "F")
                {
                    ViewBag.Total = inv.CT_Fixed + inv.CT_Allowance + inv.Reschedule_Charge;
                }
                else if (inv.CT_Payment_Mode == "H")
                {
                    ViewBag.Total = ViewBag.BussinessnonChg + ViewBag.BussinessChg + inv.CT_Allowance + inv.Reschedule_Charge;
                }

            }



            return View(inv);
        }

        [HttpPost]
        public ActionResult GenerateInvoiceEU(Header_Invoice_Detail_EU sa)
        {
            int cnt = (from s in db.Header_Invoice_Detail_EU where s.Ticket_no == sa.Ticket_no select s).Count();

            if (cnt > 0)
            {
                TempData["message"] = "Invoice allready Generated !! ";
            }
            else
            {
                try
                {
                    decimal amt_total = sa.Business_hour_amt + sa.Non_Business_hour_amt + sa.Travel_Charge + sa.Part_Handling_Charge + sa.Cancel_Charge + sa.Reschedule_Charge + sa.Fixed_amt;
                    Header_Invoice_EU hf = new Header_Invoice_EU();
                    hf.EU_ID = sa.EU_ID;
                    hf.Total_Amt = amt_total;
                    hf.Invoice_Date = DateTime.Now.Date;
                    hf.CreatedBy = User.Identity.GetUserId();
                    hf.CreatedOn = DateTime.Now;
                    db.Header_Invoice_EU.Add(hf);
                    int res = db.SaveChanges();
                    if (res > 0)
                    {

                        sa.Inv_no = hf.Id;
                        sa.CreatedBy = User.Identity.GetUserId();
                        sa.CreatedOn = DateTime.Now;
                        db.Header_Invoice_Detail_EU.Add(sa);
                        db.SaveChanges();


                    }

                    TempData["message"] = "EU Invoice Generated : Invoice No !!" + " " + hf.Id;
                    TempData["link"] = "Yes";
                    TempData["INV"] = hf.Id;
                }
                catch (Exception ex)
                {
                    TempData["message"] = "Invoice Not Generated !! ";
                }

            }

            return RedirectToAction("AllInvoiceEU", "Transaction");
        }
        public ActionResult PrintInvoiceEU(int InvoiceNo)
        {
            Invoice_EU_Print inv = new Invoice_EU_Print();
            inv = db.Database.SqlQuery<Invoice_EU_Print>("GetEUInvoicePrint @id", new SqlParameter("@id ", InvoiceNo)).FirstOrDefault();

            return View(inv);
        }

        public ActionResult AllInvoiceEU()
        {
            List<Invoice_EU_Print> inv = new List<Invoice_EU_Print>();
            try
            {
                inv = db.Database.SqlQuery<Invoice_EU_Print>("GetEUInvoiceAll").ToList();
                ViewBag.Payment_Mode = (from c in db.HeaderDescription where c.header_id == 10 && c.Status == 1 select new SelectListItem { Text = c.header_description, Value = SqlFunctions.StringConvert((double)c.id).Trim() }).ToList();

            }
            catch (Exception ex)
            {


            }

            return View(inv);
        }
        /*----------------------------------------------------------------------------------------------*/
        public ActionResult FEPayment(Header_Invoice_FE sa)
        {
            try
            {
                Header_Invoice_FE hi = new Header_Invoice_FE();
                hi = (from s in db.Header_Invoice_FE where s.Id == sa.Id select s).FirstOrDefault();
                hi.FE_PaymentMode = sa.FE_PaymentMode;
                hi.FE_Payment_Status = sa.FE_Payment_Status;
                hi.FE_Pay_Date = sa.FE_Pay_Date;
                hi.FE_Reference_ID = sa.FE_Reference_ID;
                hi.ModifiedBy = User.Identity.GetUserId();
                hi.ModifiedOn = DateTime.Now;
                db.Entry(hi).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                TempData["message"] = "Payment Status Updated!! ";


            }
            catch (Exception ex)
            {
                TempData["message"] = "Payment Status Not Updated!! ";
            }
            return RedirectToAction("AllInvoiceFE", "Transaction");
        }

        /*----------------------------------------------------------------------------------------------*/

        [AllowAnonymous]
        public ActionResult TicketSelection(int? Id)
        {
            try
            {
                if (Id != null)
                {
                    Ticket_FE_Selection sa = new Ticket_FE_Selection();
                    sa = (from s in db.Ticket_FE_Selection where s.Id == Id.Value select s).FirstOrDefault();
                    var fe = (from s in db.FE_Master_Personal where s.Id == sa.FE_ID select s).FirstOrDefault();
                    var tic = (from s in db.Ticket where s.Id == sa.Ticket_no select s).FirstOrDefault();
                    string oem = (from s in db.HeaderDescription where s.id == tic.OEM select s.header_description).FirstOrDefault();

                    ViewBag.Site_address = tic.Street_Address;
                    ViewBag.Datetime = tic.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt");
                    ViewBag.Scope = tic.Job_Description;
                    ViewBag.OEM = oem;
                    if (sa.Status == "Sent")
                    {
                        ViewBag.Message = "Yes";
                    }
                    return View(sa);
                }
                else
                {

                    return View();
                }

            }
            catch
            {
                return View();
            }
         
         

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult TicketSelection(Ticket_FE_Selection sa)
        {

            try
            {
                sa.ModifiedOn = DateTime.Now;
                db.Entry(sa).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var fe = (from s in db.FE_Master_Personal where s.Id == sa.FE_ID select s).FirstOrDefault();
                var tic = (from s in db.Ticket where s.Id == sa.Ticket_no select s).FirstOrDefault();
                string oem = (from s in db.HeaderDescription where s.id == tic.OEM select s.header_description).FirstOrDefault();
                string body = utlity.FESelectionStatus("Team", tic.Street_Address, tic.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"), tic.Job_Description, oem, tic.Ticket_No,sa.Remark,sa.Status,fe.First_Name + " " + fe.Last_Name);

                if (sa.Status == "Accepted")
                {
                    if (fe.Status == 1)
                    {
                        TempData["message"] = "Thank you for accepting the opportunity.  Our support team will share further details to you post confirmation from client.!!";

                        utlity.sendmail("support@inwinteck.com", "Request for onsite/call out/intervention for Inwinteck Ticket No :" + tic.Ticket_No + " has been Accepted", body, mailer);

                    }
                    else
                    {
                        TempData["message"] = "Thank you for accepting the opportunity.<br/> To proceed further, you need to activate your account and confirm your charges for this job . <br/> You can also share your charges on our email id support@inwinteck.com. <br/> For activation of account,  kindly login to https://fms.inwinteck.com/ .<br/>Your email id  is the user id .  You have to reset the password and proceed.<br/> Our support team will reach out to you with further details. ";

                        utlity.sendmail("support@inwinteck.com", "Request for onsite/call out/intervention for Inwinteck Ticket No :" + tic.Ticket_No + " has been Accepted and FE Status is Deactive", body, mailer);

                    }
                }
                else if(sa.Status == "Rejected")
                {

                    if (fe.Status == 1)
                    { 
                        TempData["message"] = "Thank you for your prompt response. Our support team will get in touch with you for new business opportunity in the future.!!";

                        utlity.sendmail("support@inwinteck.com", "Request for onsite/call out/intervention for Inwinteck Ticket No :" + tic.Ticket_No + " has been rejected", body, mailer);
                    }
                    else
                    {
                        TempData["message"] = "Dear Partner,<br/> We appreciate your time and response. Unfortunately, you have denied this Onsite request for the reason mentioned by you .<br/>  Our support team will get in touch with you for new business opportunity. <br/> We request you to kindly activate your account by completing the profile so that we can  reach out to you again.<br/> Thank you and appreciate it.";

                        utlity.sendmail("support@inwinteck.com", "Request for onsite/call out/intervention for Inwinteck Ticket No :" + tic.Ticket_No + " has been rejected", body, mailer);
                    }
                  
                }
            }
            catch
            {
                TempData["message"] = "Status Not updated !!";
            }
         

            return View();
        }

        [AllowAnonymous]
        public ActionResult CSAT(int? Id)
        {
            try
            {
                if (Id != null)
                {
                    int cnt = (from s in db.CSAT where s.TicketNo == Id select s).Count();

                    if (cnt > 0)
                    {
                        ViewBag.cnt = 1;
                    }
                    ViewBag.Ticket = Id;
                    ViewBag.caseno = (from s in db.Ticket where s.Id == Id select s.Case_No).FirstOrDefault();
                    ViewBag.inwinteck = (from s in db.Ticket where s.Id == Id select s.Ticket_No).FirstOrDefault();
                }
                else
                {
                    ViewBag.cnt = 1;
                }

            }
            catch
            {
               
            }

            return View();

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CSAT(CSAT sa)
        {
            int cnt = (from s in db.CSAT where s.TicketNo == sa.TicketNo select s).Count();

            try
            {
                if (cnt > 0)
                {
                    ViewBag.cnt = 1;
                    ViewBag.Ticket = sa.TicketNo;
                }
                else
                {
                    sa.CreatedOn = DateTime.Now;
                    db.CSAT.Add(sa);
                    db.SaveChanges();

                    int cs = sa.Q1 + sa.Q2 + sa.Q3 + sa.Q4 + sa.Q5;
                    decimal avg = Convert.ToDecimal(cs / 5);
                    TempData["CsatRating"] = avg;
                    TempData["message"] = "Dear Customer, we appreciate your valuable feedback . Thank you.";


                    //if (sa.Q3 == 1 || sa.Q3 == 2)
                    //{
                    //    string sub = "Dissatisfied customer (Feedback Rating 1 / 2)";
                    //    var tkt = (from a in db.Ticket where a.id == sa.TicketNo select a).FirstOrDefault();
                    //    var customer = (from s in db.Customer where s.id == tkt.Customer_ID select s).FirstOrDefault();
                    //    var cp = (from s in db.CollectionPoint where s.id == tkt.CP_ID select s).FirstOrDefault();
                    //    string branch = (from s in db.HeaderDescriptions where s.id.ToString() == cp.cp_branchcode select s.header_description).FirstOrDefault();

                    //    string body = mailutility.CSAT(customer.cust_name, customer.cust_cont, sa.TicketNo.ToString(), cp.cp_shop, branch, sa.Q3.ToString(), DateTime.Now.ToString("dd-MM-yyyy"), sa.Remarks);
                    //    string to = "feedback@vipbags.com, bhavisha.saraiya@jetairservices.in, aadesh.kumar@jetairservices.in, anoop.gopinathan@vipbags.com, neeraj.rajwal@vipbags.com, sonali.desai@vipbags.com, anjan.sengupta@vipbags.com,serviceteamleaders@vipbags.com";

                    //    string From = "Customer Feedback <smtp@vipbags.com>";
                    //    string ress = mailutility.sendmail_csat(to, sub, body, From);
                    //}
                }
            }
            catch(Exception ex )
            {
                TempData["message"] = "Not updated !!";
            }


            return RedirectToAction("CSAT", new { Id = sa.TicketNo });
        }

        [HttpGet]
        public JsonResult GetContact(int Cnt)
        {
            string res = "";
            List<EU_Master_Contacts> CC = new List<EU_Master_Contacts>();
            CC = (from c in db.EU_Master_Contacts where c.Office_ID == Cnt select c).ToList();
            if (CC != null)
            {
                res = "Success";
            }
            return Json(new { res, CC }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetOffice(int Cnt)
        {
            string res = "";
            List<EU_Master_Branch> CC = new List<EU_Master_Branch>();
            CC = (from c in db.EU_Master_Branch where c.EU_ID == Cnt select c).ToList();
            if (CC != null)
            {
                res = "Success";
            }
            return Json(new { res, CC }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getFEdetails(string PinCode, string Country, int ticket)
        {
            string res = "";
            List<FEDetails> CC = new List<FEDetails>();

            CC = db.Database.SqlQuery<FEDetails>("getFEdetails @id,@cnt", new SqlParameter("@id", PinCode), new SqlParameter("@cnt", Country)).ToList();
            if (CC != null)
            {
                res = "Success";
            }
            return Json(new { res, CC }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getFEInfo(int id)
        {
            string res = "";
            int i = 0;
            string cert = "";
            FEDetails CC = new FEDetails();

            CC = db.Database.SqlQuery<FEDetails>("getFEdetails @id", new SqlParameter("@id", id)).FirstOrDefault();


            if (CC != null)
            {
                res = "Success";

                if (CC.Certification != null)
                {
                    string[] fields = CC.Certification.Split(',');
                    if (fields.Length > 0)
                    {
                        if (i == 0)
                        {
                            foreach (var item in fields)
                            {
                                cert = cert + "," + (from s in db.Certification_Master where s.Id.ToString() == item select s.Certification_Name).FirstOrDefault();
                            }
                            i++;
                        }
                    }


                    cert = cert.Substring(1);

                }
            }

            return Json(new { res, CC, cert }, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult GetTicketHistory(int tn)
        {
            string tv = "";
            List<TicketHist> hist = new List<TicketHist>();
            hist = db.Database.SqlQuery<TicketHist>("gethistory @id", new SqlParameter("@id ", tn)).ToList();

            if (hist != null)
            {
                tv = "success";

            }
            else
            {
                tv = "failure";
            }


            return Json(new { tv, hist }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetLocation(string PinCode)
        {
            string res = "";
            string pzm = "";
            int cnt;
            PinCodeMaster Location = new PinCodeMaster();
            List<PinCodeMaster> PZ = new List<PinCodeMaster>();
            Location = (from c in db.PinCode where c.pincode == PinCode && c.status == 1 select c).FirstOrDefault();
            cnt = (from s in db.PinCode where s.pincode == PinCode && s.status == 1 select s).Count();
            if (Location != null)
            {
                res = "Success";
                if (cnt > 1)
                {
                    PZ = (from s in db.PinCode where s.pincode == PinCode && s.status == 1 select s).ToList();
                    pzm = "List";
                }
            }
            return Json(new { res, Location, PZ, pzm }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetLocationCity(string City)
        {
            string res = "";
            string pzm = "";
            int cnt;
            PinCodeMaster Location = new PinCodeMaster();
            List<PinCodeMaster> PZ = new List<PinCodeMaster>();
            Location = (from c in db.PinCode where c.city == City && c.status == 1 select c).FirstOrDefault();
            cnt = (from s in db.PinCode where s.city == City && s.status == 1 select s.Country).Distinct().Count();
            if (Location != null)
            {
                res = "Success";
                if (cnt > 1)
                {
                    PZ = (from s in db.PinCode where s.city == City && s.status == 1 select s).Distinct().ToList();
                    pzm = "List";
                }
            }
            return Json(new { res, Location, PZ, pzm }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult getFEdetailsMap(int id)
        {
           // db.Database.CommandTimeout = 180;
            //string res = "";
            //List<FEDetails> CC = new List<FEDetails>();
            //List<FEDetailsmap> FEM = new List<FEDetailsmap>();

            //CC = db.Database.SqlQuery<FEDetails>("getFEdetailsmap").ToList();
            //FEM = db.Database.SqlQuery<FEDetailsmap>("getFEdetailsmap").ToList();
            //if (CC != null)
            //{
            //    res = "Success";
            //}

            string username = User.Identity.GetUserName();
            List<FEDetails> data = new List<FEDetails>();

            if (username == "test@inwinteckfms.com")
            {

                data = db.Database.SqlQuery<FEDetails>("getFEdetailsmaptest @id", new SqlParameter("@id", id)).ToList();
            }
            else
            {
                data = db.Database.SqlQuery<FEDetails>("getFEdetailsmap @id", new SqlParameter("@id", id)).ToList();
            }
            

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getFEdetailsMaptest(int id)
        {
            //string res = "";
            //List<FEDetails> CC = new List<FEDetails>();
            //List<FEDetailsmap> FEM = new List<FEDetailsmap>();

            //CC = db.Database.SqlQuery<FEDetails>("getFEdetailsmap").ToList();
            //FEM = db.Database.SqlQuery<FEDetailsmap>("getFEdetailsmap").ToList();
            //if (CC != null)
            //{
            //    res = "Success";
            //}

            string username = User.Identity.GetUserName();
            List<FEDetails> data = new List<FEDetails>();

       
                data = db.Database.SqlQuery<FEDetails>("getFEdetailsmap @id", new SqlParameter("@id", id)).ToList();
    


            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ContactEUMaster(EU_Master_Contacts sa, int Contact_ID)
        {
            if (Contact_ID != 0)
            {
                try
                {
                    sa.Id = Contact_ID;
                    sa.CreatedBy = (from s in db.EU_Master_Contacts where s.Id == Contact_ID select s.CreatedBy).FirstOrDefault();
                    sa.CreatedOn = (from s in db.EU_Master_Contacts where s.Id == Contact_ID select s.CreatedOn).FirstOrDefault();
                    sa.ModifiedBy = User.Identity.GetUserId();
                    sa.ModifiedOn = DateTime.Now;
                    db.Entry(sa).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();


                    TempData["message"] = "EU Master's Contact Updated !!";

                }
                catch (Exception ex)
                {
                    TempData["message"] = "EU Master's Contact Not Updated !! ";
                }
            }
            else
            {
                try
                {
                    sa.CreatedBy = User.Identity.GetUserId();
                    sa.CreatedOn = DateTime.Now;
                    db.EU_Master_Contacts.Add(sa);
                    db.SaveChanges();


                    TempData["message"] = "EU Master's Contact Added !!";

                }
                catch (Exception ex)
                {
                    TempData["message"] = "EU Master's Contact Not Created !! ";
                }
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult deleteContact(int OC)
        {
            int od = (from s in db.EU_Master_Contacts where s.Id == OC select s.Office_ID).FirstOrDefault();
            int ED = (from s in db.EU_Master_Branch where s.Id == od select s.EU_ID).FirstOrDefault();
            if (OC > 0)
            {

                EU_Master_Contacts sa = new EU_Master_Contacts();
                sa = (from c in db.EU_Master_Contacts where c.Id == OC select c).FirstOrDefault();
                if (sa != null)
                {
                    db.Entry(sa).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }

            }
            else
            {
                TempData["message"] = "Empty Data !! ";
            }


            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getFEMail(int id, int Ticket)
        {
            string res = "";
            string fename = "";
            Ticket_FE_Selection TF = new Ticket_FE_Selection();

            TF.FE_ID = id;
            TF.Ticket_no = Ticket;
            TF.Status = "Sent";
            TF.CreatedBy = User.Identity.GetUserId();
            TF.CreatedOn = DateTime.Now;
            db.Ticket_FE_Selection.Add(TF);
            int cc = db.SaveChanges();
            if (cc > 0)
            {
                res = "Success";
                var fe = (from s in db.FE_Master_Personal where s.Id == TF.FE_ID select s).FirstOrDefault();
                fename = fe.First_Name + " " + fe.Last_Name;
                var tic = (from s in db.Ticket where s.Id == Ticket select s).FirstOrDefault();
                string oem = (from s in db.HeaderDescription where s.id == tic.OEM select s.header_description).FirstOrDefault();
                var callbackUrl = Url.Action("TicketSelection", "Transaction", new { Id = TF.Id }, protocol: Request.Url.Scheme);
                string body = utlity.FESelection(fe.First_Name + " " + fe.Last_Name , tic.Street_Address,tic.Dispatch_Date.Value.ToString("dd MMM yyy HH':'mm tt"),tic.Job_Description,oem, callbackUrl,tic.Ticket_No);
                utlity.sendmail(fe.Email, "Request for onsite/call out/intervention for Inwinteck Ticket No :" + tic.Ticket_No , body, mailer);
            }

            return Json(new { res, fename }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFETicketStatus(int Ticket)
        {
            string res = "";
            List<FETicket> PZ = new List<FETicket>();
            PZ = (from s in db.Ticket_FE_Selection join a in db.FE_Master_Personal  on s.FE_ID equals a.Id
                  where s.Ticket_no == Ticket select new FETicket { FE_ID = a.Id, Name = a.First_Name + " " + a.Last_Name , Status = s.Status , Sent = s.CreatedOn.ToString(), Remark = s.Remark , Request = s.ModifiedOn.ToString()}).ToList();
            res = "Success";
            return Json(new { res, PZ }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PartSaveData(string[] empdata)
        {
            List<Part_Ticket_Data> LPTD = new List<Part_Ticket_Data>();
           
            string res = "";
           Part_Ticket_Data PTD = new Part_Ticket_Data();

            //Loop and insert records.
            foreach (Part_Ticket_Data iv in LPTD)
            {
                PTD.Ticket_No = iv.Ticket_No;
                PTD.Make_Model = iv.Make_Model;
                PTD.Part_type = iv.Part_type;
                PTD.Serial_No = iv.Serial_No;
                PTD.Part_Description = iv.Part_Description;
                PTD.CreatedBy = User.Identity.GetUserId();
                PTD.CreatedOn = DateTime.Now;
                db.Part_Ticket_Data.Add(PTD);
                db.SaveChanges();
            }
           
                res = "Success";
           
            return Json(new { res }, JsonRequestBehavior.AllowGet);
        }

    }
}