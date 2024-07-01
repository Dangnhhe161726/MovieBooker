using JWT.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieBooker_backend.Models;
using MovieBooker_backend.Repositories.UserRepository;
using MovieBooker_backend.Repositories.CloudinaryRepository;
using StackExchange.Redis;
using System.Text;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Repositories.ScheduleRepository;
using MovieBooker_backend.Repositories.TimeSlotRepository;
using MovieBooker_backend.Repositories.RoleRepository;
using MovieBooker_backend.Repositories.MovieRepository;
using MovieBooker_backend.Responses;
using MovieBooker_backend.Repositories.YoutubeRepository;
using MovieBooker_backend.Repositories.MovieCategoryRepository;
using MovieBooker_backend.Repositories.MovieStatusRepository;
using MovieBooker_backend.Repositories.MovieImageRepository;
using MovieBooker_backend.Repositories.TheaterRepository;
using MovieBooker_backend.Repositories.SeatRepository;

namespace MovieBooker_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();


            //Config OData
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<UserDTO>("User");
            modelBuilder.EntitySet<ScheduleDTO>("Schedule");
            modelBuilder.EntitySet<MovieResponse>("Movie");
            builder.Services.AddControllers().AddOData(opt => opt
                .Select()
                .Expand()
                .Filter()
                .OrderBy()
                .Count()
                .SetMaxTop(100)
            .AddRouteComponents("odata", modelBuilder.GetEdmModel())
            );

			//Config auto mapper
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			//Config cloudinary
			builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddSingleton(sp =>
             {
                 var cloudinarySettings = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                 return new Cloudinary(new Account(
                     cloudinarySettings.CloudName,
                     cloudinarySettings.ApiKey,
                     cloudinarySettings.ApiSecret));
             });
			builder.Services.AddTransient<ICloudinaryRepository, CloudinaryRepository>();
		
	
			//builder.Services.AddControllers().AddOData(opt => opt
			//	.Select()
			//	.Expand()
			//	.Filter()
			//	.OrderBy()
			//	.Count()
			//	.SetMaxTop(100)
			//.AddRouteComponents("odata", modelBuilder.GetEdmModel()));
			//Config Youtube
			builder.Services.AddTransient<IYoutubeRepository, YoutubeRepository>();
			//Config Swagger
			builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            //Config Identity libraries sercurity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<bookMovieContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddCors();


            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379")); //redis


            builder.Services.AddDbContext<bookMovieContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
            builder.Services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
			builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<ITheaterRepository, TheaterRepository>();
            builder.Services.AddScoped<ISeatRepository, SeatRepository>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });




            builder.Services.AddAuthorization();
			//Config interface vs class
			builder.Services.AddScoped<IMovieCategoryRepsitory, MovieCategoryRepository>();
			builder.Services.AddScoped<IMovieStatusRepository, MovieStatusRepository>();
			builder.Services.AddScoped<IMovieImageRepository, MovieImageRepository>();
			builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
