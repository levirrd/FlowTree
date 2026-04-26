using FlowTree.Avalonia.Data;
using FlowTree.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowTree.Avalonia.Services
{
    public class ProjectService
    {
        private readonly string _connectionString;

        public ProjectService(string connectionString)
        {
            _connectionString = connectionString;
        }
        // GET all projects, ordered by their ID.
        public async Task<List<Project>> GetProjectsAsync()
        {
            await using var context = new FlowTreeDbContext(_connectionString);

            return await context.Projects
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
        // Creates a new project with the specified name, subtitle, and description, and saves it to the database.
        public async Task CreateProjectAsync(string name, string subtitle, string description)
        {
            await using var context = new FlowTreeDbContext(_connectionString);

            context.Projects.Add(new Project
            {
                Name = name,
                Subtitle = subtitle,
                Description = description
            });

            await context.SaveChangesAsync();
        }
    }
}