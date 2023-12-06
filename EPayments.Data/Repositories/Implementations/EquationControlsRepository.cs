using EPayments.Common.Data;
using EPayments.Common.Linq;
using EPayments.Data.Repositories.Interfaces;
using EPayments.Data.ViewObjects.Admin;
using EPayments.Model.DataObjects;
using EPayments.Model.Enums;
using EPayments.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPayments.Data.Repositories.Implementations
{
    public class EquationControlsRepository : IEquationControlsRepository
    {
        private readonly IUnitOfWork UnitOfWork;

        public EquationControlsRepository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork ?? throw new ArgumentNullException("unitOfWork is null");
        }

        public async Task<int> CountUndistributetPayments(string filterPaymentIdentifier,
            DateTime? filterDateFrom,
            DateTime? filterDateTo,
            decimal? filterAmountFrom,
            decimal? filterAmountTo,
            string filterServiceProvider,
            string filterPaymentReason,
            ObligationStatusEnum? obligationStatus)
        {
            var predicate = CreatePaymentRequestPredicate(
                filterPaymentIdentifier,
                filterDateFrom,
                filterDateTo,
                filterAmountFrom,
                filterAmountTo,
                filterServiceProvider,
                filterPaymentReason,
                obligationStatus);

            return await this.UnitOfWork.DbContext.Set<PaymentRequest>()
                .Where(predicate)
                .CountAsync();
        }

        public async Task<List<UndistributedPaymentRequestVO>> GetUndistributetPayments(string filterPaymentIdentifier,
            DateTime? filterDateFrom,
            DateTime? filterDateTo,
            decimal? filterAmountFrom,
            decimal? filterAmountTo,
            string filterServiceProvider,
            string filterPaymentReason,
            ObligationStatusEnum? obligationStatus,
            string sortBy,
            bool sortDescending,
            int page,
            int resultsPerPage)
        {
            var predicate = CreatePaymentRequestPredicate(filterPaymentIdentifier,
                filterDateFrom,
                filterDateTo,
                filterAmountFrom,
                filterAmountTo,
                filterServiceProvider,
                filterPaymentReason,
                obligationStatus);

            return await this.SortPaymentRequests(this.UnitOfWork.DbContext.Set<PaymentRequest>()
                .Where(predicate), sortBy, sortDescending)
                .Skip((page - 1) * resultsPerPage)
                .Take(resultsPerPage)
                .Select(UndistributedPaymentRequestVO.Map)
                .ToListAsync();
        }

        public async Task<List<PaymentRequestVO>> GetAllUndistributetPayments(string filterPaymentIdentifier,
            DateTime? filterDateFrom,
            DateTime? filterDateTo,
            decimal? filterAmountFrom,
            decimal? filterAmountTo,
            string filterServiceProvider,
            string filterPaymentReason,
            ObligationStatusEnum? obligationStatus,
            string sortBy,
            bool sortDescending)
        {
            var predicate = CreatePaymentRequestPredicate(filterPaymentIdentifier,
                filterDateFrom,
                filterDateTo,
                filterAmountFrom,
                filterAmountTo,
                filterServiceProvider,
                filterPaymentReason,
                obligationStatus);

            return await this.SortPaymentRequests(this.UnitOfWork.DbContext.Set<PaymentRequest>()
                .Where(predicate), sortBy, sortDescending)
                .Select(PaymentRequestVO.Map)
                .ToListAsync();
        }

        private Expression<Func<PaymentRequest, bool>> CreatePaymentRequestPredicate(string filterPaymentIdentifier,
            DateTime? filterDateFrom,
            DateTime? filterDateTo,
            decimal? filterAmountFrom,
            decimal? filterAmountTo,
            string filterServiceProvider,
            string filterPaymentReason,
            ObligationStatusEnum? obligationStatus)
        {
            var predicate = PredicateBuilder.True<PaymentRequest>();

            if (!String.IsNullOrWhiteSpace(filterPaymentIdentifier))
            {
                predicate = predicate.And(e => e.PaymentRequestIdentifier == filterPaymentIdentifier);
            }

            if (filterDateFrom.HasValue)
            {
                predicate = predicate.And(e => e.PaymentRequestStatusChangeTime >= filterDateFrom.Value);
            }

            if (filterDateTo.HasValue)
            {
                predicate = predicate.And(e => e.PaymentRequestStatusChangeTime <= filterDateTo.Value);
            }

            if (filterAmountFrom.HasValue)
            {
                predicate = predicate.And(e =>
                    ((e.PaymentRequestStatusId == PaymentRequestStatus.Authorized ||
                      e.PaymentRequestStatusId == PaymentRequestStatus.Ordered ||
                      e.PaymentRequestStatusId == PaymentRequestStatus.Paid) ? e.PaymentAmount : 0) >= filterAmountFrom.Value);
            }

            if (filterAmountTo.HasValue)
            {
                predicate = predicate.And(e =>
                    ((e.PaymentRequestStatusId == PaymentRequestStatus.Authorized ||
                      e.PaymentRequestStatusId == PaymentRequestStatus.Ordered ||
                      e.PaymentRequestStatusId == PaymentRequestStatus.Paid) ? e.PaymentAmount : 0) <= filterAmountTo.Value);
            }

            if (!String.IsNullOrWhiteSpace(filterServiceProvider))
            {
                predicate = predicate.AndStringContains(e => e.ServiceProviderName, filterServiceProvider);
            }

            if (!String.IsNullOrWhiteSpace(filterPaymentReason))
            {
                predicate = predicate.AndStringContains(e => e.PaymentReason, filterPaymentReason);
            }

            if (obligationStatus.HasValue)
            {
                predicate = predicate.And(e => e.ObligationStatusId == obligationStatus.Value);
            }

            return predicate;
        }

        private IQueryable<PaymentRequest> SortPaymentRequests(
            IQueryable<PaymentRequest> query,
            string sortBy,
            bool sortAscending)
        {
            switch (sortBy)
            {
                case nameof(PaymentRequest.CreateDate):
                    query = sortAscending == true ?
                        query.OrderBy(pr => pr.CreateDate) :
                        query.OrderByDescending(pr => pr.CreateDate);
                    break;
                case nameof(PaymentRequest.PaymentRequestIdentifier):
                    query = sortAscending == true ?
                        query.OrderBy(pr => pr.PaymentRequestIdentifier) :
                        query.OrderByDescending(pr => pr.PaymentRequestIdentifier);
                    break;
                case nameof(PaymentRequest.ServiceProviderName):
                    query = sortAscending == true ?
                        query.OrderBy(pr => pr.ServiceProviderName) :
                        query.OrderByDescending(pr => pr.ServiceProviderName);
                    break;
                case nameof(PaymentRequest.PaymentReason):
                    query = sortAscending == true ?
                        query.OrderBy(pr => pr.PaymentReason) :
                        query.OrderByDescending(pr => pr.PaymentReason);
                    break;
                case nameof(PaymentRequest.PaymentAmount):
                    query = sortAscending == true ?
                        query.OrderBy(pr => pr.PaymentAmount) :
                        query.OrderByDescending(pr => pr.PaymentAmount);
                    break;
                case nameof(PaymentRequest.ObligationStatusId):
                    query = sortAscending == true ?
                        query.OrderBy(pr => pr.ObligationStatusId) :
                        query.OrderByDescending(pr => pr.ObligationStatusId);
                    break;
            }

            return query;        }



        public async Task<int> CountOldPayments(DateTime? filterDateFrom,
            DateTime? filterDateTo,
            ObligationStatusEnum? obligationStatus)
        {
            var predicate = CreateOldPaymentRequestPredicate(filterDateFrom,
                filterDateTo,
                obligationStatus);

            return await this.UnitOfWork.DbContext.Set<PaymentRequest>()
                .Where(predicate)
                .CountAsync();
        }

        public async Task<List<PaymentRequestVO>> GetOldPayments(DateTime? filterDateFrom,
            DateTime? filterDateTo,
            ObligationStatusEnum? obligationStatus,
            string sortBy,
            bool sortDescending,
            int page,
            int resultsPerPage)
        {
            var predicate = CreateOldPaymentRequestPredicate(filterDateFrom,
                filterDateTo,
                obligationStatus);

            return await this.SortPaymentRequests(this.UnitOfWork.DbContext.Set<PaymentRequest>()
                .Where(predicate), sortBy, sortDescending)
                .Skip((page - 1) * resultsPerPage)
                .Take(resultsPerPage)
                .Select(PaymentRequestVO.Map)
                .ToListAsync();
        }

        public async Task<List<PaymentRequestVO>> GetAllOldPayments(DateTime? filterDateFrom,
            DateTime? filterDateTo,
            ObligationStatusEnum? obligationStatus,
            string sortBy,
            bool sortDescending)
        {
            var predicate = CreateOldPaymentRequestPredicate(filterDateFrom,
                filterDateTo,
                obligationStatus);

            return await this.SortPaymentRequests(this.UnitOfWork.DbContext.Set<PaymentRequest>()
                .Where(predicate), sortBy, sortDescending)
                .Select(PaymentRequestVO.Map)
                .ToListAsync();
        }

        public async Task<List<BoricaTransactionVO>> GetBoricaTransactions(DateTime? dateFrom, 
            DateTime? dateTo,  
            int? transactionStatus, 
            string sortBy, 
            bool sortDescending, 
            int page, 
            int resultsPerPage)
        {
            var predicate = this.CreateBoricaTransactionStatus(transactionStatus);

            var transactions = await this.SortBoricaTransactionRequests(
                this.UnitOfWork.DbContext.Set<BoricaTransaction>().Where(predicate), sortBy, sortDescending)
                .Select(BoricaTransactionVO.MapFrom).ToListAsync();

            foreach (var transaction in transactions)
            {
                if (string.IsNullOrEmpty(transaction.BoricaTransactionSettlement))
                {
                    var json = new BoricaTransactionSettlementJson() { TransactionDate = transaction.TransactionDate };
                    transaction.BoricaTransactionSettlementJson = json;
                }
                else
                {
                    transaction.BoricaTransactionSettlementJson = JsonConvert.DeserializeObject<BoricaTransactionSettlementJson>(transaction.BoricaTransactionSettlement);
                }
            }

            transactions = transactions
                                .Where(t => t.BoricaTransactionSettlementJson.TransactionDate.Value.Date >= dateFrom || !dateFrom.HasValue)
                                .Where(t => t.BoricaTransactionSettlementJson.TransactionDate.Value.Date <= dateTo || !dateTo.HasValue)
                                .Skip((page - 1) * resultsPerPage)
                                .Take(resultsPerPage)
                                .ToList();

            return transactions;
        }

        public async Task<TotalsBoricaTransactionVO> CountBoricaTransactions(DateTime? dateFrom,
                                                        DateTime? dateTo,
                                                        int? transactionStatus)
        {
            var predicate = this.CreateBoricaTransactionStatus(transactionStatus);
            var transactions = await this.UnitOfWork.DbContext.Set<BoricaTransaction>()
                                                .Where(predicate)
                                                .Select(BoricaTransactionVO.MapFrom)
                                                .ToListAsync();


            foreach (var transaction in transactions)
            {
                if (string.IsNullOrEmpty(transaction.BoricaTransactionSettlement))
                {
                    var json = new BoricaTransactionSettlementJson() { TransactionDate = transaction.TransactionDate };
                    transaction.BoricaTransactionSettlementJson = json;
                }
                else
                {
                    transaction.BoricaTransactionSettlementJson = JsonConvert.DeserializeObject<BoricaTransactionSettlementJson>(transaction.BoricaTransactionSettlement);
                }
            }

            var request = transactions.Where(t => t.BoricaTransactionSettlementJson.TransactionDate.Value.Date >= dateFrom || !dateFrom.HasValue)
                                .Where(t => t.BoricaTransactionSettlementJson.TransactionDate.Value.Date <= dateTo || !dateTo.HasValue);

            return new TotalsBoricaTransactionVO() { TotalPages = request.Count(), Commission = request.Sum(t => t.Commission.GetValueOrDefault()), TotalFee = request.Sum(t => t.Fee.GetValueOrDefault()), TotalAmount = request.Sum(t => t.Amount) };
        }

        public async Task<List<BoricaTransactionVO>> GetBoricaTransactions(DateTime? dateFrom, 
            DateTime? dateTo, 
            int? transactionStatus, 
            string sortBy, 
            bool sortDescending)
        {
            var predicate = this.CreateBoricaTransactionStatus(transactionStatus);

            var transactions = await this.SortBoricaTransactionRequests(this.UnitOfWork.DbContext.Set<BoricaTransaction>()
                .Where(predicate), sortBy, sortDescending)
                .Select(BoricaTransactionVO.MapFrom)
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                if (string.IsNullOrEmpty(transaction.BoricaTransactionSettlement))
                {
                    var json = new BoricaTransactionSettlementJson() { TransactionDate = transaction.TransactionDate };
                    transaction.BoricaTransactionSettlementJson = json;
                }
                else
                {
                    transaction.BoricaTransactionSettlementJson = JsonConvert.DeserializeObject<BoricaTransactionSettlementJson>(transaction.BoricaTransactionSettlement);
                }
            }

            transactions= transactions
                                .Where(t => t.BoricaTransactionSettlementJson.TransactionDate.Value.Date >= dateFrom || !dateFrom.HasValue)
                                .Where(t => t.BoricaTransactionSettlementJson.TransactionDate.Value.Date <= dateTo || !dateTo.HasValue)
                                .ToList();

            return transactions;
        }

        public async Task<BoricaTransactionVO> GetBoricaTransaction(int transactionId)
        {
            BoricaTransaction boricaTransaction = await this.UnitOfWork.DbContext.Set<BoricaTransaction>()
                .FirstOrDefaultAsync(bt => bt.BoricaTransactionId == transactionId);

            if (boricaTransaction != null)
            {
                return BoricaTransactionVO.MapFrom.Compile()(boricaTransaction);
            }

            return null;
        }

        public async Task<List<PaymentRequestVO>> GetTransactionPaymentRequests(int transactionId)
        {
            return await this.UnitOfWork.DbContext.Set<PaymentRequest>()
                .Where(pr => pr.BoricaTransactions.Any(bt => bt.BoricaTransactionId == transactionId))
                .Select(PaymentRequestVO.Map)
                .ToListAsync();
        }

        private Expression<Func<PaymentRequest, bool>> CreateOldPaymentRequestPredicate(DateTime? filterDateFrom,
            DateTime? filterDateTo,
            ObligationStatusEnum? obligationStatus)
        {
            var predicate = PredicateBuilder.True<PaymentRequest>();

            if (filterDateFrom.HasValue)
            {
                predicate = predicate.And(e => e.PaymentRequestStatusChangeTime >= filterDateFrom.Value);
            }

            if (filterDateTo.HasValue)
            {
                predicate = predicate.And(e => e.PaymentRequestStatusChangeTime <= filterDateTo.Value);
            }           

            if (obligationStatus.HasValue)
            {
                predicate = predicate.And(e => e.ObligationStatusId == obligationStatus.Value);
            }

            return predicate;
        }

        private Expression<Func<BoricaTransaction, bool>> CreateBoricaTransactionStatus(int? transactionStatus)
        {
            var predicate = PredicateBuilder.True<BoricaTransaction>();

            if (transactionStatus.HasValue)
            {
                predicate = predicate.And(e => e.TransactionStatusId == transactionStatus.Value);
            }

            return predicate;
        }

        private IQueryable<BoricaTransaction> SortBoricaTransactionRequests(
            IQueryable<BoricaTransaction> query,
            string sortBy,
            bool sortDescending)
        {
            switch (sortBy)
            {
                case nameof(BoricaTransaction.Order):
                    query = sortDescending == true ? query.OrderByDescending(q => q.Order) : query.OrderBy(q => q.Order);
                    break;
                case nameof(BoricaTransaction.Amount):
                    query = sortDescending == true ? query.OrderByDescending(q => q.Amount) : query.OrderBy(q => q.Amount);
                    break;
                case nameof(BoricaTransaction.Fee):
                    query = sortDescending == true ? query.OrderByDescending(q => q.Fee) : query.OrderBy(q => q.Fee);
                    break;
                case nameof(BoricaTransaction.Commission):
                    query = sortDescending == true ? query.OrderByDescending(q => q.Commission) : query.OrderBy(q => q.Commission);
                    break;
                case nameof(BoricaTransaction.TransactionDate):
                    query = sortDescending == true ? query.OrderByDescending(q => q.TransactionDate) : query.OrderBy(q => q.TransactionDate);
                    break;
                case nameof(BoricaTransaction.Card):
                    query = sortDescending == true ? query.OrderByDescending(q => q.Card) : query.OrderBy(q => q.Card);
                    break;
                case nameof(BoricaTransaction.SettlementDate):
                    query = sortDescending == true ? query.OrderByDescending(q => q.SettlementDate) : query.OrderBy(q => q.SettlementDate);
                    break;
                case nameof(BoricaTransaction.StatusMessage):
                    query = sortDescending == true ? query.OrderByDescending(q => q.StatusMessage) : query.OrderBy(q => q.StatusMessage);
                    break;
            }

            return query;
        }
    }
}
