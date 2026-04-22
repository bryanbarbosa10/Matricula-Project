


using SQLite;

namespace AMPS
{
    public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //Using unique to validate by it and avoid repetition
        [Unique]
        public string StudentId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}