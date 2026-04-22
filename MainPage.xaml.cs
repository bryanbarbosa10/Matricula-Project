


//Regex verification
using System.Text.RegularExpressions;

namespace AMPS;

public partial class MainPage : ContentPage
{
    // DB service
    private readonly DataBaseServices _dbService;

    public MainPage(DataBaseServices dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    // OnStarrt
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        bool hasStudents = await _dbService.HasStudentsAsync();

        // If there are saved profiles, skip MainPage
        //toBechanged later when user selector is made
        if (hasStudents)
        {
            await Shell.Current.GoToAsync(nameof(Secuencial));
        }
    }

    // Save profile
    private async void OnSaveProfileClicked(object sender, EventArgs e)
    {
        // Clean spaces
        string name = NameEntry.Text?.Trim() ?? string.Empty;
        string studentId = StudentIdEntry.Text?.Trim() ?? string.Empty;
        string email = EmailEntry.Text?.Trim() ?? string.Empty;

        // Name
        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Missing Data", "Name is required.", "OK");
            return;
        }

        // ID
        if (string.IsNullOrWhiteSpace(studentId))
        {
            await DisplayAlert("Missing Data", "Student ID is required.", "OK");
            return;
        }

        // Extra safety for name length
        if (name.Length > 30)
        {
            await DisplayAlert("Invalid Data", "Name cannot exceed 30 characters.", "OK");
            return;
        }

        // Extra safety for student ID length
        if (studentId.Length > 25)
        {
            await DisplayAlert("Invalid Data", "Student ID cannot exceed 25 characters.", "OK");
            return;
        }

        // Validate email format only if typed
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

        // Create profile as object
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

            // Next page
            await Shell.Current.GoToAsync(nameof(Secuencial));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not save profile: {ex.Message}", "OK");
        }
    }
}