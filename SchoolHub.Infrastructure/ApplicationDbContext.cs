
using Microsoft.EntityFrameworkCore;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<Score> Scores => Set<Score>();
        public DbSet<Class> Classes => Set<Class>();
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        // public override Task<int> SaveChangeAsync(
        //     CancellationToken cancellationToken =default
        // )
        // {
        //     return base.SaveChangesAsync(cancellationToken);
        // }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(x => x.DateOfBirth);

                entity.Property(x => x.Gender);

                entity.HasOne(x => x.Class)
                    .WithMany(c => c.Students)
                    .HasForeignKey(s => s.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);// k cho xóa lớp nếu bên trong có học sinh
                entity.Metadata.FindNavigation(nameof(Student.Scores))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(x => x.ClassId);
                entity.Property(x => x.ClassName)
                     .IsRequired()
                     .HasMaxLength(50);
                entity.Metadata.FindNavigation(nameof(Class.Students))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
            });
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(x => x.SubjectId);
                entity.Property(x => x.SubjectName)
                     .IsRequired();
                entity.Metadata.FindNavigation(nameof(Subject.Scores))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
            });
            modelBuilder.Entity<Score>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasOne(score => score.Subject)
                    .WithMany(subj => subj.Scores)
                    .HasForeignKey(score => score.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict); // không được xóa môn nếu đã có điểm
                entity.HasOne(score => score.Student)
                    .WithMany(stu => stu.Scores)
                    .HasForeignKey(score => score.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Token).IsRequired();
                entity.Property(x => x.JwtId).IsRequired();
                entity.Property(x => x.UserId).IsRequired();
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(u => u.Username).IsUnique(); // Đảm bảo không trùng tên đăng nhập
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
            });
        }

    }
}













