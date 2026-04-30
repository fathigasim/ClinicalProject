
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<string>>
    {
        private readonly IAppointmentRepository _repository;
        private readonly ISequenceService _sequenceSerivce;
        public CreateAppointmentCommandHandler(IAppointmentRepository repository, ISequenceService sequenceSerivce)
        {
            _repository = repository;
            _sequenceSerivce = sequenceSerivce;
        }
        public async Task<Result<string>> Handle (CreateAppointmentCommand request, CancellationToken cancellationToken)
        {

        //var IsDoctorBusy=   await _repository.IsDoctorAppointmentsBusy(request.DoctorId, request.AppointmentDate);
              
        //    if (IsDoctorBusy)
        //    {
                
        //            return Result<string>.Failure("Doctor is busy at the selected time.");
        //    }
       var isOccupiedAppointment=    await _repository.IsSlotOccupied(request.DoctorId, request.AppointmentDate,15);
            if (isOccupiedAppointment)
            {

                return Result<string>.Failure("Appointment slot is already occupied.");
            }
            var sequence =await _sequenceSerivce.GenerateOrderNumberAsync();
            var appointment = new Appointment
            {
                AppointmentNumber = sequence,
                PatientId = request.PatiendId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                Notes = request.Notes,
            };
            appointment.Schedule(request.AppointmentDate);
          await  _repository.AddAsync(appointment);
            return Result<string>.Success( $"Appointment Confirmed for {appointment.AppointmentNumber}");
        }
    }
}
