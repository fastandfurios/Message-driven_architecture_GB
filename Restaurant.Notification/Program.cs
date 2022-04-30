#region references
using System.Text;
using MassTransit.Audit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Restaurant.Messages.Repositories.Implementation;
using Restaurant.Messages.Repositories.Interfaces;
using Restaurant.Notification;
using Restaurant.Notification.Audit;
using Restaurant.Notification.Extensions;
using Restaurant.Notification.Models;
#endregion

Console.OutputEncoding = Encoding.UTF8;

#region services
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(urls: "http://localhost:5100");

builder.Services.AddControllers();

builder.Services.AddSingleton<IMessageAuditStore, AuditStore>();

builder.Services.AddAndConfigMassTransit();

builder.Services.AddSingleton<Notifier>();

builder.Services.AddSingleton<IInMemoryRepository<NotifyModel>, InMemoryRepository<NotifyModel>>();
#endregion

#region pipeline
var app = builder.Build();

app.UseRouting();

app.MapMetrics();

app.MapControllers();

app.Run();
#endregion