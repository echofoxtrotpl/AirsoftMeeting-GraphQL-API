using AirsoftMeetingGraphQL.Entities;
using AirsoftMeetingGraphQL.GraphQL;
using AirsoftMeetingGraphQL.GraphQL.Events;
using AirsoftMeetingGraphQL.GraphQL.Locations;
using AirsoftMeetingGraphQL.GraphQL.Mutations;
using AirsoftMeetingGraphQL.GraphQL.Players;
using AirsoftMeetingGraphQL.GraphQL.Teams;
using AirsoftMeetingGraphQL.Services;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AirsoftMeetingGraphQL
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<EventService>();
            
            services.AddPooledDbContextFactory<AirsoftDbContext>(opt =>
                opt.UseNpgsql(_configuration.GetConnectionString("ConnectPSQL")));

            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<Query>()
                .AddMutationType<EventMutation>()
                .AddTypeExtension<ImageMutation>()
                .AddTypeExtension<PlayerMutation>()
                .AddType<EventType>()
                .AddType<LocationType>()
                .AddType<PlayerType>()
                .AddType<TeamType>()
                .AddType<UploadType>()
                .AddSorting()
                .AddFiltering();
            
            // TODO: Change this according to your JWT credentials
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.Authority = "https://example.com";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://example.com",
                        ValidateAudience = true,
                        ValidAudience = "example",
                        ValidateLifetime = true
                    }; 
                });
            
            // TODO: Change this according to your frontend address
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://example.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Location")
                        .AllowCredentials();
                });
            });
            services.AddScoped<IImageService, ImageService>();
            services.Configure<FormOptions>(options =>
            {
                // Setting the upload limit to 5 MB
                options.MultipartBodyLengthLimit = 5 * 1024 * 1024;
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
