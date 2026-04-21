

namespace AMPS;

public partial class MainPage : ContentPage
{
    private readonly DataBaseServices _dbService;

    public MainPage(DataBaseServices dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Validación básica de campos vacíos antes de llamar a la DB
        if (string.IsNullOrWhiteSpace(StudentID.Text) || string.IsNullOrWhiteSpace(Password.Text))
        {
            await DisplayAlert("Atención", "Por favor, completa todos los campos", "OK");
            return;
        }

        try
        {
            var estudiante = await _dbService.LoginStudentAsync(StudentID.Text, Password.Text);

            if (estudiante != null)
            {
                
                await Shell.Current.GoToAsync("//Matricula");
            }
            else
            {
                await DisplayAlert("Error", "ID o contraseña incorrectos", "Intentar de nuevo");
            }
        }
        catch (Exception ex)
        {
            
            await DisplayAlert("Error de Sistema", "No se pudo conectar con la base de datos.", "OK");
        }

    }
    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        // Navega a la página de registro
        await Shell.Current.GoToAsync("Registrar");
    }


}