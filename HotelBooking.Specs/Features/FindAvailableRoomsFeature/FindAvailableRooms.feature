Feature: Find Available Rooms
To ensure guests can see what rooms are available for booking.

    @mytag
    Scenario Outline: Hotel rooms that are not booked should be listed as available rooms
        Given there are hotel rooms available for the period from "<available_start_date>" to "<available_end_date>"
        And some rooms are booked for the period from "<booking_start_date>" to "<booking_end_date>"
        When a user searches for available rooms for the period from "<search_start_date>" to "<search_end_date>"
        Then the available rooms should include "<expected_available_rooms_id>"

        Examples:
          | available_start_date | available_end_date | booking_start_date | booking_end_date | search_start_date | search_end_date | expected_available_rooms_id |
          | 2024-12-01           | 2024-12-10         | 2024-12-03         | 2024-12-05       | 2024-12-06        | 2024-12-10      | 1, 2                        |
          | 2024-12-01           | 2024-12-10         | 2024-12-03         | 2024-12-08       | 2024-12-02        | 2024-12-04      | 2                           |
          | 2024-12-01           | 2024-12-10         | 2024-12-01         | 2024-12-10       | 2024-12-01        | 2024-12-10      | -1        |