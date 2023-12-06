using ClosedXML.Excel;
using EPayments.Admin.Auth;
using EPayments.Admin.Models.Shared;
using EPayments.Admin.Models.Transactions;
using EPayments.Common;
using EPayments.Common.Helpers;
using EPayments.Data.Repositories.Interfaces;
using EPayments.Data.ViewObjects.Admin;
using EPayments.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPayments.Admin.Controllers
{
    [AdminAuthorize(InternalAdminUserPermissionEnum.ViewReferences)]
    public partial class TransactionController : BaseController
    {
        private readonly IEquationControlsRepository EquationControlsRepository;

        public TransactionController(IEquationControlsRepository equationControlsRepository)
        {
            this.EquationControlsRepository = equationControlsRepository ?? throw new ArgumentNullException("equationControlsRepository is null");
        }

        public virtual async Task<ActionResult> List(TransactionSearchDO searchDO)
        {
            TransactionVM model = new TransactionVM();

            model.RequestsPagingOptions = new PagingVM();
            model.SearchDO = searchDO;

            model.RequestsPagingOptions.CurrentPageIndex = searchDO.TtPage;
            model.RequestsPagingOptions.ControllerName = MVC.Transaction.Name;
            model.RequestsPagingOptions.ActionName = MVC.Transaction.ActionNames.List;
            model.RequestsPagingOptions.PageIndexParameterName = "TtPage";
            model.RequestsPagingOptions.RouteValues = searchDO.ToRequestsRouteValues();

            DateTime? dateFrom = Parser.BgFormatDateStringToDateTime(searchDO.TtDateFrom);
            DateTime? dateTo = Parser.BgFormatDateStringToDateTime(searchDO.TtDateTo);

            int? transactionStatus;

            if (searchDO.TtTransactionStatus.HasValue)
            {
                transactionStatus = (int)searchDO.TtTransactionStatus.Value;
            }
            else
            {
                transactionStatus = null;
            }

            var transactions = await this.EquationControlsRepository.GetBoricaTransactions(
                dateFrom,
                dateTo,
                transactionStatus,
                Enum.GetName(searchDO.TtSortBy.GetType(), searchDO.TtSortBy),
                searchDO.TtSortDesc,
                searchDO.TtPage,
                AppSettings.EPaymentsWeb_MaxSearchResultsPerPage);

            model.Transactions = transactions;

            var totals = await EquationControlsRepository.CountBoricaTransactions(dateFrom, dateTo, transactionStatus);
            model.RequestsPagingOptions.TotalItemCount = totals.TotalPages;
            model.CalculateTotalAmount = totals.TotalAmount;
            model.CalculateTotalFee = totals.TotalFee;
            model.CalculateTotalCommission = totals.Commission;

            return View(model);
        }

        public virtual async Task<ActionResult> Payments(int transactionId)
        {
            if (transactionId <= 0)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Транзакцията не е намерена.";

                return this.RedirectToAction(MVC.Transaction.ActionNames.List, MVC.Transaction.Name);
            }

            BoricaTransactionVO boricaTransaction = await this.EquationControlsRepository.GetBoricaTransaction(transactionId);

            if (boricaTransaction == null)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Транзакцията не е намерена.";

                return this.RedirectToAction(MVC.Transaction.ActionNames.List, MVC.Transaction.Name);
            }

            List<PaymentRequestVO> paymentRequests = await this.
                EquationControlsRepository.GetTransactionPaymentRequests(transactionId);
            
            return View(new TransactionWithPaymentsVM()
            {
                BoricaTransaction = boricaTransaction,
                Payments = paymentRequests
            });
        }

        public virtual async Task<ActionResult> DownloadExcel(TransactionSearchDO searchDO)
        {
            DateTime? dateFrom = Parser.BgFormatDateStringToDateTime(searchDO.TtDateFrom);
            DateTime? dateTo = Parser.BgFormatDateStringToDateTime(searchDO.TtDateTo);

            DateTime defaultDate = new DateTime();

            int? transactionStatus;

            if (searchDO.TtTransactionStatus.HasValue)
            {
                transactionStatus = (int)searchDO.TtTransactionStatus.Value;
            }
            else
            {
                transactionStatus = null;
            }

            List<BoricaTransactionVO> transactions = await this.EquationControlsRepository.GetBoricaTransactions(
                dateFrom,
                dateTo,
                transactionStatus,
                Enum.GetName(searchDO.TtSortBy.GetType(), searchDO.TtSortBy),
                searchDO.TtSortDesc);

            decimal calculateTotalAmount = transactions.Sum(t => t.Amount);
            decimal calculateTotalFee = transactions.Sum(t => t.Fee.GetValueOrDefault());
            decimal calculateTotalCommission = transactions.Sum(t => t.Commission.GetValueOrDefault());

            XLWorkbook workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Справка на транзакции");

            worksheet.Cell("A1").Value = "Номер на транзакцията";
            worksheet.Cell("B1").Value = "Сума";
            worksheet.Cell("C1").Value = "Такса";
            worksheet.Cell("D1").Value = "Комисионна";
            worksheet.Cell("E1").Value = "Дата на транзакцията";
            worksheet.Cell("F1").Value = "Номер на карта";
            worksheet.Cell("G1").Value = "Дата на стълмент";
            worksheet.Cell("H1").Value = "Съобщение от Борика";

            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("B1").Style.Font.Bold = true;
            worksheet.Cell("C1").Style.Font.Bold = true;
            worksheet.Cell("D1").Style.Font.Bold = true;
            worksheet.Cell("E1").Style.Font.Bold = true;
            worksheet.Cell("F1").Style.Font.Bold = true;
            worksheet.Cell("G1").Style.Font.Bold = true;
            worksheet.Cell("H1").Style.Font.Bold = true;

            int counter = 0;

            for (int i = 0; i < transactions.Count; i++)
            {
                worksheet.Cell(string.Format("A{0}", (i + 2))).SetValue<string>(transactions[i].Order);
                worksheet.Cell(string.Format("B{0}", (i + 2))).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(transactions[i].Amount) + " лв.");
                worksheet.Cell(string.Format("C{0}", (i + 2))).SetValue<string>(transactions[i].Fee != null ? Formatter.DecimalToTwoDecimalPlacesFormat(transactions[i].Fee) + " лв." : string.Empty);
                worksheet.Cell(string.Format("D{0}", (i + 2))).SetValue<string>(transactions[i].Commission != null ? Formatter.DecimalToTwoDecimalPlacesFormat(transactions[i].Commission) + " лв." : string.Empty);
                worksheet.Cell(string.Format("E{0}", (i + 2))).SetValue<string>(Formatter.DateToBgFormatWithoutYearSuffix(transactions[i].TransactionDate));
                worksheet.Cell(string.Format("F{0}", (i + 2))).SetValue<string>(transactions[i].Card);
                worksheet.Cell(string.Format("G{0}", (i + 2))).SetValue<string>(transactions[i].SettlementDate != defaultDate ? Formatter.DateToBgFormatWithoutYearSuffix(transactions[i].SettlementDate) : string.Empty);
                worksheet.Cell(string.Format("H{0}", (i + 2))).SetValue<string>(transactions[i].StatusMessage);

                counter = i;
            }

            counter += 5;

            worksheet.Cell(string.Format("A{0}", counter)).Value = "Общо сума за всички транзакции";
            worksheet.Cell(string.Format("B{0}", counter++)).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(calculateTotalAmount) + " лв.");
            worksheet.Cell(string.Format("A{0}", counter)).Value = "Общо такси за всички транзакции";
            worksheet.Cell(string.Format("B{0}", counter++)).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(calculateTotalFee) + " лв.");
            worksheet.Cell(string.Format("A{0}", counter)).Value = "Общо комисионни за всички транзакции";
            worksheet.Cell(string.Format("B{0}", counter)).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(calculateTotalCommission) + " лв.");

            MemoryStream excelStream = new MemoryStream();
            workbook.SaveAs(excelStream);
            excelStream.Position = 0;

            return File(excelStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Справка на транзакции през ЦВПОС.xlsx");
        }

        public virtual async Task<ActionResult> DownloadPdf(TransactionSearchDO searchDO)
        {
            DateTime? dateFrom = Parser.BgFormatDateStringToDateTime(searchDO.TtDateFrom);
            DateTime? dateTo = Parser.BgFormatDateStringToDateTime(searchDO.TtDateTo);

            int? transactionStatus;

            if (searchDO.TtTransactionStatus.HasValue)
            {
                transactionStatus = (int)searchDO.TtTransactionStatus.Value;
            }
            else
            {
                transactionStatus = null;
            }

            List<BoricaTransactionVO> transactions = await this.EquationControlsRepository.GetBoricaTransactions(
                dateFrom,
                dateTo,
                transactionStatus,
                Enum.GetName(searchDO.TtSortBy.GetType(), searchDO.TtSortBy),
                searchDO.TtSortDesc);

            decimal calculateTotalAmount = transactions.Sum(t => t.Amount);
            decimal calculateTotalFee = transactions.Sum(t => t.Fee.GetValueOrDefault());
            decimal calculateTotalCommission = transactions.Sum(t => t.Commission.GetValueOrDefault());

            string htmlContent = RenderHelper.RenderHtmlByMvcView(MVC.Shared.Views._TransactionsPdf,
                new TransactionPdfVM()
                {
                    Transactions = transactions,
                    CalculateTotalAmount = calculateTotalAmount,
                    CalculateTotalFee = calculateTotalFee,
                    CalculateTotalCommission = calculateTotalCommission
                });

            byte[] data = RenderHelper.RenderPdf(MVC.Shared.Views._PrintPdf, htmlContent);

            string fileName = "Справка на транзакции през ЦВПОС" + MimeTypeFileExtension.GetFileExtenstionByMimeType(MimeTypeFileExtension.MIME_APPLICATION_PDF);

            return File(data, MimeTypeFileExtension.MIME_APPLICATION_PDF, fileName);
        }

        public virtual async Task<ActionResult> DownloadTransactionPaymentsExcel(int transactionId)
        {
            if (transactionId <= 0)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Транзакцията не е намерена.";

                return this.RedirectToAction(MVC.Transaction.ActionNames.List, MVC.Transaction.Name);
            }

            BoricaTransactionVO boricaTransaction = await this.EquationControlsRepository.GetBoricaTransaction(transactionId);

            if (boricaTransaction == null)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Транзакцията не е намерена.";

                return this.RedirectToAction(MVC.Transaction.ActionNames.List, MVC.Transaction.Name);
            }

            List<PaymentRequestVO> paymentRequests = await this.
                EquationControlsRepository.GetTransactionPaymentRequests(transactionId);

            XLWorkbook workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Справка транзакция " + boricaTransaction.Order);

            DateTime defaultDate = new DateTime();

            worksheet.Cell("A1").Value = "Номер на транзакцията";
            worksheet.Cell("B1").Value = "Сума";
            worksheet.Cell("C1").Value = "Такса";
            worksheet.Cell("D1").Value = "Комисионна";
            worksheet.Cell("E1").Value = "Дата на транзакцията";
            worksheet.Cell("F1").Value = "Номер на карта";
            worksheet.Cell("G1").Value = "Дата на стълмент";
            worksheet.Cell("H1").Value = "Съобщение от Борика";

            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("B1").Style.Font.Bold = true;
            worksheet.Cell("C1").Style.Font.Bold = true;
            worksheet.Cell("D1").Style.Font.Bold = true;
            worksheet.Cell("E1").Style.Font.Bold = true;
            worksheet.Cell("F1").Style.Font.Bold = true;
            worksheet.Cell("G1").Style.Font.Bold = true;
            worksheet.Cell("H1").Style.Font.Bold = true;

            worksheet.Cell("A2").SetValue<string>(boricaTransaction.Order);
            worksheet.Cell("B2").SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(boricaTransaction.Amount) + " лв.");
            worksheet.Cell("C2").SetValue<string>(boricaTransaction.Fee != null ? Formatter.DecimalToTwoDecimalPlacesFormat(boricaTransaction.Fee) + " лв." : string.Empty);
            worksheet.Cell("D2").SetValue<string>(boricaTransaction.Commission != null ? Formatter.DecimalToTwoDecimalPlacesFormat(boricaTransaction.Commission) + " лв." : string.Empty);
            worksheet.Cell("E2").SetValue<string>(Formatter.DateToBgFormatWithoutYearSuffix(boricaTransaction.TransactionDate));
            worksheet.Cell("F2").SetValue<string>(boricaTransaction.Card);
            worksheet.Cell("G2").SetValue<string>(boricaTransaction.SettlementDate != defaultDate ? Formatter.DateToBgFormatWithoutYearSuffix(boricaTransaction.SettlementDate) : string.Empty);
            worksheet.Cell("H2").SetValue<string>(boricaTransaction.StatusMessage);

            worksheet.Cell("A4").Value = "Номер на задължението";
            worksheet.Cell("B4").Value = "Референтен номер на задължението";
            worksheet.Cell("C4").Value = "Дата на създаване";
            worksheet.Cell("D4").Value = "Дата на изтичане";
            worksheet.Cell("E4").Value = "Задължено лице";
            worksheet.Cell("F4").Value = "Основание за плащане";
            worksheet.Cell("G4").Value = "Сума";
            worksheet.Cell("H4").Value = "Заявител";
            worksheet.Cell("I4").Value = "Статус на плащането";
            worksheet.Cell("J4").Value = "Статус на задължението";

            worksheet.Cell("A4").Style.Font.Bold = true;
            worksheet.Cell("B4").Style.Font.Bold = true;
            worksheet.Cell("C4").Style.Font.Bold = true;
            worksheet.Cell("D4").Style.Font.Bold = true;
            worksheet.Cell("E4").Style.Font.Bold = true;
            worksheet.Cell("F4").Style.Font.Bold = true;
            worksheet.Cell("G4").Style.Font.Bold = true;
            worksheet.Cell("H4").Style.Font.Bold = true;
            worksheet.Cell("I4").Style.Font.Bold = true;
            worksheet.Cell("J4").Style.Font.Bold = true;

            for (int i = 0; i < paymentRequests.Count; i++)
            {
                worksheet.Cell(string.Format("A{0}", i + 5)).SetValue<string>(paymentRequests[i].PaymentRequestIdentifier);
                worksheet.Cell(string.Format("B{0}", i + 5)).SetValue<string>(paymentRequests[i].PaymentReferenceNumber);
                worksheet.Cell(string.Format("C{0}", i + 5)).SetValue<string>(Formatter.DateToBgFormatWithoutYearSuffix(paymentRequests[i].CreateDate));
                worksheet.Cell(string.Format("D{0}", i + 5)).SetValue<string>(Formatter.DateToBgFormatWithoutYearSuffix(paymentRequests[i].ExpirationDate));
                worksheet.Cell(string.Format("E{0}", i + 5)).SetValue<string>(paymentRequests[i].ApplicantName);
                worksheet.Cell(string.Format("F{0}", i + 5)).SetValue<string>(paymentRequests[i].PaymentReason);
                worksheet.Cell(string.Format("G{0}", i + 5)).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(paymentRequests[i].PaymentAmount) + " лв.");
                worksheet.Cell(string.Format("H{0}", i + 5)).SetValue<string>(paymentRequests[i].ServiceProviderName);
                worksheet.Cell(string.Format("I{0}", i + 5)).SetValue<string>(Formatter.EnumToDescriptionString(paymentRequests[i].PaymentRequestStatusId));
                worksheet.Cell(string.Format("J{0}", i + 5)).SetValue<string>(paymentRequests[i].ObligationStatusId != null ? Formatter.EnumToDescriptionString(paymentRequests[i].ObligationStatusId) : "Няма стойност");
            }

            worksheet.Rows().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Rows().Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            worksheet.Column("A").AdjustToContents();
            worksheet.Column("B").AdjustToContents();
            worksheet.Column("C").AdjustToContents();
            worksheet.Column("D").AdjustToContents();
            worksheet.Column("E").AdjustToContents();
            worksheet.Column("F").AdjustToContents();
            worksheet.Column("G").AdjustToContents();
            worksheet.Column("H").AdjustToContents();
            worksheet.Column("I").AdjustToContents();
            worksheet.Column("J").AdjustToContents();

            MemoryStream excelStream = new MemoryStream();
            workbook.SaveAs(excelStream);
            excelStream.Position = 0;

            return File(excelStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                string.Format("Справка на транзакция {0}.xlsx", boricaTransaction.Order));
        }

        public virtual async Task<ActionResult> DownloadTransactionPaymentsPdf(int transactionId)
        {
            if (transactionId <= 0)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Транзакцията не е намерена.";

                return this.RedirectToAction(MVC.Transaction.ActionNames.List, MVC.Transaction.Name);
            }

            BoricaTransactionVO boricaTransaction = await this.EquationControlsRepository.GetBoricaTransaction(transactionId);

            if (boricaTransaction == null)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Транзакцията не е намерена.";

                return this.RedirectToAction(MVC.Transaction.ActionNames.List, MVC.Transaction.Name);
            }

            List<PaymentRequestVO> paymentRequests = await this.
                EquationControlsRepository.GetTransactionPaymentRequests(transactionId);

            string htmlContent = RenderHelper.RenderHtmlByMvcView(MVC.Shared.Views._PrintTransactionPaymentsPdf,
                new TransactionWithPaymentsVM()
                {
                    BoricaTransaction = boricaTransaction,
                    Payments = paymentRequests
                });

            byte[] data = RenderHelper.RenderPdf(MVC.Shared.Views._PrintPdf, htmlContent);

            string fileName = string.Format("Справка заявки за плащане на транзакция {0}", boricaTransaction.Order) + MimeTypeFileExtension.GetFileExtenstionByMimeType(MimeTypeFileExtension.MIME_APPLICATION_PDF);

            return File(data, MimeTypeFileExtension.MIME_APPLICATION_PDF, fileName);
        }
    }
}