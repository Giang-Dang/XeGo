namespace XeGo.Services.Price.API.Models
{
    public class CreateDiscountRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public double Percent { get; set; }
        public int Quantity { get; set; }
        public DateTime FromDay { get; set; }
        public DateTime ToDay { get; set; }
        public string ModifiedBy { get; set; } = string.Empty!;
    }
}
