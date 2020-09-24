using Anthill.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Interfaces
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Get all projects
        /// </summary>
        IQueryable<Project> projects { get; }

        /// <summary>
        /// Get all completed projects
        /// </summary>
        IQueryable<Project> completed { get; }

        /// <summary>
        /// Save a project with a change.
        /// </summary>
        /// <param name="project">Input project.</param>
        void SaveProjectAsync(Project project);


        /// <summary>
        /// Get all projects by category
        /// </summary>
        /// <param name="nameCategory"></param>
        /// <returns></returns>
        IQueryable<Project> projectByCategory(string nameCategory);


        /// <summary>
        /// Delete project by id.
        /// </summary>
        /// <param name="id">Project id.</param>
        /// <returns>Remote project.</returns>
        Task<Project> DeleteProjectAsync(int id);
    }
}
