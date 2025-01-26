using sales_system.Models;

namespace sales_system.Interfaces
{
    public interface IProductService
    {
        void ManageProducts();
        void AddProduct();
        void DisplayProducts();
        void RemoveProduct();
        void UpdateStock();
        List<Product> LoadProducts();
        void SaveProducts(List<Product> products);
    }
}