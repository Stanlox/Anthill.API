using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Models
{
    public class Projects
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryProjects Category { get; set; }
    }
}
