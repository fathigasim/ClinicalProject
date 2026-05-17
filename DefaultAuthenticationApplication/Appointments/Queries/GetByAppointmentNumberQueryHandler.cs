using AutoMapper;
using AutoMapper.Configuration;
using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectApplication.Common;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Queries
{
    public class GetByAppointmentNumberQueryHandler : IRequestHandler<GetByAppointmentNumberQuery,Result<List<AppointmentDto>>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        public GetByAppointmentNumberQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
              _appointmentRepository = appointmentRepository;
            _mapper= mapper;    
        }
        public async Task<Result<List<AppointmentDto>>> Handle(GetByAppointmentNumberQuery request, CancellationToken cancellationToken)
        {

         var Appointments=   await _appointmentRepository.GetListOfNotInvoicedAppointmentsAsync(cancellationToken);
            if (Appointments == null) {
                return Result<List<AppointmentDto>>.Failure("Appointment not exist");
            }
            var appointmentsDto= _mapper.Map<List<AppointmentDto>>(Appointments);
            return Result<List<AppointmentDto>>.Success(appointmentsDto);
        }
    }
}
