using ClosedXML.Excel;
using EPayments.Admin.Auth;
using EPayments.Admin.Models.Distributions;
using EPayments.Admin.Models.Shared;
using EPayments.Common;
using EPayments.Common.Helpers;
using EPayments.Data.Repositories.Interfaces;
using EPayments.Data.ViewObjects.Admin;
using EPayments.Distributions.Interfaces;
using EPayments.Distributions.Models.BNB;
using EPayments.Model.Enums;
using EPayments.Model.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace EPayments.Admin.Controllers
{
    [AdminAuthorize(InternalAdminUserPermissionEnum.DistributionReferences)]
    public partial class DistributionController : BaseController
    {
        private readonly IDistributionRepository DistributionRepository;
        private readonly IDistributionFactory DistributionFactory;
        private static readonly ILog Logger = LogManager.GetLogger(nameof(DistributionController));

        public DistributionController(IDistributionRepository distributionRepository,
            IDistributionFactory distributionFactory)
        {
            this.DistributionRepository = distributionRepository ?? throw new ArgumentNullException("distributionRepository is null.");
            this.DistributionFactory = distributionFactory ?? throw new ArgumentNullException("distributionFactory is null");
        }

        public virtual async Task<ActionResult> Distributions(DistributionRevenueSearchDO searchDO)
        {
            if (searchDO == null)
            {
                searchDO = new DistributionRevenueSearchDO();
            }
            DistributionRevenueVM model = new DistributionRevenueVM();
            try 
            {
                model.RequestsPagingOptions = new PagingVM();
                model.RequestsPagingOptions.ActionName = MVC.Distribution.ActionNames.Distributions;
                model.RequestsPagingOptions.ControllerName = MVC.Distribution.Name;
                model.RequestsPagingOptions.CurrentPageIndex = searchDO.CurrentPage;
                model.RequestsPagingOptions.PageIndexParameterName = searchDO.PageIndexParameterName;
                model.RequestsPagingOptions.RouteValues = searchDO.ToDistributionRouteValues(searchDO.SortBy);
                model.RequestsPagingOptions.TotalItemCount = await this.CountDistributions(searchDO);
                model.DistributionRevenues = await this.GetDistributions(searchDO, model.RequestsPagingOptions.PageSize);
                model.DistribtutionTypes = await this.GetDistribtutionTypes();
                model.SearchDO = searchDO;
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
            }

            return View(model);
        }

        public virtual async Task<ActionResult> Payments(PaymentSearchDO searchDO)
        {
            DistributionRevenueVO distributionRevenue = await this.DistributionRepository.GetDistributionById(searchDO.Id);

            if (distributionRevenue == null)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Разпределението не е намерено.";

                return this.RedirectToAction(MVC.Distribution.ActionNames.Distributions, MVC.Distribution.Name);
            }

            PaymentVM model = new PaymentVM();

            model.SearchDO = searchDO;
            model.DistributionRevenue = distributionRevenue;
            model.Payments = await this.DistributionRepository.GetDistributionPaymentRequests(searchDO.Id,
            Enum.GetName(searchDO.SortBy.GetType(), searchDO.SortBy),
            searchDO.SortDesc);
            model.DistribtutionTypes = await this.GetDistribtutionTypes();

            return View(model);
        }

        public virtual async Task<ActionResult> DownloadPdf(PaymentSearchDO searchDO)
        {
            DistributionRevenueVO distributionRevenue = await this.DistributionRepository.GetDistributionById(searchDO.Id);

            if (distributionRevenue == null)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Разпределението не е намерено.";

                return this.RedirectToAction(MVC.Distribution.ActionNames.Distributions, MVC.Distribution.Name);
            }

            PaymentVM model = new PaymentVM();

            model.DistributionRevenue = distributionRevenue;

            model.Payments = await this.DistributionRepository.GetAllDistributionPaymentRequests(searchDO.Id,
            Enum.GetName(searchDO.SortBy.GetType(), searchDO.SortBy),
            searchDO.SortDesc);
            model.DistribtutionTypes = await this.GetDistribtutionTypes();

            string htmlContent = RenderHelper.RenderHtmlByMvcView(MVC.Shared.Views._DistributionRevenuePrintPdf, model);

            byte[] data = RenderHelper.RenderPdf(MVC.Shared.Views._PrintPdf, htmlContent);

            string fileName = "Справка разпределение" + MimeTypeFileExtension.GetFileExtenstionByMimeType(MimeTypeFileExtension.MIME_APPLICATION_PDF);

            return File(data, MimeTypeFileExtension.MIME_APPLICATION_PDF, fileName);
        }

        public virtual async Task<ActionResult> DownloadExcel(PaymentSearchDO searchDO)
        {
            DistributionRevenueVO distributionRevenue = await this.DistributionRepository.GetDistributionById(searchDO.Id);

            if (distributionRevenue == null)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Разпределението не е намерено.";

                return this.RedirectToAction(MVC.Distribution.ActionNames.Distributions, MVC.Distribution.Name);
            }

            PaymentVM model = new PaymentVM();

            model.DistributionRevenue = distributionRevenue;

            model.Payments = await this.DistributionRepository.GetAllDistributionPaymentRequests(searchDO.Id,
            Enum.GetName(searchDO.SortBy.GetType(), searchDO.SortBy),
            searchDO.SortDesc);

            model.DistribtutionTypes = await this.GetDistribtutionTypes();

            XLWorkbook workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Справка заявки за плащане");

            worksheet.Cell("A1").Value = "Справката е генерирана на";
            worksheet.Cell("B1").Value = "Справката е разпределена на";
            worksheet.Cell("C1").Value = "Обща сума на разпределението";
            worksheet.Cell("D1").Value = "Дали е разпределенa";
            worksheet.Cell("E1").Value = "Изпратена към Борика";
            worksheet.Cell("F1").Value = "Вид на разпределението";

            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("B1").Style.Font.Bold = true;
            worksheet.Cell("C1").Style.Font.Bold = true;
            worksheet.Cell("D1").Style.Font.Bold = true;
            worksheet.Cell("E1").Style.Font.Bold = true;
            worksheet.Cell("F1").Style.Font.Bold = true;

            worksheet.Cell("A2").SetValue<string>(Formatter.DateTimeToBgFormatWithoutSeconds(model.DistributionRevenue.CreatedAt));
            worksheet.Cell("B2").SetValue<string>(Formatter.DateTimeToBgFormatWithoutSeconds(model.DistributionRevenue.DistributedDate) ?? "не е разпределено");
            worksheet.Cell("C2").SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(model.DistributionRevenue.TotalSum) + " лв.");
            worksheet.Cell("D2").SetValue<string>(model.DistributionRevenue.IsDistributed ? "Да" : "Не");
            worksheet.Cell("E2").SetValue<string>(model.DistributionRevenue.IsFileGenerated ? "Да" : "Не");
            worksheet.Cell("F2").SetValue<string>(model.DistribtutionTypes.FirstOrDefault(dt => dt.DistributionTypeId == model.DistributionRevenue.DistributionType)?.Name ?? string.Empty);

            worksheet.Cell("A4").Value = "Задължения в разпределението";

            worksheet.Range(worksheet.Cell(4, 1), worksheet.Cell(4, 8)).Merge();

            worksheet.Cell("A4").Style.Font.Bold = true;

            worksheet.Cell("A5").Value = "Номер на задължението";
            worksheet.Cell("B5").Value = "Основание за плащане";
            worksheet.Cell("C5").Value = "Сума";
            worksheet.Cell("D5").Value = "Заявител";
            worksheet.Cell("E5").Value = "АИС разпоредител";
            worksheet.Cell("F5").Value = "Задължено лице";
            worksheet.Cell("G5").Value = "Статус на плащането";
            worksheet.Cell("H5").Value = "Статус на задължението";

            worksheet.Cell("A5").Style.Font.Bold = true;
            worksheet.Cell("B5").Style.Font.Bold = true;
            worksheet.Cell("C5").Style.Font.Bold = true;
            worksheet.Cell("D5").Style.Font.Bold = true;
            worksheet.Cell("E5").Style.Font.Bold = true;
            worksheet.Cell("F5").Style.Font.Bold = true;
            worksheet.Cell("G5").Style.Font.Bold = true;
            worksheet.Cell("H5").Style.Font.Bold = true;

            for (int i = 0; i < model.Payments.Count; i++)
            {
                worksheet.Cell(string.Format("A{0}", i + 6)).SetValue<string>(model.Payments[i].PaymentRequestIdentifier);
                worksheet.Cell(string.Format("B{0}", i + 6)).SetValue<string>(model.Payments[i].PaymentReason);
                worksheet.Cell(string.Format("C{0}", i + 6)).SetValue<string>(Formatter.DecimalToTwoDecimalPlacesFormat(model.Payments[i].PaymentAmount) + " лв.");
                worksheet.Cell(string.Format("D{0}", i + 6)).SetValue<string>(model.Payments[i].EServiceClientName);
                worksheet.Cell(string.Format("E{0}", i + 6)).SetValue<string>(model.Payments[i].TargetEServiceClientName);
                worksheet.Cell(string.Format("F{0}", i + 6)).SetValue<string>(model.Payments[i].ApplicantName);
                worksheet.Cell(string.Format("G{0}", i + 6)).SetValue<string>(Formatter.EnumToDescriptionString(model.Payments[i].PaymentRequestStatus));
                worksheet.Cell(string.Format("H{0}", i + 6)).SetValue<string>(model.Payments[i].ObligationStatus != null ? Formatter.EnumToDescriptionString(model.Payments[i].ObligationStatus) : "Няма стойност");
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

            MemoryStream excelStream = new MemoryStream();

            workbook.SaveAs(excelStream);
            excelStream.Position = 0;

            return File(excelStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Справка разпределение.xlsx");
        }

        public virtual async Task<ActionResult> Distribute(int id)
        {
            DistributionRevenue distributionRevenue = await DistributionRepository.GetDistribution(id);

            if (distributionRevenue == null)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Разпределението не е намерено.";

                return this.RedirectToAction(MVC.Distribution.ActionNames.Distributions, MVC.Distribution.Name);
            }

            if (distributionRevenue.IsDistributed == true)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Разпределението вече е било разпределено.";

                return this.RedirectToAction(MVC.Distribution.ActionNames.Distributions, MVC.Distribution.Name);
            }

            List<PaymentRequest> paymentRequests = await DistributionRepository.GetDistributionPaymentRequests(id);

            distributionRevenue.IsDistributed = true;

            paymentRequests.ForEach(pr => pr.ObligationStatusId = ObligationStatusEnum.CheckedAccount);

            this.DistributionRepository.Save();

            TempData[Common.TempDataKeys.Message] = "Разпределението e маркирано като разпределено.";

            return this.RedirectToAction(MVC.Distribution.ActionNames.Payments, MVC.Distribution.Name, new { id = id });
        }

        public virtual async Task<ActionResult> GetFile(int id)
        {
            DistributionRevenue distributionRevenue = await DistributionRepository.GetDistribution(id);

            if (distributionRevenue == null)
            {
                TempData[Common.TempDataKeys.ErrorMessage] = "Разпределението не е намерено.";

                return this.RedirectToAction(MVC.Distribution.ActionNames.Distributions, MVC.Distribution.Name);
            }

            string bulstat = AppSettings.EPaymentsJobHost_DistributionBulstat;
            string bicCode = AppSettings.EPaymentsJobHost_DistributionBICCode;
            string eGov = AppSettings.EPaymentsJobHost_DistributionSenderName;
            string iban = AppSettings.EPaymentsJobHost_DistributionIban;
            string xsdName = AppSettings.EPaymentsJobHost_XsdFileName;
            string vpn = AppSettings.EPaymentsJobHost_Vpn;
            string vd = AppSettings.EPaymentsJobHost_Vd;
            string firstDescription = AppSettings.EPaymentsJobHost_FirstDescription;
            string secondDescription = AppSettings.EPaymentsJobHost_SecondDescription;
            string xsdDirectoryPath = AppSettings.EPaymentsJobHost_SchemasDirectory;
            var obligationTypeList = await DistributionRepository.GetAllObligationTypes();

            distributionRevenue.DistributionRevenuePayments = await DistributionRepository
                .GetDistributionRevenuePayments(distributionRevenue.DistributionRevenueId);

            BnbFile bnbFile = this.DistributionFactory.BnbModelCreator()
                           .Create(distributionRevenue, bulstat, eGov, iban, bicCode, vpn, vd, firstDescription, secondDescription, obligationTypeList);

            IBnbXmlDocumentCreator bnbXmlDocumentCreator = this.DistributionFactory.BnbXmlDocumentCreator();

            XDocument document = bnbXmlDocumentCreator.CreateDocument(bnbFile);

            List<string> errors = bnbXmlDocumentCreator.ValidateDocument(document, xsdDirectoryPath, xsdName);

            if (errors.Count > 0)
            {
                errors.ForEach(e =>
                {
                    if (!string.IsNullOrWhiteSpace(e) && !distributionRevenue.DistributionErrors.Any(de => string.Equals(de.Error, e, StringComparison.OrdinalIgnoreCase)))
                    {
                        distributionRevenue.DistributionErrors.Add(new DistributionError()
                        {
                            CreatedAt = DateTime.Now.ToUniversalTime(),
                            Error = e.Length > 500 ? e.Substring(0, 500) : e
                        });
                    }
                });

                DistributionRepository.Save();
            }

            MemoryStream xmlStream = new MemoryStream();
            
            document.Save(xmlStream);
            
            xmlStream.Position = 0;

            return File(xmlStream, "application/xml", string.Format("{0}-{1}.xml", distributionRevenue.DistributionRevenueId, Formatter.DateToBgFormatWithoutYearSuffix(DateTime.Today)));
        }

        private async Task<List<DistributionRevenueVO>> GetDistributions(DistributionRevenueSearchDO searchDO, int pageLength)
        {
            return await this.DistributionRepository.GetDistributionRevenues(
                searchDO.CurrentPage,
                pageLength,
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.StartDate)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.EndDate)),
                searchDO.DistributionType,
                Enum.GetName(searchDO.SortBy.GetType(), searchDO.SortBy),
                searchDO.SortDesc);
        }

        private async Task<int> CountDistributions(DistributionRevenueSearchDO searchDO)
        {
            return await this.DistributionRepository.CountDistributionRevenues(
                Parser.GetDateFirstMinute(Parser.BgFormatDateStringToDateTime(searchDO.StartDate)),
                Parser.GetDateLastMinute(Parser.BgFormatDateStringToDateTime(searchDO.EndDate)),
                searchDO.DistributionType);
        }

        private async Task<List<DistribtutionTypeVO>> GetDistribtutionTypes()
        {
            return await this.DistributionRepository.GetDistributionTypes();
        }
    }
}