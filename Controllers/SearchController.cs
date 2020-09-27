using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using Anthill.API.DTO;
using Anthill.API.Interfaces;
using Anthill.API.Models;
using Anthill.API.Repository;
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
        private readonly ManagerRepository managerRepository;
        private readonly IMapper mapper;
        private IEnumerable<Project> projectsByCategoria;
        private IEnumerable<Project> foundProjects;

        public SearchController(ManagerRepository managerRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.managerRepository = managerRepository;
        }

        [HttpGet("[action]")]
        public IActionResult Projects([FromQuery]string nameProject)
        {
            if (string.IsNullOrEmpty(nameProject))
            {
                projectsByCategoria = this.managerRepository.Project.projects;
                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria));
            }

            var mostSimilarProjectsName = this.managerRepository.Search.GetMostSimilarProjectsName(nameProject);

            if(!mostSimilarProjectsName.Any())
            {
                projectsByCategoria = this.managerRepository.Project.projects;
                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria));
            }

            else
            {               
                foreach (var foundProjectName in mostSimilarProjectsName)
                {
                    foundProjects = this.managerRepository.Project.projects.Where(x => x.Name == foundProjectName);
                }

                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(foundProjects));
            }
        }

        [HttpGet("[action]")]
        public IActionResult Completed()
        {
            return Ok(mapper.Map<CompletedProjectsDTO[]>(this.managerRepository.Project.getCompletedProjects));
        }

        [HttpGet("[action]")]
        public IActionResult New()
        {
            return Ok(mapper.Map<ProjectsByCategoryDTO[]>(this.managerRepository.Project.getNewProjects));
        }

        [HttpGet("[action]")]
        public IActionResult Terminating()
        {
            projectsByCategoria = this.managerRepository.Project.getTerminatingProjects;
            var model = mapper.Map<ProjectsByCategoryDTO[]>(this.projectsByCategoria);
            return Ok(model); 
        }

        [HttpGet("[action]")]
        public IActionResult Category([FromQuery]string nameCategory)
        {
            if (string.IsNullOrEmpty(nameCategory))
            {
                 projectsByCategoria = this.managerRepository.Project.projects;
            }

            else
            {
                projectsByCategoria = this.managerRepository.Project.ProjectByCategory(nameCategory);

                if (!projectsByCategoria.Any())
                {
                    projectsByCategoria = this.managerRepository.Project.projects;
                }
            }

            var model = mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria);
            return Ok(model);
        }
    }
}
