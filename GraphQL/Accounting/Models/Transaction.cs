namespace ConferencePlanner.GraphQL.Accounting.Models
{
    using System;
    using System.Collections.Generic;

    public class Transaction
    {
        public Transaction(string code, DateTime transactionDate)
        {
            Id = 0;
            Code = code;
            TransactionDate = transactionDate;
        }

        private Transaction() : this("ERR_TRANSAC", default)
        {
            // JUST FOR EF
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime TransactionDate { get; set; }

        public ICollection<JournalEntry>? JournalEntries { get; set; }
    }
}