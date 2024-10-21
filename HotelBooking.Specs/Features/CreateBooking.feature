Feature: Create Booking
To ensure guests can book available rooms for specific dates

    @mytag
    Scenario Outline: A hotel room can be booked for a period (start date – end date) in the future
        Given a hotel room is available for the period from "<room_available_start_date>" to "<room_available_end_date>"
        When a user attempts to book the room for the period from "<booking_start_date>" to "<booking_end_date>"
        Then the booking status "IsActive" should be "<is_active>"

        Examples:
          | room_available_start_date | room_available_end_date | booking_start_date | booking_end_date | is_active |
          | 2024-12-01                | 2024-12-07              | 2024-12-01         | 2024-12-07       | true      |
          | 2024-12-01                | 2024-12-07              | 2024-12-08         | 2024-12-10       | false     |
          | 2024-12-01                | 2024-12-07              | 2024-11-30         | 2024-12-02       | false     |
          | 2024-12-01                | 2024-12-07              | 2024-12-03         | 2024-12-06       | true      |
          | 2024-12-01                | 2024-12-07              | 2024-12-05         | 2024-12-09       | false     |