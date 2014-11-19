using FS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using Infarstructure;
using System.Web.Security;
using FS.Customer;
using System.IO;
namespace FS.Web.Controllers
{
    public class UserLoginController : BaseController
    {
        //
        // GET: /UserLogin/

        public UserLoginController()
            : base()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            var model = new CustomerModel();
            ViewData["StateList"] = model.StateList;
            model.UserLogin = new UserLogin();
            if(CurrentUser != null)
                model.UserLogin =  Persistance.ResolveKey<UserLogin>(CurrentUser.UserId);
            return View(model);
        }

        [Authorize]
        public ActionResult EditUser()
        {
            var model = new CustomerModel();
            model.UserLogin = Persistance.ResolveKey<UserLogin>(CurrentUser.UserId);
            ViewData["StateList"] = model.StateList;
            return View(model.UserLogin);
        }
         [HttpPost]
         [Authorize]
        public ActionResult EditUser(Customer.UserLogin model)
        {
            //if (ModelState.IsValid)
            //{
                
                if (Customer.UserLogin.GetUserByEmailAndUserName(model.UserName, model.Email))
                {
                    var filename = "";

                    var user = Persistance.ResolveKey<UserLogin>(model.UserId);
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        filename = filename = Path.GetFileName(Request.Files[0].FileName);
                    }
                    if(!string.IsNullOrWhiteSpace(filename))
                        model.PicturePath = filename;
                    else if(user !=null)
                        model.PicturePath = user.PicturePath; 
                    //model.Password = Infarstructure.Common.Encrypt(model.Password);
                    model = Persistance.Update<Customer.UserLogin>(model);

                    


                    if (Request.Files != null && Request.Files.Count > 0 && !string.IsNullOrWhiteSpace(filename))
                    {
                        var dir = string.Format("{0}", model.UserId.ToString());
                        if (!Directory.Exists(Server.MapPath("//" + PictureFilePath + "//" + dir)))
                        {
                            Directory.CreateDirectory(Server.MapPath("//" + PictureFilePath + "//" + dir));


                        }

                        Request.Files[0].SaveAs(Server.MapPath("//" + PictureFilePath + "//" + dir + "//" + filename));
                    }

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
                    ViewData["Error"] = "Cannot find user";
                }
            //}
            var modelc = new CustomerModel();
            modelc.UserLogin = model;
            ViewData["StateList"] = modelc.StateList;
             var cntPerList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
             if(cntPerList!= null && cntPerList.Count>0)
                 return new RedirectResult("~/Account/ContactPersonEdit");

            return new RedirectResult("~/Account/ContactPersonView");
        }
        [HttpPost]
        public ActionResult Register(Customer.UserLogin model)
        {
           // var isvalid = ModelState.IsValidField("Password");
           // var othesValid = ModelState.Where(p => p.Key != "Password");
            
            //if (ModelState.IsValidField("Password"))
            //{
                //if (model.UserName != model.Email)
                //{
                //    ViewData["Error"] = "Confirm and";
                //    return View(model);
                //}
                if (true || !Customer.UserLogin.GetUserByEmailAndUserName(model.UserName, model.Email))
                {
                    var filename = "";


                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        filename = filename = Path.GetFileName(Request.Files[0].FileName);
                    }
                    model.PicturePath = filename;

                    model.Password = Infarstructure.Common.Encrypt(model.Password);
                    if(model.UserId == 0)
                        model = Persistance.Create<Customer.UserLogin>(model);
                    else model = Persistance.Update<Customer.UserLogin>(model);

                    if (Session["FileName"] != null)
                    {
                        var FileName = Session["FileName"] as string[];
                        var docmodel = new DocumentModel();
                        if (FileName != null)
                        {
                            foreach (string key in FileName)
                            {
                                var doc = new CustomerDocument();
                                doc.DocumentKey = key;
                                doc.DateUpload = DateTime.Now;
                                doc.UserId = model.UserId;
                                doc.DocumentName = "";
                                doc.FileName = "";


                                Persistance.Create<CustomerDocument>(doc);
                            }
                        }

                    }
                   

                    if (Request.Files != null && Request.Files.Count > 0 && !string.IsNullOrWhiteSpace(filename))
                    {
                        var dir = string.Format("{0}", model.UserId.ToString());
                        if (!Directory.Exists(Server.MapPath("//" + PictureFilePath + "//" + dir)))
                        {
                            Directory.CreateDirectory(Server.MapPath("//" + PictureFilePath + "//" + dir));


                        }

                        Request.Files[0].SaveAs(Server.MapPath("//" + PictureFilePath + "//" + dir + "//" + filename));
                    }

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
                }
            //}
            var modelc = new CustomerModel();
            modelc.UserLogin = model;
            ViewData["StateList"] = modelc.StateList;
            return RedirectToAction("ContactPersonView","Account",null);
        }

        [HttpPost]
        public ActionResult SaveUser(Customer.UserLogin model)
        {
            if (ModelState.IsValid)
            {
                //if (model.UserName != model.Email)
                //{
                //    ViewData["Error"] = "Confirm and";
                //    return View(model);
                //}
                if (!Customer.UserLogin.GetUserByEmailAndUserName(model.UserName, model.Email))
                {
                    model.Password = Infarstructure.Common.Encrypt(model.Password);
                    model = Persistance.Create<Customer.UserLogin>(model);

                    if (Session["FileName"] != null)
                    {
                        var FileName = Session["FileName"] as string[];
                        var docmodel = new DocumentModel();
                        if (FileName != null)
                        {
                            foreach (string key in FileName)
                            {
                                var doc = new CustomerDocument();
                                doc.DocumentKey = key;
                                doc.DateUpload = DateTime.Now;
                                doc.UserId = model.UserId;
                                doc.DocumentName = "";
                                doc.FileName = "";


                                Persistance.Create<CustomerDocument>(doc);
                            }
                        }

                    }

                    var filename = "";
                   

                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        filename = filename = Path.GetFileName(Request.Files[0].FileName);
                    }
                    model.PicturePath = filename;

                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.UserId.ToString());
                        if (!Directory.Exists(Server.MapPath("//" + PictureFilePath + "//" + dir)))
                        {
                            Directory.CreateDirectory(Server.MapPath("//" + PictureFilePath + "//" + dir));


                        }
                        if (!string.IsNullOrWhiteSpace(filename))
                            Request.Files[0].SaveAs(Server.MapPath("//" + PictureFilePath + "//" + dir + "//" + filename));
                    }

                    var p = new PrincipalProvider(HttpContext.User.Identity);
                    p.AuthenticateUser(model.UserName, model.Password);
                    if (p.User == null)
                        p.AuthenticateUser(model.UserName, Infarstructure.Common.Encrypt(model.Password));

                    if (p.User != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);

                        System.Web.HttpContext.Current.User = p;

                        return new RedirectResult("~/UserLogin/MyAccount");  //RedirectToAction("Index", "MyController");
                    }

                }
                else
                {
                    ViewData["Error"] = "User with same user name or email exists. Please login to system in login window";
                }
            }
            var modelc = new CustomerModel();
            modelc.UserLogin = model;
            return View(modelc);
        }

        [Authorize]
        public ActionResult MyAccount()
        {
            return View();
        }

    }
}
