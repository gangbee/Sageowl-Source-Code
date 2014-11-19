using FS.Customer;
using Infarstructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace FS.Web.Controllers
{
    public class BaseController : Controller,IDisposable
    {
        //
        // GET: /Base/
        SqlConnection con;
        public BaseController()
        {
             con = ConnectionProvider.GetGlobalConnection();
            DataAccess.Persistance.ConnectionProvider = delegate { return con; };
        }

       

        public UserLogin CurrentUser
        {
            get
            {
                var _user = System.Web.HttpContext.Current.User as PrincipalProvider;
                if (_user != null)
                {
                    return _user.User as UserLogin;
                }
                return null;
            }
        }
     

        public string PictureFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["PictureFilePath"];
            }
        }

        public string DocumentPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DocumentPath"];
            }
        }

        public string HostName
        {
            get
            {
                return ConfigurationManager.AppSettings["HostName"];
            }
        }
        public string FromEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["FromEmail"];
            }
        }

        

        void IDisposable.Dispose()
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
