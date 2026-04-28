using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AMPS
{
    public partial class ProfileManagement : ContentPage
    {
        // DB service
        private readonly DataBaseServices _dbService;

        // Local list
        public ObservableCollection<Student> Profiles { get; set; } = new();

        public ProfileManagement(DataBaseServices dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            BindingContext = this;
        }

        // Reload profiles every time page opens
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadProfilesAsync();
        }

        // Load all profiles
        private async Task LoadProfilesAsync()
        {
            var students = await _dbService.GetStudentsAsync();

            Profiles.Clear();

            foreach (var student in students)
            {
                Profiles.Add(student);
            }

            ProfilesCollectionView.ItemsSource = Profiles;
        }

        // Open create profile page
        private async void OnAddProfileClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(ProfileCreation)}?from=profiles");
        }

        // Edit one profile
        private async void OnEditProfileClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Student selectedStudent)
            {
                // Ask for new name
                string? newName = await DisplayPromptAsync(
                    "Edit Name",
                    "Enter the new name:",
                    initialValue: selectedStudent.Name,
                    maxLength: 30);

                if (newName == null)
                    return;

                newName = newName.Trim();

                if (string.IsNullOrWhiteSpace(newName))
                {
                    await DisplayAlert("Invalid Data", "Name cannot be empty.", "OK");
                    return;
                }

                // Ask for new email
                string? newEmail = await DisplayPromptAsync(
                    "Edit Email",
                    "Enter the new email (optional):",
                    initialValue: selectedStudent.Email);

                if (newEmail == null)
                    return;

                newEmail = newEmail.Trim();

                // Validate email only if typed
                if (!string.IsNullOrWhiteSpace(newEmail))
                {
                    bool validEmail = Regex.IsMatch(
                        newEmail,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                    );

                    if (!validEmail)
                    {
                        await DisplayAlert("Invalid Email", "Please enter a valid email.", "OK");
                        return;
                    }
                }

                // Keep StudentId unchanged
                selectedStudent.Name = newName;
                selectedStudent.Email = newEmail;

                await _dbService.UpdateStudentAsync(selectedStudent);

                await DisplayAlert("Success", "Profile updated successfully.", "OK");

                await LoadProfilesAsync();
            }
        }
    }
}