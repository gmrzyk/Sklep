using sales_system.Models;

namespace sales_system.Interfaces
{
    public interface ISaleService
    {
        void CreateNewSale();
        void DisplaySalesReports();
        void ProcessPayment();
        void ShowCustomerPurchaseHistory();
        void ShowSalesInDateRange();
    }
}