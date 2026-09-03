using AutoMapper;
using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectApplication.Common;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Queries.GetTodayAppointments
{
    public class GetTodaysAppointmentsQueryHandler : IRequestHandler<GetTodaysAppointmentsQuery, Result<PagedResult<AppointmentDto>>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
      //  private readonly IMapper _mapper;
        public GetTodaysAppointmentsQueryHandler(IAppointmentRepository appointmentRepository
            //, IMapper mapper
            )
        {
            _appointmentRepository = appointmentRepository;
          //  _mapper = mapper;
        }
        public async Task<Result<PagedResult<AppointmentDto>>> Handle(GetTodaysAppointmentsQuery request, CancellationToken cancellationToken)
        {
           var result=  await  _appointmentRepository.GetTodaysAppointmentsAsync(request.page,request.pageSize,cancellationToken);

            if (result.Items.Any())
            {
                return Result<PagedResult<AppointmentDto>>.Success(new PagedResult<AppointmentDto>
                {
                    Items= result.Items.Select(p=>new AppointmentDto() {
                        AppointmentNumber=p.AppointmentNumber,
                        //DayOfWeek=p.DayOfWeek,
                        DoctorId=p.DoctorId,
                        Notes=p.Notes,
                        DurationMinutes=p.DurationMinutes,
                        PatientId=p.PatientId,
                        StartTime=p.StartTime,
                        status=p.Status
                    
                    }).ToList(),//_mapper.Map<List<AppointmentDto>>(result.Items),
                    Page=request.page,
                    PageSize=request.pageSize,
                    TotalCount=result.TotalCount,
                });
            }
            return Result<PagedResult<AppointmentDto>>.Failure("No appointments found today");


        }
    }
}
