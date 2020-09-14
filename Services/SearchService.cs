using Anthill.API.Interfaces;
using Anthill.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Services
{
    public class SearchService
    {
        private readonly IProjectRepository project;
        private List<string> listOfProjectsName = new List<string>();
        private IEnumerable<Project> listOfProjects = new List<Project>();

        public SearchService(IProjectRepository project, IEnumerable<Project> listOfProjects)
        {
            this.project = project;
            this.listOfProjects = listOfProjects;
            foreach (var proj in project.projects)
            {
                listOfProjectsName.Add(proj.Name);
            }
        }
        public IEnumerable<string> GetMostSimilarProjectsName(string nameProject)
        {

            var requestCommandSymbols = nameProject.ToUpperInvariant();
            var projectIntersactions = listOfProjectsName.Select(command => (command, command.ToUpperInvariant()))
                .Select(commandTuple => (commandTuple.command, commandTuple.Item2.Intersect(requestCommandSymbols).Count()));
            var max = projectIntersactions.Max(tuple => tuple.Item2);
            return max > 2 ? projectIntersactions.Where(tuple => tuple.Item2.Equals(max)).Select(tuple => tuple.command) : Enumerable.Empty<string>();
        }
    }
}
