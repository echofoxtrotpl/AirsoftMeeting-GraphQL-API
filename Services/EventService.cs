using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirsoftMeetingGraphQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AirsoftMeetingGraphQL.Services
{
    // This is a background service for maintaining and cleaning inactive events and their data (images in the filesystem and db records)
    public class EventService : BackgroundService
    {
        private readonly IDbContextFactory<AirsoftDbContext> _dbContextFactory;

        public EventService(IDbContextFactory<AirsoftDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start this service at 00:00 and execute every 24 hours
            var interval = TimeSpan.FromHours(24);
            var firstDelay = DateTime.Today.AddDays(1).AddTicks(-1).Subtract(DateTime.Now);
            
            await Task.Delay(firstDelay, stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                SetInactiveEvents();
                DeleteEventsWithImages();
                await Task.Delay(interval, stoppingToken);
            }
        }

        private async void SetInactiveEvents()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var events = context.Events.Where(e => e.Active && e.EndDate < DateTime.Now).ToList();
                foreach (var e in events)
                {
                    e.Active = false;
                    e.DeleteDate = DateTime.Today.AddDays(30);
                }

                await context.SaveChangesAsync();
            }
        }

        private async void DeleteEventsWithImages()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var events = context.Events
                    .Include(r => r.Location)
                    .Include(r => r.Image)
                    .Include(r => r.PlayersJoinedEvent)
                    .Where(e => !e.Active && e.DeleteDate <= DateTime.Now).ToList();
                
                foreach (var e in events)
                {
                    context.Remove(e.Location);
                    context.Remove(e.Image);

                    foreach (var p in e.PlayersJoinedEvent)
                    {
                        context.Remove(p);
                    }
                    
                    var path = $"../uploaded/images/{e.Image.Folder}/";

                    try
                    {
                        File.Delete(Path.Combine(path, $"original_{e.Image.Url}"));
                        File.Delete(Path.Combine(path, $"thumbnail_{e.Image.Url}"));
                        File.Delete(Path.Combine(path, $"fullscreen_{e.Image.Url}"));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}