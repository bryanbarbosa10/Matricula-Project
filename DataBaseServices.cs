using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMPS
{
    public class DataBaseServices
    {
        private readonly SQLiteAsyncConnection _database;

        public DataBaseServices(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            
            _database.CreateTableAsync<Student>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Grade>().Wait();
        }

        
        public Task<int> RegisterStudentAsync(Student student) => _database.InsertAsync(student);
        public Task<Student> LoginStudentAsync(string id, string pass) =>
            _database.Table<Student>().Where(s => s.StudentId == id && s.Password == pass).FirstOrDefaultAsync();

       
        public Task<List<Course>> GetCoursesAsync() => _database.Table<Course>().ToListAsync();
        public Task<int> SaveCourseAsync(Course course) => _database.InsertOrReplaceAsync(course);

        
        public Task<List<Grade>> GetGradesAsync() => _database.Table<Grade>().ToListAsync();
        public Task<int> SaveGradeAsync(Grade grade) => _database.InsertAsync(grade);
        public Task<int> ClearGradesAsync() => _database.DeleteAllAsync<Grade>();
    }
}