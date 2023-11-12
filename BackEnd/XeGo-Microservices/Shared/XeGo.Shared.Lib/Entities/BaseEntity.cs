namespace XeGo.Shared.Lib.Entities
{
    public abstract class BaseEntity
    {
        public string CreatedBy { get; set; } = string.Empty!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string LastModifiedBy { get; set; } = string.Empty!;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
