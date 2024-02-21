using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Organization;
using Promrub.Services.API.Models.ResponseModels.Organization;
using Promrub.Services.API.Services.User;

namespace Promrub.Services.API.Services.Organization
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly IMapper mapper;
        private readonly IOrganizationRepository repository;
        private readonly IUserService userService;

        public OrganizationService(IMapper mapper,
            IOrganizationRepository repository, 
            IUserService userService) : base()
        {
            this.mapper = mapper;
            this.repository = repository;
            this.userService = userService;
        }

        public void AddOrganization(string orgId, OrganizationRequest org)
        {
            var customOrgId = org.OrgCustomId;
            if (repository!.IsCustomOrgIdExist(customOrgId!))
                throw new ArgumentException("1111");
            repository!.SetCustomOrgId(customOrgId!);
            var request = mapper.Map<OrganizationRequest,OrganizationEntity>(org);
            repository!.AddOrganization(request);
        }

        public async Task<OrganizationResponse> GetOrganization(string orgId)
        {
            repository!.SetCustomOrgId(orgId);
            var query = await repository!.GetOrganization();
            return mapper.Map<OrganizationEntity, OrganizationResponse>(query);
        }

        public async Task<List<OrganizationUserResponse>> GetUserAllowedOrganization(string userName)
        {
            var query = await repository!.GetUserAllowedOrganizationAsync(userName);
            return mapper.Map<IEnumerable<OrganizationUserEntity>,List<OrganizationUserResponse>>(query);
        }

        public bool IsUserNameExist(string orgId, string userName)
        {
            repository!.SetCustomOrgId(orgId);
            var result = repository!.IsUserNameExist(userName);
            return result;
        }

        public void AddUserToOrganization(string orgId, OrganizationUserRequest user)
        {
            repository!.SetCustomOrgId(orgId);
            if (userService.IsUserNameExist(orgId, user!.UserName!) || userService.IsUserIdExist(orgId, user!.UserId!))
                throw new ArgumentException("1111");
            var request = mapper.Map<OrganizationUserRequest, OrganizationUserEntity>(user);
            repository!.AddUserToOrganization(request);
        }

        public async Task<bool> VerifyUserInOrganization(string orgId, string userName)
        {
            repository!.SetCustomOrgId(orgId);
            if (await userService.GetUserByName(orgId, userName) == null || await repository!.GetUserInOrganization(userName) == null)
                throw new KeyNotFoundException("1102");
            return true;
        }
    }
}
