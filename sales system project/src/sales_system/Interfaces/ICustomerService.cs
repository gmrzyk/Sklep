using sales_system.Models;

namespace sales_system.Interfaces
{
    public interface ICustomerService
    {
        void ManageCustomers();
        void AddCustomer();
        void DisplayCustomers();
        void RemoveCustomer();
        void SetUnpaidLimit();
        List<Customer> LoadCustomers();
    }
}