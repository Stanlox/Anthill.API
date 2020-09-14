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
        private IEnumerable<Project> projectsByCategoria = new List<Project>();
        private IEnumerable<Project> listOfProjects = new List<Project>();
        private IEnumerable<Project> foundProjects = new List<Project>();
        private SearchService service;

        public SearchController(IProjectRepository project, SearchService service)
        {
            this.project = project;
            this.listOfProjects = project.projects;
            this.service = new SearchService(project, listOfProjects);         
        }

        public IActionResult Search(string nameProject)
        {
            if (string.IsNullOrEmpty(nameProject))
            {
                projectsByCategoria = project.projects;
                return Ok(projectsByCategoria);
            }

            var mostSimilarProjectsName = this.service.GetMostSimilarProjectsName(nameProject);

            if(!mostSimilarProjectsName.Any())
            {
                projectsByCategoria = project.projects;
                return Ok(projectsByCategoria);
            }

            else
            {
                
                foreach (var foundProjectName in mostSimilarProjectsName)
                {
                    foundProjects = listOfProjects.Where(x => x.Name == foundProjectName);
                }

                return Ok(foundProjects);
                
            }
        }      
    }
}
