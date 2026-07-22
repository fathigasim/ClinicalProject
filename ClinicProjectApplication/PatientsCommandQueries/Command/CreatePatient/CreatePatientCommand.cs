using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;


namespace ClinicProjectApplication.PatientsCommandQueries.Command.CreatePatient
{
    public record CreatePatientCommand
        (string FirstName, string LastName, DateTime DOB,string Gender ,string Phone, string Email, DateTime CreatedAt)
        :IRequest<Result<string>>,ITransactionalRequest;




}
