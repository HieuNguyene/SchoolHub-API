namespace SchoolHub.Domain.Entities
{
    public class Subject
    {
        public string SubjectId { get; private set; }
        public string SubjectName { get; private set; }

        private readonly List<Score> _scores = new List<Score>();
        public IReadOnlyCollection<Score> Scores => _scores.AsReadOnly();

        protected Subject()
        {
            SubjectId = string.Empty;
            SubjectName = string.Empty;
        }
        public Subject(string subjectId, string subjectName)
        {
            if (string.IsNullOrWhiteSpace(subjectId)) throw new ArgumentException("Mã môn học không được để trống", nameof(subjectId));
            if (string.IsNullOrWhiteSpace(subjectName)) throw new ArgumentException("Tên môn học không được để trống", nameof(subjectName));

            SubjectId = subjectId;
            SubjectName = subjectName;
        }

        public void UpdateSubjectName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Tên môn học không được để trống", nameof(newName));
            }
            SubjectName = newName;
        }

    }
}









