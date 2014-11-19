using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FS.Customer
{
    public class CheckList
    {
        [DataColumn("UserId")]
        private int _userId;

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        [DataColumn("Hpoa")]
        private int _hpoa;

        public int Hpoa
        {
            get { return _hpoa; }
            set { _hpoa = value; }
        }

        [DataColumn("Poa")]
        private int _poa;

        public int Poa
        {
            get { return _poa; }
            set { _poa = value; }
        }

        [DataColumn("Will")]
        private int _will;

        public int Will
        {
            get { return _will; }
            set { _will = value; }
        }

          [DataColumn("CareGiver")]
        private int _careGiver;

        public int CareGiver
        {
            get { return _careGiver; }
            set { _careGiver = value; }
        }

         [DataColumn("CareReceiver")]
        private int _careReceiver;

        public int CareReceiver
        {
            get { return _careReceiver; }
            set { _careReceiver = value; }
        }

        [DataColumn("HIPPA")]
        private int _hippa;

        public int Hippa
        {
            get { return _hippa; }
            set { _hippa = value; }
        }

         [DataColumn("LW")]
        private int _livingWill;

         public int LW
        {
            get { return _livingWill; }
            set { _livingWill = value; }
        }

        [DataColumn("DC")]
        private int _dc;

        public int DC
        {
            get { return _dc; }
            set { _dc = value; }
        }
        

        public static CheckList GetCheckList(int userId)
        {
            var list = Persistance.ReadList<CheckList>("GetUserCheckList", new SqlParameter ( "@UserId", userId ));
            if (list.Count > 0)
                return list[0];

            return new CheckList();
        }
        
        
    }

    //GetUserCheckList
}
