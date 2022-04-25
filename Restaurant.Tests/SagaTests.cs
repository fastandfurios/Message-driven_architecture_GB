using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Restaurant.Booking.Consumers;
using Restaurant.Booking.Models;
using Restaurant.Booking.Saga;
using Restaurant.Kitchen;
using Restaurant.Kitchen.Consumers;
using Restaurant.Messages;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;
using Restaurant.Messages.Repositories.Implementation;
using Restaurant.Messages.Repositories.Interfaces;
using DependencyInjectionTestingExtensions = MassTransit.Testing.DependencyInjectionTestingExtensions;

namespace Restaurant.Tests
{
    public class SagaTests : ITests
    {
        private ServiceProvider _provider;
        private InMemoryTestHarness _harness;

        [OneTimeSetUp]
        [Obsolete("Obsolete")]
        public async Task Init()
        {
            _provider = DependencyInjectionTestingExtensions.AddMassTransitInMemoryTestHarness(new ServiceCollection(), cfg =>
                {
                    cfg.AddConsumer<KitchenTableBookedConsumer>();
                    cfg.AddConsumerTestHarness<KitchenTableBookedConsumer>();
                    cfg.AddConsumer<BookingRequestConsumer>();

                    cfg.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>().InMemoryRepository();
                    cfg.AddSagaStateMachineTestHarness<RestaurantBookingSaga, RestaurantBooking>();
                })
                .AddLogging()
                .AddTransient<Booking.Restaurant>()
                .AddTransient<Manager>()
                .AddSingleton<IInMemoryRepository<BookingRequestModel>, InMemoryRepository<BookingRequestModel>>()
                .BuildServiceProvider(validateScopes: true);

            _harness = _provider.GetRequiredService<InMemoryTestHarness>();

            await _harness.Start();
        }

        public async Task TearDown()
        {
            await _harness.Stop();
            await _provider.DisposeAsync();
        }

        [Test]
        [Obsolete("Obsolete")]
        public async Task Should_be_easy()
        {
            var orderId = NewId.NextGuid();
            var clientId = NewId.NextGuid();

            await _harness.Bus.Publish(new BookingRequest(orderId,
                clientId,
                new Dish{ Id = Random.Shared.Next(1, 6) },
                TimeSpan.FromSeconds(Random.Shared.Next(7, 15)),
                DateTime.Now));

            Assert.That(await _harness.Published.Any<IBookingRequest>());
            Assert.That(await _harness.Consumed.Any<IBookingRequest>());

            var sagaHarness = _provider.GetRequiredService<IStateMachineSagaTestHarness<RestaurantBooking, RestaurantBookingSaga>>();

            Assert.That(await sagaHarness.Consumed.Any<IBookingRequest>());
            Assert.That(await sagaHarness.Created.Any(x => x.CorrelationId == orderId));

            var saga = sagaHarness.Created.Contains(orderId);

            Assert.That(actual: saga, Is.Not.Null);
            Assert.That(actual: saga.ClientId, Is.EqualTo(clientId));
            Assert.That(await _harness.Published.Any<ITableBooked>());
            Assert.That(await _harness.Published.Any<IKitchenReady>());
            Assert.That(await _harness.Published.Any<INotify>());
            Assert.That(saga.CurrentState, Is.EqualTo(2));

            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        }
    }
}
