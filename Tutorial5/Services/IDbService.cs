using Tutorial5.DTOs;

namespace Tutorial5.Services;

public interface IDbService
{
    Task AddPrescriptionAsync(CreatePrescriptionDto dto);
    Task<GetPatientDto> GetPatientAsync(int idPatient);
}