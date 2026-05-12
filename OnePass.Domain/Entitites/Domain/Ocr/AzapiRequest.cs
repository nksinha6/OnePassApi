using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class AzapiRequest
    {
        public string document_type { get; set; } = "passport";
        public string image { get; set; }
    }

    public class AzapiResponse
    {
        public string status { get; set; }
        public PassportData data { get; set; }
    }

    public class PassportData
    {
        public string full_name { get; set; }
        public string birth_place { get; set; }
        public string passport_number { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string issue_date { get; set; }
        public string expiry_date { get; set; }
        public string nationality { get; set; }

        public string type { get; set; }
        public string mrz { get; set; }
        public string face_image { get; set; }
        public string address { get; set; }
        public string country_code { get; set; }
        public string place_of_issue { get;set; }

        public string passport_type { get; set; }
    }
}
