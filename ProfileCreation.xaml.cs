using System.Text.RegularExpressions;

namespace AMPS
{
    [QueryProperty(nameof(From), "from")]
    public partial class ProfileCreation : ContentPage
    {
        // DB service
        private readonly DataBaseServices _dbService;

        // Where did this page open from?
        public string From { get; set; } = string.Empty;

        public ProfileCreation(DataBaseServices dbService)
        {
            InitializeComponent();
            _dbService = dbService;
        }

        // Clear fields every time page opens
        protected override void OnAppearing()
        {
            base.OnAppearing();

            NameEntry.Text = string.Empty;
            StudentIdEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
        }

        // Save profile
        private async void OnSaveProfileClicked(object sender, EventArgs e)
        {
            // Clean text
            string name = NameEntry.Text?.Trim() ?? string.Empty;
            string studentId = StudentIdEntry.Text?.Trim() ?? string.Empty;
            string email = EmailEntry.Text?.Trim() ?? string.Empty;

            // Name required
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Missing Data", "Name is required.", "OK");
                return;
            }

            // Student ID required
            if (string.IsNullOrWhiteSpace(studentId))
            {
                await DisplayAlert("Missing Data", "Student ID is required.", "OK");
                return;
            }

            // Max lengths
            if (name.Length > 30)
            {
                await DisplayAlert("Invalid Data", "Name cannot exceed 30 characters.", "OK");
                return;
            }

            if (studentId.Length > 25)
            {
                await DisplayAlert("Invalid Data", "Student ID cannot exceed 25 characters.", "OK");
                return;
            }

            // Validate email only if typed
            if (!string.IsNullOrWhiteSpace(email))
            {
                bool validEmail = Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                );

                if (!validEmail)
                {
                    await DisplayAlert("Invalid Email", "Please enter a valid email.", "OK");
                    return;
                }
            }

            // Check duplicate student ID
            var existingStudent = await _dbService.GetStudentByStudentIdAsync(studentId);
            if (existingStudent != null)
            {
                await DisplayAlert("Duplicate Student ID", "That student ID is already registered.", "OK");
                return;
            }

            // Create object
            var student = new Student
            {
                Name = name,
                StudentId = studentId,
                Email = email
            };

            try
            {
                // Save profile
                await _dbService.SaveStudentAsync(student);

                await DisplayAlert("Success", "Profile saved successfully.", "OK");

                // If opened from profile management, go back there
                if (From == "profiles")
                {
                    await Shell.Current.GoToAsync("//ProfileManagement");
                }
                else
                {
                    // First boot
                    await Shell.Current.GoToAsync("//Dashboard");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not save profile: {ex.Message}", "OK");
            }
        }
    }
}