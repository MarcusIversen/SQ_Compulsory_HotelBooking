Feature: Create Booking
As a user I want to create a booking

    #Test case 1 
    Scenario Outline: Create valid booking within available dates
        Given the fully occupied period from day "<occupied_start_date>" to day "<occupied_end_date>"
        When a user attempts to book the room for the period from "<booking_start_date>" to "<booking_end_date>"
        Then the booking status should be "<booking_created>"

        Examples:
          | occupied_start_date | occupied_end_date | booking_start_date | booking_end_date | booking_created |
          | 2024-12-01          | 2024-12-14        | 2025-12-10         | 2025-12-14       | true            |
          
    #Test case 2 
    Scenario Outline: Create invalid booking on occupied date
        Given the fully occupied period from day "<occupied_start_date>" to day "<occupied_end_date>"
        When a user attempts to book the room for the period from "<booking_start_date>" to "<booking_end_date>"
        Then the booking status should be "<booking_created>"

        Examples:
          | occupied_start_date | occupied_end_date | booking_start_date | booking_end_date | booking_created |
          | 2024-12-10          | 2024-12-14        | 2024-12-10         | 2024-12-14       | false           |
          
    #Test case 3 
    Scenario Outline: Create invalid booking with start date before occupied start date and end date in occupied period
        Given the fully occupied period from day "<occupied_start_date>" to day "<occupied_end_date>"
        When a user attempts to book the room for the period from "<booking_start_date>" to "<booking_end_date>"
        Then the booking status should be "<booking_created>"

        Examples:
          | occupied_start_date | occupied_end_date | booking_start_date | booking_end_date | booking_created |
          | 2024-12-10          | 2024-12-14        | 2024-12-8          | 2024-12-13       | false           |
          
    #Test case 4 
    Scenario Outline: Create invalid booking with start date in occupied period and end date outside occupied end period
        Given the fully occupied period from day "<occupied_start_date>" to day "<occupied_end_date>"
        When a user attempts to book the room for the period from "<booking_start_date>" to "<booking_end_date>"
        Then the booking status should be "<booking_created>"

        Examples:
          | occupied_start_date | occupied_end_date | booking_start_date | booking_end_date | booking_created |
          | 2024-12-10          | 2024-12-14        | 2024-12-11         | 2024-12-20       | false           |    
          