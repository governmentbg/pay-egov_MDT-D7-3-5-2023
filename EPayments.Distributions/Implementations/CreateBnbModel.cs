﻿using EPayments.Distributions.Enums;
using EPayments.Distributions.Interfaces;
using EPayments.Distributions.Models.BNB;
using EPayments.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace EPayments.Distributions.Implementations
{
    public class CreateBnbModel : ICreateBnbModel
    {
        private int counter = 1;

        public BnbFile Create(DistributionRevenue distributionRevenue, string bulstat, string senderName, string iban,
                              string bicCode, string vpn, string vd, string firstDescription, string secondDescription,
                              List<ObligationType> oblicationTypeList)
        {
            var distributionGroupped = distributionRevenue.DistributionRevenuePayments.GroupBy(p => new { p.PaymentRequest.ObligationTypeId, p.EserviceClient })
                .ToDictionary(g => g.Key, g => g.Sum(s => s.PaymentRequest.PaymentAmount));

            return new BnbFile()
            {
                Header = new BnbHeader()
                {
                    RefId = distributionRevenue.DistributionRevenueId.ToString(),
                    TimeStamp = distributionRevenue.CreatedAt,
                    Sender = bulstat,
                    SenderName = senderName,
                },
                Accounts = new List<BnbAccount>()
                {
                    new BnbAccount()
                    {
                        Acc = iban,
                        Bic = bicCode,
                        Do = distributionRevenue.TotalSum,
                        Documents = new List<BnbDocument>(distributionGroupped
                            .Select(g =>
                            {
                                string nextNumber = counter.ToString();

                                BnbDocument bnbDocument = new BnbDocument()
                                {
                                    Doc = DocumentEnum.PNVB,
                                    Nok = nextNumber,
                                    IPol = g.Key.EserviceClient.AisName,
                                    Iban = g.Key.EserviceClient.AccountIBAN,
                                    Bic = g.Key.EserviceClient.AccountBIC,
                                    Su = g.Value,
                                    O1 = (firstDescription != null && firstDescription.Length > 35 ? firstDescription.Substring(0, 35) : firstDescription),
                                    //O2 = (secondDescription != null && secondDescription.Length > 35 ? secondDescription.Substring(0, 35) : secondDescription),
                                    O2 = $"refid {distributionRevenue.DistributionRevenueId.ToString()}",
                                    Sys = g.Value <= 100000 ?  PaymentSystemEnum.BISERA : PaymentSystemEnum.RINGS
                                };

                                int algorithmId = oblicationTypeList.FirstOrDefault(x => x.ObligationTypeId == g.Key.ObligationTypeId).AlgorithmId;

                                if (ShouldIncludeVpp(g.Key.EserviceClient.AccountIBAN, algorithmId))
                                {
                                    var obligationType = oblicationTypeList.FirstOrDefault(o => o.ObligationTypeId == g.Key.ObligationTypeId);
                                    if(obligationType != null)
                                    {
                                        bnbDocument.Vpp = obligationType.PaymentTypeCode.Trim();
                                    }
                                }
                                else
                                {
                                    bnbDocument.Vpp = null;
                                }

                                bnbDocument.BnbBudget = new BnbBudget()
                                {
                                    //Vd = vd,
                                    Bul = g.Key.EserviceClient.Department.UniqueIdentificationNumber,
                                    //Db = distributionRevenue.CreatedAt,
                                    //De = distributionRevenue.CreatedAt
                                };

                                counter++;

                                return bnbDocument;
                            }))
                    }
                }
            };
        }

        private bool ShouldIncludeVpp(string iban, int algorithmId)
        {
            bool isValid = false;
            if (iban != null && iban.Length >= 13)
            {
                if (algorithmId == 0)
                {
                    if (iban[12] == '8')
                    {
                        isValid = true;
                    }
                }
                else if (algorithmId == 2 && iban[12] == '8')
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }
            return isValid;
        }
    }
}
