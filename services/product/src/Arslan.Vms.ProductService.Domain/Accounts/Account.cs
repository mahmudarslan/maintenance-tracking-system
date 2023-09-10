using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Accounts
{
    public class Account : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public int AccountType { get; set; }
        public Guid? Attr1 { get; set; }
        public Guid? Attr2 { get; set; }
        public Guid? CurrencyId { get; set; }
        public Guid? ProductId { get; set; }
        public string Sublocation { get; set; }

        protected Account() { }

        protected Account(int id)
        {
            AccountType = id;
        }

        public Account(Guid id, Guid? tenantId, int accountType, Guid? attr1, Guid? attr2, Guid? currencyId, Guid? productId, string subLocation) : base(id)
        {
            AccountType = accountType;
            Attr1 = attr1;
            Attr2 = attr2;
            CurrencyId = currencyId;
            ProductId = productId;
            Sublocation = subLocation;
            TenantId = tenantId;
        }

        public Account SetProductId(Guid id)
        {
            ProductId = id;
            return this;
        }

        public virtual bool IdInitialized
        {
            get
            {
                return Id != Guid.Empty;
            }
        }

        //public virtual Account GetAggregateAccount()
        //{
        //    return null;
        //}


        //[NotMapped]
        //public virtual Guid SetId
        //{
        //    set { Id = value; }
        //}


        public bool Equals(Account other)
        {
            if (other == null)
            {
                return false;
            }
            if (this == other)
            {
                return true;
            }
            if (other.AccountType != AccountType ||
                other.Attr1 != Attr1 ||
                other.Attr2 != Attr2 ||
                !Equals(other.ProductId, ProductId)) //||
                                                     //!SqlStringComparer.StaticEquals(other.Sublocation, Sublocation))
            {
                return false;
            }
            var currencyId = other.CurrencyId;
            object obj = currencyId.HasValue ? currencyId.GetValueOrDefault() : Guid.Empty;
            currencyId = CurrencyId;
            return Equals(obj, currencyId.HasValue ? currencyId.GetValueOrDefault() : Guid.Empty);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this == obj)
            {
                return true;
            }
            if (!(obj is Account))
            {
                return false;
            }
            return Equals((Account)obj);
        }

        public override int GetHashCode()
        {
            //int accountType = (
            //    (((this.AccountType * 397 ^ this.Attr1) * 397 ^ this.Attr2) * 397 ^ 
            //    (this.ProductTag != null ? this.ProductTag.GetHashCode() : 0)) * 397 ^
            //    (this.Sublocation != null ? this.Sublocation.TrimEnd(new char[0]).ToLowerInvariant().GetHashCode() : 0)) * 397;

            //var currencyId = this.CurrencyId;
            //return accountType ^ (currencyId.HasValue ? currencyId.GetValueOrDefault() : Guid.Empty);


            //int accountType = (
            //(((this.AccountType * 397) * 397) * 397 ^
            //(this.ProductTag != null ? this.ProductTag.GetHashCode() : 0)) * 397 ^
            //(this.Sublocation != null ? this.Sublocation.TrimEnd(new char[0]).ToLowerInvariant().GetHashCode() : 0)) * 397;

            //var currencyId = this.CurrencyId;
            //return accountType  ;

            return 1;
        }
    }
}


//public Account(int accountType)
//{
//    AccountType = (AccountType)accountType;
//}

//[NotMapped]
//public ProductTag ProductTag { get; set; }

//public Account(int accountType)
//{
//    this.AccountType = accountType;
//    this.Sublocation = string.Empty;
//}

//public Account(int accountType, ProductTag productTag, int attr1, int attr2, string sublocation, int? currencyId)
//{
//    //this.ProductTag = productTag;
//    this.AccountType = accountType;
//    this.Attr1 = attr1;
//    this.Attr2 = attr2;
//    this.Sublocation = (sublocation ?? "").Trim();
//    this.CurrencyId = currencyId;
//}

//public override string ToString()
//{
//    return string.Format("Account Type={0}, CurrencyId={1}", Enum.GetName(typeof(AccountType), this.AccountType), this.CurrencyId);
//}