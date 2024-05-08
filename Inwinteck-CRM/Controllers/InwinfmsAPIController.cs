using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Inwinteck_CRM.Models;
using System.IO;
using System.Web.Http.Cors;
using System.Web.Hosting;
using System.Web;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Xml.Linq;
using Inwinteck_CRM.ApiAuthorization;
using System.Threading;

namespace Inwinteck_CRM.Controllers
{

    [RoutePrefix("api/InwinfmsAPI")]
    public class InwinfmsAPIController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        string url = "https://fms.inwinteck.com/Upload/";
        string mailer = "Support<support@inwinteck.com>";
        [HttpPost]
        [InwinAuthentication]
        public HttpResponseMessage CreateTicket(apiTicket ticket)
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;

            generalresponse gr = new generalresponse();


            if (userName != "")
            {

                Ticket_Stagging log = new Ticket_Stagging();
                log.Email_Subject = ticket.Email_Subject;
                log.Case_No = ticket.Case_No;
                log.EU_Name = ticket.EU_Name;
                log.Street_Address = ticket.Street_Address;
                log.City = ticket.City;
                log.Status = "Added";
                log.Country = ticket.Country;
                log.Zip_Pin_Code = ticket.Zip_Pin_Code;
                log.CreatedOn = DateTime.Now;
                log.Dispatch_Date = ticket.Dispatch_Date;
                log.EU_Contact = ticket.EU_Contact;
                log.Job_Description = ticket.Job_Description;
                log.OEM = ticket.OEM;
                log.Site_Name = ticket.Site_Name;
                log.State = ticket.State;
                log.Type = "CreateTicket";
                log.EU_Email = ticket.EU_Email;
                log.Certification_Name = ticket.Certification_Name;
                db.Ticket_Stagging.Add(log);
                int res = db.SaveChanges();


                Ticket_Stagging ts = new Ticket_Stagging();
                ts = (from s in db.Ticket_Stagging where s.ID == log.ID select s).FirstOrDefault();

                try
                {
                    Ticket sa = new Ticket();
                    if (res > 0)
                    {
                        sa.Case_No = ticket.Case_No;
                        sa.EU_Name = ticket.EU_Name;
                        sa.Street_Address = ticket.Street_Address;
                        sa.City = ticket.City;
                        sa.EU_Email = ticket.EU_Email;
                        sa.Country = ticket.Country;
                        sa.Zip_Pin_Code = ticket.Zip_Pin_Code;
                        sa.CreatedOn = DateTime.Now;
                        sa.Dispatch_Date = ticket.Dispatch_Date;
                        sa.EU_Contact = ticket.EU_Contact;
                        sa.Job_Description = ticket.Job_Description;
                        sa.OEM = (from s in db.HeaderDescription where s.header_id == 4 && s.header_description.Contains(ticket.OEM) select s.id).FirstOrDefault();
                        sa.Site_Name = ticket.Site_Name;
                        sa.State = ticket.State;
                        sa.Ticket_No = utlity.CheckTicketNo(4);
                        sa.Status = 18;
                        sa.latitude = "0";
                        sa.longitude = "0";
                        sa.Certification_Need = 0;
                        sa.EU_ID = 4;
                        sa.CreatedBy = "27b3bb6b-9331-4470-8026-d02e9125bab4";
                        sa.CreatedOn = DateTime.Now;
                        sa.pregame_detail = "1. Please reach the site on time Important.  2. Please follow the formal dress Code.  3. Make sure you carry all desired tools onsite as requested.  4. Phone should be completely charged.  5. Engineer has to share system picture with serial number before commencing the work.  6. If the parts are damaged or any fault in the system made by the end customer, you should inform us immediately  or before leaving the site.  7. We need to verify the serial number of the device prior to starting any repairs and we need to get the customer's approval before starting any repairs.  8. Once activity is completed, make sure the site is left neat & clean always.  Support Team needs to confirm the repair with the customer prior to your release.  9. If any issue's or any doubt, please call.  10. Be polite, patient and do not use any profanity language while onsite.  11. If any issue related to payment, extra hours, pricing, or any other issues related to payments please do not communicate or discuss with the client, do not mention on WhatsApp group  12. Please carry your Photo ID along with you and face mask for Covid 19 requirements…  13. Technician also requires to bring own coveralls, face mask, face shield, gloves.  14. If any faulty part needs to be shipped please write a mail aparna.toraskar@inwinteck.com with all details such as Serial No and Return Label details with address of location from where parts to be picked up.";
                        db.Ticket.Add(sa);
                        int ress = db.SaveChanges();
                        if (ress > 0)
                        {
                            Ticket_History th = new Ticket_History();
                            th.Ticket_no = sa.Id;
                            th.Comments = "Ticket Created from source support api";
                            th.WhatsappChat = sa.WhatsappChat;
                            th.FE_ID = sa.FE_ID;
                            th.status = 18;
                            th.CreatedBy = User.Identity.GetUserId();
                            th.CreatedOn = DateTime.Now;
                            db.Ticket_History.Add(th);
                            db.SaveChanges();


                            if (ts != null)
                            {
                                ts.Status = "Sucess";
                                ts.Ticket_No = sa.Ticket_No;
                                ts.Error_Msg = "NA";
                                db.Entry(ts).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }




                        Ticket_Email TE = new Ticket_Email();
                        TE.Ticket_no = sa.Id;
                        TE.Email = sa.EU_Email;
                     
                        if (ticket.Email_Subject != "")
                        {
                            TE.Email_Subject = ticket.Email_Subject;

                        }
                        else
                        {
                            TE.Email_Subject = "Ticket Created :: Ticket Number : " + sa.Ticket_No;
                        }
                        TE.CreatedBy = User.Identity.GetUserId();
                        TE.CreatedOn = DateTime.Now;
                        db.Ticket_Email.Add(TE);
                        db.SaveChanges();

                        Ticket_EU_Detail TSEU = new Ticket_EU_Detail();

                        TSEU.Ticket_no = sa.Id;
                        TSEU.EU_Name = sa.EU_Name;
                        TSEU.EU_Email = sa.EU_Email;
                        TSEU.EU_Contact = sa.EU_Contact;
                        TSEU.CreatedBy = sa.CreatedBy;
                        TSEU.CreatedOn = DateTime.Now;
                        db.Ticket_EU_Detail.Add(TSEU);
                        db.SaveChanges();
                        string eu = (from s in db.EU_Master where s.Id == sa.EU_ID select s.Customer_Name).FirstOrDefault();
                        string body = utlity.TicketGenerated(eu, sa.Ticket_No, sa.Case_No, sa.Site_Name, sa.Street_Address, sa.Dispatch_Date.ToString(), sa.Job_Description);
                        utlity.sendmail(sa.EU_Email, TE.Email_Subject, body, mailer);

                    }

                    gr.statuscode = 200;
                    gr.statusmessage = "Ticket Created : Number :" + sa.Ticket_No;

                }
                catch (Exception ex)
                {
                    gr.statuscode = 201;
                    gr.statusmessage = "Ticket Not Created !! " + ex.InnerException.ToString();

                    if (ts != null)
                    {
                        ts.Status = "Error";
                        ts.Error_Msg = ex.InnerException.ToString();
                        db.Entry(ts).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                gr.statuscode = 401;
                gr.statusmessage = "Unauthorised Access.";

                return Request.CreateResponse(HttpStatusCode.Unauthorized, gr);

            }

            return Request.CreateResponse(HttpStatusCode.OK, gr);
        }

        [HttpPost]
        [InwinAuthentication]
        public HttpResponseMessage UpdateTicket(apiTicketUpdate ticket)
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;

            generalresponse gr = new generalresponse();


            if (userName != "")
            {

                Ticket_Stagging log = new Ticket_Stagging();
                log.Case_No = ticket.Case_No;
                log.EU_Name = ticket.EU_Name;
                log.Street_Address = ticket.Street_Address;
                log.City = ticket.City;
                log.Status = "Added";
                log.Country = ticket.Country;
                log.Zip_Pin_Code = ticket.Zip_Pin_Code;
                log.CreatedOn = DateTime.Now;
                log.Dispatch_Date = ticket.Dispatch_Date;
                log.EU_Contact = ticket.EU_Contact;
                log.Job_Description = ticket.Job_Description;
                log.OEM = ticket.OEM;
                log.Site_Name = ticket.Site_Name;
                log.State = ticket.State;
                log.Type = "UpdateTicket";
                log.EU_Email = ticket.EU_Email;
                log.Ticket_No = ticket.Ticket_No;
                log.Certification_Name = ticket.Certification_Name;

                db.Ticket_Stagging.Add(log);
                int res = db.SaveChanges();


                Ticket_Stagging ts = new Ticket_Stagging();
                ts = (from s in db.Ticket_Stagging where s.ID == log.ID select s).FirstOrDefault();
                Ticket sa = new Ticket();
                sa = (from s in db.Ticket where s.Ticket_No == log.Ticket_No select s).FirstOrDefault();
                try
                {

                    if (res > 0)
                    {

                        sa.Case_No = ticket.Case_No;
                        sa.EU_Name = ticket.EU_Name;
                        sa.Street_Address = ticket.Street_Address;
                        sa.City = ticket.City;
                        sa.EU_Email = ticket.EU_Email;
                        sa.Country = ticket.Country;
                        sa.Zip_Pin_Code = ticket.Zip_Pin_Code;
                        sa.CreatedOn = DateTime.Now;
                        sa.Dispatch_Date = ticket.Dispatch_Date;
                        sa.EU_Contact = ticket.EU_Contact;
                        sa.Job_Description = ticket.Job_Description;
                        sa.OEM = (from s in db.HeaderDescription where s.header_id == 4 && s.header_description.Contains(ticket.OEM) select s.id).FirstOrDefault();
                        sa.Site_Name = ticket.Site_Name;
                        sa.State = ticket.State;
                        sa.ModifiedBy = "27b3bb6b-9331-4470-8026-d02e9125bab4";
                        sa.ModifiedOn = DateTime.Now;
                        db.Entry(sa).State = System.Data.Entity.EntityState.Modified;
                        int ress = db.SaveChanges();
                        if (ress > 0)
                        {
                            Ticket_History th = new Ticket_History();
                            th.Ticket_no = sa.Id;
                            th.Comments = ticket.Comments;
                            th.WhatsappChat = sa.WhatsappChat;
                            th.FE_ID = sa.FE_ID;
                            th.status = sa.Status;
                            th.CreatedBy = User.Identity.GetUserId();
                            th.CreatedOn = DateTime.Now;
                            db.Ticket_History.Add(th);
                            db.SaveChanges();

                            if (ts != null)
                            {
                                ts.Status = "Sucess";
                                ts.Ticket_No = sa.Ticket_No;
                                ts.Error_Msg = "NA";
                                db.Entry(ts).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }

                    }

                    gr.statuscode = 200;
                    gr.statusmessage = "Ticket Updated : Number :" + sa.Ticket_No;

                }
                catch (Exception ex)
                {
                    gr.statuscode = 201;
                    gr.statusmessage = "Ticket Not Updated !! " + ex.InnerException.ToString();

                    if (ts != null)
                    {
                        ts.Status = "Error";
                        ts.Error_Msg = ex.InnerException.ToString();
                        db.Entry(ts).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                gr.statuscode = 401;
                gr.statusmessage = "Unauthorised Access.";

                return Request.CreateResponse(HttpStatusCode.Unauthorized, gr);

            }

            return Request.CreateResponse(HttpStatusCode.OK, gr);
        }

    }
}