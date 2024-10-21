using System;
using HotelBooking.Core;
using Moq;
using Xunit;

namespace HotelBooking.Specs.StepDefinitions
{
    [Binding]
    public class CreateBookingStepDefinitions
    {
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> mockBookingRepository;
        private Mock<IRepository<Room>> mockRoomRepository;

        private List<Room> rooms;
        private Booking booking;
        private bool bookingResult;

        public CreateBookingStepDefinitions()
        {
            mockBookingRepository = new();
            mockRoomRepository = new();
            bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
            
            // Create a list of rooms
            rooms = new List<Room>
            {
                new Room { Id = 1, Description = "Deluxe Room" },
                new Room { Id = 2, Description = "Suite Room" },
                new Room { Id = 3, Description = "Standard Room" }
            };
            
            // Setup mock room repository to return the list of rooms
            mockRoomRepository.Setup(repo => repo.GetAll()).Returns(rooms);
        }
        
        

        [Given(@"a hotel room is available for the period from ""(.*)"" to ""(.*)""")]
        public void GivenAHotelRoomIsAvailableForThePeriodFromTo(string roomAvailableStartDate, string roomAvailableEndDate)
        {
            // Arrange the booking with available room period
            var availableStartDate = DateTime.Parse(roomAvailableStartDate);
            var availableEndDate = DateTime.Parse(roomAvailableEndDate);
            
            booking = new Booking
            {
                StartDate = availableStartDate,
                EndDate = availableEndDate,
                CustomerId = 20,
                IsActive = true,
                RoomId = rooms[0].Id,  // The first room in the list is assigned
                Room = rooms[0],
                Customer = new Customer { Id = 20, Name = "John Doe", Email = "johndoe@email.com" }
            };
        }

        [When(@"a user attempts to book the room for the period from ""(.*)"" to ""(.*)""")]
        public void WhenAUserAttemptsToBookTheRoomForThePeriodFromTo(string bookingStartDate, string bookingEndDate)
        {
            // Act: try to create a booking for the specified period
            var bookingAttempt = new Booking
            {
                RoomId = booking.Room.Id,
                StartDate = DateTime.Parse(bookingStartDate),
                EndDate = DateTime.Parse(bookingEndDate),
                CustomerId = booking.CustomerId
            };
            
            bookingResult = bookingManager.CreateBooking(bookingAttempt);
        }

        [Then(@"the booking status ""(.*)"" should be ""(.*)""")]
        public void ThenTheBookingStatusShouldBe(string bookingStatus, string expectedStatus)
        {
            // Assert: check if the booking status matches the expected result
            var expectedResult = bool.Parse(expectedStatus);
            Assert.Equal(expectedResult, bookingResult);
        }
    }
}
