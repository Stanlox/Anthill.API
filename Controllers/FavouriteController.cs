using Anthill.Infastructure.Interfaces;
using Anthill.Infastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Anthill.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase 
    {

        private IUnitOfWork unitOfWork;
        public FavouriteController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [HttpPost("[action]")]
        public IActionResult Add([FromBody] Project project)
        {
            this.unitOfWork.Favourites.AddProjectToFavouritesAsync(project);
            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete([FromQuery] int id)
        {
            this.unitOfWork.Favourites.DeleteProjectFromFavouritesAsync(id);
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult Get()
        {
            this.unitOfWork.Favourites.GetProjectFromFavourites();
            return Ok();
        }
    }
}
