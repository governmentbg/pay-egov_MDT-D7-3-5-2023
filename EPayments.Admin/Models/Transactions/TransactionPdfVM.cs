using EPayments.Data.ViewObjects.Admin;
using System.Collections.Generic;

namespace EPayments.Admin.Models.Transactions
{
    public class TransactionPdfVM
    {
        public IList<BoricaTransactionVO> Transactions { get; set; }

        public decimal? CalculateTotalAmount { get; set; }

        public decimal? CalculateTotalFee { get; set; }

        public decimal? CalculateTotalCommission { get; set; }
    }
}