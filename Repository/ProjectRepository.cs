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
        private DateTime terminatingDate = DateTime.Now.AddMonths(4);

        public ProjectRepository(ApplicationDbContent dbContent)
        {
            this.dbContent = dbContent;
        }

        public IQueryable<Project> projects
        {
            get
            {
                return dbContent.Projects.Include(x => x.Category);
            }
        }

        public IQueryable<Project> getCompletedProjects
        {
            get
            {
                return dbContent.Projects.Where(x => x.IsCompleted == true);
            }
        }

        public IQueryable<Project> getNewProjects
        {
            get
            {
                return dbContent.Projects.Where(x => x.EndDate > x.EndDate.AddYears(-1) && x.IsCompleted == false);
            }
        }

        public IQueryable<Project> getTerminatingProjects
        {
            get
            {
                return dbContent.Projects.Where(x => x.EndDate <= terminatingDate && x.EndDate >= DateTime.Now);
            }
        }

        public IQueryable<Project> ProjectByCategory(string nameCategory)
        {
            return dbContent.Projects.Where(x => x.Category.CategoryName == nameCategory).Include(x => x.Category);
        }

        public async void SaveProjectAsync(Project project)
        {
            Project proj = await dbContent.Projects.FindAsync(project.Id); 

            if(proj != null)
            {
                proj.Name = project.Name;
                proj.IsCompleted = project.IsCompleted;
                proj.EndDate = project.EndDate;
                proj.ShortDescription = project.ShortDescription;
                proj.LongDescription = project.LongDescription;
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
                this.dbContent.Projects.Remove(project);
                await dbContent.SaveChangesAsync();
            }

            await dbContent.SaveChangesAsync();
            return project;
        }


    }
}
