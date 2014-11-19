using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Infarstructure;
using System.IO;
using DataAccess;
using FS.Customer;
using FS.Web.Models;
using FS.Common;

namespace FS.Web.Controllers
{

    [Authorize]
    public class AccountController : BaseController
    {
        
        public AccountController():base()
        {
           
        }

        //
        // GET: /Account/Login

      

        //
        // POST: /Account/Login
        public ActionResult CheckList()
        {
            var model = new FS.Customer.CheckList();
            if (CurrentUser != null)
                model = FS.Customer.CheckList.GetCheckList(CurrentUser.UserId);

            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserLogin model)
        {
            if (!string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password))
            {
                PrincipalProvider p = new PrincipalProvider(HttpContext.User.Identity);
                p.AuthenticateUser(model.UserName, model.Password);
                if(p.User==null)
                    p.AuthenticateUser(model.UserName, Infarstructure.Common.Encrypt(model.Password));

                if (p.User!=null)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName,false);
                    
                    System.Web.HttpContext.Current.User = p;
                    var checklistb = FS.Customer.CheckList.GetCheckList(p.User.UserId);
                    if(checklistb.CareGiver ==0)
                        return new RedirectResult("~/UserLogin/Register");
                    if (checklistb.CareReceiver == 0)
                        return new RedirectResult("~/Account/ContactPersonView");
                    else if (checklistb.Poa == 0)
                           return new RedirectResult("~/UploadCenter/SpecialDocument/1");
                    else if (checklistb.Hpoa == 0)
                        return new RedirectResult("~/UploadCenter/SpecialDocument/2");
                    else if (checklistb.Will == 0)
                        return new RedirectResult("~/UploadCenter/SpecialDocument/3");

                    return new RedirectResult("~/Account/ContactPerson");  //RedirectToAction("Index", "MyController");
                }
               
            }
            ViewData["Error"] = "Incorrect username or password";
            // If we got this far, something failed, redisplay form
            return View("LoginView",model);
        }

        [AllowAnonymous]
        public ActionResult LoginView()
        {
            UserLogin model = new UserLogin();
            // If we got this far, something failed, redisplay form
            return View( model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult GotoHome()
        {
            return RedirectToAction("Index", "Home");
        }
        

        public ActionResult ContactPerson()
        {

            var model = new CustomerModel();
            model.ContactPerson = new Customer.ContactPerson();
            ViewData["StateList"]  =model.StateList;
            ViewData["Relatives"] = model.Relatives;
            model.ContactPersonList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            model.ContactPersonList.ForEach(x => { x.RelationshipName = (ViewData["Relatives"] as List<SelectListItem>).FirstOrDefault(s => s.Value == x.Relationship.ToString()).Text; });
           
            return View(model);

        }
        [Authorize]
        public ActionResult ContactPersonView()
        {

            var model = new CustomerModel();
            model.ContactPerson = new Customer.ContactPerson();
            ViewData["StateList"] = model.StateList;
            ViewData["Relatives"] = model.Relatives;
            model.ContactPersonList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            model.ContactPersonList.ForEach(x => { x.RelationshipName = (ViewData["Relatives"] as List<SelectListItem>).FirstOrDefault(s => s.Value == x.Relationship.ToString()).Text; });
            model.ContactPerson.IsFromWizaed = true;
            return View(model);

        }

        [HttpPost]
        public ActionResult ContactPersonView(ContactPerson model)
        {
            return SaveContact(model);  
        }


        private ActionResult SaveContact(ContactPerson model)
        {
            var ssfrmWzrd = false;
            if (ModelState.IsValid)
            {
                ssfrmWzrd = model.IsFromWizaed;
                var filename = "";
                model.UserId = CurrentUser.UserId;
                model.Address2 = model.Address2 ?? "";
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    filename = filename = Path.GetFileName(Request.Files[0].FileName);
                }
                model.PicturePath = filename;
                if (model.ContactPersonId == 0)
                    model = Persistance.Create<ContactPerson>(model);
                else
                    model = Persistance.Update<ContactPerson>(model);


                if (Request.Files != null && Request.Files.Count > 0 && !string.IsNullOrWhiteSpace(filename))
                {
                    var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.ContactPersonId.ToString());
                    if (!Directory.Exists(Server.MapPath("//" + PictureFilePath + "//" + dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath("//" + PictureFilePath + "//" + dir));


                    }

                    Request.Files[0].SaveAs(Server.MapPath("//" + PictureFilePath + "//" + dir + "//" + filename));
                }

                ViewData["Success"] = "Person added to successfuly";
            }
            else
            {
                var err = ModelState.FirstOrDefault(p => p.Value.Errors.Count > 0);
                if (err.Key != null)
                {
                    ViewData["Error"] = err.Value.Errors[0].ErrorMessage;
                }

                
            }
            var modelc = new CustomerModel();
            ViewData["StateList"] = modelc.StateList;
            ViewData["Relatives"] = modelc.Relatives;
            modelc.ContactPerson = model;
            modelc.ContactPersonList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            modelc.ContactPersonList.ForEach(x => { x.RelationshipName = (ViewData["Relatives"] as List<SelectListItem>).FirstOrDefault(s => s.Value == x.Relationship.ToString()).Text; });
            //return View(modelc);
            if (ssfrmWzrd)
                return new RedirectResult("~/UploadCenter/SpecialDocument/1");
            else return View(modelc);

        }

        [HttpPost]
        public ActionResult ContactPerson(ContactPerson model)
        {
             return SaveContact(model);  
            //var ssfrmWzrd = false;
            //if (ModelState.IsValid)
            //{
            //    ssfrmWzrd = model.IsFromWizaed;
            //    var filename = "";
            //    model.UserId = CurrentUser.UserId;
                
            //    if (Request.Files != null && Request.Files.Count > 0)
            //    {
            //        filename = filename = Path.GetFileName(Request.Files[0].FileName);
            //    }
            //    model.PicturePath = filename;
            //    if(model.ContactPersonId ==0)
            //        model = Persistance.Create<ContactPerson>(model);
            //    else 
            //        model = Persistance.Update<ContactPerson>(model);
                

            //    if (Request.Files != null && Request.Files.Count > 0 && !string.IsNullOrWhiteSpace(filename))
            //    {
            //        var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.ContactPersonId.ToString());
            //        if (!Directory.Exists(Server.MapPath("//" + PictureFilePath + "//" + dir)))
            //        {
            //            Directory.CreateDirectory(Server.MapPath("//" + PictureFilePath + "//" + dir));


            //        }

            //        Request.Files[0].SaveAs(Server.MapPath("//" + PictureFilePath + "//" + dir + "//" + filename));
            //    }

            //    ViewData["Success"] = "Person added to successfuly";
            //}
            //var modelc = new CustomerModel();
            //ViewData["StateList"] = modelc.StateList;
            //ViewData["Relatives"] = modelc.Relatives;
            //modelc.ContactPerson = model;
            //modelc.ContactPersonList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            //modelc.ContactPersonList.ForEach(x => { x.RelationshipName = (ViewData["Relatives"] as List<SelectListItem>).FirstOrDefault(s => s.Value == x.Relationship.ToString()).Text; });
            ////return View(modelc);
            //if (ssfrmWzrd)
            //    return new RedirectResult("~/UploadCenter/SpecialDocument/1");
            //else return View(modelc);

        }

        [HttpPost]
        public ActionResult ContactPersonEdit(ContactPerson model)
        {
            var ssfrmWzrd = false;
            if (ModelState.IsValid)
            {
                ssfrmWzrd = model.IsFromWizaed;
                var filename = "";
                model.UserId = CurrentUser.UserId;
                model.Address2 = model.Address2 ?? "";
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    filename = filename = Path.GetFileName(Request.Files[0].FileName);
                }
                var cntPer = Persistance.ResolveKey<ContactPerson>(model.ContactPersonId);
                if (!string.IsNullOrWhiteSpace(filename))
                    model.PicturePath = filename;
                else if (model.ContactPersonId != 0 && cntPer !=null)
                    model.PicturePath = cntPer.PicturePath; 

                if (model.ContactPersonId == 0)
                    model = Persistance.Create<ContactPerson>(model);
                else
                    model = Persistance.Update<ContactPerson>(model);


                if (Request.Files != null && Request.Files.Count > 0 && !string.IsNullOrWhiteSpace(filename))
                {
                    var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.ContactPersonId.ToString());
                    if (!Directory.Exists(Server.MapPath("//" + PictureFilePath + "//" + dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath("//" + PictureFilePath + "//" + dir));


                    }

                    Request.Files[0].SaveAs(Server.MapPath("//" + PictureFilePath + "//" + dir + "//" + filename));
                }

                ViewData["Success"] = "Person added to successfuly";
            }
            var modelc = new CustomerModel();
            ViewData["StateList"] = modelc.StateList;
            ViewData["Relatives"] = modelc.Relatives;
            modelc.ContactPerson = model;
            modelc.ContactPersonList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            modelc.ContactPersonList.ForEach(x => { x.RelationshipName = (ViewData["Relatives"] as List<SelectListItem>).FirstOrDefault(s => s.Value == x.Relationship.ToString()).Text; });
            //return View(modelc);
            if (ssfrmWzrd)
                return new RedirectResult("~/UploadCenter/SpecialDocument/1");
            else return View(modelc);

        }


        public ActionResult ContactPersonEdit()
        {
            var model = new CustomerModel();
            var cntPerList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            model.ContactPerson = cntPerList.Count>0 ? cntPerList[0] : new Customer.ContactPerson();
            ViewData["StateList"] = model.StateList;
            ViewData["Relatives"] = model.Relatives;
            model.ContactPersonList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            model.ContactPersonList.ForEach(x => { x.RelationshipName = (ViewData["Relatives"] as List<SelectListItem>).FirstOrDefault(s => s.Value == x.Relationship.ToString()).Text; });
            model.ContactPerson.IsFromWizaed = true;
            return View("ContactPersonView", model);
        }

        public ActionResult EditContactPerson(int id)
        {
            var model = new CustomerModel();
            model.ContactPerson = Persistance.ResolveKey<Customer.ContactPerson>(id);
            ViewData["StateList"] = model.StateList;
            ViewData["Relatives"] = model.Relatives;
            model.ContactPersonList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            model.ContactPersonList.ForEach(x => { x.RelationshipName = (ViewData["Relatives"] as List<SelectListItem>).FirstOrDefault(s => s.Value == x.Relationship.ToString()).Text; });

            return View("ContactPerson",model);
        }
        [HttpPost]
        public ActionResult EditContactPerson(ContactPerson model)
        {
            if (ModelState.IsValid)
            {
                var filename = "";
               
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    filename =  Path.GetFileName(Request.Files[0].FileName);
                    
                    var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.ContactPersonId.ToString());
                    if (!Directory.Exists(Server.MapPath("//"+PictureFilePath+"//" + dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath("//" + PictureFilePath + "//" + dir));
                        

                    }
                    if(!string.IsNullOrWhiteSpace(filename))
                        Request.Files[0].SaveAs(Server.MapPath("//" + PictureFilePath + "//" + dir + "//" + filename));
                }
              

                model.Address2 = model.Address2 ?? "";
                model.UserId = CurrentUser.UserId;
                model = Persistance.Update<ContactPerson>(model);

                ViewData["Error"] = "Person saved to successfuly";
            }
            var modelc = new CustomerModel();
            ViewData["StateList"] = modelc.StateList;
            ViewData["Relatives"] = modelc.Relatives;
            modelc.ContactPerson = model;
            modelc.ContactPersonList = Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            modelc.ContactPersonList.ForEach(x => { x.RelationshipName = (ViewData["Relatives"] as List<SelectListItem>).FirstOrDefault(s => s.Value == x.Relationship.ToString()).Text; });
            return View("ContactPerson", modelc);
        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangePassword(RestPassword model)
        {
            if (ModelState.IsValid)
            {
                var smtp = FS.Common.ValuseData.GetValueData("SMTP");
                var host = smtp.FirstOrDefault(s => s.DataValue == "HOST").DataText;
                var pwd = smtp.FirstOrDefault(s => s.DataValue == "PWD").DataText;
                var port = smtp.FirstOrDefault(s => s.DataValue == "PORT").DataText;
                var ssl = smtp.FirstOrDefault(s => s.DataValue == "SSL").DataText == "1";

                var body="<p>Please click the link below to rest the password</p>";
                body += "<a href='"+ HostName + "Account/ResetPassword/'>Reset my password.</a>";
                body += " If you did not make the request. Please ignore this email.";
                if (Infarstructure.Common.SendEmail(FromEmail, model.Email, "Password reset request", body, host, pwd, int.Parse(port), ssl))
                {


                    ViewData["Success"] = "An email has been sent with a link to reset your password. Please check your inbox";
                }
                else
                {
                    ViewData["Error"] = "Unknown error. Please try again";
                }
            }
            else
            {
                ViewData["Error"] = "Invalid detail found. Please correct them";
            }
            return View();
        }


        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(UserLogin model)
        {
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                //var user = UserLogin.GetUserByEmail(model.Email);
                model.Password = Infarstructure.Common.Encrypt(model.Password);
                UserLogin.RestPassword(model.Email, model.Password);
                ViewData["Success"] = "Password changes successfully";
            }
            else
            {
                ViewData["Error"] = "Invalid email address please try again";
            }

                return View();
          
        }

        public ActionResult RegisterSummary()
        {
            var model = new CustomerModel();
            if (CurrentUser == null)
                return View(model);

            

            model.Documents = CustomerDocument.GetDocuments(CurrentUser.UserId);
            var cntPersons = FS.Customer.ContactPerson.GetContactPersons(CurrentUser.UserId);
            model.ContactPerson = cntPersons.Count > 0 ? cntPersons[0] : new FS.Customer.ContactPerson();
            if(model.ContactPerson.Relationship >0)
                model.ContactPerson.RelationshipName = Persistance.ResolveKey<ValuseData>(model.ContactPerson.Relationship).DataText;
            model.CheckList = FS.Customer.CheckList.GetCheckList(CurrentUser.UserId);
            return View(model);
        }

    }


}
