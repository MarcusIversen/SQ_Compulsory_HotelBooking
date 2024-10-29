using HotelBooking.Core;
using Moq;
using Xunit;

namespace HotelBooking.Specs.Steps;

[Binding]
public class CreateBookingThrowsExceptionStep
{
    private IBookingManager bookingManager;
    private Mock<IRepository<Booking>> mockBookingRepository;
    private Mock<IRepository<Room>> mockRoomRepository;
    
    private ArgumentException _exceptionObject;
    
    public CreateBookingThrowsExceptionStep()
    {
        mockBookingRepository = new();
        mockRoomRepository = new();
    }
    
    [Given(@"the occupied period from day ""(.*)"" to day ""(.*)""")]
    public void GivenAHotelRoomIsAvailableForThePeriodFromTo(string roomAvailableStartDate, string roomAvailableEndDate)
    {
        // Arrange the booking with available room period
        var availableStartDate = DateTime.Parse(roomAvailableStartDate);
        var availableEndDate = DateTime.Parse(roomAvailableEndDate);

        var rooms = new List<Room>() { new Room { Id = 1, Description = "Hallo world" } };

        var bookings = new List<Booking>() { new Booking { Id = 1, StartDate = availableStartDate, EndDate = availableEndDate, IsActive = true, RoomId = 1 } };
        
        mockRoomRepository.Setup(x => x.GetAll()).Returns(rooms);
        mockBookingRepository.Setup(x => x.GetAll()).Returns(bookings);
        
        bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
    }

    [When(@"a user attempts to book a room for the period from ""(.*)"" to ""(.*)""")]
    public void WhenAUserAttemptsToBookTheRoomForThePeriodFromTo(string bookingStartDate, string bookingEndDate)
    {
        // Act: try to create a booking for the specified period
        var bookingAttempt = new Booking
        {
            RoomId = 2,
            StartDate = DateTime.Parse(bookingStartDate),
            EndDate = DateTime.Parse(bookingEndDate),
        };
        
        _exceptionObject = Assert.Throws<ArgumentException>(() => bookingManager.CreateBooking(bookingAttempt));

    }

    [Then(@"error message should be ""(.*)""")]
    public void ThenAnErrorMessageAppears(string expectedErrorMessage)
    {
        // Assert
        Assert.Equal(expectedErrorMessage, _exceptionObject.Message);
    }
}