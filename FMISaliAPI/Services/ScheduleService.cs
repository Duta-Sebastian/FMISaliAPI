using FMISaliAPI.DTO;
using FMISaliAPI.Models;

namespace FMISaliAPI.Services
{
    public static class ScheduleService
    {
        public static List<CalendarEvent> GenerateCalendarEvents(Schedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule), "Schedule should not be null!");
            }
            var events = new List<CalendarEvent>();

            if (schedule.Recurrence == RecurrenceType.OneTime)
            {
                events.Add(new CalendarEvent
                {
                    Title = schedule.Description,
                    Start = schedule.RecurrenceStartDate?.Date + schedule.Start.ToTimeSpan(),
                    End = schedule.RecurrenceStartDate?.Date + schedule.End.ToTimeSpan()
                });
                return events;
            }

            var currentDate = schedule.RecurrenceStartDate ??
              throw new ArgumentNullException(nameof(schedule.RecurrenceStartDate),
                  "RecurrenceStartDate should not be null!");

            var endDate = schedule.RecurrenceEndDate ??
                throw new ArgumentNullException(nameof(schedule.RecurrenceEndDate),
                    "RecurrenceEndDate should not be null!");

            var startOfYear = new DateTime(currentDate.Year, 1, 1);
            var startWeekNumber = (currentDate - startOfYear).Days / 7 + 1;

            

            while (currentDate <= endDate)
            {
                var currentWeekNumber = (currentDate - startOfYear).Days / 7 + 1;
                var relativeWeekNumber = currentWeekNumber - startWeekNumber + 1;

                var shouldAddEvent = schedule.Recurrence switch
                {
                    RecurrenceType.Weekly => true,
                    RecurrenceType.OddWeeks => relativeWeekNumber % 2 == 1,
                    RecurrenceType.EvenWeeks => relativeWeekNumber % 2 == 0,
                    RecurrenceType.OneTime => false,
                    _ => throw new ArgumentException(schedule.Recurrence + "not expected")
                };

                if (shouldAddEvent)
                {
                    events.Add(new CalendarEvent
                    {
                        Title = schedule.Description,
                        Start = currentDate.Date + schedule.Start.ToTimeSpan(),
                        End = currentDate.Date + schedule.End.ToTimeSpan()
                    });
                }

                currentDate = currentDate.AddDays(7);
            }

            return events;
        }

    }
}