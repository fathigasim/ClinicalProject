using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.DoctorsCommandQueries.Dto;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public class GetDoctorScheduleByIdQueryHandler(IDoctorScheduleRepository repository,IMapper mapper) : IRequestHandler<GetDoctorScheduleByIdQuery, Result<DoctorScheduleDto>>
    {

        public async Task<Result<DoctorScheduleDto>> Handle(
     GetDoctorScheduleByIdQuery request,
     CancellationToken cancellationToken)
        {
            // 1. Guard against Guid.Empty
            if (request.id == Guid.Empty)
            {
                return Result<DoctorScheduleDto>.Failure("Provided ID is empty.");
            }

            // 2. Fetch from repository
            var doctorSchedule = await repository.DoctorsScheduleById(request.id, cancellationToken);

            // 3. Handle null result cleanly without throwing unnecessary unhandled exceptions
            if (doctorSchedule is null)
            {
                return Result<DoctorScheduleDto>.Failure($"Doctor schedule with ID {request.id} was not found.");
            }

            // 4. Map & Return
            var dto = mapper.Map<DoctorScheduleDto>(doctorSchedule);
            return Result<DoctorScheduleDto>.Success(dto);
        }
    }
}
