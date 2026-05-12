namespace AMPS
{
    public static class ActiveProfileService
    {
        private const string ActiveStudentIdKey = "ActiveStudentId";

        public static Student? CurrentStudent { get; private set; }

        public static bool HasActiveProfile => CurrentStudent != null;

        public static async Task SetActiveStudentAsync(Student student)
        {
            CurrentStudent = student;
            await SecureStorage.SetAsync(ActiveStudentIdKey, student.StudentId);
        }

        public static async Task<string?> GetSavedActiveStudentIdAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(ActiveStudentIdKey);
            }
            catch
            {
                return null;
            }
        }

        public static void ClearActiveStudent()
        {
            CurrentStudent = null;
            SecureStorage.Remove(ActiveStudentIdKey);
        }
    }
}