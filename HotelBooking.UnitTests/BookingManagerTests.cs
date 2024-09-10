using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using System.Linq;


namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
        IRepository<Booking> bookingRepository;

        private readonly Room room;
        private readonly Customer customer;
        private readonly Booking booking;

        public BookingManagerTests()
        {
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);

            room = new Room
            {
                Id = 10,
                Description = "testSuite"
            };

            customer = new Customer
            {
                Id = 20,
                Email = "johndoe@email.com",
                Name = "John Doe",
            };


            booking = new Booking
            {
                Id = 1,
                Room = room,
                StartDate = DateTime.Now,
                EndDate = DateTime.Today.AddDays(2),
                RoomId = 10,
                Customer = customer,
                CustomerId = 20,
                IsActive = true,
            };
        }


        //Tests for createBooking

        [Fact]
        public void CreateBooking_SuccessfulBooking_ReturnsTrue()
        {
            //Act 
            var result = bookingManager.CreateBooking(booking);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CreateBooking_UnsuccessfulBooking_ReturnsFalse()
        {
            //Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(212312)
            };

            // Act
            var result = bookingManager.CreateBooking(booking);

            // Assert
            Assert.False(result);
        }
        

        // Tests for FindAvailableRoom

        [Fact]
        public void FindAvailableRoom_WithValidDateRange_ReturnsRoomId()
        {
            // Arrange
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Today.AddDays(1);
            
            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);
            
            // Assert
            Assert.InRange(roomId, 0, int.MaxValue);
        }
        
        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today;

            // Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_ReturnsAvailableRoom()
        {
            // This test was added to satisfy the following test design
            // principle: "Tests should have strong assertions".

            // Arrange
            DateTime date = DateTime.Today.AddDays(1);

            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);

            var bookingForReturnedRoomId = bookingRepository.GetAll().Where(
                b => b.RoomId == roomId
                     && b.StartDate <= date
                     && b.EndDate >= date
                     && b.IsActive);

            // Assert
            Assert.Empty(bookingForReturnedRoomId);
        }
        
        
        [Fact]
        public void FindAvailableRoom_NoRoomsAvailable_ReturnsMinus1()
        {
            // Arrange
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Today.AddDays(210000);

            // Act
            var result = bookingManager.FindAvailableRoom(startDate, endDate);

            // Assert
            Assert.Equal(-1, result);
        }
        
        
        [Fact]  
        public void GetFullyOccupiedDates_RangeWithFullyOccupiedRooms_ReturnsFullyOccupiedDates()  
        {  
            // Arrange  
            var startDate = new DateTime(2023, 10, 1);  
            var endDate = new DateTime(2023, 10, 5);  
  
            // var rooms = new List<Room>   
            // {   
            //     new Room { Id = 1 },   
            //     new Room { Id = 2 }   
            // };  
  
            // var bookings = new List<Booking>  
            // {  
            //     new Booking { RoomId = 1, StartDate = new DateTime(2023, 10, 1), EndDate = new DateTime(2023, 10, 3), IsActive = true },  
            //     new Booking { RoomId = 2, StartDate = new DateTime(2023, 10, 1), EndDate = new DateTime(2023, 10, 3), IsActive = true },  
            //     new Booking { RoomId = 1, StartDate = new DateTime(2023, 10, 4), EndDate = new DateTime(2023, 10, 5), IsActive = true },  
            //     new Booking { RoomId = 2, StartDate = new DateTime(2023, 10, 4), EndDate = new DateTime(2023, 10, 5), IsActive = true }  
            // };  
  
            // Act  
            var result = bookingManager.GetFullyOccupiedDates(startDate, endDate);  
  
            // Assert  
            var expectedDates = new List<DateTime>   
            {   
                new DateTime(2023, 10, 1),  
                new DateTime(2023, 10, 2),  
                new DateTime(2023, 10, 3),  
                new DateTime(2023, 10, 4),  
                new DateTime(2023, 10, 5)  
            };  
  
            Assert.Equal(expectedDates, result);  
        }  
    }
    
}