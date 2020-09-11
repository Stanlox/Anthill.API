using Anthill.API.Interfaces;
using Anthill.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContent dbContent;

        public ProjectRepository(ApplicationDbContent dbContent)
        {
            this.dbContent = dbContent;
        }

        public IEnumerable<Project> projects
        {
            get
            {
                return dbContent.Projects.Include(x => x.Category);
            }
        }

        public IEnumerable<Project> completed
        {
            get
            {
                return dbContent.Projects.Where(x => x.IsCompleted).Include(x => x.Category);
            }
        }

        public async void SaveProjectAsync(Project project)
        {
            Project proj = await dbContent.Projects.FindAsync(project.Id); 

            if(proj != null)
            {
                proj.Name = project.Name;
                proj.IsCompleted = project.IsCompleted;
                proj.EndDate = project.EndDate;
                proj.Description = project.Description;
                proj.Category = proj.Category;
                proj.CategoryId = proj.CategoryId;
            }

            await dbContent.SaveChangesAsync();
        }

        public async Task<Project> DeleteProjectAsync(int id)
        {
            Project project = await dbContent.Projects.FindAsync(id);
            if(project != null)
            {
                this.dbContent.Remove(project);
                await dbContent.SaveChangesAsync();
            }

            return project;
        }


    }
}
