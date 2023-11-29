using System;
using System.Collections.Generic;
using System.Linq;

class Bus
{
    private readonly int[,] seats;
    private readonly string[,] ticketTime;
    private const int Rows = 10;
    private const int Columns = 4;
    private const double AcBusFareMultiplier = 1.3;

    public Bus()
    {
        seats = new int[Rows, Columns];
        ticketTime = new string[Rows, Columns];
    }

    public void DisplaySeats()
    {
        Console.WriteLine("\nAvailable Seats: ");
        Console.Write("   ");
        for (int j = 0; j < Columns; j++)
        {
            Console.Write($"{(char)('A' + j)} ");
        }
        Console.WriteLine();

        for (int i = 0; i < Rows; i++)
        {
            Console.Write($"{i + 1:D2} ");
            for (int j = 0; j < Columns; j++)
            {
                Console.Write(seats[i, j] == 0 ? "O " : "X ");
            }
            Console.WriteLine();
        }
    }

    public bool BookSeats(string seatList, int routeNumber, bool isAC)
    {
        var seatsToBook = seatList
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(seat => seat.ToUpper().Trim());

        foreach (string seat in seatsToBook)
        {
            if (ValidateSeatSelection(seat) && TryGetSeatIndices(seat, out int row, out int column))
            {
                int fare = CalculateSeatFare(routeNumber, isAC);

                if (seats[row, column] == 0)
                {
                    seats[row, column] = 1;
                    ticketTime[row, column] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Console.WriteLine($"Seat {seat} booked successfully at {ticketTime[row, column]}.");
                    Console.WriteLine($"Total Fare: {fare} Taka");
                }
                else
                {
                    Console.WriteLine($"Seat {seat} is already booked. Please try again.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"Invalid seat selection: {seat}. Please try again.");
                return false;
            }
        }

        return true;
    }

    private bool ValidateSeatSelection(string seat)
    {
        return seat.Length == 2 && char.IsDigit(seat[0]) && char.IsLetter(seat[1]) && char.ToUpper(seat[1]) <= 'D';
    }

    private bool TryGetSeatIndices(string seat, out int row, out int column)
    {
        row = int.Parse(seat[0].ToString()) - 1;
        column = char.ToUpper(seat[1]) - 'A';
        return row >= 0 && row < Rows && column >= 0 && column < Columns;
    }

    private int CalculateSeatFare(int routeNumber, bool isAC)
    {
        int baseFare = GetBaseFare(routeNumber);
        return isAC ? (int)(baseFare * AcBusFareMultiplier) : baseFare;
    }

    private static int GetBaseFare(int routeNumber)
    {
        return routeNumber switch
        {
            1 => 1200,
            2 => 800,
            3 => 900,
            _ => 0,
        };
    }

    public int CalculateRevenue()
    {
        return seats.Cast<int>().Count(booked => booked == 1) * GetBaseFare(1); // Considering Route 1 for revenue calculation
    }
}

class BusTransportationSystem
{
    static void Main()
    {
        var buses = Enumerable.Range(1, 6).Select(_ => new Bus()).ToArray();

        while (true)
        {
            Console.WriteLine("\n======= Bus Transportation Management System =======\n");
            Console.WriteLine("1. Display Available Seats");
            Console.WriteLine("2. Book Seat(s)");
            Console.WriteLine("3. Calculate Revenue");
            Console.WriteLine("4. Exit\n");
            Console.Write("\n\nEnter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Write("Enter Bus Number (1 to 6): ");
                    if (int.TryParse(Console.ReadLine(), out int busNumber) && busNumber >= 1 && busNumber <= 6)
                    {
                        Console.Clear();
                        Console.WriteLine($"======= Bus {busNumber} - Available Seats =======");
                        buses[busNumber - 1].DisplaySeats();
                    }
                    else
                    {
                        Console.WriteLine("Invalid bus number. Please try again.");
                    }
                    break;

                case 2:
                    Console.Write("Enter Bus Number (1 to 6): ");
                    if (int.TryParse(Console.ReadLine(), out int selectedBusNumber) && selectedBusNumber >= 1 && selectedBusNumber <= 6)
                    {
                        Console.Write("Enter seat(s) (e.g., 1a,2b,3c): ");
                        string seatList = Console.ReadLine();

                        Console.WriteLine("Available Routes:");
                        Console.WriteLine("1. DHAKA-COX");
                        Console.WriteLine("2. DHAKA-KHULNA");
                        Console.WriteLine("3. DHAKA-RANGPUR");
                        Console.Write("Enter Route Number: ");
                        if (int.TryParse(Console.ReadLine(), out int selectedRouteNumber) && selectedRouteNumber >= 1 && selectedRouteNumber <= 3)
                        {
                            Console.Write("Is it an AC bus? (yes/no): ");
                            bool isAC = Console.ReadLine().ToLower() == "yes";

                            Console.Clear();
                            Console.WriteLine($"======= Bus {selectedBusNumber} - Booking Confirmation =======");
                            buses[selectedBusNumber - 1].BookSeats(seatList, selectedRouteNumber, isAC);
                        }
                        else
                        {
                            Console.WriteLine("Invalid route number. Please try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid bus number. Please try again.");
                    }
                    break;

                case 3:
                    int totalRevenue = buses.Sum(bus => bus.CalculateRevenue());
                    Console.Clear();
                    Console.WriteLine($"======= Total Revenue =======\nTotal Revenue: {totalRevenue} Taka");
                    break;

                case 4:
                    Console.WriteLine("Exiting the Bus Transportation Management System. Goodbye!");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
