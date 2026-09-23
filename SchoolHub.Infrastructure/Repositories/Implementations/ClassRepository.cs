using System.Data;
using Dapper;
using SchoolHub.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Infrastructure.Data;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Infrastructure.Repositories.Implementations
{
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;

        public ClassRepository(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }


        public async Task<Class> CreateAsync(Class @class)
        {
            _context.Add(@class);
            await _context.SaveChangesAsync();
            return @class;
        }

        public async Task<bool> DeleteByIdAsync(string classId)
        {
            Class? @class = await GetByIdAsync(classId);
            if (@class == null) return false;

            _context.Remove(@class);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Class newClass)
        {
            _context.Update(newClass);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Class>> GetAllClassAsync()
        {
            var sql = "SELECT * FROM Classes";
            var classes = await _dbConnection.QueryAsync<Class>(sql);
            return classes.ToList();
        }

        public async Task<Class?> GetByIdAsync(string classId)
        {
            const string sql = "SELECT * FROM Classes WHERE ClassId = @ClassId";
            return await _dbConnection.QueryFirstOrDefaultAsync<Class>(sql, new { ClassId = classId });
        }

        public async Task<int> CountStudentsInClassAsync(string classId)
        {
            const string sql = "SELECT COUNT(1) FROM Students WHERE ClassId = @ClassId";
            return await _dbConnection.ExecuteScalarAsync<int>(sql, new { ClassId = classId });
        }
    }
}
