using AutoMapper;
using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Queries.GetNotInvoicedAppointments
{
    public class GetNotInvoicedAppointmentsQueryHandler(IMapper mapper,IAppointmentRepository appointmentRepository) : IRequestHandler<GetNotInvoicedAppointmentsQuery,List<NotInvoicedAppointmentDto>>
    {
        
     
        public async Task<List<NotInvoicedAppointmentDto>> Handle(GetNotInvoicedAppointmentsQuery request, CancellationToken cancellationToken)
        {
         var notInvoicedAppointments=await   appointmentRepository.GetListOfNotInvoicedAppointmentsAsync(cancellationToken);
            if(notInvoicedAppointments == null)
            {
                return new List<NotInvoicedAppointmentDto>();
            }

            return mapper.Map<List<NotInvoicedAppointmentDto>>(notInvoicedAppointments);
        }
    }
}
