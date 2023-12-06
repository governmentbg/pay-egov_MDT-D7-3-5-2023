using EPayments.Admin.Models.Shared;
using EPayments.Data.ViewObjects.Admin;
using System.Collections.Generic;

namespace EPayments.Admin.Models.Transactions
{
    public class TransactionVM
    {
        public IList<BoricaTransactionVO> Transactions { get; set; }

        public PagingVM RequestsPagingOptions { get; set; }

        public TransactionSearchDO SearchDO { get; set; }

        public decimal? CalculateTotalAmount { get; set; }

        public decimal? CalculateTotalFee { get; set; }

        public decimal? CalculateTotalCommission { get; set; }
    }
}