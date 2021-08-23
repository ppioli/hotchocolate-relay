namespace ConferencePlanner.GraphQL.Accounting.Models
{
    using System;
    using Common;

    public class DoubleEntryValue
    {
        public DoubleEntryValue(decimal debit, decimal credit)
        {
            SetValue(debit, credit);
        }

        public DoubleEntryValue() : this(0, 0)
        {
        }

        public decimal Debit { get; private set; }
        public decimal Credit { get; private set; }

        /// <summary>
        ///     Sets the credit/debit value.
        /// </summary>
        /// <param name="debitAmount"> Non-negative decimal </param>
        /// <param name="creditAmount"> Non-negative decimal </param>
        /// <exception cref="SigaException"> If both numbers are set or any of them is negative</exception>
        public void SetValue(decimal debitAmount, decimal creditAmount)
        {
            Credit = creditAmount;
            Debit = debitAmount;
            Check();
        }

        /// <summary>
        ///     Validates that only one of them is set at the same time, and that neither is a
        ///     non-positive number.
        /// </summary>
        /// <exception cref="SigaException"></exception>
        public void Check()
        {
            if (Debit != 0 && Credit != 0 || Debit < 0 || Credit < 0)
                throw new ClientError("INVALID_DOUBLE_ENTRY_VALUE", "Cann");
        }

        /// <summary>
        ///     Adds the two values. The result is returned in a new DoubleEntryValue.
        /// </summary>
        /// <returns></returns>
        public DoubleEntryValue Add(DoubleEntryValue value)
        {
            var debit = Debit + value.Debit;
            var credit = Credit + value.Credit;
            if (debit > credit)
                return new DoubleEntryValue(debit - credit, 0);
            return new DoubleEntryValue(0, credit - debit);
        }

        /// <summary>
        ///     Returns a signed decimal that the value
        /// </summary>
        public decimal AsAmount(AccountClass @class)
        {
            if (Credit > 0)
                return @class.IncreasesBy() == DoubleEntrySide.Credit ? Credit : -Credit;
            return @class.IncreasesBy() == DoubleEntrySide.Debit ? Debit : -Debit;
        }

        public static DoubleEntryValue DebitAmount(decimal amount)
        {
            return new DoubleEntryValue(amount, 0);
        }

        public static DoubleEntryValue CreditAmount(decimal amount)
        {
            return new DoubleEntryValue(0, amount);
        }

        /// <summary>
        ///     Creates a double entry value from a signed int an an account
        /// </summary>
        public static DoubleEntryValue FromAmount(Account account, decimal value)
        {
            return FromAmount(account.Type, value);
        }

        /// <summary>
        ///     Creates a double entry value from a signed int an an account
        /// </summary>
        public static DoubleEntryValue FromValues(decimal debitAmount, decimal creditAmount)
        {
            return creditAmount > debitAmount
                ? CreditAmount(creditAmount - debitAmount)
                : DebitAmount(debitAmount - creditAmount);
        }

        /// <summary>
        ///     Creates a double entry value from a signed int an an account type
        /// </summary>
        public static DoubleEntryValue FromAmount(AccountClass @class, decimal value)
        {
            DoubleEntryValue deValue;
            if (@class.IncreasesBy() == DoubleEntrySide.Credit)
                deValue = value > 0 ? CreditAmount(value) : DebitAmount(Math.Abs(value));
            else
                deValue = value > 0 ? DebitAmount(value) : CreditAmount(Math.Abs(value));

            return deValue;
        }
    }
}