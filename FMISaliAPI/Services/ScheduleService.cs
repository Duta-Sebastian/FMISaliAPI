using FMISaliAPI.DTO;
using FMISaliAPI.Models;

namespace FMISaliAPI.Services
{
    public static class ScheduleService
    {
        public static List<CalendarEvent> GenerateCalendarEvents(Schedule schedule)
        {
            var events = new List<CalendarEvent>();

            var currentDate = schedule.RecurrenceStartDate ??
                              throw new ArgumentNullException(nameof(schedule.RecurrenceStartDate));

            var endDate = schedule.RecurrenceEndDate ??
                          throw new ArgumentNullException(nameof(schedule.RecurrenceEndDate));

            var startOfYear = new DateTime(currentDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
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
                    _ => false
                };

                if (shouldAddEvent)
                {
                    events.Add(new CalendarEvent
                    {
                        RoomId = schedule.RoomId,
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