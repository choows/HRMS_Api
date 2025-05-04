
using HRMS_Api.DatabaseContext;
using HRMS_Api.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using System;

namespace HRMS_Api
{
	public static class SetupServices
	{
		//add dependency 
		/**
		 * Transient => always different; a new instance is provided to every controller and every service
		 * scoped => objects are the same within a request
		 * singlation => same for every object and every request 
		 * refer to https://stackoverflow.com/questions/38138100/addtransient-addscoped-and-addsingleton-services-differences
		 */

		public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();
			services.AddCors();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			var connectionString = configuration.GetConnectionString("HRMS") ?? throw new NullReferenceException("DB Connection String HRMS not configured.");
			services.AddDbContext<HRMSDbContext>(options =>
				options.UseSqlServer(connectionString));
			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.ClearProviders();
				loggingBuilder.AddNLog();
			});
			RegisterDependency(services, configuration);
		}

		private static void RegisterDependency(IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<ILocationService, LocationService>();
			services.AddScoped<IDepartmentService, DepartmentService>();
			services.AddScoped<IJobService, JobService>();
			services.AddScoped<IEmployeeService, EmployeeService>();
			services.AddScoped<IJobHistoryService, JobHistoryService>();
		}

	}
}
