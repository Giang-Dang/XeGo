namespace XeGo.Services.Price.API.Models
{
    public class EditDiscountRequestDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Percent { get; set; }
        public int? Quantity { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }
        public string ModifiedBy { get; set; } = string.Empty!;
    }
}
