using Anthill.API.Interfaces;
using Anthill.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Repository
{
    public class CategoryRepository : IProjectCategoryRepository
    {
        private readonly ApplicationDbContent dbContent;

        private CategoryRepository(ApplicationDbContent dbContent)
        {
            this.dbContent = dbContent;
        }

        public IEnumerable<CategoryProjects> allСategories
        {
            get
            {
                return dbContent.Categories;
            }
        }
    }
}
