using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class UserPersistsService(IPersistRepository<User> userPersistsRepository) : IUserPersistsService
    {
        private readonly IPersistRepository<User> _userPersistsRepository = userPersistsRepository;
       
        public async Task<User> PersistsAsync(User user)
        {
            var result = await _userPersistsRepository.AddOrUpdateAllAsync(new List<User> { user });
            return result.First();
        }

        public Task<User> PersistsIfNotExistsAsync(string phoneNo)
        => _userPersistsRepository.AddIfNotExistAsync(new User() { Phone = phoneNo });

        public Task<User> UpdateStatusAsync(UpdateUserStatusParam param)
       => _userPersistsRepository.UpdatePartialAsync(new User() { Phone = param.PhoneNo, Status = param.Status }, x => x.Status);

        public Task<User> VerifyEmailAsync(string phoneNo)
       => _userPersistsRepository.UpdatePartialAsync(new User() { Phone = phoneNo, IsEmailVerified = true }, x => x.IsEmailVerified);

    }
}
