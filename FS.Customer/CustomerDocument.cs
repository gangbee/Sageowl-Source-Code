using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.SqlClient;
namespace FS.Customer
{
     [StoredProcedure(CreateProcedure = "CustomerDocumentInsert",
        ReadProcedure = "CustomerDocumentSelect", UpdateProcedure = "CustomerDocumentUpdate", DeleteProcedure = "CustomerDocumentDelete")]
    public class CustomerDocument
    {

        [DataColumn("CustomerDocumentId")]
        [PrimaryKey]
        private int _CustomerDocumentId;
        public int CustomerDocumentId
        {
            get { return _CustomerDocumentId; }
            set { _CustomerDocumentId = value; }
        }

         [DataColumn("UserId")]
        private int _userId;

         
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        
        [DataColumn("DocumentKey")]
        private string _DocumentKey;
      // [Required(ErrorMessage = "Document required")]
        [DisplayName("Document")]
        public string DocumentKey
        {
            get { return _DocumentKey; }
            set { _DocumentKey = value; }
        }
        [DataColumn("DocumentName")]
        private string _DocumentName;
        [Required(ErrorMessage = "Document Name required")]
        [DisplayName("Other")]
        public string DocumentName
        {
            get { return _DocumentName; }
            set { _DocumentName = value; }
        }
        [DataColumn("FileName")]
        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        [DataColumn("DateUpload")]
        private DateTime _DateUpload;
        public DateTime DateUpload
        {
            get { return _DateUpload; }
            set { _DateUpload = value; }
        }

        public  List<CustomerDocument> CustomerDocumentList
        {
            get
            {
                var list =  GetDocuments(this.UserId);
                return list.Where(d => d.FileName != "").ToList();
            }


        }

        public static List<CustomerDocument> GetDocuments(int userId)
        {
            return Persistance.ReadList<CustomerDocument>("CustomerDocumentGetByUer", new SqlParameter("@UserId", userId));


        }

        public static List<CustomerDocument> GetAllDocuments()
        {
            return Persistance.ReadList<CustomerDocument>("CustomerDocumentSelect", new SqlParameter("@CustomerDocumentId", "0"));
        }

        
    }
}
