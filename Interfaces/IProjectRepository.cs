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
        IEnumerable<Project> projects { get; }

        /// <summary>
        /// Get all completed projects
        /// </summary>
        IEnumerable<Project> completed { get; }

        /// <summary>
        /// Save a project with a change.
        /// </summary>
        /// <param name="project">Input project.</param>
        void SaveProjectAsync(Project project);


        /// <summary>
        /// Delete project by id.
        /// </summary>
        /// <param name="id">Project id.</param>
        /// <returns>Remote project.</returns>
        Task<Project> DeleteProjectAsync(int id);
    }
}
