using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.MedicalRecord.Dtos;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.MedicalRecord.Queries.GetAllMedicalRecords
{
    public class GetAllMedicalRecordsQueryHandler : IRequestHandler<GetAllMedicalRecordsQuery, Result<PagedResult<MedicalRecordDto>>>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;

        public GetAllMedicalRecordsQueryHandler(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _medicalRecordRepository=medicalRecordRepository;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<MedicalRecordDto>>> Handle(GetAllMedicalRecordsQuery request, CancellationToken cancellationToken)
        {

            var medicalRecords = await _medicalRecordRepository.GetAllPatientsMedicalReocrd(request.page, request.pageSize, cancellationToken);
            if (medicalRecords == null)
            {
                return Result<PagedResult<MedicalRecordDto>>.Failure("No medical records found");
            }

            return Result<PagedResult<MedicalRecordDto>>.Success(
                             new PagedResult<MedicalRecordDto>
                             {
                                 Items = _mapper.Map<List<MedicalRecordDto>>(medicalRecords.Items),
                                 Page = request.page,
                                 PageSize = request.pageSize,
                                 TotalCount = medicalRecords.TotalCount,
                             }
                );

        }
    }
}
