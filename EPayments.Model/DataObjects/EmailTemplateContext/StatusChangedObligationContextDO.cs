﻿using EPayments.Common.Helpers;
using EPayments.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EPayments.Model.DataObjects.EmailTemplateContext
{
    public class StatusChangedObligationContextDO
    {
        public string PaymentRequestIdentifier { get; set; }
        public string ServiceProviderName { get; set; }
        public string PaymentReason { get; set; }
        public string PaymentAmount { get; set; }
        public string Status { get; set; }
        public string PortalUrl { get; set; }
        public string FeedbackUrl { get; set; }

        public StatusChangedObligationContextDO(string paymentRequestIdentifier, string serviceProviderName, string paymentReason, decimal paymentAmount, string statusObligation)
        {
            this.PaymentRequestIdentifier = paymentRequestIdentifier;
            this.ServiceProviderName = serviceProviderName;
            this.PaymentReason = paymentReason;
            this.PaymentAmount = String.Format("{0} лв.", Formatter.DecimalToTwoDecimalPlacesFormat(paymentAmount));
            this.Status = statusObligation;
            this.PortalUrl = AppSettings.EPaymentsCommon_WebAddress;
            this.FeedbackUrl = Formatter.UriCombine(this.PortalUrl, "Home", "Feedback").ToString();
        }
    }
}
