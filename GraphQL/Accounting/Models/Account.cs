namespace ConferencePlanner.GraphQL.Accounting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Account
    {
        public Account(string code,
            string name,
            AccountClass type,
            bool isActive = true)
        {
            Code = code;
            Name = name;
            IsActive = isActive;
            Type = type;
        }

        private Account() : this("0", "UNITIALIZED_ACCOUNT", AccountClass.Liability)
        {
        }

        public int Id { get; set; }

        /// <summary>
        ///     Identifying code of the account
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Name of the account
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Parent account
        /// </summary>
        [InverseProperty("Children")]
        public Account? Parent { get; set; }

        public int? ParentId { get; set; }

        /// <summary>
        ///     Indicates that this account is in fact, a category ( and therefore, can have children )
        /// </summary>
        public bool IsCategory { get; set; }

        /// <summary>
        ///     Child accounts
        /// </summary>
        [InverseProperty("Parent")]
        public ICollection<Account>? Children { get; set; }

        /// <summary>
        ///     Indica que la cuenta esta activa y puede ser utilizada en asientos contables
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///     Tipo de la cuenta contable
        /// </summary>
        public AccountClass Type { get; set; }

        /// <summary>
        ///     Transform the given signed amount to an absolute debit value or zero
        /// </summary>
        /// <param name="amount"> The signed amount representing the amount </param>
        public decimal DebitAmount(decimal amount)
        {
            if (Type.IncreasesBy() == DoubleEntrySide.Debit && amount > 0) return amount;
            if (Type.IncreasesBy() == DoubleEntrySide.Credit && amount < 0) return Math.Abs(amount);
            return 0;
        }

        /// <summary>
        ///     Transform the given signed amount to an absolute credit value or zero
        /// </summary>
        /// <param name="amount"> The signed amount representing the amount </param>
        public decimal CreditAmount(decimal amount)
        {
            if (Type.IncreasesBy() == DoubleEntrySide.Credit && amount > 0) return amount;
            if (Type.IncreasesBy() == DoubleEntrySide.Debit && amount < 0) return Math.Abs(amount);
            return 0;
        }

        /// <summary>
        ///     Converts a double entry value to a
        /// </summary>
        public decimal SignedAmount(DoubleEntryValue value)
        {
            if (Type.IncreasesBy() == DoubleEntrySide.Debit)
                return value.Debit > 0 ? value.Debit : -value.Credit;
            return value.Credit > 0 ? value.Credit : -value.Debit;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Code)}: {Code}";
        }

        /// <summary>
        ///     Recives a signed int and converts it to a DoubleEntryValue depending on the account type
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public DoubleEntryValue ConvertToDoubleEntryValue(decimal amount)
        {
            return new DoubleEntryValue(DebitAmount(amount), CreditAmount(amount));
        }
    }
}