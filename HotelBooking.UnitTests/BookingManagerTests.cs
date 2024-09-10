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
        private IRepository<Booking> bookingRepository;
        private IRepository<Room> roomRepository;

        private readonly Room room;
        private readonly Customer customer;
        private readonly Booking booking;

        public BookingManagerTests()
        {
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            bookingRepository = new FakeBookingRepository(start, end);
            roomRepository = new FakeRoomRepository();
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
        
        [Theory]
        [InlineData(10, 20, 11)]
        [InlineData(16, 66, 5)]
        [InlineData(20, 26, 1)]
        public void GetFullyOccupiedDates_RangeWithFullyOccupiedRooms_ReturnsDates(int startDate, int endDate, int expectedCount)  
        {  
            // Arrange  
            var start = DateTime.Today.AddDays(startDate);
            var end = DateTime.Today.AddDays(endDate);  
  
            // Act  
            var result = bookingManager.GetFullyOccupiedDates(start, end);  
  
            // Assert  
            Assert.Equal(expectedCount, result.Count);  
        }  
        
        [Fact]  
        public void GetFullyOccupiedDates_ReturnsEmpty()  
        {  
            // Arrange  
            var startDate = DateTime.Today.AddDays(40);
            var endDate = DateTime.Today.AddDays(80);  
  
            // Act  
            var result = bookingManager.GetFullyOccupiedDates(startDate, endDate);  
  
            // Assert  
            Assert.Empty(result);  
        } 
        
        [Fact]
        public void GetFullyOccupiedDates_InvalidDateRange_ThrowsArgumentException()  
        {  
            // Arrange  
            var startDate = DateTime.Today.AddDays(340);
            var endDate = DateTime.Today.AddDays(80);  
  
            // Act  
            Action act = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);  
  
            // Assert  
            Assert.Throws<ArgumentException>(act);  
        } 
    }
}