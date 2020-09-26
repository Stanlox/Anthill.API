using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using Anthill.API.DTO;
using Anthill.API.Interfaces;
using Anthill.API.Models;
using Anthill.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
        private IEnumerable<Project> projectsByCategoria;
        private IEnumerable<Project> foundProjects;

        public SearchController(IProjectRepository projectRepository, ISearchProject search, IMapper mapper)
        {
            this.mapper = mapper;
            this.search = search;
            this.projectRepository = projectRepository; 
        }

        [HttpGet("[action]")]
        public IActionResult Projects([FromQuery]string nameProject)
        {
            if (string.IsNullOrEmpty(nameProject))
            {
                projectsByCategoria = projectRepository.projects;
                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria));
            }

            var mostSimilarProjectsName = this.search.GetMostSimilarProjectsName(nameProject);

            if(!mostSimilarProjectsName.Any())
            {
                projectsByCategoria = projectRepository.projects;
                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria));
            }

            else
            {               
                foreach (var foundProjectName in mostSimilarProjectsName)
                {
                    foundProjects = projectRepository.projects.Where(x => x.Name == foundProjectName);
                }

                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(foundProjects));
            }
        }

        [HttpGet("[action]")]
        public IActionResult Completed()
        {
            return Ok(mapper.Map<CompletedProjectsDTO[]>(this.projectRepository.getCompletedProjects));
        }

        [HttpGet("[action]")]
        public IActionResult New()
        {
            return Ok(mapper.Map<ProjectsByCategoryDTO[]>(this.projectRepository.getNewProjects));
        }

        [HttpGet("[action]")]
        public IActionResult Terminating()
        {
            projectsByCategoria = projectRepository.getTerminatingProjects;
            var model = mapper.Map<ProjectsByCategoryDTO[]>(this.projectsByCategoria);
            return Ok(model); 
        }

        [HttpGet("[action]")]
        public IActionResult Category([FromQuery]string nameCategory)
        {
            if (string.IsNullOrEmpty(nameCategory))
            {
                 projectsByCategoria = projectRepository.projects;
            }

            else
            {
                projectsByCategoria = projectRepository.ProjectByCategory(nameCategory);

                if (!projectsByCategoria.Any())
                {
                    projectsByCategoria = projectRepository.projects;
                }
            }

            var model = mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria);
            return Ok(model);
        }
    }
}
