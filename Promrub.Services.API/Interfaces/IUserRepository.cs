using Promrub.Services.API.Entities;

namespace Promrub.Services.API.Interfaces
{
    public interface IUserRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public IEnumerable<UserEntity> GetUsers();
        public Task<UserEntity> GetUserByName(string userName);
        public void AddUser(UserEntity user);
        public bool IsEmailExist(string email);
        public bool IsUserNameExist(string userName);
        public bool IsUserIdExist(string userId);
    }
}
