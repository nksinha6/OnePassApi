namespace OnePass.Domain
{
    public interface IDigilockerService
    {
        Task<VerifyAccountResponse> VerifyAccountAsync(string verificationId, string mobile);
        Task<CreateUrlResponse> CreateUrlAsync(string verificationId, List<string> documents, string redirectUrl, string userFlow);
        Task<VerificationStatusResponse> GetStatusAsync(string verificationId, long? referenceId = null);
        Task<AadhaarDocumentResponse> GetAadhaarDocumentAsync(AadhaarFetchRequest request);
    }
}
