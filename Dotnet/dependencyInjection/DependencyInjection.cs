using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sabio.Data;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Services.Interfaces.Surveys;
using Sabio.Services.Services;
using Sabio.Services.Surveys;
using Sabio.Web.Core.Services;
using System;
using Sabio.Services.Interfaces.Security;

using Amazon.S3;

namespace Sabio.Web.StartUp
{
    public class DependencyInjection
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is IConfigurationRoot)
            {
                services.AddSingleton<IConfigurationRoot>(configuration as IConfigurationRoot);   // IConfigurationRoot
            }

            services.AddSingleton<IConfiguration>(configuration);   // IConfiguration explicitly

            string connString = configuration.GetConnectionString("Default");
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2
            // The are a number of differe Add* methods you can use. Please verify which one you
            // should be using services.AddScoped<IMyDependency, MyDependency>();

            // services.AddTransient<IOperationTransient, Operation>();

            // services.AddScoped<IOperationScoped, Operation>();

            // services.AddSingleton<IOperationSingleton, Operation>();

            services.AddSingleton<IAuthenticationService<int>, WebAuthenticationService>();

            services.AddSingleton<Sabio.Data.Providers.IDataProvider, SqlDataProvider>(delegate (IServiceProvider provider)
            {
                return new SqlDataProvider(connString);
            }
            );
            services.AddSingleton<IAnswerService, AnswerService>();
            services.AddSingleton<IAnswerOptionsService, AnswerOptionsService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IFAQsService, FAQsService>();
            services.AddSingleton<IFAQCategoryService, FAQCategoryService>();
            services.AddSingleton<IFAQsService, FAQsService>();
            services.AddSingleton<IFilesService, FilesService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IFileTypeService, FileTypeService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IIdentityProvider<int>, WebAuthenticationService>();
            services.AddSingleton<IInstancesService, InstancesService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserTokensService, UserTokensService>();
            services.AddSingleton<IUserProfileService, UserProfileService>();
            services.AddSingleton<ILocationsService, LocationsService>();
            services.AddSingleton<ILocationTypesService, LocationTypesService>();
            services.AddSingleton<IQuestionServices, QuestionServices>();
            services.AddSingleton<IRecommendationsService, RecommendationsService>();
            services.AddSingleton<IResourceCategoryService, ResourceCategoryService>();
            services.AddSingleton<IResourceRecommendationConfigService, ResourceRecommendationConfigService>();
            services.AddSingleton<IResourceService, ResourceService>();
            services.AddSingleton<ISectionsService, SectionsService>();
            services.AddSingleton<IStatesService, StatesService>();
            services.AddSingleton<ISurveyService, SurveyService>();
            services.AddSingleton<ISurveyDetailsService, SurveyDetailsService >();
            services.AddSingleton<IS3Service, S3Service>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserProfileService, UserProfileService>();
            services.AddSingleton<IUserTokensService, UserTokensService>();
            services.AddSingleton<IVenueServices, VenueServices>();
        }
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}