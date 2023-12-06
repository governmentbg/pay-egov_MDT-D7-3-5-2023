using ClosedXML.Excel;
using EPayments.Admin.Auth;
using EPayments.Admin.Models.Shared;
using EPayments.Admin.Models.UndistributedPayments;
using EPayments.Common;
using EPayments.Common.Helpers;
using EPayments.Data.Repositories.Interfaces;
using EPayments.Data.ViewObjects.Admin;
using EPayments.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPayments.Admin.Controllers
{
    [AdminAuthorize(InternalAdminUserPermissionEnum.ViewReferences)]
    public partial class UndistributedPaymentsController : BaseController
    {
        private readonly IEquationControlsRepository EquationControlsRepository;

        public UndistributedPaymentsController(IEquationControlsRepository equationControlsRepository)
        {
            this.EquationControlsRepository = equationControlsRepository ?? throw new ArgumentNullException("equationControlsRepository is null");
        }

        [HttpGet]
        public virtual async Task<ActionResult> List(UndistributedPaymentSearchDO searchDO)
        {
            UndistributedPaymentVM model = new UndistributedPaymentVM();

            model.RequestsPagingOptions = new PagingVM();
            model.SearchDO = searchDO;

            model.RequestsPagingOptions.CurrentPageIndex = searchDO.UpPage;
            model.RequestsPagingOptions.ControllerName = MVC.UndistributedPayments.Name;
            model.RequestsPagingOptions.ActionName = MVC.UndistributedPayments.ActionNames.List;
            model.RequestsPagingOptions.PageIndexParameterName = "upPage";
            model.RequestsPagingOptions.RouteValues = searchDO.ToRequestsRouteValues();

            model.RequestsPagingOptions.TotalItemCount = await this.EquationControlsRepository.CountUndistributetPayments(
                searchDO.UpId,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.UpDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.UpDateTo)),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.UpAmountFrom),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.UpAmountTo),
                searchDO.UpProvider,
                searchDO.UpReason,
                searchDO.UpObligationStatus);

            model.Requests = await this.EquationControlsRepository.GetUndistributetPayments(
                searchDO.UpId,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.UpDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.UpDateTo)),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.UpAmountFrom),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.UpAmountTo),
                searchDO.UpProvider,
                searchDO.UpReason,
                searchDO.UpObligationStatus,
                Enum.GetName(searchDO.UpSortBy.GetType(), searchDO.UpSortBy),
                searchDO.UpSortDesc,
                searchDO.UpPage,
                AppSettings.EPaymentsWeb_MaxSearchResultsPerPage);

            return View(model);
        }

        public virtual async Task<ActionResult> DownloadPdf(UndistributedPaymentSearchDO searchDO)
        {
            List<PaymentRequestVO> model = await this.EquationControlsRepository.GetAllUndistributetPayments(
                searchDO.UpId,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.UpDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.UpDateTo)),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.UpAmountFrom),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.UpAmountTo),
                searchDO.UpProvider,
                searchDO.UpReason,
                searchDO.UpObligationStatus,
                Enum.GetName(searchDO.UpSortBy.GetType(), searchDO.UpSortBy),
                searchDO.UpSortDesc);

            string htmlContent = RenderHelper.RenderHtmlByMvcView(MVC.Shared.Views._UndistributedPaymentsPrintPdf, model);

            byte[] data = RenderHelper.RenderPdf(MVC.Shared.Views._PrintPdf, htmlContent);

            string fileName = "Контрола за равнение по неразпределени задължения" + MimeTypeFileExtension.GetFileExtenstionByMimeType(MimeTypeFileExtension.MIME_APPLICATION_PDF);

            return File(data, MimeTypeFileExtension.MIME_APPLICATION_PDF, fileName);
        }

        public virtual async Task<ActionResult> DownloadExcel(UndistributedPaymentSearchDO searchDO)
        {
            List<PaymentRequestVO> model = await this.EquationControlsRepository.GetAllUndistributetPayments(
                searchDO.UpId,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.UpDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.UpDateTo)),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.UpAmountFrom),
                Parser.TwoDecimalPlacesFormatStringToDecimal(searchDO.UpAmountTo),
                searchDO.UpProvider,
                searchDO.UpReason,
                searchDO.UpObligationStatus,
                Enum.GetName(searchDO.UpSortBy.GetType(), searchDO.UpSortBy),
                searchDO.UpSortDesc);

            XLWorkbook workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Неразпределени задължения");

            worksheet.Cell("A1").Value = "Номер на задължението";
            worksheet.Cell("B1").Value = "Дата на създаване";
            worksheet.Cell("C1").Value = "Сума";
            worksheet.Cell("D1").Value = "Заявител";
            worksheet.Cell("E1").Value = "Статус на задължението";
            worksheet.Cell("F1").Value = "Причина за неразпределение";

            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("B1").Style.Font.Bold = true;
            worksheet.Cell("C1").Style.Font.Bold = true;
            worksheet.Cell("D1").Style.Font.Bold = true;
            worksheet.Cell("E1").Style.Font.Bold = true;
            worksheet.Cell("F1").Style.Font.Bold = true;

            for (int i = 0; i < model.Count; i++)
            {
                worksheet.Cell(string.Format("A{0}", i + 2)).SetValue<string>(model[i].PaymentRequestIdentifier);
                worksheet.Cell(string.Format("B{0}", i + 2)).SetValue<string>(Formatter.DateToBgFormatWithoutYearSuffix(model[i].CreateDate));
                worksheet.Cell(string.Format("C{0}", i + 2)).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(model[i].PaymentAmount) + " лв.");
                worksheet.Cell(string.Format("D{0}", i + 2)).SetValue<string>(model[i].ServiceProviderName);
                worksheet.Cell(string.Format("E{0}", i + 2)).SetValue<string>(model[i].ObligationStatusId != null ? Formatter.EnumToDescriptionString(model[i].ObligationStatusId) : "Няма стойност");
                worksheet.Cell(string.Format("F{0}", i + 2)).SetValue<string>(model[i].PaymentReason);
            }

            worksheet.Rows().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Rows().Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            worksheet.Column("A").AdjustToContents();
            worksheet.Column("B").AdjustToContents();
            worksheet.Column("C").AdjustToContents();
            worksheet.Column("D").AdjustToContents();
            worksheet.Column("E").AdjustToContents();
            worksheet.Column("F").AdjustToContents();

            MemoryStream excelStream = new MemoryStream();
            workbook.SaveAs(excelStream);
            excelStream.Position = 0;

            return File(excelStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Контрола за равнение по неразпределени задължения.xlsx");
        }
    }
}