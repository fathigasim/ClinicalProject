using ClinicProjectDomain.Exceptions;
using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Patient :BaseEntity, IAuditableEntity
    {
        private Patient(string firstName,string lastName,string email,DateTime dOB,string phone,string gender)
        {
            FirstName= firstName;
            LastName= lastName;
            Email= email;
            DOB = dOB;
            Phone= phone;
            Gender= gender;
        }
   

        public string FirstName { get;private set; }
        public string LastName { get; private set; }
        public string Email { get;private set; }
        public DateTime DOB { get;private set; }
        public string Phone { get;private set; }
        public string Gender { get;private set; }

        private List<Appointment> _Appointments;
        public IReadOnlyCollection<Appointment> Appointments => _Appointments;

        public static Patient Create(string firstName, string lastName, string email, DateTime dob, string phone, string gender)
        {
            var today = DateTime.UtcNow.Date;

            if (dob.Date > today)
            {
                throw new DomainException("Date of birth cannot be in the future.");
            }

            if (dob.Date > today.AddYears(-18))
            {
                throw new DomainException("Adult members only allowed.");
            }
            if (string.IsNullOrEmpty(firstName)) {
                throw new DomainException(" firstName  cannot be null");
            }
            if (string.IsNullOrEmpty(lastName))
            {
                throw new DomainException(" lastName  cannot be null");
            }
            if (string.IsNullOrEmpty(email))
            {
                throw new DomainException(" email  cannot be null");
            }

         

            return new Patient(firstName,  lastName,  email,  dob,  phone,  gender);
        }

        public void Update(string firstName,string lastName, string email,DateTime dob,string phone,string gender)
        {
            if (dob.Date > DateTime.UtcNow.Date || (DateTime.UtcNow.Date.Year- dob.Date.Year) < 18)
            {
                throw new DomainException(" please enter a vaid date");
            }
            FirstName =firstName;
            LastName=lastName;
            Email=email;
            DOB= dob;
            Phone=phone;
            Gender=gender;
        }

    }
}
