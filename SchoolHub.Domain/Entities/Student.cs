using SchoolHub.Domain.Enums;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { private set; get; } = string.Empty;
        public DateTime DateOfBirth { get; private set; }
        public GenderType Gender { get; private set; }

        //liên hệ với lớp học
        public string? ClassId { get; private set; }
        public Class? Class { get; private set; }//Navigation property trỏ về Class

        // liên hệ với điểm số
        private readonly List<Score> _scores = new List<Score>();
        public IReadOnlyCollection<Score> Scores => _scores.AsReadOnly();

        protected Student()
        {
            Name = string.Empty;
            ClassId = string.Empty;
        }
        public Student(Guid id, string name, DateTime dob, GenderType gender, string? classId)
        {
            Id = id;
            Name = name;
            DateOfBirth = dob;
            Gender = gender;
            ClassId = classId;
        }
        public void TransferToClass(string newClassId)
        {
            if (string.IsNullOrWhiteSpace(newClassId))
            {
                throw new ArgumentException("Mã lớp không hợp lệ", nameof(newClassId));
            }
            ClassId = newClassId;
        }
        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Tên không hợp lệ", nameof(newName));
            }
            Name = newName;
        }
        public void ChangeDob(DateTime newDob)
        {
            if (newDob > DateTime.UtcNow)
            {
                throw new ArgumentException("Ngày sinh không thể lớn hơn ngày hiện tại", nameof(newDob));
            }
            DateOfBirth = newDob;
        }
        public void ChangeGender(GenderType newGender)
        {
            if (!Enum.IsDefined(typeof(GenderType), newGender))
            {
                throw new ArgumentException("Giới tính không hợp lệ", nameof(newGender));
            }
            Gender = newGender;
        }
    }

}










