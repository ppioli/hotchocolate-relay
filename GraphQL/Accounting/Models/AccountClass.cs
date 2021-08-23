namespace ConferencePlanner.GraphQL.Accounting.Models
{
    using System;

    public enum AccountClass
    {
        Asset = 1,
        Liability, //2
        Capital, //3
        Revenue, //4
        Expenses //5
    }

    public enum DoubleEntrySide
    {
        Debit = 1,
        Credit
    }

    public static class AccountTypeExtensions
    {
        /// <summary>
        ///     Returns the side on the double-entry book keeping system that increases the account balance.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static DoubleEntrySide IncreasesBy(this AccountClass @class)
        {
            switch (@class)
            {
                case AccountClass.Asset:
                case AccountClass.Expenses:
                    return DoubleEntrySide.Debit;
                case AccountClass.Liability:
                case AccountClass.Revenue:
                case AccountClass.Capital:
                    return DoubleEntrySide.Credit;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@class), @class,
                        "El tipo provisto no tiene definido un lado de incremento");
            }
        }

        public static string DisplayValue(this AccountClass @class)
        {
            switch (@class)
            {
                case AccountClass.Asset:
                    return "Activo";
                case AccountClass.Liability:
                    return "Pasivo";
                case AccountClass.Revenue:
                    return "Ingreso";
                case AccountClass.Expenses:
                    return "Egreso";
                case AccountClass.Capital:
                    return "Patrimonio Neto";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@class), @class, "El tipo provisto no existe");
            }
        }
    }
}