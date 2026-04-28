
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
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IAppointmentRepository _repository;
        private readonly ISequenceService _sequenceSerivce;
        public CreateAppointmentCommandHandler(IAppointmentRepository repository, ISequenceService sequenceSerivce)
        {
            _repository = repository;
            _sequenceSerivce = sequenceSerivce;
        }
        public async Task<Guid> Handle (CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var sequence =await _sequenceSerivce.GenerateOrderNumberAsync();
            var appointment = new Appointment
            {
                AppointmentNumber = sequence,
                PatiendId = request.PatiendId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                Notes = request.Notes,
            };
            appointment.Schedule(request.AppointmentDate);
          await  _repository.AddAsync(appointment);
            return appointment.Id;
        }
    }
}
