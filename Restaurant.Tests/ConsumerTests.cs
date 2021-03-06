using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Restaurant.Booking.Consumers;
using Restaurant.Booking.Models;
using Restaurant.Kitchen;
using Restaurant.Kitchen.Consumers;
using Restaurant.Kitchen.DAL.Models;
using Restaurant.Kitchen.DAL.Repositories.Implementation;
using Restaurant.Kitchen.DAL.Repositories.Interfaces;
using Restaurant.Messages;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;
using Restaurant.Messages.Repositories.Implementation;
using Restaurant.Messages.Repositories.Interfaces;
using Restaurant.Notification.Consumers;

namespace Restaurant.Tests
{
    public class ConsumerTests : ITests
    {
        private ServiceProvider _provider;
        private ITestHarness _harness;

        [OneTimeSetUp]
        public async Task Init()
        {
            _provider = new ServiceCollection()
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<BookingRequestConsumer>();
                    cfg.AddConsumer<KitchenTableBookedConsumer>();
                    cfg.AddConsumer<NotifyConsumer>();
                    cfg.AddConsumer<KitchenAccidentConsumer>();
                    cfg.AddConsumer<BookingRequestFaultConsumer>();
                })
                .AddLogging()
                .AddTransient<Booking.Restaurant>()
                .AddSingleton<IInMemoryRepository<BookingRequestModel>, InMemoryRepository<BookingRequestModel>>()
                .AddSingleton<IKitchenMessageRepository<KitchenTableBookedModel>, KitchenMessageRepository>()
                .AddSingleton<IConnection, Connection>()
                .AddSingleton<Manager>()
                .BuildServiceProvider(validateScopes: true);

            _harness = _provider.GetTestHarness();

            await _harness.Start();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            //await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
            await _provider.DisposeAsync();
        }

        [Test]
        public async Task Any_booking_request_consumed()
        {
            await _harness.Bus.Publish(
                new BookingRequest(
                    Guid.NewGuid(), 
                    Guid.NewGuid(), 
                    new Dish{ Id = Random.Shared.Next(1, 6) },
                    TimeSpan.FromSeconds(Random.Shared.Next(7, 15)), 
                    DateTime.Now));

            Assert.That(await _harness.Consumed.Any<IBookingRequest>());

            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        }

        [Test]
        public async Task Booking_request_consumer_published_table_booked_message()
        {
            var consumer = _harness.GetConsumerHarness<BookingRequestConsumer>();

            var orderId = Guid.NewGuid();
            var bus = _provider.GetRequiredService<IBus>();

            await bus.Publish<IBookingRequest>(
                new BookingRequest(
                    orderId, 
                    orderId,
                   new Dish { Id = Random.Shared.Next(1, 6) },
                    TimeSpan.FromSeconds(Random.Shared.Next(7, 15)),
                    DateTime.Now));

            Assert.That(consumer.Consumed.Select<IBookingRequest>()
                .Any(predicate => predicate.Context.Message.OrderId == orderId), Is.True);

            Assert.That(_harness.Published.Select<ITableBooked>()
                .Any(predicate => predicate.Context.Message.OrderId == orderId), Is.True);

            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        }

        [Test]
        public async Task Any_consumer_request_to_kitchen()
        {
            await _harness.Bus.Publish(
                new BookingRequest(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    new Dish { Id = Random.Shared.Next(1, 6) },
                    TimeSpan.FromSeconds(Random.Shared.Next(7, 15)),
                    DateTime.Now));

            Assert.That(await _harness.Consumed.Any<IBookingRequest>());

            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        }

        [Test]
        public async Task Any_message_notification_service_consumption()
        {
            await _harness.Bus.Publish(new Notify(
                Guid.NewGuid(), 
                Guid.NewGuid(), 
                ""));

            Assert.That(await _harness.Consumed.Any<INotify>());

            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        }

        [Test]
        public async Task Any_message_case_accident_booking_service()
        {
            await _harness.Bus.Publish(new KitchenAccident(
                Guid.NewGuid(), 
                new Dish()));

            Assert.That(await _harness.Consumed.Any<IKitchenAccident>());

            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        }

        [Test]
        public async Task Booking_request_consumer_publication_accident_report()
        {
            var consumer = _harness.GetConsumerHarness<KitchenTableBookedConsumer>();

            var orderId = Guid.NewGuid();
            var bus = _provider.GetRequiredService<IBus>();

            await bus.Publish<IBookingRequest>(
                new BookingRequest(
                    orderId,
                    orderId,
                    new Dish { Id = 5 },
                    TimeSpan.FromSeconds(Random.Shared.Next(7, 15)),
                    DateTime.Now));

            Assert.That(consumer.Consumed.Select<IBookingRequest>()
                .Any(predicate => predicate.Context.Message.OrderId == orderId), Is.True);

            Assert.That(_harness.Published.Select<IKitchenAccident>()
                .Any(predicate => predicate.Context.Message.OrderId == orderId), Is.True);

            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        }
    }
}