using Arslan.Vms.ProductService.Accounts.AccountTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Services;

namespace Arslan.Vms.ProductService.Accounts.TransactionTypes
{
    public class Transaction : ITransaction
    {
        public List<Entry> AllEntries { get; set; }
        public DateTime Date { get; set; }
        public TransactionId Id { get; set; }
        public TransactionOrdering Ordering => new(Id, Date);

        private readonly IAccountTypeProvider _accountTypeProvider;

        public Transaction(TransactionId id, DateTime date, List<Entry> simpleOrAllEntries, bool calculateAggregates)
        {
            _accountTypeProvider = new AccountTypeProvider();

            Id = id;
            Date = date;
            var array = simpleOrAllEntries;

            if (calculateAggregates)
            {
                if (!IsBalanced(array))
                {
                    throw new ArgumentException(string.Concat("Simple entries to transaction are not balanced", array));
                }
                //array.AddRange(GetAggregates(array));
            }

            AllEntries = MergeEntries(array);
        }

        public List<Entry> GetAggregates(List<Entry> primitiveEntries)
        {
            return _accountTypeProvider.GetAggregates(primitiveEntries);
        }

        public static bool IsBalanced(List<Entry> entries)
        {
            if (entries == null)
            {
                return true;
            }

            decimal num = new decimal(1, 0, 0, false, 5);

            var amount = Math.Abs((
                from e in entries
                where ((AccountType)e.Account.AccountType).IsSimpleInventory()
                select e).Sum((e) => e.Amount));

            if (amount > num)
            {
                return false;
            }

            return true;
        }

        public static List<Entry> MergeEntries(List<Entry> entries)
        {
            if (OneEntryPerAccount(entries))
            {
                return entries;
            }

            return
                (from x in entries
                 group x by x.Account into g
                 select new Entry(g.Key, g.Sum((x) => x.Amount))).ToList();
        }

        public static bool OneEntryPerAccount(List<Entry> entries)
        {
            var data = entries.Select(s => s.Account).Distinct().ToList();

            return data.Count == entries.Count;
        }
    }
}