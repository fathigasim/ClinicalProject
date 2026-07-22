
namespace DefaultAuthenticationApplication.PatientsCommandQueries.Dto;
public record DoctorDto
{
    public Guid DoctorId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Specialization { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
}