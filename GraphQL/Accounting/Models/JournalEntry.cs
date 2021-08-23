namespace ConferencePlanner.GraphQL.Accounting.Models
{
    using System;
    using System.Collections.Generic;

    public class JournalEntry
    {
        public JournalEntry(string detail, DateTime date)
        {
            Date = date;
            Detail = detail;
            LastEdit = DateTime.Now;
            Consolidated = false;
        }

        private JournalEntry() : this("UNINITIALIZED_ENTRY", DateTime.MinValue)
        {
            JournalEntryLines = new List<JournalEntryLine>();
        }

        public int Id { get; set; }

        /// <summary>
        ///     Incremental integer that identifies on journal entry. It is set when a ledger entry has been consolidated
        ///     and can't be changed.
        /// </summary>
        public int? SequenceId { get; private set; }

        /// <summary>
        ///     JournalEntry date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     JournalEntry last edited
        /// </summary>
        public DateTime LastEdit { get; set; }

        /// <summary>
        ///     Has the journal entry been printed ?
        /// </summary>
        public bool Consolidated { get; set; }

        /// <summary>
        ///     Journal entries.
        /// </summary>
        public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = null!;

        public Transaction? Transaction { get; set; }
        public int? TransactionId { get; set; }
        public string Detail { get; set; }

        // /// <summary>
        // /// Summation of the the JournalEntry lines' credit amount.
        // /// </summary>
        // public decimal TotalDebit()
        // {
        //     return JournalEntryLines.Sum(l => l.Value.Debit);
        // }
        //
        // /// <summary>
        // /// Summation of the JournalEntry lines' debit amount.
        // /// </summary>
        // public decimal TotalCredit()
        // {
        //     return JournalEntryLines.Sum(l => l.Value.Credit);
        // }
        //
        // /// <summary>
        // /// Return the balance of the account. That is TotalDebit minus TotalCredit.
        // /// </summary>
        // /// <returns> A positive value represents a debit amount. Otherwise, its a credit amount</returns>
        // public decimal Balance()
        // {
        //     return TotalDebit() - TotalCredit();
        // }
        //
        // /// <summary>
        // /// Consolidates the journal entry ( sets flag and number )
        // /// </summary>
        // public void Consolidate(int sequenceNumber)
        // {
        //     SequenceId = sequenceNumber;
        //     Consolidated = true;
        // }
        //
        // /// <summary>
        // /// Add a new line or updates a line in the entry. If the line already existe, the provided value es added to the
        // /// existing line.
        // /// </summary>
        // public void PatchLine(string detalle, Account account, DoubleEntryValue value)
        // {
        //     if (account == null) throw new ArgumentNullException(nameof(account));
        //     var ac = FindLine(account);
        //
        //     if (ac == null)
        //     {
        //         AddLine(detalle, account, value);
        //     }
        //     else
        //     {
        //         ac.Value = ac.Value.Add(value);
        //     }
        // }
        //
        // /// <summary>
        // /// Add a new line or updates a line in the entry. If the line already existe, the provided value es set to the provided one
        // /// </summary>
        // public void Overwrite(string detalle, Account account, DoubleEntryValue value)
        // {
        //     if (account == null) throw new ArgumentNullException(nameof(account));
        //     var ac = FindLine(account);
        //
        //     if (ac == null)
        //     {
        //         AddLine(detalle, account, value);
        //     }
        //     else
        //     {
        //         ac.Value = value;
        //     }
        // }
        //
        // /// <summary>
        // /// Finds a line in the entry with the provided account.
        // /// </summary>
        // /// <returns>The entry line ( or null if any or the lines uses the given account )</returns>
        // private JournalEntryLine FindLine(Account account)
        // {
        //     if (account == null) throw new ArgumentNullException(nameof(account));
        //     return JournalEntryLines.FirstOrDefault(a => a.Account.Id == account.Id);
        // }
        //
        // /// <summary>
        // /// Adds a new line with the provided params to the journal entry. Raises an exception if the line already exist.
        // /// </summary>
        // public void AddLine(string detail, Account account, DoubleEntryValue value)
        // {
        //     if (account == null) throw new ArgumentNullException(nameof(account));
        //     AddLine(new JournalEntryLine(detail, account, value));
        // }
        //
        // public void AddLine(JournalEntryLine line)
        // {
        //     if (line is null)
        //     {
        //         throw new ArgumentNullException(nameof(line));
        //     }
        //
        //     if (JournalEntryLines.Any(a => a.Account.Id == line.Account.Id))
        //     {
        //         throw new SigaException(
        //             $"The account {line.Account} is already present in the entry. Use patch or overwrite if you want to modify an existing account.");
        //     }
        //
        //     JournalEntryLines.Add(line);
        // }
        //
        // public void RemoveLine(Account account)
        // {
        //     if (account is null)
        //         throw new ArgumentNullException(nameof(account));
        //     var line = JournalEntryLines.FirstOrDefault(s => s.AccountId == account.Id);
        //     if (line == null)
        //         throw new SigaException($"The account {account} is not present in the entry.");
        //     JournalEntryLines.Remove(line);
        // }
        //
        // public override string ToString()
        // {
        //     StringBuilder builder = new StringBuilder();
        //     builder.AppendLine("".PadLeft(54, '-'));
        //     builder.AppendLine(Detail);
        //     builder.AppendLine("".PadLeft(54, '-'));
        //     builder.AppendFormat("|Fecha: {0}".PadRight(26, ' '), Date.ToString(Constants.DateFormat));
        //     builder.Append("|Debe".PadRight(16, ' '));
        //     builder.Append("|Haber".PadRight(16, ' '));
        //     builder.AppendLine("|");
        //     foreach (JournalEntryLine line in JournalEntryLines)
        //     {
        //         builder.AppendLine(line.ToString());
        //     }
        //
        //     builder.AppendLine(JournalEntryLine.WriteLine("Total", TotalDebit(), TotalCredit()));
        //     return builder.ToString();
        // }
    }
}