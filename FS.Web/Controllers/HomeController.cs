using DataAccess;
using FS.Customer;
using FS.Web.Models;
using Infarstructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FS.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //return RedirectToAction("UderConstruction");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        public ActionResult Info()
        {
           

            return View();
        }

        public ActionResult CheckList()
        {
            var model = new FS.Customer.CheckList();
            if(CurrentUser != null)
                model = FS.Customer.CheckList.GetCheckList(CurrentUser.UserId);

            return View(model);
        }

        public ActionResult UderConstruction()
        {
            ViewBag.Message = "Under construction";

            return View();
        }
        

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult FileList(int id)
        {
            Session["Reason"] = id;
            var model = new DocumentModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveFileList( string[] FileName)
        {

            Session["FileName"] = FileName;
            //ViewData["Documents"] =
            return PartialView("_Register", new UserLogin());
        }

        [HttpPost]
        public ActionResult FileList(CustomerDocument model, string[] FileName)
        {

            Session["FileName"] = FileName;
            //ViewData["Documents"] =
            return RedirectToAction("Register", "UserLogin");
        }
        public ActionResult Reason()
        {
            return View(new DocumentModel());
        }
        [HttpPost]
        public ActionResult Reason(FormCollection f)
        {

            Session["Reason"] = f["Reason"];
            var model = new DocumentModel();

            return RedirectToAction("FileList", new { Id = f["Reason"] });
        }

        public ActionResult CreateUser()
        {


            return View("CreateAccount", new UserInfoModel());



        }
        [HttpPost]
        public ActionResult CreateUser(UserInfoModel model)
        {
            var reqx = new Regex(@"^.*(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[\d])(?=.*[\w]).*$");

            if(!reqx.IsMatch(model.Password))
            {
                ViewData["Error"] = "Password should be 8 charactors long, with atleast one lower case, one upper case, one digit and one special charactor";
                return View("CreateAccount", model);
            }


            if (ModelState.IsValid) 
            {


                if (!Customer.UserLogin.GetUserByEmailAndUserName(model.UserName, "-"))
                {



                    var loginModel = new Customer.UserLogin();
                    loginModel.Address1 = "";
                    loginModel.Address2 = "";
                    loginModel.City = "";
                    loginModel.Email = "";
                    loginModel.Fax = "";
                    loginModel.FirstName = "";
                    loginModel.LastName = "";
                    loginModel.Mobile = "";
                    loginModel.Password = Infarstructure.Common.Encrypt(model.Password);
                    loginModel.UserName = model.UserName;
                    loginModel.PicturePath = "";
                    loginModel.Zip = "";
                    loginModel.StateId = 0;
                    loginModel = Persistance.Create<Customer.UserLogin>(loginModel);






                    var p = new PrincipalProvider(HttpContext.User.Identity);
                    p.AuthenticateUser(model.UserName, model.Password);
                    if (p.User == null)
                        p.AuthenticateUser(model.UserName, Infarstructure.Common.Encrypt(model.Password));

                    if (p.User != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);

                        System.Web.HttpContext.Current.User = p;

                        //return new RedirectResult("~/UserLogin/MyAccount");  //RedirectToAction("Index", "MyController");
                    }

                }
                else
                {
                    ViewData["Error"] = "User with same user name or email exists. Please login to system in login window";
                    return View("CreateAccount", model);
                }




                return RedirectToAction("Register", "UserLogin");
            }
            return View("CreateAccount", model);

           
        }

       
    }
}
