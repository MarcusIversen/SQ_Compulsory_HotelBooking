Feature: Create invalid Booking
As a user I want an error message
    
    #Test case 1
    Scenario Outline: Create invalid booking where start date is after end date
        Given the occupied period from day "<occupied_start_date>" to day "<occupied_end_date>"
        When a user attempts to book a room for the period from "<booking_start_date>" to "<booking_end_date>"
        Then error message should be "<error_message>"

        Examples:
          | occupied_start_date | occupied_end_date | booking_start_date | booking_end_date | error_message                                                    |
          | 2024-12-01          | 2024-12-14        | 2025-12-10         | 2024-12-14       | The start date cannot be in the past or later than the end date. |               
          
    #Test case 2
    Scenario Outline: Create invalid booking where start date before today
        Given the occupied period from day "<occupied_start_date>" to day "<occupied_end_date>"
        When a user attempts to book a room for the period from "<booking_start_date>" to "<booking_end_date>"
        Then error message should be "<error_message>"

        Examples:
          | occupied_start_date | occupied_end_date | booking_start_date | booking_end_date | error_message                                                    |
          | 2024-12-01          | 2024-12-14        | 2023-12-10         | 2024-12-14       | The start date cannot be in the past or later than the end date. |