namespace ToggleAPI.Models
{
    public class TogglesServices
    {
        public int TogglesServicesId { get; set; }
        public int? ToggleId { get; set; }
        public int? ServiceId { get; set; }
        public virtual Toggle Toggle { get; set; }
        public virtual Service Service { get; set; }
    }
}