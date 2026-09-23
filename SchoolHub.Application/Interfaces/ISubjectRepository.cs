using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Interfaces
{
    public interface ISubjectRepository
    {
        Task<Subject> CreateAsync(Subject subject);
        Task<List<Subject>> GetAllAsync();
        Task<bool> DeleteAsync(string subjecId);
        Task<bool> UpdateAsync(Subject subject);
        Task<Subject?> GetByIdAsync(string subjecId);
    }
}












