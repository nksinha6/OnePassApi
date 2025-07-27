using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePass.Domain.Services
{
    public class UserReadService : ReadServiceBase, IUserReadService
    {
        public UserReadService(IReadRepositoryFactory repositoryFactory) : base(repositoryFactory) { }
        public Task<User> GetUserAsync(GetUserQuery query) =>
            HandleSingleOrDefaultAsync<GetUserQuery, User>(
                query,
                useStoredProcedure: false,
                notFoundMessage: "No Premise found for given Id");
    }
}
