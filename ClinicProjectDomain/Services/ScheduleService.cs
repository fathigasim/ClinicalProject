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
        //    public IEnumerable<TimeOnly> GetAvailableSlots(
        // WeeklySchedule schedule,
        // IEnumerable<Appointment> appointments)
        //    {
        //        var slots = schedule.GenerateSlots();

        //        return slots.Where(slot =>
        //            !appointments.Any(a =>
        //                schedule.IsOverlapping(
        //                    slot,
        //                    slot.AddMinutes(schedule.SlotDurationMinutes),
        //                    a.StartTime,
        //                    a.StartTime.AddMinutes(a.DurationMinutes) // ✅ FIX HERE
        //                )
        //            )
        //        );
        //    }
        public IEnumerable<TimeOnly> GetAvailableSlots(
       WeeklySchedule schedule,
       IEnumerable<Appointment> appointments)
        {
            var slots = schedule.GenerateSlots();

            // Only block slots for active/scheduled appointments
            var activeAppointments = appointments
                .Where(a => a.DayOfWeek == schedule.DayOfWeek
                         && a.status != AppointmentStatus.Cancelled)
                .ToList();

            return slots.Where(slot =>
            {
                var slotEnd = slot.AddMinutes(schedule.SlotDurationMinutes);

                return !activeAppointments.Any(a =>
                {
                    var apptEnd = a.StartTime.AddMinutes(a.DurationMinutes);
                    return schedule.IsOverlapping(slot, slotEnd, a.StartTime, apptEnd);
                });
            });
        }
    }
    }
