﻿using EPayments.Common.DataObjects;
using EPayments.Data.ViewObjects.Web;
using EPayments.Web.Models.Shared;
using System;
using System.Collections.Generic;

namespace EPayments.Web.Models.Shared
{
    public class TaxAgreementDskVM
    {
        public Guid? Gid { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool IsInternalPayment { get; set; }
        public AuthRequestDO ExternalRequestDO { get; set; }
    }
}