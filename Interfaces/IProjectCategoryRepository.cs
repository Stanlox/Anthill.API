using Anthill.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Interfaces
{
    public interface IProjectCategoryRepository
    {
        IQueryable<CategoryProjects> allСategories { get; }
    }
}
