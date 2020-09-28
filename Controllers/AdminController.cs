using Anthill.Infastructure.Interfaces;
using Anthill.Infastructure.Models;
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
        private readonly IUnitOfWork unitOfWork;
        public AdminController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [HttpPut("EditProject/{id}")]
        public IActionResult EditProject(Project project)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.Projects.SaveProjectAsync(project);
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
            var project = this.unitOfWork.Projects.DeleteProjectAsync(id).Result;
            if (project != null)
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
