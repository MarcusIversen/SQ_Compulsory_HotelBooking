using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests;

public class BookingManagerTestsMoq
{
    private IBookingManager bookingManager;
    private Mock<IRepository<Booking>> mockBookingRepository;
    private Mock<IRepository<Room>> mockRoomRepository;

    public BookingManagerTestsMoq()
    {
        mockBookingRepository = new();
        mockRoomRepository = new();
        bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
    }

    [Theory]
    [InlineData(false, 10)] // Test case 1: booking is inactive, expect room ID 10 to be returned
    [InlineData(true, 20)] // Test case 2: booking is active, expect room ID 20 to be returned
    public void FindAvailableRoom_ReturnsAvailableRoomId(bool isActive, int expectedRoomId)
    {
        // Arrange
        DateTime startDate = DateTime.Now;
        DateTime endDate = startDate.AddDays(5);

        // Create two room objects with different IDs and descriptions
        var room1 = new Room
        {
            Id = 10,
            Description = "testSuite"
        };
        var room2 = new Room
        {
            Id = 20, 
            Description = "SuiteTest"
        };

        // Add the rooms to a list (to simulate available rooms in the repository)
        var rooms = new List<Room> { room1, room2 };

        // Create a booking object for room1
        var booking = new Booking
        {
            Id = 1, 
            Room = room1,
            StartDate = startDate,
            EndDate = endDate,
            RoomId = room1.Id, 
            IsActive = isActive,
        };

        // Add the booking to a list (to simulate existing bookings in the repository)
        var bookings = new List<Booking> { booking };

        // We mock to find available rooms, without this the test does not work
        mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);
        mockBookingRepository.Setup(b => b.GetAll()).Returns(bookings);

        // Act
        var result = bookingManager.FindAvailableRoom(startDate, endDate);

        // Assert
        // Verify that the result matches the expected room ID for this test case
        Assert.Equal(expectedRoomId, result);
    }
}