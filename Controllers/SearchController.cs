using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Anthill.API.DTO;
using Anthill.API.Interfaces;
using Anthill.API.Models;
using Anthill.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Anthill.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IProjectRepository projectRepository;
        private readonly ISearchProject search;
        private readonly IMapper mapper;
        private readonly Project project = new Project();
        private IEnumerable<Project> projectsByCategoria;
        private IEnumerable<Project> foundProjects;

        public SearchController(IProjectRepository projectRepository, ISearchProject search, IMapper mapper)
        {
            this.mapper = mapper;
            this.search = search;
            this.projectRepository = projectRepository; 
        }

        [HttpGet("[action]/{nameProject}")]
        public IActionResult Search(string nameProject)
        {
            if (string.IsNullOrEmpty(nameProject))
            {
                projectsByCategoria = projectRepository.projects;
                return Ok(projectsByCategoria);
            }

            var mostSimilarProjectsName = this.search.GetMostSimilarProjectsName(nameProject);

            if(!mostSimilarProjectsName.Any())
            {
                projectsByCategoria = projectRepository.projects;
                return Ok(projectsByCategoria);
            }

            else
            {               
                foreach (var foundProjectName in mostSimilarProjectsName)
                {
                    foundProjects = projectRepository.projects.Where(x => x.Name == foundProjectName);
                }

                return Ok(foundProjects);               
            }
        }
        
        [HttpGet("[action]/{nameCategory}")]
        public IActionResult Category(string nameCategory)
        {
            if (string.IsNullOrEmpty(nameCategory))
            {
                projectsByCategoria = projectRepository.projects;
            }

            else
            {
                projectsByCategoria = projectRepository.projectByCategory(nameCategory);              
            }

            var model = mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria);
            return Ok(model);
        }
    }
}
