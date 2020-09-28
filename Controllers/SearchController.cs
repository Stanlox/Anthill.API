using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using Anthill.Infastructure.DTO;
using Anthill.API.Services;
using Anthill.Infastructure.Interfaces;
using Anthill.Infastructure.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Anthill.Infastructure.Repository;

namespace Anthill.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMapper mapper;
        private ServiceRepository service;
        private IEnumerable<Project> projectsByCategoria;
        private IEnumerable<Project> foundProjects;
        private IUnitOfWork unitOfWork;

        public SearchController(IMapper mapper, IUnitOfWork unitOfWork, ServiceRepository service )
        {
            this.service = service;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public IActionResult Projects([FromQuery] string nameProject)
        {
            if (string.IsNullOrEmpty(nameProject))
            {
                projectsByCategoria = this.unitOfWork.Projects.projects;
                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria));
            }

            var mostSimilarProjectsName = this.service.Search.GetMostSimilarProjectsName(nameProject);

            if (!mostSimilarProjectsName.Any())
            {
                projectsByCategoria = this.unitOfWork.Projects.projects;
                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria));
            }

            else
            {
                foreach (var foundProjectName in mostSimilarProjectsName)
                {
                    foundProjects = this.unitOfWork.Projects.projects.Where(x => x.Name == foundProjectName);
                }

                return Ok(mapper.Map<ProjectsByCategoryDTO[]>(foundProjects));
            }
        }

        [HttpGet("[action]")]
        public IActionResult Completed()
        {
            return Ok(mapper.Map<CompletedProjectsDTO[]>(this.unitOfWork.Projects.getCompletedProjects));
        }

        [HttpGet("[action]")]
        public IActionResult New()
        {
            return Ok(mapper.Map<ProjectsByCategoryDTO[]>(this.unitOfWork.Projects.getNewProjects));
        }

        [HttpGet("[action]")]
        public IActionResult Terminating()
        {
            projectsByCategoria = this.unitOfWork.Projects.getTerminatingProjects;
            var model = mapper.Map<ProjectsByCategoryDTO[]>(this.projectsByCategoria);
            return Ok(model);
        }

        [HttpGet("[action]")]
        public IActionResult Category([FromQuery] string nameCategory)
        {
            if (string.IsNullOrEmpty(nameCategory))
            {
                projectsByCategoria = this.unitOfWork.Projects.projects;
            }

            else
            {
                projectsByCategoria = this.unitOfWork.Projects.ProjectByCategory(nameCategory);

                if (!projectsByCategoria.Any())
                {
                    projectsByCategoria = this.unitOfWork.Projects.projects;
                }
            }

            var model = mapper.Map<ProjectsByCategoryDTO[]>(projectsByCategoria);
            return Ok(model);
        }
    }
}
