using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;

namespace HotelBooking.UnitTests.Fakes
{
    public class FakeBookingRepository : IRepository<Booking>
    {
        private DateTime fullyOccupiedStartDate;
        private DateTime fullyOccupiedEndDate;

        public FakeBookingRepository(DateTime start, DateTime end)
        {
            fullyOccupiedStartDate = start;
            fullyOccupiedEndDate = end;
        }

        // This field is exposed so that a unit test can validate that the
        // Add method was invoked.
        public bool addWasCalled = false;

        public void Add(Booking entity)
        {
            addWasCalled = true;
        }

        // This field is exposed so that a unit test can validate that the
        // Edit method was invoked.
        public bool editWasCalled = false;

        public void Edit(Booking entity)
        {
            editWasCalled = true;
        }

        public Booking Get(int id)
        {
            return new Booking { Id = 1, StartDate = fullyOccupiedStartDate, EndDate = fullyOccupiedEndDate, IsActive = true, CustomerId = 1, RoomId = 1 };
        }

        public IEnumerable<Booking> GetAll()
        {
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=DateTime.Today.AddDays(1), EndDate=DateTime.Today.AddDays(1), IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=1, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=2, RoomId=2 },
            };
            return bookings;
        }

        // This field is exposed so that a unit test can validate that the
        // Remove method was invoked.
        public bool removeWasCalled = false;

        public void Remove(int id)
        {
            removeWasCalled = true;
        }
        
        public List<DateTime> GetFullyOccupiedDates(DateTime startDate, DateTime endDate, int numberOfRooms)
        {
            List<DateTime> fullyOccupiedDates = new List<DateTime>();
            for (DateTime d = startDate; d <= endDate; d = d.AddDays(1))
            {
                var noOfBookings = from b in GetAll()
                    where b.IsActive && d >= b.StartDate && d <= b.EndDate
                    select b;
                if (noOfBookings.Count() >= numberOfRooms)
                    fullyOccupiedDates.Add(d);
            }

            return fullyOccupiedDates;
        }
    }
}
