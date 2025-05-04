using NLog.Web;
using HRMS_Api;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container.

	SetupServices.RegisterServices(builder.Services, builder.Configuration);

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
		app.UseCors(builder => builder
		 .AllowAnyOrigin()
		 .AllowAnyMethod()
		 .AllowAnyHeader());
	}
	app.UseHttpsRedirection();

	app.UseAuthorization();
	app.MapControllers();

	app.Run();
}
catch(Exception ex)
{
	// Log the exception
	logger.Error(ex, "An error occurred");
}
finally
{
	NLog.LogManager.Shutdown();
}

