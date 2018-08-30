using System.Collections.Generic;

namespace ToggleAPI.Dtos
{
    public class SavingToggleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
        public int Value { get; set; }
        public virtual IEnumerable<string> ServicesList { get; set; }
    }
}