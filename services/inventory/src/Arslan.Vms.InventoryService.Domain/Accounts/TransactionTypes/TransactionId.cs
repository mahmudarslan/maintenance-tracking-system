using System;

namespace Arslan.Vms.InventoryService.Accounts.TransactionTypes
{
    [Serializable]
    public class TransactionId : IComparable<TransactionId>
    {
        public Guid? ChildId { get; set; }
        public Guid EntityId { get; set; }
        public int Type { get; set; }

        protected TransactionId(TransactionId other)
        {
            Type = other.Type;
            EntityId = other.EntityId;
            ChildId = other.ChildId;
        }

        public TransactionId(int type, Guid entityId)
        {
            Type = type;
            EntityId = entityId;
            ChildId = null;
        }

        public TransactionId(int type, Guid entityId, Guid? childId)
        {
            Type = type;
            EntityId = entityId;
            ChildId = childId;
        }

        public TransactionId()
        {
        }

        public int CompareTo(TransactionId other)
        {
            if (Type.CompareTo(other.Type) != 0)
            {
                return 1;
            }

            if (EntityId != other.EntityId)
            {
                return 1;
            }

            return ChildId != other.ChildId ? 1 : 0;
        }

        public bool Equals(TransactionId other)
        {
            if (other == null)
            {
                return false;
            }
            if (this == other)
            {
                return true;
            }
            if (other.Type != Type || other.EntityId != EntityId)
            {
                return false;
            }
            return other.ChildId == ChildId;
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
            if (obj.GetType() != typeof(TransactionId))
            {
                return false;
            }
            return Equals((TransactionId)obj);
        }

        //public override int GetHashCode()
        //{
        //    return (this.Type * 397 ^ this.EntityId) * 397 ^ this.ChildId;
        //}
    }
}
