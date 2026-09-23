using SchoolHub.Domain.Entities;

namespace SchoolHub.Domain.Entities
{
    public class Score
    {
        public Guid Id { get; private set; }
        public float Value { get; private set; }

        public Guid StudentId { get; private set; }
        public string SubjectId { get; private set; }

        public Student? Student { get; private set; }
        public Subject? Subject { get; private set; }

        protected Score()
        {
            SubjectId = string.Empty;
        }
        public Score(Guid id, float value, Guid studentId, string subjectId)
        {
            if (value < 0 || value > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Điểm số phải từ 0 đến 10");
            }
            Id = id;
            Value = value;
            StudentId = studentId;
            SubjectId = subjectId;
        }
        public void UpdateValue(float newValue)
        {
            if (newValue < 0 || newValue > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(newValue), "Điểm số phải từ 0 đến 10");
            }
            Value = newValue;
        }
    }
}









