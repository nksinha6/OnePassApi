using System.Text.Json.Serialization;

namespace OnePass.Domain
{
    public class VerifyAccountRequest
    {
        [JsonPropertyName("verification_id")]
        public string VerificationId { get; set; }

        [JsonPropertyName("mobile_number")]
        public string MobileNumber { get; set; }
    }
    public class VerifyAccountResponse
    {
        [JsonPropertyName("verification_id")]
        public string VerificationId { get; set; }
        [JsonPropertyName("reference_id")]
        public long ReferenceId { get; set; }
       
        [JsonPropertyName("digilocker_id")]
        public string DigilockerId { get; set; }
        public string Status { get; set; }
    }

    // Models/CreateUrlRequest.cs
    public class CreateUrlRequest
    {
        [JsonPropertyName("verification_id")]
        public string VerificationId { get; set; }

        [JsonPropertyName("document_requested")]
        public List<string> DocumentRequested { get; set; }

        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonPropertyName("user_flow")]
        public string UserFlow { get; set; }
    }

    // Models/CreateUrlResponse.cs
    public class CreateUrlResponse
    {
        [JsonPropertyName("verification_id")]
        public string VerificationId { get; set; }

        [JsonPropertyName("reference_id")]
        public long ReferenceId { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("document_requested")]
        public List<string> DocumentRequested { get; set; }

        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonPropertyName("user_flow")]
        public string UserFlow { get; set; }
    }

    // Models/UserDetails.cs
    public class UserDetails
    {
        public string Name { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string Eaadhaar { get; set; }
        public string Mobile { get; set; }
    }

    // Models/VerificationStatusResponse.cs
    public class VerificationStatusResponse
    {
        public string Status { get; set; }
        public UserDetails UserDetails { get; set; }
        public List<string> DocumentRequested { get; set; }
        public List<string> DocumentConsent { get; set; }
        public string VerificationId { get; set; }
        public long ReferenceId { get; set; }
    }

    // Models/Address.cs
    public class Address
    {
        public string Country { get; set; }
        public string Dist { get; set; }
        public string House { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }
        public string Po { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Subdist { get; set; }
        public string Vtc { get; set; }
    }

    // Models/AadhaarDocumentResponse.cs
    public class AadhaarDocumentResponse
    {
        [JsonPropertyName("reference_id")]
        public long ReferenceId { get; set; }

        [JsonPropertyName("verification_id")]
        public string VerificationId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("care_of")]
        public string CareOf { get; set; }

        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("photo_link")]
        public string PhotoLink { get; set; }

        [JsonPropertyName("split_address")]
        public Address SplitAddress { get; set; }

        [JsonPropertyName("year_of_birth")]
        public int YearOfBirth { get; set; }

        [JsonPropertyName("xml_file")]
        public string XmlFile { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
