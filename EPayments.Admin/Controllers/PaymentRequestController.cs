using ClosedXML.Excel;
using EPayments.Admin.Auth;
using EPayments.Admin.Common;
using EPayments.Admin.DataObjects;
using EPayments.Admin.Models.PaymentRequests;
using EPayments.Admin.Models.Shared;
using EPayments.Common;
using EPayments.Common.Helpers;
using EPayments.Data.Repositories.Interfaces;
using EPayments.Data.ViewObjects.Admin;
using EPayments.Model.Enums;
using EPayments.Model.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPayments.Admin.Controllers
{
    [AdminAuthorize(InternalAdminUserPermissionEnum.ViewReferences)]
    public partial class PaymentRequestController : BaseController
    {
        private readonly IPaymentRequestRepository PaymentRequestRepository;

        public PaymentRequestController(IPaymentRequestRepository paymentRequestRepository)
        {
            this.PaymentRequestRepository = paymentRequestRepository ?? throw new ArgumentNullException("paymentRequestRepository is null");
        }

        [HttpPost]
        public virtual ActionResult ListSearch(PaymentRequestSearchDO searchDO)
        {
            return RedirectToAction(MVC.PaymentRequest.ActionNames.List, MVC.PaymentRequest.Name,
                new
                {
                    @prId = searchDO.PrId,
                    @prRefenceNumber = searchDO.PrRefenceNumber,
                    @prDateFrom = searchDO.PrDateFrom,
                    @prDateTo = searchDO.PrDateTo,
                    @prAmountFrom = searchDO.PrAmountFrom,
                    @prAmountTo = searchDO.PrAmountTo,
                    @prProvider = searchDO.PrProvider,
                    @prReason = searchDO.PrReason,
                    @prApplicantName = searchDO.PrApplicantName,
                    @PrApplicantUin = searchDO.PrApplicantUin,
                    @prPaymentStatus = searchDO.PrPaymentStatus,
                    @prPaymentStatusChanged = searchDO.PrPaymentStatusChanged,
                    @prObligationStatus = searchDO.PrObligationStatus,

                    @prPage = searchDO.PrPage,
                    @prSortBy = searchDO.PrSortBy,
                    @prSortDesc = searchDO.PrSortDesc,

                    @focus = searchDO.Focus,
                });
        }

        [HttpGet]
        public virtual async Task<ActionResult> List(PaymentRequestSearchDO searchDO)
        {
            if (!searchDO.IsSearchForm && searchDO.PrPaymentStatusChanged.HasValue)
            {
                searchDO.PrPage = 1;

                await this.PaymentRequestRepository.ChangePaymentRequestsStatus(searchDO.PrId,
                    searchDO.PrRefenceNumber,
                    Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateFrom)),
                    Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateTo)),
                    Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountFrom),
                    Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountTo),
                    searchDO.PrProvider,
                    searchDO.PrReason,
                    searchDO.PrPaymentStatus,
                    searchDO.PrObligationStatus,
                    searchDO.PrApplicantName,
                    searchDO.PrApplicantUin,
                    searchDO.PrPaymentStatusChanged.Value);

                searchDO.IsSearchForm = true;
                await this.List();
            }
            PaymentRequestVM model = new PaymentRequestVM();

            model.RequestsPagingOptions = new PagingVM();

            model.SearchDO = searchDO;

            model.RequestsPagingOptions.CurrentPageIndex = searchDO.PrPage;
            model.RequestsPagingOptions.ControllerName = MVC.PaymentRequest.Name;
            model.RequestsPagingOptions.ActionName = MVC.PaymentRequest.ActionNames.List;
            model.RequestsPagingOptions.PageIndexParameterName = "prPage";
            model.RequestsPagingOptions.RouteValues = searchDO.ToRequestsRouteValues();

            model.RequestsPagingOptions.TotalItemCount = await this.PaymentRequestRepository.CountPaymentRequests(
                searchDO.PrId,
                searchDO.PrRefenceNumber,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateTo)),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountFrom),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountTo),
                searchDO.PrProvider,
                searchDO.PrReason,
                searchDO.PrPaymentStatus,
                searchDO.PrObligationStatus,
                searchDO.PrApplicantName,
                searchDO.PrApplicantUin);

            model.Requests = await this.PaymentRequestRepository.GetPaymentRequests(
                searchDO.PrId,
                searchDO.PrRefenceNumber,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateTo)),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountFrom),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountTo),
                searchDO.PrProvider,
                searchDO.PrReason,
                searchDO.PrPaymentStatus,
                searchDO.PrObligationStatus,
                searchDO.PrApplicantName,
                searchDO.PrApplicantUin,
                Enum.GetName(searchDO.PrSortBy.GetType(), searchDO.PrSortBy),
                searchDO.PrSortDesc,
                searchDO.PrPage,
                AppSettings.EPaymentsWeb_MaxSearchResultsPerPage);

            return View(model);
        }

        public virtual async Task<ActionResult> DownloadPdf(PaymentRequestSearchDO searchDO)
        {
            List<PaymentRequestVO> paymentRequests = await this.PaymentRequestRepository.GetAllRequests(searchDO.PrId,
                searchDO.PrRefenceNumber,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateTo)),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountFrom),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountTo),
                searchDO.PrProvider,
                searchDO.PrReason,
                searchDO.PrPaymentStatus,
                searchDO.PrObligationStatus,
                searchDO.PrApplicantName,
                searchDO.PrApplicantUin,
                Enum.GetName(searchDO.PrSortBy.GetType(), searchDO.PrSortBy),
                searchDO.PrSortDesc);

            string htmlContent = RenderHelper.RenderHtmlByMvcView(MVC.Shared.Views._PaymentRequestPrint,
                paymentRequests);

            byte[] data = RenderHelper.RenderPdf(MVC.Shared.Views._PrintPdf, htmlContent);

            string fileName = "Справка заявки за плащане" + MimeTypeFileExtension.GetFileExtenstionByMimeType(MimeTypeFileExtension.MIME_APPLICATION_PDF);

            return File(data, MimeTypeFileExtension.MIME_APPLICATION_PDF, fileName);
        }

        public virtual async Task<ActionResult> DownloadExcel(PaymentRequestSearchDO searchDO)
        {
            List<PaymentRequestVO> paymentRequests = await this.PaymentRequestRepository.GetAllRequests(searchDO.PrId,
                searchDO.PrRefenceNumber,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.PrDateTo)),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountFrom),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.PrAmountTo),
                searchDO.PrProvider,
                searchDO.PrReason,
                searchDO.PrPaymentStatus,
                searchDO.PrObligationStatus,
                searchDO.PrApplicantName,
                searchDO.PrApplicantUin,
                Enum.GetName(searchDO.PrSortBy.GetType(), searchDO.PrSortBy),
                searchDO.PrSortDesc);

            XLWorkbook workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Справка заявки за плащане");

            worksheet.Cell("A1").Value = "Номер на задължението";
            worksheet.Cell("B1").Value = "Референтен номер на задължението";
            worksheet.Cell("C1").Value = "Дата на създаване";
            worksheet.Cell("D1").Value = "Дата на изтичане";
            worksheet.Cell("E1").Value = "Задължено лице";
            worksheet.Cell("F1").Value = "Основание за плащане";
            worksheet.Cell("G1").Value = "Сума";
            worksheet.Cell("H1").Value = "Заявител";
            worksheet.Cell("I1").Value = "Статус на плащането";
            worksheet.Cell("J1").Value = "Статус на задължението";

            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("B1").Style.Font.Bold = true;
            worksheet.Cell("C1").Style.Font.Bold = true;
            worksheet.Cell("D1").Style.Font.Bold = true;
            worksheet.Cell("E1").Style.Font.Bold = true;
            worksheet.Cell("F1").Style.Font.Bold = true;
            worksheet.Cell("G1").Style.Font.Bold = true;
            worksheet.Cell("H1").Style.Font.Bold = true;
            worksheet.Cell("I1").Style.Font.Bold = true;
            worksheet.Cell("J1").Style.Font.Bold = true;

            for (int i = 0; i < paymentRequests.Count; i++)
            {
                worksheet.Cell(string.Format("A{0}", i + 2)).SetValue<string>(paymentRequests[i].PaymentRequestIdentifier);
                worksheet.Cell(string.Format("B{0}", i + 2)).SetValue<string>(paymentRequests[i].PaymentReferenceNumber);
                worksheet.Cell(string.Format("C{0}", i + 2)).SetValue<string>(Formatter.DateToBgFormatWithoutYearSuffix(paymentRequests[i].CreateDate));
                worksheet.Cell(string.Format("D{0}", i + 2)).SetValue<string>(Formatter.DateToBgFormatWithoutYearSuffix(paymentRequests[i].ExpirationDate));
                worksheet.Cell(string.Format("E{0}", i + 2)).SetValue<string>(paymentRequests[i].ApplicantName);
                worksheet.Cell(string.Format("F{0}", i + 2)).SetValue<string>(paymentRequests[i].PaymentReason);
                worksheet.Cell(string.Format("G{0}", i + 2)).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(paymentRequests[i].PaymentAmount) + " лв.");
                worksheet.Cell(string.Format("H{0}", i + 2)).SetValue<string>(paymentRequests[i].ServiceProviderName);
                worksheet.Cell(string.Format("I{0}", i + 2)).SetValue<string>(Formatter.EnumToDescriptionString(paymentRequests[i].PaymentRequestStatusId));
                worksheet.Cell(string.Format("J{0}", i + 2)).SetValue<string>(paymentRequests[i].ObligationStatusId != null ? Formatter.EnumToDescriptionString(paymentRequests[i].ObligationStatusId) : "Няма стойност");
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
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Справка заявки за плащане.xlsx");
        }
    }
}