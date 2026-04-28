


using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMPS
{
    public class DataBaseServices
    {
        //Main connection to db
        private readonly SQLiteAsyncConnection _database;

        public DataBaseServices(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            
            //Create tables if not already
            _database.CreateTableAsync<Student>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Grade>().Wait();
        }



        //STUDENTS-----------------------------------------

        // Check for profiles
        public async Task<bool> HasStudentsAsync()
        {
            var students = await _database.Table<Student>().ToListAsync();
            return students.Count > 0;
        }

        //New Student profiles
        public Task<int> SaveStudentAsync(Student student)
        {
            return _database.InsertAsync(student);
        }

        //All profiles avaible
        public Task<List<Student>> GetStudentsAsync()
        {
            return _database.Table<Student>().ToListAsync();
        }

        //Validate unique student id
        //Holds A.I fix!---^
        public async Task<Student?> GetStudentByStudentIdAsync(string studentId)
        {
            var student = await _database.Table<Student>()
                .Where(s => s.StudentId == studentId)
                .FirstOrDefaultAsync();

            return student;
        }

        // Update an existing student profile
        public Task<int> UpdateStudentAsync(Student student)
        {
            return _database.UpdateAsync(student);
        }


        // COURSES-----------------------------------------

        // Shows all courses saved
        public Task<List<Course>> GetCoursesAsync()
        {
            return _database.Table<Course>().ToListAsync();
        }

        // Add or edit courses
        public Task<int> SaveCourseAsync(Course course)
        {
            return _database.InsertOrReplaceAsync(course);
        }



        // GRADES------------------------------------------

        //SHow grades
        public Task<List<Grade>> GetGradesAsync()
        {
            return _database.Table<Grade>().ToListAsync();
        }

        // Save grades
        public Task<int> SaveGradeAsync(Grade grade)
        {
            return _database.InsertAsync(grade);
        }

        // Delete
        public Task<int> ClearGradesAsync()
        {
            return _database.DeleteAllAsync<Grade>();
        }
    }
}