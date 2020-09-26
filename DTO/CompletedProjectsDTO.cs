using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.DTO
{
    public class CompletedProjectsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EndDate { get; set; }
        public string ShortDescription { get; set; }
    }
}
