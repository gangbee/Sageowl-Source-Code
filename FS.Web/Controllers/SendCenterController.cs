using DataAccess;
using FS.Customer;
using FS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FS.Web.Controllers
{
    [Authorize]
    public class SendCenterController : BaseController
    {
        //
        // GET: /SendCenter/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new Institute();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Institute model)
        {
            if (ModelState.IsValid)
            {
                if (model.InstituteId == 0)
                    model= Persistance.Create<Institute>(model);
                else
                    model = Persistance.Update<Institute>(model);
            }else
                ViewData["Error"] = "Invalid data found. Please correct them and try again";
            if (model.IsFromSendCenter)
                return RedirectToAction("SendDocuments");
            return View(model);
        }


        public ActionResult SendDocuments()
        {

            var docmodel = new DocumentModel();

            var userFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);
            ViewData["Documents"] = userFiles.Where(u => !string.IsNullOrWhiteSpace(u.DocumentKey))
                .Select(d => new SelectListItem { Value = d.CustomerDocumentId.ToString(), Text = docmodel.DocumentTypes.FirstOrDefault(p => p.DataValue == d.DocumentKey).DataText });

            var institues = Institute.GetAllInstitues();
            institues.Add(new Institute { InstituteId=-1, Name="Other" });
            ViewData["Institutes"] = institues
                .Select(d => new SelectListItem { Value = d.InstituteId.ToString(), Text = d.Name });
            var model = new SendDocument();
            return View(model);


        }
        [HttpPost]
        public ActionResult SendDocuments(SendDocument model)
        {

            var docmodel = new DocumentModel();

            var userFiles = CustomerDocument.GetDocuments(CurrentUser.UserId);
            ViewData["Documents"] = userFiles.Where(u => !string.IsNullOrWhiteSpace(u.DocumentKey))
                .Select(d => new SelectListItem { Value = d.CustomerDocumentId.ToString(), Text = docmodel.DocumentTypes.FirstOrDefault(p => p.DataValue == d.DocumentKey).DataText });

            var institues = Institute.GetAllInstitues();
            institues.Add(new Institute { InstituteId = -1, Name = "Other" });
            ViewData["Institutes"] = institues
                .Select(d => new SelectListItem { Value = d.InstituteId.ToString(), Text = d.Name });
            model.UserId = CurrentUser.UserId;
            if (model.SendDate.Year == 1)
                model.SendDate = DateTime.Now;
            if (model.DateReceived.Year == 1)
                model.DateReceived = new DateTime(1900, 1, 1);

            if (model.SendId == 0)
                Persistance.Create<SendDocument>(model);
            else 
                Persistance.Create<SendDocument>(model);

            // Email the document. 
            var doc = userFiles.FirstOrDefault(d => d.CustomerDocumentId == model.CustomerDocumentId);
            var inst = institues.FirstOrDefault(i => i.InstituteId == model.InstiteId);
            if (doc != null && inst != null)
            {
                var toMail = inst != null ? inst.Email : "";
                var dir = string.Format("{0}_{1}", model.UserId.ToString(), doc.DocumentKey);
                var path = Server.MapPath("//" + DocumentPath + "//" + dir + "//" + doc.FileName);

                if (SendFile(path, toMail))
                {
                    ViewData["Message"] = "Document sent successfully";
                }
                else
                {
                    ViewData["Error"] = "Error sending documents";
                }

            }else
                ViewData["Error"] = "Error sending documents";

           
            return View(model);


        }

        private bool SendFile(string fileName, string toEmail)
        {
            var smtp = FS.Common.ValuseData.GetValueData("SMTP");
            var host = smtp.FirstOrDefault(s => s.DataValue == "HOST").DataText;
            var pwd = smtp.FirstOrDefault(s => s.DataValue == "PWD").DataText;
            var port = smtp.FirstOrDefault(s => s.DataValue == "PORT").DataText;
            var ssl = smtp.FirstOrDefault(s => s.DataValue == "SSL").DataText == "1";

            var body = "<p>Please find the attached document for your attention</p>";
           // body += "<a href='" + HostName + "Account/ResetPassword/'>Reset my password.</a>";
            body += "Reply to this email address if you need any clarifications.";
            if (Infarstructure.Common.SendDocumentsToInstitute(FromEmail, toEmail, "Document for your recording", body, host, pwd, int.Parse(port), ssl, fileName))
            {


                return true;
            }
            return false;
        }

    }
}
