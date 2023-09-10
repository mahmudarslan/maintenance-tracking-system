using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Companies
{
    public class Company : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid? LogoAttachmentId { get; set; }
        public string Name { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string PhoneNumber { get; protected set; }
        public virtual string FaxNumber { get; protected set; }
        public virtual string TaxNumber { get; protected set; }
        public virtual string WebsiteUrl { get; protected set; }
        public virtual List<CompanyAddress> Addresses { get; protected set; }
        public virtual List<CompanyAttachment> Attachments { get; protected set; }


        protected Company() { }

        public Company(Guid id, Guid? tenantId, Guid? logoAttachmentId, [NotNull] string name, string email = "", string phoneNumber = "", string faxNumber = "", string taxNumber = "", string websiteUrl = "") : base(id)
        {
            TenantId = tenantId;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
            TaxNumber = taxNumber;
            WebsiteUrl = websiteUrl;
            LogoAttachmentId = logoAttachmentId;
            Addresses = new List<CompanyAddress>();
            Attachments = new List<CompanyAttachment>();
        }

        public void AddAddress(Guid addressId)
        {
            Addresses.Add(new CompanyAddress(TenantId, Id, addressId));
        }

        public void AddAttachment(Guid fileAttachmentId)
        {
            Attachments.Add(new CompanyAttachment(TenantId, Id, fileAttachmentId));
        }

        //public void AddPreference(Guid id, Guid? currencyId, Guid? costingMethodId, Guid? measureLenghtId, Guid? measureWeightId)
        //{
        //    CompanyPreference = new CompanyPreference(id, TenantId, Id, currencyId, costingMethodId, measureLenghtId, measureWeightId);
        //}

        public virtual Company SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            return this;
        }

        public virtual Company SetEmail([NotNull] string email)
        {
            Email = Check.NotNullOrWhiteSpace(email, nameof(email));
            return this;
        }

        public virtual Company SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            return this;
        }

        public virtual Company SetFaxNumber(string faxNumber)
        {
            FaxNumber = faxNumber;
            return this;
        }

        public virtual Company SetTaxNumber(string taxNumber)
        {
            TaxNumber = taxNumber;
            return this;
        }

        public virtual Company SetWebsiteUrl(string websiteUrl)
        {
            WebsiteUrl = websiteUrl;
            return this;
        }

    }
}