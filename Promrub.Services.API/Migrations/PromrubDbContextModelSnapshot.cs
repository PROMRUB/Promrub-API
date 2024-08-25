﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Promrub.Services.API.PromServiceDbContext;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    [DbContext(typeof(PromrubDbContext))]
    partial class PromrubDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Promrub.Services.API.Entities.ApiKeyEntity", b =>
                {
                    b.Property<Guid?>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("key_id");

                    b.Property<string>("ApiKey")
                        .HasColumnType("text")
                        .HasColumnName("api_key");

                    b.Property<DateTime?>("KeyCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("key_created_date");

                    b.Property<string>("KeyDescription")
                        .HasColumnType("text")
                        .HasColumnName("key_description");

                    b.Property<DateTime?>("KeyExpiredDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("key_expired_date");

                    b.Property<string>("OrgId")
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.Property<string>("RedirectUrl")
                        .HasColumnType("text")
                        .HasColumnName("redirect_url");

                    b.Property<string>("RolesList")
                        .HasColumnType("text")
                        .HasColumnName("roles_list");

                    b.HasKey("KeyId");

                    b.HasIndex("ApiKey")
                        .IsUnique();

                    b.HasIndex("OrgId");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.BankEntity", b =>
                {
                    b.Property<Guid?>("BankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("bank_id");

                    b.Property<string>("BankAbbr")
                        .HasColumnType("text")
                        .HasColumnName("bank_abbr");

                    b.Property<int?>("BankCode")
                        .HasColumnType("integer")
                        .HasColumnName("bank_code");

                    b.Property<string>("BankNameEn")
                        .HasColumnType("text")
                        .HasColumnName("bank_name_en");

                    b.Property<string>("BankNameTh")
                        .HasColumnType("text")
                        .HasColumnName("bank_name_th");

                    b.Property<string>("BankSwiftCode")
                        .HasColumnType("text")
                        .HasColumnName("bank_swift_code");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.HasKey("BankId");

                    b.ToTable("Bank");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.DistrictEntity", b =>
                {
                    b.Property<Guid?>("DistrictId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("district_id");

                    b.Property<int?>("DistrictCode")
                        .HasColumnType("integer")
                        .HasColumnName("district_code");

                    b.Property<string>("DistrictNameEn")
                        .HasColumnType("text")
                        .HasColumnName("district_name_en");

                    b.Property<string>("DistrictNameTh")
                        .HasColumnType("text")
                        .HasColumnName("district_name_th");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text")
                        .HasColumnName("postal_code");

                    b.Property<int?>("ProvinceCode")
                        .HasColumnType("integer")
                        .HasColumnName("province_code");

                    b.HasKey("DistrictId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.OrganizationEntity", b =>
                {
                    b.Property<Guid?>("OrgId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<string>("BrnId")
                        .HasColumnType("text")
                        .HasColumnName("branch_id");

                    b.Property<string>("CallbackUrl")
                        .HasColumnType("text")
                        .HasColumnName("callback_url");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("District")
                        .HasColumnType("text")
                        .HasColumnName("district");

                    b.Property<bool?>("HvCard")
                        .HasColumnType("boolean")
                        .HasColumnName("hv_card");

                    b.Property<bool?>("HvMobileBanking")
                        .HasColumnType("boolean")
                        .HasColumnName("hv_mobile_banking");

                    b.Property<bool?>("HvPromptPay")
                        .HasColumnType("boolean")
                        .HasColumnName("hv_promtpay");

                    b.Property<string>("No")
                        .HasColumnType("text")
                        .HasColumnName("house_no");

                    b.Property<DateTime?>("OrgCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("org_created_date");

                    b.Property<string>("OrgCustomId")
                        .HasColumnType("text")
                        .HasColumnName("org_custom_id");

                    b.Property<string>("OrgDescription")
                        .HasColumnType("text")
                        .HasColumnName("org_description");

                    b.Property<string>("OrgLogo")
                        .HasColumnType("text")
                        .HasColumnName("org_logo");

                    b.Property<string>("OrgName")
                        .HasColumnType("text")
                        .HasColumnName("org_name");

                    b.Property<string>("PostCode")
                        .HasColumnType("text")
                        .HasColumnName("post_code");

                    b.Property<string>("Provice")
                        .HasColumnType("text")
                        .HasColumnName("provice");

                    b.Property<string>("Road")
                        .HasColumnType("text")
                        .HasColumnName("road");

                    b.Property<int>("Security")
                        .HasColumnType("integer")
                        .HasColumnName("authorization_type");

                    b.Property<string>("SecurityCredential")
                        .HasColumnType("text")
                        .HasColumnName("security_credential");

                    b.Property<string>("SecurityPassword")
                        .HasColumnType("text")
                        .HasColumnName("security_password");

                    b.Property<string>("SubDistrict")
                        .HasColumnType("text")
                        .HasColumnName("sub_district");

                    b.Property<string>("TaxId")
                        .HasColumnType("text")
                        .HasColumnName("tax_id");

                    b.HasKey("OrgId");

                    b.HasIndex("OrgCustomId")
                        .IsUnique();

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.OrganizationUserEntity", b =>
                {
                    b.Property<Guid?>("OrgUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("org_user_id");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("OrgCustomId")
                        .HasColumnType("text")
                        .HasColumnName("org_custom_id");

                    b.Property<string>("RolesList")
                        .HasColumnType("text")
                        .HasColumnName("roles_list");

                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("OrgUserId");

                    b.HasIndex("OrgCustomId");

                    b.HasIndex(new[] { "OrgCustomId", "UserId" }, "OrgUser_Unique1")
                        .IsUnique();

                    b.ToTable("OrganizationsUsers");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.PaymentChannelEntity", b =>
                {
                    b.Property<Guid?>("PaymentChannelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("payment_channel_id");

                    b.Property<int?>("BankCode")
                        .HasColumnType("integer")
                        .HasColumnName("bank_code");

                    b.Property<string>("BillerId")
                        .HasColumnType("text")
                        .HasColumnName("biller_id");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_at");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<bool?>("IsExist")
                        .HasColumnType("boolean")
                        .HasColumnName("is_exist");

                    b.Property<string>("OrgId")
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.Property<int?>("PaymentChannelType")
                        .HasColumnType("integer")
                        .HasColumnName("payment_channel_type");

                    b.HasKey("PaymentChannelId");

                    b.ToTable("PaymentChannels");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.PaymentMethodEntity", b =>
                {
                    b.Property<Guid?>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("payment_method_id");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<int?>("PaymentMethodCode")
                        .HasColumnType("integer")
                        .HasColumnName("payment_method_code");

                    b.Property<string>("PaymentMethodNameEn")
                        .HasColumnType("text")
                        .HasColumnName("payment_method_name_en");

                    b.Property<string>("PaymentMethodNameTh")
                        .HasColumnType("text")
                        .HasColumnName("payment_method_name_th");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.PaymentTransactionEntity", b =>
                {
                    b.Property<Guid?>("PaymentTransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("payment_transaction_id");

                    b.Property<string>("ApiKey")
                        .HasColumnType("text")
                        .HasColumnName("api_Key");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_at");

                    b.Property<int>("ItemTotal")
                        .HasColumnType("integer")
                        .HasColumnName("item_total");

                    b.Property<string>("OrgId")
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.Property<int?>("PaymentStatus")
                        .HasColumnType("integer")
                        .HasColumnName("payment_status");

                    b.Property<string>("PosId")
                        .HasColumnType("text")
                        .HasColumnName("pos_id");

                    b.Property<int>("QuantityTotal")
                        .HasColumnType("integer")
                        .HasColumnName("quantity_total");

                    b.Property<decimal?>("ReceiptAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("receipt_amount");

                    b.Property<DateTime?>("ReceiptDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("receipt_date");

                    b.Property<string>("ReceiptNo")
                        .HasColumnType("text")
                        .HasColumnName("receipt_no");

                    b.Property<string>("RefTransactionId")
                        .HasColumnType("text")
                        .HasColumnName("ref_transaction_id");

                    b.Property<decimal>("TotalDiscount")
                        .HasColumnType("numeric")
                        .HasColumnName("total_discount");

                    b.Property<decimal>("TotalItemsPrices")
                        .HasColumnType("numeric")
                        .HasColumnName("totalItems_prices");

                    b.Property<decimal>("TotalTransactionPrices")
                        .HasColumnType("numeric")
                        .HasColumnName("total_transaction_prices");

                    b.Property<string>("TransactionId")
                        .HasColumnType("text")
                        .HasColumnName("transaction_id");

                    b.HasKey("PaymentTransactionId");

                    b.ToTable("PaymenTransactions");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.PaymentTransactionItemEntity", b =>
                {
                    b.Property<Guid?>("PaymentTransactionItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("payment_transaction_item_id");

                    b.Property<string>("ItemName")
                        .HasColumnType("text")
                        .HasColumnName("item_name");

                    b.Property<string>("OrgId")
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.Property<Guid?>("PaymentTransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("payment_transaction_id");

                    b.Property<decimal?>("Percentage")
                        .HasColumnType("numeric")
                        .HasColumnName("percentage");

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<decimal?>("TotalDiscount")
                        .HasColumnType("numeric")
                        .HasColumnName("total_discount");

                    b.Property<decimal?>("TotalPrices")
                        .HasColumnType("numeric")
                        .HasColumnName("total_prices");

                    b.HasKey("PaymentTransactionItemId");

                    b.ToTable("PaymentTransactionItems");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.PosEntity", b =>
                {
                    b.Property<Guid>("PosId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("pos_id");

                    b.Property<string>("OrgId")
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.Property<string>("PosKey")
                        .HasColumnType("text")
                        .HasColumnName("pos_key");

                    b.HasKey("PosId");

                    b.HasIndex("PosId")
                        .IsUnique();

                    b.ToTable("Pos");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.ProvinceEntity", b =>
                {
                    b.Property<Guid?>("ProviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("provice_id");

                    b.Property<int?>("ProvinceCode")
                        .HasColumnType("integer")
                        .HasColumnName("provice_code");

                    b.Property<string>("ProvinceNameEn")
                        .HasColumnType("text")
                        .HasColumnName("province_name_en");

                    b.Property<string>("ProvinceNameTh")
                        .HasColumnType("text")
                        .HasColumnName("province_name_th");

                    b.HasKey("ProviceId");

                    b.ToTable("Provices");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.ReceiptNumbersEntity", b =>
                {
                    b.Property<Guid?>("ReceiptId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("reciept_id");

                    b.Property<int?>("Allocated")
                        .HasColumnType("integer")
                        .HasColumnName("allocated");

                    b.Property<string>("OrgId")
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.Property<string>("ReceiptDate")
                        .HasColumnType("text")
                        .HasColumnName("reciept_date");

                    b.HasKey("ReceiptId");

                    b.ToTable("RecieptNo");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.ReceiptPaymentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ReceiptId")
                        .HasColumnType("uuid")
                        .HasColumnName("receipt_id");

                    b.Property<string>("ReceiptNo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("receipt_no");

                    b.Property<Guid?>("ReceiptScheduleEntityReceiptId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReceiptId");

                    b.HasIndex("ReceiptScheduleEntityReceiptId");

                    b.ToTable("ReceiptReceipt");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.ReceiptScheduleEntity", b =>
                {
                    b.Property<Guid>("ReceiptId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("receipt_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<string>("PosId")
                        .HasColumnType("text")
                        .HasColumnName("pos_id");

                    b.Property<DateTime>("ReceiptDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("receipt_date");

                    b.Property<string>("ReceiveNo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("receive_no");

                    b.HasKey("ReceiptId");

                    b.ToTable("ReceiptScheduleEntity");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.RoleEntity", b =>
                {
                    b.Property<Guid?>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.Property<DateTime?>("RoleCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("role_created_date");

                    b.Property<string>("RoleDefinition")
                        .HasColumnType("text")
                        .HasColumnName("role_definition");

                    b.Property<string>("RoleDescription")
                        .HasColumnType("text")
                        .HasColumnName("role_description");

                    b.Property<string>("RoleLevel")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_level");

                    b.Property<string>("RoleName")
                        .HasColumnType("text")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId");

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.SubDistrictEntity", b =>
                {
                    b.Property<Guid?>("SubDistrictId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("sub_district_id");

                    b.Property<int?>("DistrictCode")
                        .HasColumnType("integer")
                        .HasColumnName("district_code");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text")
                        .HasColumnName("postal_code");

                    b.Property<int?>("ProvinceCode")
                        .HasColumnType("integer")
                        .HasColumnName("province_code");

                    b.Property<int?>("SubDistrictCode")
                        .HasColumnType("integer")
                        .HasColumnName("sub_district_code");

                    b.Property<string>("SubDistrictNameEn")
                        .HasColumnType("text")
                        .HasColumnName("sub_district_name_en");

                    b.Property<string>("SubDistrictNameTh")
                        .HasColumnType("text")
                        .HasColumnName("sub_district_name_th");

                    b.HasKey("SubDistrictId");

                    b.ToTable("SubDistricts");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.TaxReceiptEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PaymentTransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("payment_transaction");

                    b.Property<string>("ReceiptNo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("receipt_no");

                    b.Property<Guid>("TaxId")
                        .HasColumnType("uuid")
                        .HasColumnName("tax_id");

                    b.Property<string>("TotalReceipt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("total_receipt");

                    b.HasKey("Id");

                    b.HasIndex("PaymentTransactionId");

                    b.HasIndex("TaxId");

                    b.ToTable("TaxReceipt");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.TaxScheduleEntity", b =>
                {
                    b.Property<Guid>("TaxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("tax_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<string>("PosId")
                        .HasColumnType("text")
                        .HasColumnName("pos_id");

                    b.Property<DateTime>("TaxDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("tax_date");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("total_amount");

                    b.Property<string>("TotalReceipt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("total_receipt");

                    b.Property<decimal>("Vat")
                        .HasColumnType("numeric")
                        .HasColumnName("vat");

                    b.HasKey("TaxId");

                    b.ToTable("TaxScheduleEntity");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.UserEntity", b =>
                {
                    b.Property<Guid?>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<DateTime?>("UserCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("user_created_date");

                    b.Property<string>("UserEmail")
                        .HasColumnType("text")
                        .HasColumnName("user_email");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("UserId");

                    b.HasIndex("UserEmail")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.ReceiptPaymentEntity", b =>
                {
                    b.HasOne("Promrub.Services.API.Entities.ReceiptScheduleEntity", "Receipt")
                        .WithMany()
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Promrub.Services.API.Entities.ReceiptScheduleEntity", null)
                        .WithMany("Item")
                        .HasForeignKey("ReceiptScheduleEntityReceiptId");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.TaxReceiptEntity", b =>
                {
                    b.HasOne("Promrub.Services.API.Entities.PaymentTransactionEntity", "PaymentTransaction")
                        .WithMany()
                        .HasForeignKey("PaymentTransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Promrub.Services.API.Entities.TaxScheduleEntity", "Tax")
                        .WithMany()
                        .HasForeignKey("TaxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentTransaction");

                    b.Navigation("Tax");
                });

            modelBuilder.Entity("Promrub.Services.API.Entities.ReceiptScheduleEntity", b =>
                {
                    b.Navigation("Item");
                });
#pragma warning restore 612, 618
        }
    }
}
