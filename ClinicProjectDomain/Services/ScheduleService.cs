using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Services
{
    public class ScheduleService
    {
        public IEnumerable<TimeOnly> GetAvailableSlots(
            DoctorSchedule schedule,
            IEnumerable<Appointment> appointments)
        {
            // 1. Generate slots safely (will throw if duration <= 0)
            var slots = schedule.GenerateSlots();

            if (!slots.Any())
            {
                return Enumerable.Empty<TimeOnly>();
            }

            // 2. Filter active appointments and convert times to TimeSpan for safe overlap checks
            var activeAppointments = appointments
                .Where(a => a.AppointmentDate == schedule.ScheduledDate
                         && a.Status != AppointmentStatus.Cancelled)
                .Select(a => new
                {
                    Start = a.StartTime.ToTimeSpan(),
                    End = a.StartTime.ToTimeSpan().Add(TimeSpan.FromMinutes(a.DurationMinutes))
                })
                .ToList();

            var slotDuration = TimeSpan.FromMinutes(schedule.SlotDurationMinutes);

            // 3. Keep slots that do NOT overlap with active appointments
            return slots.Where(slot =>
            {
                TimeSpan slotStart = slot.ToTimeSpan();
                TimeSpan slotEnd = slotStart + slotDuration;

                // Overlap formula: (StartA < EndB) AND (EndA > StartB)
                bool overlaps = activeAppointments.Any(a =>
                    slotStart < a.End && slotEnd > a.Start);

                return !overlaps;
            }).ToList();
        }
    }
}
