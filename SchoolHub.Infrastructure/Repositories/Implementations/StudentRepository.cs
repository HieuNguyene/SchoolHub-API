using System.Data;
using Dapper;
using SchoolHub.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Domain.Entities;
using SchoolHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SchoolHub.Infrastructure.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;

        public StudentRepository(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }



        public async Task<Student> CreateStudentAsync(Student student)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var student = await GetByIdAsync(id);
            if (student == null) return false;

            _context.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Student> UpdateAsync(Student newStudent)
        {
            _context.Students.Update(newStudent);
            await _context.SaveChangesAsync();
            return newStudent;
        }



        public async Task<List<Student>> GetAllStudentAsync()
        {
            var sql = "SELECT * FROM Students";
            var students = await _dbConnection.QueryAsync<Student>(sql);
            return students.ToList();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM Students WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Student>(sql, new { Id = id });
        }

        public async Task<List<Student>> GetStudentByKeyWordAsync(string? keyword, int pageSize, int pageNumber)
        {
            var parameters = new DynamicParameters();
            var sql = "SELECT * FROM Students WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND Name LIKE @Keyword";
                parameters.Add("Keyword", $"%{keyword}%");
            }

            sql += " ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add("Offset", (pageNumber - 1) * pageSize);
            parameters.Add("PageSize", pageSize);

            var students = await _dbConnection.QueryAsync<Student>(sql, parameters);
            return students.ToList();
        }

        public async Task<List<Student>> GetStudentsByClassIdAsync(string classId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ClassId", classId);

            var students = await _dbConnection.QueryAsync<Student>(
                "sp_GetStudentsByClass",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return students.ToList();
        }

    }
}
