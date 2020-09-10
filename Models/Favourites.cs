using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Models
{
    public class Favourites
    {
        public int id { get; set; }
        public Projects Projects { get; set; }
        public string ProjectIdInFavourites { get; set; }
    }
}
