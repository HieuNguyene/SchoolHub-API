using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Interfaces
{
    public interface IClassRepository
    {
        Task<Class> CreateAsync(Class Class);
        Task<List<Class>> GetAllClassAsync();
        Task<Class?> GetByIdAsync(String ClassId);
        Task<bool> UpdateAsync(Class newClass);
        Task<bool> DeleteByIdAsync(String ClassId);
        Task<int> CountStudentsInClassAsync(string classId);
    }
}












