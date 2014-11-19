using DataAccess;
using FS.Common;
using FS.Customer;
using FS.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FS.Web.Controllers
{
    public class UploadCenterController : BaseController
    {
        //
        // GET: /UploadCenter/

        public ActionResult Index()
        {
            var model = new DocumentModel(); 
            return View(model);
        }

        [HttpPost]
        public ActionResult SelectFiles(CustomerDocument model, string[] FileName)
        {
            var docmodel = new DocumentModel();
            if (FileName != null)
            {
                foreach (string key in FileName)
                {
                    var doc = new CustomerDocument();
                    doc.DocumentKey = key;
                    doc.DateUpload = DateTime.Now;
                    doc.UserId = CurrentUser.UserId;
                    doc.DocumentName = "";
                    doc.FileName = "";


                    Persistance.Create<CustomerDocument>(doc);
                }
            }

            //ViewData["Documents"] =
            return RedirectToAction("UploadFiles"); 
        }

        public ActionResult UploadFiles()
        {
            var docmodel = new DocumentModel();
            var model = new CustomerDocument();
            model.UserId = CurrentUser.UserId;

            var userFiles = docmodel.DocumentTypes; //CustomerDocument.GetDocuments(CurrentUser.UserId);
            ViewData["Documents"] = userFiles//.Where(u => !string.IsNullOrWhiteSpace(u.DocumentKey))
                .Select(d => new SelectListItem { Value = d.DataValue, Text = docmodel.DocumentTypes.FirstOrDefault(p => p.DataValue == d.DataValue).DataText });

            return View(model);
        }

        [HttpPost]
        public ActionResult UploadFiles(CustomerDocument model)
        {
            var docmodel = new DocumentModel();

            var userFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);
            //ViewData["Documents"] = userFiles.Where(u => !string.IsNullOrWhiteSpace(u.DocumentKey))
            //    .Select(d => new SelectListItem { Value = d.DocumentKey, Text = docmodel.DocumentTypes.FirstOrDefault(p => p.DataValue == d.DocumentKey).DataText });
            var userDocs = docmodel.DocumentTypes;
            ViewData["Documents"] = userDocs//.Where(u => !string.IsNullOrWhiteSpace(u.DocumentKey))
                .Select(d => new SelectListItem { Value = d.DataValue, Text = docmodel.DocumentTypes.FirstOrDefault(p => p.DataValue == d.DataValue).DataText });


            if (Request.Files == null || Request.Files.Count == 0 || 
                Path.GetFileName(Request.Files[0].FileName) == string.Empty)
            {
                ViewData["Error"] = "Please select a file to upload";
                return View(model);
            }
           
            

            
            model.UserId = CurrentUser.UserId;
            model.DateUpload = DateTime.Now;
            var filename = "";
            var doc = userFiles.FirstOrDefault(d => d.DocumentKey == model.DocumentKey);
            if (doc != null)
                model.CustomerDocumentId = doc.CustomerDocumentId;

            if (Request.Files != null && Request.Files.Count > 0)
            {
                filename = Path.GetFileName(Request.Files[0].FileName);

                var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.DocumentKey);
                if (!Directory.Exists(Server.MapPath("//" + DocumentPath + "//" + dir)))
                {
                    Directory.CreateDirectory(Server.MapPath("//" + DocumentPath + "//" + dir));


                }
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    Request.Files[0].SaveAs(Server.MapPath("//" + DocumentPath + "//" + dir + "//" + filename));
                }
            }
            model.FileName = filename;
            if (model.CustomerDocumentId==0)
            {
                //model.DocumentKey = "";
                Persistance.Create<CustomerDocument>(model);
            }
            else
            {
                Persistance.Update<CustomerDocument>(model);
            }
            return RedirectToAction("AdditionalFiles");
        }

        public ActionResult SpecialDocument(int Id)
        {
            var docmodel = new DocumentModel();
            var model = new CustomerDocument();
            model.UserId = CurrentUser.UserId;
            var userFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);
            //var docs = ValuseData.GetValueData("DOCTYPE");
            if (Id == 1)
            {
                var poa = userFiles.FirstOrDefault(d => d.DocumentKey == "POA");
                model = poa ?? new CustomerDocument(); 
                model.DocumentKey = "POA";
              
                model.FileName = poa != null ? poa.FileName : "";
                ViewData["Title"] = "Upload the Power of Attorney Document for the Care Receiver.";
            }
            else if (Id == 2)
            {
                var hpoa = userFiles.FirstOrDefault(d => d.DocumentKey == "HPOA");
                model = hpoa ?? new CustomerDocument();
                model.DocumentKey = "HPOA";
               
                model.FileName = hpoa != null ? hpoa.FileName : "";
                ViewData["Title"] = "Upload the Healthcare Power of Attorney Document for the Care Receiver.";
            }
            else if (Id == 3)
            {
                var lwill = userFiles.FirstOrDefault(d => d.DocumentKey == "LWIL");
                model = lwill ?? new CustomerDocument();
                model.DocumentKey = "LWIL";
               
                model.FileName = lwill != null ? lwill.FileName : "";
                ViewData["Title"] = "Upload the Living Will for the care receiver.";
            }
            else if (Id == 4)
            {
                var will = userFiles.FirstOrDefault(d => d.DocumentKey == "WIL");
                model = will ?? new CustomerDocument();
                model.DocumentKey = "WIL";
                
                model.FileName = will != null ? will.FileName : "";
                ViewData["Title"] = "Upload the Will for the Care Receiver.";
            }

            return View(model);
        }


        public ActionResult DeleteDocument(int id)
        {
            var doc = Persistance.ResolveKey<CustomerDocument>(id);
            if (doc != null)
            {
                Persistance.Delete<CustomerDocument>(doc);
            }
            //Redirect(Request.Referrer);
            return new RedirectResult(Request.UrlReferrer.LocalPath);
            var x = Request.UrlReferrer;
            string did = "";
            if (RouteData.Values["action"].ToString().ToLower() == "registersummary")
                return new RedirectResult("~/Account/RegisterSummary");
            if (doc.DocumentKey == "POA")
            {
                ViewData["Title"] = "Upload the Healthcare Power of Attorney Document for the Care Receiver.";
                return RedirectToAction("SpecialDocument", new { Id = 1 });
                did = "1";

            }
            if (doc.DocumentKey == "HPOA")
            {
                ViewData["Title"] = "Upload the Living Will for the care receiver.";
                return RedirectToAction("SpecialDocument", new { Id = 2 });
                did = "2";
            }
            if (doc.DocumentKey == "LWIL")
            {
                ViewData["Title"] = "Upload the Will for the Care Receiver.";
                return RedirectToAction("SpecialDocument", new { Id = 3 });
                did = "3";
            }
            if (doc.DocumentKey == "LIL")
            {
                ViewData["Title"] = "Upload the Will for the Care Receiver.";
                return RedirectToAction("SpecialDocument", new { Id = 4 });
                did = "4";
            }
            //string url = "~/" + RouteData.Values["controller"].ToString() + "/" + RouteData.Values["action"].ToString() + (!string.IsNullOrWhiteSpace(did) ? "/" + did : "");
            return new RedirectResult("~/Account/RegisterSummary");
           
        }

        [HttpPost]
        public ActionResult SpecialDocument(CustomerDocument model, FormCollection f)
        {
            if (Request.Files.Count > 0 && !string.IsNullOrWhiteSpace(Request.Files[0].FileName))
            {
                var docmodel = new DocumentModel();

                var userFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);


                if (Request.Files == null || Request.Files.Count == 0 ||
                    Path.GetFileName(Request.Files[0].FileName) == string.Empty)
                {
                    ViewData["Error"] = "Please select a file to upload";
                    return View(model);
                }




                model.UserId = CurrentUser.UserId;
                model.DateUpload = DateTime.Now;
                var filename = "";
                var doc = userFiles.FirstOrDefault(d => d.DocumentKey == model.DocumentKey);
                model.DocumentName = model.DocumentKey;
                if (doc != null)
                {
                    model.CustomerDocumentId = doc.CustomerDocumentId;
                    model.DocumentName = doc.DocumentName;
                }

                if (Request.Files != null && Request.Files.Count > 0 )
                {
                    filename = Path.GetFileName(Request.Files[0].FileName);

                    var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.DocumentKey);
                    if (!Directory.Exists(Server.MapPath("//" + DocumentPath + "//" + dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath("//" + DocumentPath + "//" + dir));
                    }
                    if (!string.IsNullOrWhiteSpace(filename))
                    Request.Files[0].SaveAs(Server.MapPath("//" + DocumentPath + "//" + dir + "//" + filename));
                }
                model.FileName = filename;

                Persistance.Create<CustomerDocument>(model);

                return Content("{\"name\":\"" + Request.Files[0].FileName + "\",\"type\":\"" + Request.Files[0].ContentType + "\",\"size\":\"" + string.Format("{0} bytes", Request.Files[0].ContentLength) + "\"}", "application/json");
            }


            return Content("{\"name\":\"Error\",\"type\":\"Error\",\"size\":\"Error\"}", "application/json");

            //ViewData["Title"] = "Upload the Power of Attorney Document for the Care Receiver.";
            //if (model.DocumentKey == "POA")
            //{
            //    ViewData["Title"] = "Upload the Healthcare Power of Attorney Document for the Care Receiver.";
            //    return RedirectToAction("SpecialDocument", new { Id = 2 });

            //}
            //if (model.DocumentKey == "HPOA")
            //{
            //    ViewData["Title"] = "Upload the Living Will for the care receiver.";
            //    return RedirectToAction("SpecialDocument", new { Id = 3 });
            //}
            //if (model.DocumentKey == "LWIL")
            //{
            //    ViewData["Title"] = "Upload the Will for the Care Receiver.";
            //    return RedirectToAction("SpecialDocument", new { Id = 4 });
            //}


            //return new RedirectResult("~/UploadCenter/AdditionalFiles"); 
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string DocumentUpload(HttpPostedFileWrapper userFile, string id)
        {
            try
            {
                var docmodel = new DocumentModel();
                var model = new CustomerDocument();
                var userFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);


                if (Request.Files == null || Request.Files.Count == 0 ||
                    Path.GetFileName(Request.Files[0].FileName) == string.Empty)
                {
                    ViewData["Error"] = "Please select a file to upload";
                    return ("{error : '', msg : 'Please select a file to upload'}");
                }




                model.UserId = CurrentUser.UserId;
                model.DateUpload = DateTime.Now;
                var filename = "";
                var doc = userFiles.FirstOrDefault(d => d.DocumentKey == id);
                model.DocumentName = id;
                model.DocumentKey = id;
                if (doc != null)
                {
                    model.CustomerDocumentId = doc.CustomerDocumentId;
                    model.DocumentName = doc.DocumentName;
                }

                if (Request.Files != null && Request.Files.Count > 0)
                {
                    filename = Path.GetFileName(Request.Files[0].FileName);

                    var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.DocumentKey);
                    if (!Directory.Exists(Server.MapPath("//" + DocumentPath + "//" + dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath("//" + DocumentPath + "//" + dir));
                    }
                    if (!string.IsNullOrWhiteSpace(filename))
                        Request.Files[0].SaveAs(Server.MapPath("//" + DocumentPath + "//" + dir + "//" + filename));
                }
                model.FileName = filename;
                var obj = Persistance.Create<CustomerDocument>(model);
                var docId = model.CustomerDocumentId.ToString();
                if (obj != null)
                    docId = obj.CustomerDocumentId.ToString();

                return ("{error : '', msg : 'File added successfuly',id :'" + docId + "',docKey :'" + id + "', userId :'" + CurrentUser.UserId.ToString() + "', filename : '" + filename + "'}");
               
               

            }
            catch (Exception ex)
            {
                return ("{error : '" + ex.Message + "', msg : 'Error occured. Please try again'}");
            }

        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string DocumentAdditionalUpload(HttpPostedFileWrapper userFilemain, string id)
        {
            var userFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);
            try
            {
                var docmodel = new DocumentModel();
                var model = new CustomerDocument();
                


                if (Request.Files == null || Request.Files.Count == 0 ||
                    Path.GetFileName(Request.Files[0].FileName) == string.Empty)
                {
                    ViewData["Error"] = "Please select a file to upload";
                    return ("{error : '', msg : 'Please select a file to upload'}");
                    
                }




                model.UserId = CurrentUser.UserId;
                model.DateUpload = DateTime.Now;
                var filename = "";
                var doc = userFiles.FirstOrDefault(d => d.DocumentKey == id);
                model.DocumentName = id;
                model.DocumentKey = id;
                if (doc != null)
                {
                    model.CustomerDocumentId = doc.CustomerDocumentId;
                    model.DocumentName = doc.DocumentName;
                }

                if (Request.Files != null && Request.Files.Count > 0)
                {
                    filename = Path.GetFileName(Request.Files[0].FileName);

                    var dir = string.Format("{0}_{1}", model.UserId.ToString(), model.DocumentKey);
                    if (!Directory.Exists(Server.MapPath("//" + DocumentPath + "//" + dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath("//" + DocumentPath + "//" + dir));
                    }
                    if (!string.IsNullOrWhiteSpace(filename))
                        Request.Files[0].SaveAs(Server.MapPath("//" + DocumentPath + "//" + dir + "//" + filename));
                }
                model.FileName = filename;
                var obj = Persistance.Create<CustomerDocument>(model);
                var docId = model.CustomerDocumentId.ToString();
                if (obj != null)
                    docId = obj.CustomerDocumentId.ToString();

                userFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);
                return ("{error : '', msg : 'File added successfuly',id :'" + docId + "',docKey :'" + id + "', userId :'" + CurrentUser.UserId.ToString() + "', filename : '" + filename + "'}");

                //return PartialView("_FileList",userFiles);

            }
            catch (Exception ex)
            {
                return ("{error : '" + ex.Message + "', msg : 'Error occured. Please try again'}");
            }

        }

        [Authorize]
        public ActionResult AdditionalFiles()
        {

            var docmodel = new DocumentModel();
            var model = new CustomerDocument();
            model.UserId = CurrentUser.UserId;
            var userUploadedFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);
            var userFiles = docmodel.DocumentTypes; //CustomerDocument.GetDocuments(CurrentUser.UserId);
            var selectedList = userUploadedFiles.Select(f => f.DocumentKey).ToArray();
            userFiles = userFiles.Where(u => ! selectedList.Contains(u.DataValue)).ToList();
            ViewData["Documents"] = userFiles
                .Select(d => new SelectListItem { Value = d.DataValue, Text = docmodel.DocumentTypes.FirstOrDefault(p => p.DataValue == d.DataValue).DataText });

            return View(model);
        }

        public ActionResult SendCenter()
        {
            return View();
        }

        public ActionResult Organizer()
        {
            return View();

        }

        public ActionResult AfterDeath()
        {
            return View();
        }

        public ActionResult Links()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult HealpContact()
        {
            return View();
        }
    }
}
