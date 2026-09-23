using System.Data;
using Dapper;
using SchoolHub.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Infrastructure.Data;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Infrastructure.Repositories.Implementations
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;

        public SubjectRepository(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public async Task<Subject> CreateAsync(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return subject;
        }

        public async Task<bool> DeleteAsync(string subjectId)
        {
            int rowsDeleted = await _context.Subjects.Where(s => s.SubjectId == subjectId).ExecuteDeleteAsync();
            return rowsDeleted > 0;
        }

        public async Task<bool> UpdateAsync(Subject subject)
        {
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            var sql = "SELECT * FROM Subjects";
            var subjects = await _dbConnection.QueryAsync<Subject>(sql);
            return subjects.ToList();
        }

        public async Task<Subject?> GetByIdAsync(string subjectId)
        {
            var sql = "SELECT * FROM Subjects WHERE SubjectId = @SubjectId";
            return await _dbConnection.QueryFirstOrDefaultAsync<Subject>(sql, new { SubjectId = subjectId });
        }
    }
}
