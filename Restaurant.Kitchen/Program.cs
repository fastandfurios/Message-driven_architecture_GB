#region references
using System.Text;
using MassTransit.Audit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Restaurant.Kitchen;
using Restaurant.Kitchen.Audit;
using Restaurant.Kitchen.DAL.Models;
using Restaurant.Kitchen.DAL.Repositories.Implementation;
using Restaurant.Kitchen.DAL.Repositories.Interfaces;
using Restaurant.Kitchen.Extensions;
#endregion

Console.OutputEncoding = Encoding.UTF8;

#region services
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(urls: "http://localhost:5050");

builder.Services.AddControllers();

builder.Services.AddSingleton<IMessageAuditStore, AuditStore>();

builder.Services.AddAndConfigMassTransit();

builder.Services.AddSingleton<Manager>();

builder.Services.AddSingleton<IKitchenMessageRepository<KitchenTableBookedModel>, KitchenMessageRepository>();

builder.Services.AddSingleton<IConnection, Connection>();

builder.Services.SqLiteConfiguring("Data Source=C:\\Test\\Messages.db;Version=3;New=True;Compress=True;");
#endregion

#region pipeline
var app = builder.Build();

app.UseRouting();

app.MapMetrics();

app.MapControllers();

app.Run();
#endregion
