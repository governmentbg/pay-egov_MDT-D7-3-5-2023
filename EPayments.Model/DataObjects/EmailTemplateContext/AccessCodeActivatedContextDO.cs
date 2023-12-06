﻿using EPayments.Common;
using EPayments.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPayments.Model.DataObjects.EmailTemplateContext
{
    public class AccessCodeActivatedContextDO
    {
        public string ApplicantName { get; set; }
        public string AccessCode { get; set; }
        public string PaymentRequestIdentifier { get; set; }
        public string ServiceProviderName { get; set; }
        public string PaymentReason { get; set; }
        public string PaymentAmount { get; set; }
        public string AccessByCodeUrl { get; set; }
        public string PortalUrl { get; set; }
        public string FeedbackUrl { get; set; }

        public AccessCodeActivatedContextDO(string applicantName, string accessCode, string paymentRequestIdentifier, string serviceProviderName, string paymentReason, decimal paymentAmount)
        {
            this.ApplicantName = applicantName;
            this.AccessCode = accessCode;
            this.PaymentRequestIdentifier = paymentRequestIdentifier;
            this.ServiceProviderName = serviceProviderName;
            this.PaymentReason = paymentReason;
            this.PaymentAmount = String.Format("{0} лв.", Formatter.DecimalToTwoDecimalPlacesFormat(paymentAmount));
            this.AccessByCodeUrl = Formatter.UriCombine(AppSettings.EPaymentsCommon_WebAddress, "Home", "AccessByCode?code=" + accessCode).ToString();
            this.PortalUrl = AppSettings.EPaymentsCommon_WebAddress;
            this.FeedbackUrl = Formatter.UriCombine(this.PortalUrl, "Home", "Feedback").ToString();
        }
    }
}