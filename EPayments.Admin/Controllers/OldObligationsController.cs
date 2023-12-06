using EPayments.Admin.Auth;
using EPayments.Admin.Models.Shared;
using EPayments.Admin.Models.OldObligations;
using EPayments.Common;
using EPayments.Common.Helpers;
using EPayments.Data.Repositories.Interfaces;
using EPayments.Model.Enums;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPayments.Data.ViewObjects.Admin;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;

namespace EPayments.Admin.Controllers
{
    [AdminAuthorize(InternalAdminUserPermissionEnum.ViewReferences)]
    public partial class OldObligationsController : BaseController
    {
        private readonly IEquationControlsRepository EquationControlsRepository;

        public OldObligationsController(IEquationControlsRepository equationControlsRepository)
        {
            this.EquationControlsRepository = equationControlsRepository ?? throw new ArgumentNullException("equationControlsRepository is null");
        }

        [HttpGet]
        public virtual async Task<ActionResult> List(OldObligationsSearchDO searchDO)
        {
            OldObligationsVM model = new OldObligationsVM();

            model.RequestsPagingOptions = new PagingVM();
            model.SearchDO = searchDO;
            if (model.SearchDO.OoObligationStatus == null)
                model.SearchDO.OoObligationStatus = ObligationStatusEnum.IrrevocableOrder;
            model.RequestsPagingOptions.CurrentPageIndex = searchDO.OoPage;
            model.RequestsPagingOptions.ControllerName = MVC.OldObligations.Name;
            model.RequestsPagingOptions.ActionName = MVC.OldObligations.ActionNames.List;
            model.RequestsPagingOptions.PageIndexParameterName = "ooPage";
            model.RequestsPagingOptions.RouteValues = searchDO.ToRequestsRouteValues();

            model.RequestsPagingOptions.TotalItemCount = await this.EquationControlsRepository.CountOldPayments(
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.OoDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.OoDateTo)),
                searchDO.OoObligationStatus);

            model.Requests = await this.EquationControlsRepository.GetOldPayments(
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.OoDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.OoDateTo)),
                searchDO.OoObligationStatus,
                Enum.GetName(searchDO.OoSortBy.GetType(), searchDO.OoSortBy),
                searchDO.OoSortDesc,
                searchDO.OoPage,
                AppSettings.EPaymentsWeb_MaxSearchResultsPerPage);

            return View(model);
        }

        public virtual async Task<ActionResult> DownloadPdf(OldObligationsSearchDO searchDO)
        {
            List<PaymentRequestVO> model = await this.EquationControlsRepository.GetAllOldPayments(
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.OoDateFrom)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.OoDateTo)),
                searchDO.OoObligationStatus,
                Enum.GetName(searchDO.OoSortBy.GetType(), searchDO.OoSortBy),
                searchDO.OoSortDesc);

            string htmlContent = RenderHelper.RenderHtmlByMvcView(MVC.Shared.Views._OldObligationsPaymentPrintPdf, model);

            byte[] data = RenderHelper.RenderPdf(MVC.Shared.Views._PrintPdf, htmlContent);

            string fileName = "Контрол за равнение по стари задължения" + MimeTypeFileExtension.GetFileExtenstionByMimeType(MimeTypeFileExtension.MIME_APPLICATION_PDF);

            return File(data, MimeTypeFileExtension.MIME_APPLICATION_PDF, fileName);
        }

        public virtual async Task<ActionResult> DownloadExcel(OldObligationsSearchDO searchDO)
        {
            List<PaymentRequestVO> model = await this.EquationControlsRepository.GetAllOldPayments(
                 Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.OoDateFrom)),
                 Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.OoDateTo)),
                 searchDO.OoObligationStatus,
                 Enum.GetName(searchDO.OoSortBy.GetType(), searchDO.OoSortBy),
                 searchDO.OoSortDesc);

            XLWorkbook workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Контрол стари задължения");

            worksheet.Cell("A1").Value = "Номер на задължението";
            worksheet.Cell("B1").Value = "Дата на създаване";
            worksheet.Cell("C1").Value = "Сума";
            worksheet.Cell("D1").Value = "Заявител";
            worksheet.Cell("E1").Value = "Статус на задължението";

            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("B1").Style.Font.Bold = true;
            worksheet.Cell("C1").Style.Font.Bold = true;
            worksheet.Cell("D1").Style.Font.Bold = true;
            worksheet.Cell("E1").Style.Font.Bold = true;

            for (int i = 0; i < model.Count; i++)
            {
                worksheet.Cell(string.Format("A{0}", i + 2)).SetValue<string>(model[i].PaymentRequestIdentifier);
                worksheet.Cell(string.Format("B{0}", i + 2)).SetValue<string>(Formatter.DateToBgFormatWithoutYearSuffix(model[i].CreateDate));
                worksheet.Cell(string.Format("C{0}", i + 2)).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(model[i].PaymentAmount) + " лв.");
                worksheet.Cell(string.Format("D{0}", i + 2)).SetValue<string>(model[i].ServiceProviderName);
                worksheet.Cell(string.Format("E{0}", i + 2)).SetValue<string>(model[i].ObligationStatusId != null ? Formatter.EnumToDescriptionString(model[i].ObligationStatusId)  : "Няма стойност");
            }

            worksheet.Rows().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Rows().Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

            worksheet.Column("A").AdjustToContents();
            worksheet.Column("B").AdjustToContents();
            worksheet.Column("C").AdjustToContents();
            worksheet.Column("D").AdjustToContents();
            worksheet.Column("E").AdjustToContents();

            MemoryStream excelStream = new MemoryStream();
            workbook.SaveAs(excelStream);
            excelStream.Position = 0;

            return File(excelStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Контрол за равнение по стари задължения.xlsx");
        }
    }
}