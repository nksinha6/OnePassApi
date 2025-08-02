using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Dto;

namespace OnePass.Domain
{
    public interface IUserPersistsService
    {
        Task<User> PersistsAsync(User user);
        Task<User> PersistsIfNotExistsAsync(string phoneNo);
        Task<User> UpdateStatusAsync(UpdateUserStatusParam param);
        Task<User> VerifyEmailAsync(string phoneNo);
        Task<User> UpdateUserProfileAsync(UserProfileUpdateDto user);
    }
}
