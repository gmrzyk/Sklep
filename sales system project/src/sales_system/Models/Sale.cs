namespace sales_system.Models
{
    public class SaleItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class Sale
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<SaleItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime Date { get; set; }
    }
}