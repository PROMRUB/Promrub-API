using Newtonsoft.Json;
using PasswordGenerator;
using Promrub.Services.API.Entities;
using Promrub.Services.API.PromServiceDbContext;
using Serilog;
using System.Xml.Linq;

namespace Promrub.Services.API.Seeder
{
    public class DataSeeder
    {
        private readonly PromrubDbContext context;
        private readonly Password pwd = new Password(32);

        public DataSeeder(PromrubDbContext context)
        {
            this.context = context;
        }

        private void SeedDefaultOrganization()
        {
            if (context == null)
            {
                return;
            }

            if (context.Organizations == null)
            {
                return;
            }

            if (!context.Organizations.Any())
            {
                var orgs = new List<OrganizationEntity>()
            {
                new OrganizationEntity
                {
                    OrgId = Guid.NewGuid(),
                    OrgName = "DEFAULT",
                    OrgDescription = "Default initial created organization",
                    OrgCreatedDate = DateTime.UtcNow,
                    OrgCustomId = "default"
                }
            };

                context.Organizations.AddRange(orgs);
                context.SaveChanges();
            }
        }

        private void SeedGlobalOrganization()
        {
            if (context == null)
            {
                return;
            }

            if (context.Organizations == null)
            {
                return;
            }

            string orgId = "global";

            var query = context.Organizations!.Where(x => x.OrgCustomId!.Equals(orgId)).FirstOrDefault();
            if (query == null)
            {
                var orgs = new List<OrganizationEntity>()
            {
                new OrganizationEntity
                {
                    OrgId = Guid.NewGuid(),
                    OrgName = "GLOBAL",
                    OrgDescription = "Global/Root initial created organization",
                    OrgCreatedDate = DateTime.UtcNow,
                    OrgCustomId = orgId
                }
            };
                context.Organizations.AddRange(orgs);

                var apiKey = new ApiKeyEntity()
                {
                    KeyId = Guid.NewGuid(),
                    KeyCreatedDate = DateTime.UtcNow,
                    OrgId = orgId,
                    ApiKey = pwd.Next(),
                    KeyDescription = "Auto created root key"
                };
                context.ApiKeys!.Add(apiKey);

                context.SaveChanges();
            }
        }

        private void UpdateDefaultOrganizationCustomId()
        {
            if (context == null)
            {
                return;
            }

            if (context.Organizations == null)
            {
                return;
            }

            var query = context.Organizations!.Where(x => x.OrgName!.Equals("DEFAULT")).FirstOrDefault();
            if (query == null)
            {
                Log.Error("Default organization 'DEFAULT' not found!!!");
                return;
            }
            query.OrgCustomId = "default";
            context.SaveChanges();
        }

        private void AddRole(string name, string definition, string level, string desc)
        {
            var query = context.Roles!.Where(x => x.RoleName!.Equals(name)).FirstOrDefault();
            if (query != null)
            {
                return;
            }

            var r = new RoleEntity()
            {
                RoleName = name,
                RoleDefinition = definition,
                RoleLevel = level,
                RoleDescription = desc
            };

            context!.Roles!.Add(r);
        }

        private void SeedDefaultRoles()
        {
            AddRole("CREATOR", "Admin:CreateOrganization,ApiKey:AddApiKey", "ADMIN", "Organization creator");
            AddRole("OWNER", ".+:.+", "ORGANIZATION", "Organization Owner");
            AddRole("VIEWER", ".+:Get.+", "ORGANIZATION", "Organization Viewer");
            AddRole("EDITOR", ".+:Add.+,.+:Update.+,.+:Delete.+", "ORGANIZATION", "Organization Editor");
            AddRole("UPLOADER", "FileUpload:Upload.+Image", "ORGANIZATION", "Organization File Uploader");

            context.SaveChanges();
        }

        private void UpdateApiKeyRole()
        {
            var apiKeys = context.ApiKeys!.Where(x => x.RolesList!.Equals(null) || x.RolesList!.Equals("")).ToList();
            apiKeys.ForEach(a => a.RolesList = "CREATOR");
            context.SaveChanges();
        }

        private void SeedDefaultRoles2()
        {
            AddRole("USER_ORGS_VIEWER", "Organization:AdminGetUserAllowedOrganization", "ADMIN", "Allow only for AdminGetUserAllowedOrganization");
            context.SaveChanges();
        }

        public void SeedProvinces()
        {
            using (StreamReader r = new StreamReader("Seeder//JsonData//provinces.json"))
            {
                string json = r.ReadToEnd();
                List<ProvinceEntity> items = JsonConvert.DeserializeObject<List<ProvinceEntity>>(json);
                foreach(var item in items)
                {
                    var query = context.Provinces!.Where(x => x.ProvinceCode!.Equals(item.ProvinceCode)).FirstOrDefault();
                    if (query == null)
                    {
                        context.Provinces!.Add(item);
                    }
                }
                context.SaveChanges();
            }
        }

        public void SeedDistrict()
        {
            using (StreamReader r = new StreamReader("Seeder//JsonData//districts.json"))
            {
                string json = r.ReadToEnd();
                List<DistrictEntity> items = JsonConvert.DeserializeObject<List<DistrictEntity>>(json);
                foreach (var item in items)
                {
                    var query = context.District!.Where(x => x.DistrictCode!.Equals(item.DistrictCode)).FirstOrDefault();
                    if (query == null)
                    {
                        context.District!.Add(item);
                    }
                }
                context.SaveChanges();
            }
        }

        public void SeedSubDistrict()
        {
            using (StreamReader r = new StreamReader("Seeder//JsonData//subdistricts.json"))
            {
                string json = r.ReadToEnd();
                List<SubDistrictEntity> items = JsonConvert.DeserializeObject<List<SubDistrictEntity>>(json);
                foreach (var item in items)
                {
                    var query = context.SubDistrict!.Where(x => x.SubDistrictCode!.Equals(item.SubDistrictCode)).FirstOrDefault();
                    if (query == null)
                    {
                        context.SubDistrict!.Add(item);
                    }
                }
                context.SaveChanges();
            }
        }

        public void SeedBank()
        {
            using (StreamReader r = new StreamReader("Seeder//JsonData//banks.json"))
            {
                string json = r.ReadToEnd();
                List<BankEntity> items = JsonConvert.DeserializeObject<List<BankEntity>>(json);
                foreach (var item in items)
                {
                    var query = context.Banks!.Where(x => x.BankAbbr!.Equals(item.BankAbbr)).FirstOrDefault();
                    if (query == null)
                    {
                        context.Banks!.Add(item);
                    }
                }
                context.SaveChanges();
            }
        }

        public void SeedPaymentMethod()
        {
            using (StreamReader r = new StreamReader("Seeder//JsonData//paymentmethods.json"))
            {
                string json = r.ReadToEnd();
                List<PaymentMethodEntity> items = JsonConvert.DeserializeObject<List<PaymentMethodEntity>>(json);
                foreach (var item in items)
                {
                    var query = context.PaymentMethods!.Where(x => x.PaymentMethodCode == item.PaymentMethodCode).FirstOrDefault();
                    if (query == null)
                    {
                        context.PaymentMethods!.Add(item);
                    }
                }
                context.SaveChanges();
            }
        }

        public void Seed()
        {
            SeedDefaultOrganization();
            UpdateDefaultOrganizationCustomId();

            SeedGlobalOrganization();
            SeedDefaultRoles();
            UpdateApiKeyRole();
            SeedDefaultRoles2();

            SeedProvinces();
            SeedDistrict();
            SeedSubDistrict();
            SeedBank();
            SeedPaymentMethod();
        }
    }
}
