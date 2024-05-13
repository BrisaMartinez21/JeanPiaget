using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.DTOs.Alumnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Alumnos
{
    public interface ITutorAppService
    {
        Task<List<Tutor>> GetTutorsAsync();

        Task<int> AddTutorAsync(Tutor tutor);

        Task DeleteTutorAsync(int tutorId);
        Task<Tutor> GetTutorAsync(int tutorId);

        Task EditTutorAsync(Tutor tutor);
    }
}
