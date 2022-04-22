#region references
using System.Text;
using MassTransit.Audit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Restaurant.Booking;
using Restaurant.Booking.Audit;
using Restaurant.Booking.Extensions;
using Restaurant.Booking.Models;
using Restaurant.Booking.Saga;
using Restaurant.Messages.Repositories.Implementation;
using Restaurant.Messages.Repositories.Interfaces;
#endregion

Console.OutputEncoding = Encoding.UTF8;

#region services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IMessageAuditStore, AuditStore>();

builder.Services.AddAndConfigMassTransit();

builder.Services.AddTransient<RestaurantBooking>();

builder.Services.AddTransient<RestaurantBookingSaga>();

builder.Services.AddTransient<Restaurant.Booking.Restaurant>();

builder.Services.AddSingleton<IInMemoryRepository<BookingRequestModel>, InMemoryRepository<BookingRequestModel>>();

builder.Services.AddHostedService<Worker>();
#endregion

#region pipeline
var app = builder.Build();

app.UseRouting();

app.MapMetrics();

app.MapControllers();

app.Run();
#endregion