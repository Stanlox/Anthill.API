﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Anthill.API.Models
{
    /// <summary>
    /// Specifies the contextual information about an application thread.
    /// </summary>
    public class ApplicationDbContent : IdentityDbContext<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContent"/> class.
        /// </summary>
        /// <param name="options">Input configurations.</param>
        public ApplicationDbContent(DbContextOptions<ApplicationDbContent> options)
            : base(options)
        {
        }
    }
}