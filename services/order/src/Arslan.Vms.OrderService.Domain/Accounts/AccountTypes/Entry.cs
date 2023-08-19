using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class Entry
    {
        public Account Account
        {
            get;
            //private set;
            set;
        }

        public Guid AccountId
        {
            get
            {
                if (Account == null)
                {
                    throw new NotSupportedException("You have to initialize the account for this entry");
                }
                return Account.Id;
            }
        }

        public decimal Amount
        {
            get;
            //private 
            set;
        }

        public Entry(Account account, decimal amount)
        {
            Account = account;
            Amount = amount;
        }

        private Entry()
        {
        }



        public bool Equals(Entry other)
        {
            if (other == null)
            {
                return false;
            }
            if (this == other)
            {
                return true;
            }
            if (!Equals(other.Account, Account))
            {
                return false;
            }
            return other.Amount == Amount;
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
            if (obj.GetType() != typeof(Entry))
            {
                return false;
            }
            return Equals((Entry)obj);
        }

        public override int GetHashCode()
        {
            return (Account != null ? Account.GetHashCode() : 0) * 397 ^ Amount.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Entry Amount={0}, Account={1}", Amount, Account);
        }
    }
}
