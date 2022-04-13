﻿using MassTransit;
using Restaurant.Booking.Models;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;
using Restaurant.Messages.Repositories.Interfaces;

namespace Restaurant.Booking.Consumers
{
    public class BookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;
        private readonly IInMemoryRepository<BookingRequestModel> _repository;

        public BookingRequestConsumer(Restaurant restaurant, IInMemoryRepository<BookingRequestModel> repository)
        {
            _restaurant = restaurant;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            Console.WriteLine($"[Заказ с номером: {context.Message.OrderId}]");
            var result = await _restaurant.BookFreeTableAsync(1);

            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }
    }
}
