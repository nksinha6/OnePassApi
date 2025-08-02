using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public interface IUserPersistsService
    {
        Task<User> PersistsAsync(User user);
        Task<User> PersistsIfNotExistsAsync(string phoneNo);
        Task<User> UpdateStatusAsync(UpdateUserStatusParam param);
        Task<User> VerifyEmailAsync(string phoneNo);
    }
}
