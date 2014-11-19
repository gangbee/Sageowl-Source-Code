using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FS.Customer
{
      [StoredProcedure(CreateProcedure = "InsertSendDocuments", ReadProcedure = "GetInstituteById", UpdateProcedure = "UpdateSendDocuments")]
    public class SendDocument
    {

        [DataColumn("SendId")]
        private int _SendId;
        public int SendId
        {
            get { return _SendId; }
            set { _SendId = value; }
        }
        [DataColumn("CustomerDocumentId")]
        private int _CustomerDocumentId;
        [Required(ErrorMessage = "Select document")]
        public int CustomerDocumentId
        {
            get { return _CustomerDocumentId; }
            set { _CustomerDocumentId = value; }
        }
        [DataColumn("InstiteId")]
        private int _InstiteId;
         [Required(ErrorMessage = "Select institute")]
        public int InstiteId
        {
            get { return _InstiteId; }
            set { _InstiteId = value; }
        }
        [DataColumn("UserId")]
        private int _UserId;
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        [DataColumn("EmailSent")]
        private bool _EmailSent;
        public bool EmailSent
        {
            get { return _EmailSent; }
            set { _EmailSent = value; }
        }
        [DataColumn("SendDate")]
        private DateTime _SendDate = DateTime.Now.Date;
        public DateTime SendDate
        {
            get { return _SendDate; }
            set { _SendDate = value; }
        }
        [DataColumn("DateReceived")]
        private DateTime _DateReceived = DateTime.Now.Date;
        public DateTime DateReceived
        {
            get { return _DateReceived; }
            set { _DateReceived = value; }
        }


    }
}
