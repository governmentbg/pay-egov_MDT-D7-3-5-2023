using EPayments.Admin.Models.Shared;
using EPayments.Data.ViewObjects.Admin;
using System.Collections.Generic;

namespace EPayments.Admin.Models.OldObligations
{
    public class OldObligationsVM
    {
        public IList<PaymentRequestVO> Requests { get; set; }

        public PagingVM RequestsPagingOptions { get; set; }

        public OldObligationsSearchDO SearchDO { get; set; }

        public decimal Total
        {
            get
            {
                if (Requests.Count == 0)
                    return 0;
                else
                {
                    decimal total = 0;
                    foreach (var request in Requests)
                    {
                        total += request.PaymentAmount;
                    }
                    return total;
                }
            }
        }
    }
}