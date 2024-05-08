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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult IndexAdmin()
        {
            List<TicketListMont> tlm = new List<TicketListMont>();
            List<FEtListMonth> fem = new List<FEtListMonth>();
            List<EUListMonth> eum = new List<EUListMonth>();
            List<CountryList> county = new List<CountryList>();
            List<TicketDetailsDash> TDO = new List<TicketDetailsDash>();
            List<TicketDetailsDash> TDS = new List<TicketDetailsDash>();
            ViewBag.FE = (from s in db.FE_Master_Personal select s).Count();
            ViewBag.Ticket = (from s in db.Ticket select s).Count();

            //ViewBag.TicketOpen = (from s in db.Ticket where s.Status == 18 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            //ViewBag.TicketClose = (from s in db.Ticket where s.Status == 20 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            //ViewBag.TicketDecline = (from s in db.Ticket where s.Status == 1362 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.TicketOpen = (from s in db.Ticket where s.Status == 18  select s).Count();
            ViewBag.TicketClose = (from s in db.Ticket where s.Status == 20  select s).Count();
            ViewBag.TicketCancel = (from s in db.Ticket where s.Status == 21 select s).Count();
            ViewBag.TicketDecline = (from s in db.Ticket where s.Status == 1362 select s).Count();

            ViewBag.FEMonth = (from s in db.FE_Master_Personal where s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.FEYear = (from s in db.FE_Master_Personal where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.FEToday = (from s in db.FE_Master_Personal where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.EUMonth = (from s in db.EU_Master where s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.EUYear = (from s in db.EU_Master where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.EUToday = (from s in db.EU_Master where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();


            ViewBag.EU = (from s in db.EU_Master select s).Count();

            string cnt = "0";
            string felm = "0";
            string felmdate = "0" ;
            string eulm = "0";

            tlm = db.Database.SqlQuery<TicketListMont>("getTicketListMonth").ToList();
            fem = db.Database.SqlQuery<FEtListMonth>("getFEtListMonth").ToList();
            eum = db.Database.SqlQuery<EUListMonth>("getEUListMonth").ToList();
            county = db.Database.SqlQuery<CountryList>("getCountryServed").ToList();
            TDO = db.Database.SqlQuery<TicketDetailsDash>("getTicketDetailsOpen").ToList();
            ViewBag.county = county;
            ViewBag.countycnt = county.Count();
            ViewBag.Citycnt = (from s in db.FE_Master_Personal where s.City != null select s).GroupBy(a=>a.City).Count();
            ViewBag.TDO = TDO;

            if (tlm.Count > 0)
            {
                for (var i = 0 ; i < tlm.Count; i++)
                {
                    cnt = cnt + "," + tlm[i].Ticket_Count;
                }

            }


            if (fem.Count > 0)
            {
                for (var i = 0; i < fem.Count; i++)
                {
                    felm = felm + "," + fem[i].Count;
                }

            }

            if (fem.Count > 0)
            {
                for (var i = 0; i < fem.Count; i++)
                {
                    felmdate = felmdate + ",'" + fem[i].Date + "'";
                }

            }

            if (felmdate != "0")
            {
                felmdate = felmdate.Remove(0, 2);

            }

            if (eum.Count > 0)
            {
                for (var i = 0; i < eum.Count; i++)
                {
                    eulm = eulm + "," + eum[i].Count;
                }

            }
            if (felm != "0")
            {
                felm = felm.Remove(0, 2);
            }
            ViewBag.Data = " data: ["+ cnt + "]";
            ViewBag.FEList = " data: [" + felm + "]";
            ViewBag.FEListDate = " labels: [" + felmdate + "],";
            ViewBag.EUList = " data: [" + eulm + "]";



            ViewBag.YTDLogged = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.MTDLogged = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month select s).Count();
            ViewBag.TDLogged = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.YTDOpen = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 18 select s).Count();
            ViewBag.MTDOpen = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 18 select s).Count();
            ViewBag.TDOpen = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 18 select s).Count();


            ViewBag.YTDFESelected = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1414 select s).Count();
            ViewBag.MTDFESelected = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1414 select s).Count();
            ViewBag.TDFESelected = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1414 select s).Count();


            ViewBag.YTDScheduled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1372 select s).Count();
            ViewBag.MTDScheduled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1372 select s).Count();
            ViewBag.TDScheduled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1372 select s).Count();

            ViewBag.YTDCheckedIn = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1415 select s).Count();
            ViewBag.MTDCheckedIn = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1415 select s).Count();
            ViewBag.TDCheckedIn = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1415 select s).Count();


            ViewBag.YTDClosed = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 20 select s).Count();
            ViewBag.MTDClosed = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 20 select s).Count();
            ViewBag.TDClosed = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 20 select s).Count();

            ViewBag.YTDCancelled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 21 select s).Count();
            ViewBag.MTDCancelled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 21 select s).Count();
            ViewBag.TDCancelled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 21 select s).Count();

            ViewBag.YTDDeclined = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1362 select s).Count();
            ViewBag.MTDDeclined = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1362 select s).Count();
            ViewBag.TDDeclined = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1362 select s).Count();

            ViewBag.YTDReschdeuled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 19 select s).Count();
            ViewBag.MTDReschdeuled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 19 select s).Count();
            ViewBag.TDReschdeuled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 19 select s).Count();

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexHelpDesk()
        {
            List<TicketListMont> tlm = new List<TicketListMont>();
            List<FEtListMonth> fem = new List<FEtListMonth>();
            List<EUListMonth> eum = new List<EUListMonth>();
            List<CountryList> county = new List<CountryList>();
            List<TicketDetailsDash> TDO = new List<TicketDetailsDash>();
            List<TicketDetailsDash> TDS = new List<TicketDetailsDash>();
            ViewBag.FE = (from s in db.FE_Master_Personal select s).Count();
            ViewBag.Ticket = (from s in db.Ticket select s).Count();

            //ViewBag.TicketOpen = (from s in db.Ticket where s.Status == 18 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            //ViewBag.TicketClose = (from s in db.Ticket where s.Status == 20 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            //ViewBag.TicketDecline = (from s in db.Ticket where s.Status == 1362 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.TicketOpen = (from s in db.Ticket where s.Status == 18 select s).Count();
            ViewBag.TicketClose = (from s in db.Ticket where s.Status == 20 select s).Count();
            ViewBag.TicketCancel = (from s in db.Ticket where s.Status == 21 select s).Count();
            ViewBag.TicketDecline = (from s in db.Ticket where s.Status == 1362 select s).Count();

            ViewBag.FEMonth = (from s in db.FE_Master_Personal where s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.FEYear = (from s in db.FE_Master_Personal where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.FEToday = (from s in db.FE_Master_Personal where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.EUMonth = (from s in db.EU_Master where s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.EUYear = (from s in db.EU_Master where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.EUToday = (from s in db.EU_Master where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();


            ViewBag.EU = (from s in db.EU_Master select s).Count();

            string cnt = "0";
            string felm = "0";
            string felmdate = "0";
            string eulm = "0";

            tlm = db.Database.SqlQuery<TicketListMont>("getTicketListMonth").ToList();
            fem = db.Database.SqlQuery<FEtListMonth>("getFEtListMonth").ToList();
            eum = db.Database.SqlQuery<EUListMonth>("getEUListMonth").ToList();
            county = db.Database.SqlQuery<CountryList>("getCountryServed").ToList();
            TDO = db.Database.SqlQuery<TicketDetailsDash>("getTicketDetailsOpen").ToList();
            ViewBag.county = county;
            ViewBag.countycnt = county.Count();
            ViewBag.Citycnt = (from s in db.FE_Master_Personal where s.City != null select s).GroupBy(a => a.City).Count();
            ViewBag.TDO = TDO;

            if (tlm.Count > 0)
            {
                for (var i = 0; i < tlm.Count; i++)
                {
                    cnt = cnt + "," + tlm[i].Ticket_Count;
                }

            }


            if (fem.Count > 0)
            {
                for (var i = 0; i < fem.Count; i++)
                {
                    felm = felm + "," + fem[i].Count;
                }

            }

            if (fem.Count > 0)
            {
                for (var i = 0; i < fem.Count; i++)
                {
                    felmdate = felmdate + ",'" + fem[i].Date + "'";
                }

            }

            if (felmdate != "0")
            {
                felmdate = felmdate.Remove(0, 2);

            }

            if (eum.Count > 0)
            {
                for (var i = 0; i < eum.Count; i++)
                {
                    eulm = eulm + "," + eum[i].Count;
                }

            }
            if (felm != "0")
            {
                felm = felm.Remove(0, 2);
            }
            ViewBag.Data = " data: [" + cnt + "]";
            ViewBag.FEList = " data: [" + felm + "]";
            ViewBag.FEListDate = " labels: [" + felmdate + "],";
            ViewBag.EUList = " data: [" + eulm + "]";



            ViewBag.YTDLogged = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.MTDLogged = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month select s).Count();
            ViewBag.TDLogged = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.YTDOpen = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 18 select s).Count();
            ViewBag.MTDOpen = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 18 select s).Count();
            ViewBag.TDOpen = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 18 select s).Count();


            ViewBag.YTDFESelected = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1414 select s).Count();
            ViewBag.MTDFESelected = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1414 select s).Count();
            ViewBag.TDFESelected = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1414 select s).Count();


            ViewBag.YTDScheduled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1372 select s).Count();
            ViewBag.MTDScheduled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1372 select s).Count();
            ViewBag.TDScheduled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1372 select s).Count();

            ViewBag.YTDCheckedIn = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1415 select s).Count();
            ViewBag.MTDCheckedIn = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1415 select s).Count();
            ViewBag.TDCheckedIn = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1415 select s).Count();


            ViewBag.YTDClosed = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 20 select s).Count();
            ViewBag.MTDClosed = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 20 select s).Count();
            ViewBag.TDClosed = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 20 select s).Count();

            ViewBag.YTDCancelled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 21 select s).Count();
            ViewBag.MTDCancelled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 21 select s).Count();
            ViewBag.TDCancelled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 21 select s).Count();

            ViewBag.YTDDeclined = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1362 select s).Count();
            ViewBag.MTDDeclined = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1362 select s).Count();
            ViewBag.TDDeclined = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1362 select s).Count();

            ViewBag.YTDReschdeuled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 19 select s).Count();
            ViewBag.MTDReschdeuled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 19 select s).Count();
            ViewBag.TDReschdeuled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 19 select s).Count();

            return View();
        }


        public ActionResult IndexSrHelpDesk()
        {
            List<TicketListMont> tlm = new List<TicketListMont>();
            List<FEtListMonth> fem = new List<FEtListMonth>();
            List<EUListMonth> eum = new List<EUListMonth>();
            List<CountryList> county = new List<CountryList>();
            List<TicketDetailsDash> TDO = new List<TicketDetailsDash>();
            List<TicketDetailsDash> TDS = new List<TicketDetailsDash>();
            ViewBag.FE = (from s in db.FE_Master_Personal select s).Count();
            ViewBag.Ticket = (from s in db.Ticket select s).Count();

            //ViewBag.TicketOpen = (from s in db.Ticket where s.Status == 18 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            //ViewBag.TicketClose = (from s in db.Ticket where s.Status == 20 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            //ViewBag.TicketDecline = (from s in db.Ticket where s.Status == 1362 && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.TicketOpen = (from s in db.Ticket where s.Status == 18 select s).Count();
            ViewBag.TicketClose = (from s in db.Ticket where s.Status == 20 select s).Count();
            ViewBag.TicketCancel = (from s in db.Ticket where s.Status == 21 select s).Count();
            ViewBag.TicketDecline = (from s in db.Ticket where s.Status == 1362 select s).Count();

            ViewBag.FEMonth = (from s in db.FE_Master_Personal where s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.FEYear = (from s in db.FE_Master_Personal where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.FEToday = (from s in db.FE_Master_Personal where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.EUMonth = (from s in db.EU_Master where s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.EUYear = (from s in db.EU_Master where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.EUToday = (from s in db.EU_Master where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();


            ViewBag.EU = (from s in db.EU_Master select s).Count();

            string cnt = "0";
            string felm = "0";
            string felmdate = "0";
            string eulm = "0";

            tlm = db.Database.SqlQuery<TicketListMont>("getTicketListMonth").ToList();
            fem = db.Database.SqlQuery<FEtListMonth>("getFEtListMonth").ToList();
            eum = db.Database.SqlQuery<EUListMonth>("getEUListMonth").ToList();
            county = db.Database.SqlQuery<CountryList>("getCountryServed").ToList();
            TDO = db.Database.SqlQuery<TicketDetailsDash>("getTicketDetailsOpen").ToList();
            ViewBag.county = county;
            ViewBag.countycnt = county.Count();
            ViewBag.Citycnt = (from s in db.FE_Master_Personal where s.City != null select s).GroupBy(a => a.City).Count();
            ViewBag.TDO = TDO;

            if (tlm.Count > 0)
            {
                for (var i = 0; i < tlm.Count; i++)
                {
                    cnt = cnt + "," + tlm[i].Ticket_Count;
                }

            }


            if (fem.Count > 0)
            {
                for (var i = 0; i < fem.Count; i++)
                {
                    felm = felm + "," + fem[i].Count;
                }

            }

            if (fem.Count > 0)
            {
                for (var i = 0; i < fem.Count; i++)
                {
                    felmdate = felmdate + ",'" + fem[i].Date + "'";
                }

            }

            if (felmdate != "0")
            {
                felmdate = felmdate.Remove(0, 2);

            }

            if (eum.Count > 0)
            {
                for (var i = 0; i < eum.Count; i++)
                {
                    eulm = eulm + "," + eum[i].Count;
                }

            }
            if (felm != "0")
            {
                felm = felm.Remove(0, 2);
            }
            ViewBag.Data = " data: [" + cnt + "]";
            ViewBag.FEList = " data: [" + felm + "]";
            ViewBag.FEListDate = " labels: [" + felmdate + "],";
            ViewBag.EUList = " data: [" + eulm + "]";



            ViewBag.YTDLogged = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year select s).Count();
            ViewBag.MTDLogged = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month select s).Count();
            ViewBag.TDLogged = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year select s).Count();

            ViewBag.YTDOpen = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 18 select s).Count();
            ViewBag.MTDOpen = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 18 select s).Count();
            ViewBag.TDOpen = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 18 select s).Count();


            ViewBag.YTDFESelected = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1414 select s).Count();
            ViewBag.MTDFESelected = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1414 select s).Count();
            ViewBag.TDFESelected = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1414 select s).Count();


            ViewBag.YTDScheduled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1372 select s).Count();
            ViewBag.MTDScheduled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1372 select s).Count();
            ViewBag.TDScheduled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1372 select s).Count();

            ViewBag.YTDCheckedIn = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1415 select s).Count();
            ViewBag.MTDCheckedIn = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1415 select s).Count();
            ViewBag.TDCheckedIn = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1415 select s).Count();


            ViewBag.YTDClosed = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 20 select s).Count();
            ViewBag.MTDClosed = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 20 select s).Count();
            ViewBag.TDClosed = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 20 select s).Count();

            ViewBag.YTDCancelled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 21 select s).Count();
            ViewBag.MTDCancelled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 21 select s).Count();
            ViewBag.TDCancelled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 21 select s).Count();

            ViewBag.YTDDeclined = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1362 select s).Count();
            ViewBag.MTDDeclined = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 1362 select s).Count();
            ViewBag.TDDeclined = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 1362 select s).Count();

            ViewBag.YTDReschdeuled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.Status == 19 select s).Count();
            ViewBag.MTDReschdeuled = (from s in db.Ticket where s.CreatedOn.Year == DateTime.Now.Year && s.CreatedOn.Month == DateTime.Now.Month && s.Status == 19 select s).Count();
            ViewBag.TDReschdeuled = (from s in db.Ticket where s.CreatedOn.Day == DateTime.Now.Day && s.CreatedOn.Month == DateTime.Now.Month && s.CreatedOn.Year == DateTime.Now.Year && s.Status == 19 select s).Count();

            return View();
        }
        public ActionResult UnAuthorisedAccess()
        {
            return View();
        }

    
    }
}