using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Anthill.API.Models;
using Anthill.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Anthill.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly ManagerRepository managerRepository;

        public FavouriteController(ManagerRepository managerRepository)
        {
            this.managerRepository = managerRepository;
        }


        [HttpPost("[action]")]
        public IActionResult Add([FromBody] Project project)
        {
            this.managerRepository.Favourites.AddProjectToFavouritesAsync(project);
            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete([FromQuery]int id)
        {
            this.managerRepository.Favourites.DeleteProjectFromFavouritesAsync(id);
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult Get()
        {
            this.managerRepository.Favourites.GetProjectFromFavourites();
            return Ok();
        }
    }
}
