using System.Collections.Generic;
using ToggleAPI.Models;

namespace ToggleAPI.Dtos
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public int version { get; set; }
        public string name { get; set; }
    }
}