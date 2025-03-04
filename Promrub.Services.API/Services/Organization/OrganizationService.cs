﻿using AutoMapper;
using Promrub.Services.API.Entities;
using Promrub.Services.API.Interfaces;
using Promrub.Services.API.Models.RequestModels.Organization;
using Promrub.Services.API.Models.ResponseModels.Organization;
using Promrub.Services.API.Services.User;
using System.Runtime.CompilerServices;

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

        public async Task<bool> UpdateOrganization(string orgId, OrganizationRequest org)
        {
            var customOrgId = org.OrgCustomId;
            repository!.SetCustomOrgId(customOrgId!);
            var orgDetail = await repository.GetOrganization(); 
            orgDetail.OrgName = !string.IsNullOrEmpty(org.OrgName) ? org.OrgName : orgDetail.OrgName;
            orgDetail.TaxId = !string.IsNullOrEmpty(org.TaxId) ? org.TaxId : orgDetail.TaxId;
            orgDetail.DisplayName = !string.IsNullOrEmpty(org.DisplayName) ? org.DisplayName : orgDetail.DisplayName;
            orgDetail.CallbackUrl = !string.IsNullOrEmpty(org.CallBackUrl) ? org.CallBackUrl : orgDetail.CallbackUrl;
            orgDetail.OrgLogo = !string.IsNullOrEmpty(org.OrgLogo) ? org.OrgLogo : orgDetail.OrgLogo;
            orgDetail.FullAddress = !string.IsNullOrEmpty(org.FullAddress) ? org.FullAddress : orgDetail.FullAddress;
            orgDetail.TelNo = !string.IsNullOrEmpty(org.TelNo) ? org.TelNo : orgDetail.TelNo;
            orgDetail.Website = !string.IsNullOrEmpty(org.Website) ? org.Website : orgDetail.Website;
            orgDetail.Email = !string.IsNullOrEmpty(org.Email) ? org.Email : orgDetail.Email;
            orgDetail.OrgAbbr = !string.IsNullOrEmpty(org.OrgAbbr) ? org.OrgAbbr : orgDetail.OrgAbbr;
            repository.Commit();
            return true;
        }
        public async Task<bool> UpdateSecurity(string orgId, OrganizationRequest org)
        {
            var customOrgId = org.OrgCustomId;
            repository!.SetCustomOrgId(customOrgId!);
            var orgDetail = await repository.GetOrganization();
            orgDetail.Security = org.Security;
            orgDetail.SecurityCredential = org.SecurityCredential;
            orgDetail.SecurityPassword = org.SecurityPassword;
            repository.Commit();
            return true;
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
