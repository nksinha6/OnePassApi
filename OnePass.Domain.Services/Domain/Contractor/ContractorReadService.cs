using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class ContractorReadService : ReadServiceBase, IContractorReadService
    {
        private readonly IFaceVerificationService faceVerificationService;

        public ContractorReadService(IReadRepositoryFactory repositoryFactory, IFaceVerificationService faceVerificationService)
            : base(repositoryFactory) 
        {
            this.faceVerificationService = faceVerificationService;
        }

        public Task<IEnumerable<ContractorSelfieImageResponse>> GetAllContractorSelfieImagesAsync()
        =>
            HandleQueryAsync<GetAllContractorSelfieImagesQuery, ContractorSelfieImageResponse>(
                new GetAllContractorSelfieImagesQuery(),
                useStoredProcedure: false);


        public async Task<(string, string?, string?)> CompareSelfie(ImageInput input)
        {
            var images = await GetAllContractorSelfieImagesAsync();
            
            foreach (var image in images)
            {
                var storedImage = ToImageInput(image);

                var res = await this.faceVerificationService.MatchFacesAsync(Guid.NewGuid().ToString(), storedImage, input);

                if(res.FaceMatchResult.ToUpper() == "YES")
                {
                    return (res.FaceMatchResult, image.PhoneCountryCode, image.PhoneNumber);
                }
            }

            return ("NO", null, null);
        }

        public static ImageInput ToImageInput(ContractorSelfieImageResponse entity)
        {
            var stream = new MemoryStream(entity.Image);
            stream.Position = 0;

            var extension = entity.ContentType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                _ => ""
            };

            var fileName = $"{entity.PhoneCountryCode}_{entity.PhoneNumber}{extension}";

            return new ImageInput(stream, fileName, entity.ContentType, entity.FileSize);
        }
    }

}
