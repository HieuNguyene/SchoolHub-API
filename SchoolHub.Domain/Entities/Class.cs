using SchoolHub.Domain.Entities;


namespace SchoolHub.Domain.Entities
{
    public class Class
    {
        public String ClassId { get; private set; }
        public String ClassName { get; private set; }
        private readonly List<Student> _students = new List<Student>(); // nơi lưu trữ thông tin thật, readonly để không cho phép dùng new
        public IReadOnlyCollection<Student> Students => _students.AsReadOnly(); //thông tin ra bên ngoài chỉ được đọc 
        protected Class()
        {
            ClassId = String.Empty;
            ClassName = String.Empty;
        }
        public Class(string classId, string className)
        {
            if (String.IsNullOrWhiteSpace(classId))
            {
                throw new ArgumentException("Mã lớp (classId) không được để trống.", nameof(classId));
            }
            if (String.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("Tên lớp (ClassName) không được để trống.", nameof(className));
            }
            ClassId = classId;
            ClassName = className;
        }
        public void UpdateClassName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Tên lớp không được để trống.", nameof(newName));
            }
            ClassName = newName;
        }
        public void AddStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            _students.Add(student);
        }
    }
}










