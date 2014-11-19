using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using FS.Customer;

namespace Infarstructure
{
    public class PrincipalProvider :System.Security.Principal.IPrincipal
    {
        SqlConnection con;
        protected IIdentity _identity;
        public PrincipalProvider(IIdentity identity)
		{
            _identity = identity;
           
		}


        public bool IsInRole(string role)
        {
            //if (_user != null)
            //{
            //    return (_user.IsAdmin && role == "Admin");
            //}

            return true;


        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        private UserLogin _user;

        public UserLogin User
        {
            get { return _user; }
        }

        public void AuthenticateUser(string username,string password)
        {
            _user = UserLogin.AuthenticateUser(username, password);
        }

        public void Load()
        {
            con = ConnectionProvider.GetGlobalConnection();
            DataAccess.Persistance.ConnectionProvider = delegate { return con; };
            _user = UserLogin.AuthenticateUser(_identity.Name);
        }
    }
}
