Feature: Create Booking
To ensure guests can book available rooms for specific dates

    @mytag
    Scenario Outline: A hotel room can be booked for a period (start date – end date) in the future
        Given a hotel room is available for the period from "<room_available_start_date>" to "<room_available_end_date>"
        When a user attempts to book the room for the period from "<booking_start_date>" to "<booking_end_date>"
        Then the booking status "BookingCreated" should be "<booking_created>"

        Examples:
          | room_available_start_date | room_available_end_date | booking_start_date | booking_end_date | booking_created |
          | 2024-12-01                | 2024-12-14              | 2024-12-10         | 2024-12-14       | true            |
          | 2024-12-01                | 2024-12-07              | 2024-12-03         | 2024-12-06       | true            |
          | 2025-12-01                | 2025-12-07              | 2025-12-03         | 2025-12-06       | true            |