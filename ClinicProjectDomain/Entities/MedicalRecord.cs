using ClinicProjectDomain.Exceptions;
using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class MedicalRecords :BaseEntity, IAuditableEntity
    {

        private MedicalRecords(Guid appointmentId, string medicalRecordNumber, string diagnosis)
        {

            AppointmentId = appointmentId;
                Diagnosis = diagnosis;
                MedicalRecordNumber= medicalRecordNumber;
        }
        private Appointment _Appointment;
        public Appointment Appointment => _Appointment;

        public string MedicalRecordNumber { get;private set; }
        public Guid AppointmentId { get;private set; }
        
        public string Diagnosis { get;private set; } = default!;

        private Prescriptions _Prescription ;
        public Prescriptions Prescription => _Prescription;
        
        public static MedicalRecords Create(Guid appointmentId,string medicalRecordSeq ,string diagnosis)
        {
            
            return new MedicalRecords(appointmentId, medicalRecordSeq, diagnosis);
        }

        public void AddPrescription(Prescriptions prescription)
        {
            if (prescription is null)
                throw new ArgumentNullException(nameof(prescription));
            if (prescription.MedicalRecordId != Id)
                throw new InvalidOperationException("Prescription does not belong to this medical record.");

            _Prescription = prescription;
        }

        public void AddAppointment(Appointment appointment)
        {
            if (appointment is null)
                throw new DomainException("Appointment Cannot be null");
            if (appointment.Id != this.AppointmentId)
                throw new DomainException("The provided appointment does not match the assigned ID for this medical record.");
            _Appointment = appointment;
        }
    }
}
