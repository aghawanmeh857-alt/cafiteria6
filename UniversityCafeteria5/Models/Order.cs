namespace UniversityCafeteria5.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; } = 0;
        public string Status { get; set; } = "";
        public int EmployeeId { get; set; }
    }
}
