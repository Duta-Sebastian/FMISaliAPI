using System;
using FMISaliAPI.Data;
using FMISaliAPI.Models;
using Microsoft.EntityFrameworkCore;

public class RoomScheduleSeed
{
    private readonly ApplicationDbContext _dbContext;

    public RoomScheduleSeed(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedSchedules()
    {
        var roomId = 3; // Room ID for which schedules are being created
        var today = DateTime.Today.ToUniversalTime(); // Convert to UTC

        // Fetch the room with Id = 1
        var room = _dbContext.Rooms.FirstOrDefault(r => r.Id == roomId);

        if (room == null)
        {
            throw new InvalidOperationException("Room with Id = 1 not found.");
        }

        // Calculate the next Monday from today
        var nextMonday = GetNextMonday(today);

        // Calculate the Sunday 6 months from the next Monday
        var endDate = nextMonday.AddMonths(6).AddDays(-1); // Subtract 1 day to get Sunday

        // Recurring Weekly Schedule (Every Monday)
        var recurringSchedule = new Schedule
        {
            RoomId = roomId,
            Room = room, // Assign the Room navigation property
            Start = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)), // 08:00
            End = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)), // 10:00
            Recurrence = RecurrenceType.Weekly,
            RecurrenceStartDate = nextMonday,
            RecurrenceEndDate = endDate,
            Type = ScheduleType.Orar,
            Description = "Weekly Recurring Event"
        };

        // Odd Week Schedule (Every Tuesday on odd weeks)
        var oddWeekSchedule = new Schedule
        {
            RoomId = roomId,
            Room = room, // Assign the Room navigation property
            Start = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)), // 09:00
            End = TimeOnly.FromTimeSpan(TimeSpan.FromHours(11)), // 11:00
            Recurrence = RecurrenceType.OddWeeks,
            RecurrenceStartDate = nextMonday,
            RecurrenceEndDate = endDate,
            Type = ScheduleType.Orar,
            Description = "Odd Week Event"
        };

        // Even Week Schedule (Every Wednesday on even weeks)
        var evenWeekSchedule = new Schedule
        {
            RoomId = roomId,
            Room = room, // Assign the Room navigation property
            Start = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)), // 10:00
            End = TimeOnly.FromTimeSpan(TimeSpan.FromHours(12)), // 12:00
            Recurrence = RecurrenceType.EvenWeeks,
            RecurrenceStartDate = nextMonday,
            RecurrenceEndDate = endDate,
            Type = ScheduleType.Orar,
            Description = "Even Week Event"
        };

        // Add schedules to the database
        _dbContext.Schedules.AddRange(recurringSchedule, oddWeekSchedule, evenWeekSchedule);
        _dbContext.SaveChanges();
    }

    private static DateTime GetNextMonday(DateTime date)
    {
        // Calculate the next Monday from the given date
        while (date.DayOfWeek != DayOfWeek.Monday)
        {
            date = date.AddDays(1);
        }
        return date;
    }
}