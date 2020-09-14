using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anthill.API.Interfaces;
using Anthill.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Anthill.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IProjectRepository project;
        public AdminController(IProjectRepository project)
        {
            this.project = project;
        }


        [HttpPut("EditProject/{id}")]
        public IActionResult EditProject(Project project)
        {
            if (ModelState.IsValid)
            {
                this.project.SaveProjectAsync(project);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("DeleteProject/{id}")]
        public IActionResult DeleteProject(int id)
        {
            var project = this.project.DeleteProjectAsync(id).Result;
            if(project != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest("There is no project with this id");
            }
        }
    }
}
