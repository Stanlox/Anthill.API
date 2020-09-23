using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Anthill.API.Interfaces;
using Anthill.API.Models;
using Anthill.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Anthill.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IProjectRepository project;
        private readonly ISearchProject search;
        private IEnumerable<Project> projectsByCategoria;
        private IEnumerable<Project> foundProjects;

        public SearchController(IProjectRepository project, ISearchProject search)
        {
            this.search = search;
            this.project = project; 
        }

        [HttpGet("[action]/{nameProject}")]
        public IActionResult Search(string nameProject)
        {
            if (string.IsNullOrEmpty(nameProject))
            {
                projectsByCategoria = project.projects;
                return Ok(projectsByCategoria);
            }

            var mostSimilarProjectsName = this.search.GetMostSimilarProjectsName(nameProject);

            if(!mostSimilarProjectsName.Any())
            {
                projectsByCategoria = project.projects;
                return Ok(projectsByCategoria);
            }

            else
            {               
                foreach (var foundProjectName in mostSimilarProjectsName)
                {
                    foundProjects = project.projects.Where(x => x.Name == foundProjectName);
                }

                return Ok(foundProjects);               
            }
        }      
    }
}
