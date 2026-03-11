using SQLite;

namespace AMPS
{
    public class DataBaseServices
    {
        private readonly SQLiteAsyncConnection _database;

        public DataBaseServices(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Student>()
                     .Wait();
        }

        public Task<int> RegisterStudentAsync(Student student)
        {
            return _database.InsertAsync(student);
        }


        public Task<Student> LoginStudentAsync(string studentId, string password)
        {
            return _database.Table<Student>()
                .Where(s => s.StudentId == studentId && s.Password == password)
                .FirstOrDefaultAsync();
        }
    }
}