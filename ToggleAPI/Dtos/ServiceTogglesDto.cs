using System.Collections.Generic;

namespace ToggleAPI.Dtos
{
    public class ServiceTogglesDto
    {
        public int Id { get; set; }
        public int version { get; set; }
        public string name { get; set; }
        public virtual IEnumerable<ToggleDto> togglesList { get; set; }
        public virtual ToggleDto mostImportantToggle { get; set; }

    }
}