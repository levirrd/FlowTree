using FlowTree.Avalonia.Data;
using FlowTree.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTree.Avalonia.Services
{
    public class CardContainerService
    {
        private readonly string _connectionString;

        public CardContainerService(string connectionString)
        {
            _connectionString = connectionString;
        }
        // GET all card containers for a specific project, ordered by their ID.
        public async Task<List<CardContainer>> GetByProjectIdAsync(int projectId)
        {
            await using var context = new FlowTreeDbContext(_connectionString);

            return await context.CardContainers
                .Where(c => c.ProjectId == projectId)
                .OrderBy(c => c.Id)
                .ToListAsync();
        }
        // Creates a new card container with the specified project ID, title, and description, and saves it to the database.
        public async Task CreateAsync(int projectId, string title, string description)
        {
            await using var context = new FlowTreeDbContext(_connectionString);

            context.CardContainers.Add(new CardContainer
            {
                ProjectId = projectId,
                Title = title,
                Description = description,
                IsCompleted = false
            });

            await context.SaveChangesAsync();
        }
    }
}
