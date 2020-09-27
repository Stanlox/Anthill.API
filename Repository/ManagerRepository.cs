using Anthill.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Repository
{
    public class ManagerRepository
    {
        public IProjectCategoryRepository ProjectCategory { get; set; }
        public IProjectRepository Project { get; set; }
        public IFavouriteRepository Favourites { get; set; }
        public ISearchProject Search { get; set; }

        public ManagerRepository(IProjectCategoryRepository projectCategory, IProjectRepository project,
            IFavouriteRepository favourites, ISearchProject search)
        {
            this.ProjectCategory = projectCategory;
            this.Project = project;
            this.Favourites = favourites;
            this.Search = search;
        }
    }
}
