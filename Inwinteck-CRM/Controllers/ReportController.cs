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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;
using Inwinteck_CRM.Models;

namespace Inwinteck_CRM.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext db = new ApplicationDbContext();

        public ReportController()
        {
        }

        public ReportController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult FEReport(int pageNo = 0, string Country = "", string City = "", DateTime? dorFrom = null, DateTime? dorTo = null, string feFristName = "", string feLastName = "")
        {
            int pageSize = 10;
            int pgc = 0;
            int pageCount = 0;
            List<FE_Master_Personal> li = new List<FE_Master_Personal>();


            if (Country != "" || City != "" || dorFrom != null || dorTo != null || feFristName != "" || feLastName != "")
            {
                if (Country != "")
                {
                    if (City != "" && dorFrom != null && dorTo != null && feFristName != "" && feLastName != "")
                    {
                        
                        pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country) 
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo && x.First_Name.Contains(feFristName) && 
                        x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).Count();
                        pageCount = pgc / pageSize;
                        li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo && x.First_Name.Contains(feFristName) &&
                        x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.City = City;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                        ViewBag.feFristName = feFristName;
                        ViewBag.feLastName = feLastName;
                    }
                    else if (City != "" && dorFrom != null && dorTo != null && feFristName != "")
                    {

                        pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo && x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).Count();
                        pageCount = pgc / pageSize;
                        li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo && x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.City = City;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                        ViewBag.feFristName = feFristName;
                    }
                    else if (City != "" && dorFrom != null && dorTo != null)
                    {

                        pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo).OrderByDescending(x => x.Id).Count();
                        pageCount = pgc / pageSize;
                        li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.City = City;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                    }
                    else if (City != "")
                    {

                        pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City)).OrderByDescending(x => x.Id).Count();
                        pageCount = pgc / pageSize;
                        li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.City = City;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                    }
                    else if (dorFrom != null)
                    {

                        pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo).OrderByDescending(x => x.Id).Count();
                        pageCount = pgc / pageSize;
                        li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                    }
                    else if (feFristName != "")
                    {

                        pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).Count();
                        pageCount = pgc / pageSize;
                        li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.feFristName = feFristName;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                    }
                    else if (feLastName != "")
                    {

                        pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).Count();
                        pageCount = pgc / pageSize;
                        li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.feLastName = feLastName;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                    }
                    else
                    {
                        pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)).OrderByDescending(x => x.Id).Count();
                        pageCount = pgc / pageSize;
                        li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                    }
                   
                }
                else if (City != "")
                {
                    pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.City.Contains(City)).OrderByDescending(x => x.Id).Count();
                    pageCount = pgc / pageSize;
                    li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.City.Contains(City)).OrderByDescending(x => x.Id).ToList();
                    ViewBag.Country = Country;
                    ViewBag.City = City;
                    ViewBag.dorFrom = dorFrom.ToString();
                    ViewBag.dorTo = dorTo.ToString();
                    ViewBag.feFristName = feFristName;
                    ViewBag.feLastName = feLastName;
                }
                else if (dorFrom != null)
                {
                    pgc = (from s in db.FE_Master_Personal where s.CreatedOn >= dorFrom && s.CreatedOn <= dorTo orderby s.Id descending select s).OrderByDescending(x => x.Id).Count();
                    pageCount = pgc / pageSize;
                    li = (from s in db.FE_Master_Personal where s.CreatedOn >= dorFrom && s.CreatedOn <= dorTo orderby s.Id descending select s).OrderByDescending(x => x.Id).ToList();
                    ViewBag.Country = Country;
                    ViewBag.City = City;
                    ViewBag.dorFrom = dorFrom.ToString();
                    ViewBag.dorTo = dorTo.ToString();
                    ViewBag.feFristName = feFristName;
                    ViewBag.feLastName = feLastName;
                }
                else if (feFristName != "")
                {
                    pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).Count();
                    pageCount = pgc / pageSize;
                    li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).ToList();
                    ViewBag.Country = Country;
                    ViewBag.City = City;
                    ViewBag.dorFrom = dorFrom.ToString();
                    ViewBag.dorTo = dorTo.ToString();
                    ViewBag.feFristName = feFristName;
                    ViewBag.feLastName = feLastName;
                }
                else if (feLastName != "")
                {
                    pgc = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).Count();
                    pageCount = pgc / pageSize;
                    li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).ToList();
                    ViewBag.Country = Country;
                    ViewBag.City = City;
                    ViewBag.dorFrom = dorFrom.ToString();
                    ViewBag.dorTo = dorTo.ToString();
                    ViewBag.feFristName = feFristName;
                    ViewBag.feLastName = feLastName;
                }
            }
            else
            {
                pgc = db.FE_Master_Personal.Count();
                pageCount = pgc / pageSize;

                li = (from s in db.FE_Master_Personal orderby s.Id descending select s).Skip(pageNo * pageSize).Take(pageSize).ToList();
                ViewBag.pageNo = pageNo;
                ViewBag.pageCount = pageCount;
            }
          
            return View(li);
        }

        public ActionResult exportFEReport(int pageNo = 0, string Country = "", string City = "", DateTime? dorFrom = null, DateTime? dorTo = null, string feFristName = "", string feLastName = "")
        {
            var gv = new GridView();
            List<FE_Master_Personal> li = new List<FE_Master_Personal>();

            if (Country != "" || City != "" || dorFrom != null || dorTo != null || feFristName != "" || feLastName != "")
            {
                if (Country != "")
                {
                    if (City != "" && dorFrom != null && dorTo != null && feFristName != "" && feLastName != "")
                    {

                        gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo && x.First_Name.Contains(feFristName) &&
                        x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.City = City;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                        ViewBag.feFristName = feFristName;
                        ViewBag.feLastName = feLastName;
                    }
                    else if (City != "" && dorFrom != null && dorTo != null && feFristName != "")
                    {
                        gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo && x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.City = City;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                        ViewBag.feFristName = feFristName;
                    }
                    else if (City != "" && dorFrom != null && dorTo != null)
                    {
                        gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City) && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.City = City;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                    }
                    else if (City != "")
                    {

                        gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.City.Contains(City)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.City = City;
                    }
                    else if (dorFrom != null)
                    {

                        gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.CreatedOn >= dorFrom && x.CreatedOn <= dorTo).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.dorFrom = dorFrom.ToString();
                        ViewBag.dorTo = dorTo.ToString();
                    }
                    else if (feFristName != "")
                    {

                        gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.feFristName = feFristName;
                    }
                    else if (feLastName != "")
                    {
                        gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)
                        && x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                        ViewBag.feLastName = feLastName;
                    }
                    else
                    {
                        gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Country.Contains(Country)).OrderByDescending(x => x.Id).ToList();
                        ViewBag.Country = Country;
                    }

                }
                else if (City != "")
                {
                    gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.City.Contains(City)).OrderByDescending(x => x.Id).ToList();
                    ViewBag.Country = Country;
                    ViewBag.City = City;
                    ViewBag.dorFrom = dorFrom.ToString();
                    ViewBag.dorTo = dorTo.ToString();
                    ViewBag.feFristName = feFristName;
                    ViewBag.feLastName = feLastName;
                }
                else if (dorFrom != null)
                {
                    gv.DataSource = (from s in db.FE_Master_Personal where s.CreatedOn >= dorFrom && s.CreatedOn <= dorTo orderby s.Id descending select s).OrderByDescending(x => x.Id).ToList();
                    ViewBag.Country = Country;
                    ViewBag.City = City;
                    ViewBag.dorFrom = dorFrom.ToString();
                    ViewBag.dorTo = dorTo.ToString();
                    ViewBag.feFristName = feFristName;
                    ViewBag.feLastName = feLastName;
                }
                else if (feFristName != "")
                {
                    gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.First_Name.Contains(feFristName)).OrderByDescending(x => x.Id).ToList();
                    ViewBag.Country = Country;
                    ViewBag.City = City;
                    ViewBag.dorFrom = dorFrom.ToString();
                    ViewBag.dorTo = dorTo.ToString();
                    ViewBag.feFristName = feFristName;
                    ViewBag.feLastName = feLastName;
                }
                else if (feLastName != "")
                {
                    gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).Where(x => x.Last_Name.Contains(feLastName)).OrderByDescending(x => x.Id).ToList();
                    ViewBag.Country = Country;
                    ViewBag.City = City;
                    ViewBag.dorFrom = dorFrom.ToString();
                    ViewBag.dorTo = dorTo.ToString();
                    ViewBag.feFristName = feFristName;
                    ViewBag.feLastName = feLastName;
                }
            }
            else
            {

                gv.DataSource = (from s in db.FE_Master_Personal orderby s.Id descending select s).ToList();
            }
            
            
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=FEMaster.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("FEReport", "Report" ,new { Country = Country,City=City,dorFrom=dorFrom,dorTo=dorTo,feFristName=feFristName,feLastName=feLastName});
        }
    }
}