using SchoolHub.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Infrastructure.Data;
using SchoolHub.Domain.Entities;
using System.Data;
using Dapper;

namespace SchoolHub.Infrastructure.Repositories.Implementations
{
    public class ScoreRepository : IScoreRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbconnection;
        public ScoreRepository(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbconnection = dbConnection;
        }

        public async Task<Score> CreateAsync(Score score)
        {
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
            return score;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {

            // Xóa trực tiếp dưới Database mà không cần tốn công lấy dữ liệu lên bộ nhớ trước
            int rowsDeleted = await _context.Scores.Where(sc => sc.Id == id).ExecuteDeleteAsync();
            return rowsDeleted > 0;
        }

        public async Task<Score?> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM Scores WHERE Id = @id";
            return await _dbconnection.QueryFirstOrDefaultAsync<Score>(sql, new { Id = id });
        }

        public async Task<List<Score>> GetScoreBySubjectAsync(string subjectId)
        {

            var sql = "SELECT * FROM Scores WHERE SubjectId = @subjectId";
            var scores = await _dbconnection.QueryAsync<Score>(sql, new { SubjectId = subjectId });
            return scores.ToList();
        }

        public async Task<List<Score>> GetScoresByStudentAsync(Guid studentId)
        {
            var sql = "SELECT * FROM Scores WHERE StudentId = @studentId";
            var scores = await _dbconnection.QueryAsync<Score>(sql, new { StudentId = studentId });
            return scores.ToList();
        }

        public async Task<Score?> GetSpecificScoreAsync(Guid studentId, string subjectId)
        {
            var sql = "SELECT * FROM Scores WHERE StudentId = @studentId AND SubjectId = @subjectId";
            return await _dbconnection.QueryFirstOrDefaultAsync<Score>(sql, new { StudentId = studentId, SubjectId = subjectId });
        }

        public async Task<bool> UpdateAsync(Score score)
        {
            _context.Scores.Update(score);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}











