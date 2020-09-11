using Anthill.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.DTO
{
    public class ProjectsDto
    {
        private static Dictionary<string, CategoryProjects> categoryDictionary = new Dictionary<string, CategoryProjects>();
        public static void Initial(ApplicationDbContent content)
        {
            if (!content.CategoryName.Any())
            {
                content.CategoryName.AddRange(Categories.Select(x => x.Value));
            }
        }
        public static Dictionary<string, CategoryProjects> Categories
        {
            get
            {
                if (!categoryDictionary.Any())
                {
                    var arrayOfCategories = new CategoryProjects[]
                    {
                        new CategoryProjects { CategoryName = "Дизайн" },
                        new CategoryProjects { CategoryName = "Музыка" },
                        new CategoryProjects { CategoryName = "Бизнес"},
                        new CategoryProjects { CategoryName = "Инициативы"}
                    };

                    foreach (var projects in arrayOfCategories)
                    {
                        categoryDictionary.Add(projects.CategoryName, projects);
                    }
                }

                return categoryDictionary;
            }
        }
    }
}
