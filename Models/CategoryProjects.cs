using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Models
{
    public class CategoryProjects
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public List<Project> Projects { get; set; }

    }
}
