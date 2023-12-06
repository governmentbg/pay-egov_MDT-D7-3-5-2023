namespace EPayments.Data.ViewObjects.Admin
{
    public class TotalsBoricaTransactionVO
    {
        public int TotalPages { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalFee { get; set; }

        public decimal Commission { get; set; }
    }
}
