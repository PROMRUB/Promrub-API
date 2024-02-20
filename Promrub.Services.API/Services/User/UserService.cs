using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.Authentications;
using Promrub.Services.API.Models.RequestModels.User;
using Promrub.Services.API.Models.ResponseModels.User;

namespace Promrub.Services.API.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper mapper;
        private readonly IUserRepository repository;

        public UserService(IMapper mapper,
            IUserRepository repository) : base()
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public List<UserResponse> GetUsers(string orgId)
        {
            repository!.SetCustomOrgId(orgId);
            var query = repository!.GetUsers();
            return mapper.Map<IEnumerable<UserEntity>, List<UserResponse>>(query);
        }

        public async Task<UserResponse> GetUserByName(string orgId, string userName)
        {
            repository!.SetCustomOrgId(orgId);
            var query = await repository!.GetUserByName(userName);
            return mapper.Map<UserEntity,UserResponse>(query);
        }

        public void AddUser(string orgId, UserRequest request)
        {
            repository!.SetCustomOrgId(orgId);
            if (IsEmailExist(orgId, request!.UserEmail!) || IsUserNameExist(orgId, request!.UserName!))
                throw new ArgumentException("1111");
            var query = mapper.Map<UserRequest, UserEntity>(request);
            repository!.AddUser(query);
        }

        public bool IsEmailExist(string orgId, string email)
        {
            repository!.SetCustomOrgId(orgId);
            var result = repository!.IsEmailExist(email);
            return result;
        }

        public bool IsUserNameExist(string orgId, string userName)
        {
            repository!.SetCustomOrgId(orgId);
            var result = repository!.IsUserNameExist(userName);
            return result;
        }

        public bool IsUserIdExist(string orgId, string userId)
        {
            repository!.SetCustomOrgId(orgId);
            var result = repository!.IsUserIdExist(userId);
            return result;
        }
    }
}
