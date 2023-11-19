using ReportService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReportService;
using ReportService.Clients;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("DbContextConnection") ?? throw new InvalidOperationException("Connection string 'etkinlikyonetimiDbContextConnection' not found.");

//builder.Services.AddDbContext<ReportDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDbContext<ReportDbContext>(options => options.UseInMemoryDatabase("reportdb"));

builder.Services.AddControllers();

builder.Services.AddMassTransit(config =>
{
	config.AddConsumer<ReportConsumer>();
	config.UsingRabbitMq((ctx, cfg) =>
	{
		cfg.UseMessageRetry(x => x.Interval(2, 1000));
		cfg.Host("amqps://iuppvhyz:iBReTgl07FWofHlh2l_Dc83sXnLypy3K@shark.rmq.cloudamqp.com/iuppvhyz", hostConfig => {
			hostConfig.Username("iuppvhyz");
			hostConfig.Password("iBReTgl07FWofHlh2l_Dc83sXnLypy3K");
		});

		cfg.ReceiveEndpoint("report-queue", e =>
		{
			e.ConfigureConsumer<ReportConsumer>(ctx);
		});
	});
});

builder.Services.AddMassTransitHostedService();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ContactClient>(a =>
{
	a.BaseAddress = new Uri("https://localhost:44330");
});

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
	var dbContext = serviceScope.ServiceProvider.GetRequiredService<ReportDbContext>();
	//dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
