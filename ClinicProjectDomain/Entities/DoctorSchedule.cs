using ClinicProjectDomain.Exceptions;
using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;

namespace ClinicProjectDomain.Entities
{
    public class DoctorSchedule : BaseEntity, IAuditableEntity
    {
        // Required for EF Core materialization
        public DoctorSchedule() { }
        private DoctorSchedule(
           Guid doctorId,
           TimeOnly startTime,
           TimeOnly endTime,
           DateOnly scheduledDate)
        {
            //DoctorId = doctorId;
            StartTime = startTime;
            EndTime = endTime;
            ScheduledDate = scheduledDate;
        }
        private DoctorSchedule(
            Guid doctorId,
            TimeOnly startTime,
            TimeOnly endTime,
            DateOnly scheduledDate,
            int slotDurationMinutes = 30)
        {
            DoctorId = doctorId;
            StartTime = startTime;
            EndTime = endTime;
            ScheduledDate = scheduledDate;
            DayOfWeek = scheduledDate.DayOfWeek;
            IsActive = true;
            SlotDurationMinutes = slotDurationMinutes > 0 ? slotDurationMinutes : 30;
        }

        public Guid DoctorId { get; private set; }

        private Doctor _Doctor = default!;
        public Doctor Doctor => _Doctor;

        public DayOfWeek DayOfWeek { get; private set; }
        public TimeOnly StartTime { get; private set; }
        public TimeOnly EndTime { get; private set; }
        public int SlotDurationMinutes { get; private set; } = 30;
        public bool IsActive { get; private set; } = true;
        public DateOnly ScheduledDate { get; private set; }

        public static DoctorSchedule Create(
            Guid doctorId,
            TimeOnly startTime,
            TimeOnly endTime,
            DateOnly scheduledDate,
            int slotDurationMinutes = 30)
        {
            if (doctorId == Guid.Empty)
            {
                throw new DomainException("Please select a doctor first.");
            }

            if (startTime >= endTime)
            {
                throw new DomainException("Start time must be before end time.");
            }

            return new DoctorSchedule(doctorId, startTime, endTime, scheduledDate, slotDurationMinutes);
        }

        public void Update(Guid doctorId,
           TimeOnly startTime,
           TimeOnly endTime,
           DateOnly scheduledDate)
        {
            if(doctorId != DoctorId)
            {
                throw new DomainException("Doctor Id entered must match entity doctor id");
            }
            DoctorId = doctorId;
            StartTime = startTime;
            EndTime = endTime;
            ScheduledDate = scheduledDate;

        }
        public bool IsClinicOpen(Clinic clinic)
        {
            return StartTime >= clinic.OpenTime && EndTime <= clinic.CloseTime;
        }

        public static bool IsHoliday(DayOfWeek day)
        {
            return day == DayOfWeek.Friday;
        }

        public bool IsValid()
        {
            return StartTime < EndTime &&
                   SlotDurationMinutes > 0 &&
                   (EndTime - StartTime).TotalMinutes % SlotDurationMinutes == 0;
        }

        public bool BeValidDate()
        {
            return ScheduledDate >= DateOnly.FromDateTime(DateTime.UtcNow);
        }

        public static bool IsTimeSlotValid(DateOnly scheduleDate, TimeOnly startTime)
        {
            var nowUtc = DateTime.UtcNow;
            var today = DateOnly.FromDateTime(nowUtc);

            if (scheduleDate < today)
            {
                return false;
            }

            if (scheduleDate == today)
            {
                return startTime > TimeOnly.FromDateTime(nowUtc);
            }

            return true;
        }

        public List<TimeOnly> GenerateSlots()
        {
            int durationMinutes = SlotDurationMinutes > 0 ? SlotDurationMinutes : 30;

            if (StartTime >= EndTime)
            {
                return new List<TimeOnly>();
            }

            var slots = new List<TimeOnly>();
            TimeSpan current = StartTime.ToTimeSpan();
            TimeSpan end = EndTime.ToTimeSpan();
            TimeSpan duration = TimeSpan.FromMinutes(durationMinutes);

            while (current + duration <= end)
            {
                slots.Add(TimeOnly.FromTimeSpan(current));
                current += duration;
            }

            return slots;
        }

        public bool IsTimeSlotValid(TimeOnly time)
        {
            return time >= StartTime &&
                   time.AddMinutes(SlotDurationMinutes) <= EndTime;
        }

        public bool IsOverlapping(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
        {
            return start1 < end2 && start2 < end1;
        }
    }
}