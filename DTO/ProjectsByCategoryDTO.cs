using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.DTO
{
    public class ProjectsByCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime EndDate { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string CategoryName { get; set; }
    }
}
