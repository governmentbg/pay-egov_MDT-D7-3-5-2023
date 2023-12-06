using EPayments.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPayments.Data.ViewObjects.Web
{
    public class SystemStatsVO
    {
        public int RegisteredRequests { get; set; }
        public int PaidViaVpos { get; set; }
        public int PaidViaBankOrder { get; set; }
        public int CanceledByUser { get; set; }
        public int PendingRequests { get; set; }
        public int RequestsWithAccessCode { get; set; }

        public decimal RegisteredRequestsAmount { get; set; }
        public decimal PaidViaVposAmount { get; set; }
        public decimal PaidViaBankOrderAmount { get; set; }
    }
}
