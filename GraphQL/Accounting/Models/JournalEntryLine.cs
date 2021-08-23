namespace ConferencePlanner.GraphQL.Accounting.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class JournalEntryLine
    {
        public JournalEntryLine(int lineNumber, string detail, Account account, DoubleEntryValue value)
        {
            LineNumber = lineNumber;
            AccountId = account.Id;
            Account = account;
            Value = value;
            Detail = detail;
        }

        private JournalEntryLine()
        {
        }

        public int LineNumber { get; set; }

        /// <summary>
        ///     Ledger entry to which this line belongs
        /// </summary>
        public JournalEntry? JournalEntry { get; set; }

        public int JournalEntryId { get; set; }

        /// <summary>
        ///     Line account
        /// </summary>
        public Account Account { get; set; }

        public int AccountId { get; set; }

        /// <summary>
        ///     Signed decimal quantity for storing purposes. Should be accessed through Debit or Credit methods accordingly
        ///     and assign through SetAmount().
        /// </summary>
        public decimal Amount { get; private set; }

        public string Detail { get; set; }

        [NotMapped]
        public DoubleEntryValue Value
        {
            get => Account.ConvertToDoubleEntryValue(Amount);
            set => Amount = Account.SignedAmount(value);
        }

        // public static JournalEntryLine CreateByValue(string detail, Account account, decimal value)
        // {
        //     DoubleEntryValue deValue;
        //     if (account.Type.IncreasesBy() == DoubleEntrySide.Credit)
        //         deValue = value > 0
        //             ? DoubleEntryValue.CreditAmount(value)
        //             : DoubleEntryValue.DebitAmount(Math.Abs(value));
        //     else
        //         deValue = value > 0
        //             ? DoubleEntryValue.DebitAmount(value)
        //             : DoubleEntryValue.CreditAmount(Math.Abs(value));
        //
        //     return new JournalEntryLine(detail, account, deValue);
        // }

        public override string ToString()
        {
            return WriteLine($"{Account.Id}-{Account.Name}", Value.Debit, Value.Credit);
        }

        public static string WriteLine(string name, decimal debit, decimal credit)
        {
            var maxSize = credit > 0 ? 40 : 45;
            var label = name.Length > maxSize ? name.Substring(0, maxSize) : name.PadRight(maxSize);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("|".PadRight(45 - maxSize + 1));
            stringBuilder.Append(label.PadRight(maxSize));
            stringBuilder.Append('|');
            stringBuilder.Append(debit.ToString("C").PadLeft(15));
            stringBuilder.Append('|');
            stringBuilder.Append(credit.ToString("C").PadLeft(15));
            stringBuilder.Append('|');
            return stringBuilder.ToString();
        }
    }
}