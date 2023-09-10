using System;
using System.Data.SqlTypes;

namespace Arslan.Vms.InventoryService.Accounts.TransactionTypes
{
    public class TransactionOrdering : IComparable<TransactionOrdering>
    {
        public DateTime DateTime
        {
            get;
            set;
        }

        public static TransactionOrdering DummyValue
        {
            get
            {
                return new TransactionOrdering((DateTime)SqlDateTime.MinValue, 0, Guid.Empty, Guid.Empty);
            }
        }

        public TransactionId Id
        {
            get;
            set;
        }

        public TransactionOrdering()
        {
        }

        public TransactionOrdering(DateTime date, int transactionType, Guid transactionEntityId, Guid? transactionChildId)
        {
            Id = new TransactionId(transactionType, transactionEntityId, transactionChildId);
            DateTime = date;
        }

        public TransactionOrdering(TransactionId id, DateTime date)
        {
            Id = id;
            DateTime = date;
        }

        public int CompareTo(TransactionOrdering ordering2)
        {
            SqlDateTime sqlDateTime = new SqlDateTime(DateTime);
            int ınt32 = sqlDateTime.CompareTo(new SqlDateTime(ordering2.DateTime));
            if (ınt32 != 0)
            {
                return ınt32;
            }
            return Id.CompareTo(ordering2.Id);
        }

        public static int Comparison(TransactionOrdering emp1, TransactionOrdering emp2)
        {
            if (ReferenceEquals(null, emp1) && ReferenceEquals(null, emp2))
            {
                return 0;
            }
            if (ReferenceEquals(null, emp1))
            {
                return -1;
            }
            if (ReferenceEquals(null, emp2))
            {
                return 1;
            }
            return emp1.CompareTo(emp2);
        }

        public bool Equals(TransactionOrdering ordering2)
        {
            return this == ordering2;
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
            if (obj.GetType() != typeof(TransactionOrdering))
            {
                return false;
            }
            return Equals((TransactionOrdering)obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0) * 397 ^ DateTime.GetHashCode();
        }

        public static bool operator ==(TransactionOrdering ordering1, TransactionOrdering ordering2)
        {
            return Comparison(ordering1, ordering2) == 0;
        }

        public static bool operator >(TransactionOrdering ordering1, TransactionOrdering ordering2)
        {
            return Comparison(ordering1, ordering2) > 0;
        }

        public static bool operator >=(TransactionOrdering ordering1, TransactionOrdering ordering2)
        {
            return Comparison(ordering1, ordering2) >= 0;
        }

        public static bool operator !=(TransactionOrdering ordering1, TransactionOrdering ordering2)
        {
            return Comparison(ordering1, ordering2) != 0;
        }

        public static bool operator <(TransactionOrdering ordering1, TransactionOrdering ordering2)
        {
            return Comparison(ordering1, ordering2) < 0;
        }

        public static bool operator <=(TransactionOrdering ordering1, TransactionOrdering ordering2)
        {
            return Comparison(ordering1, ordering2) <= 0;
        }
    }
}