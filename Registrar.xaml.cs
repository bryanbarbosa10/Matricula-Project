namespace AMPS;

public partial class Registrar : ContentPage
{
    private readonly DataBaseServices _dbService;

    public Registrar(DataBaseServices dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    private async void OnBtnGuardarClicked(object sender, EventArgs e)
    {
      
        if (string.IsNullOrWhiteSpace(TxtStudentId.Text) ||
            string.IsNullOrWhiteSpace(TxtFullName.Text) ||
            string.IsNullOrWhiteSpace(TxtEmail.Text) ||
            string.IsNullOrWhiteSpace(TxtPassword.Text))
        {
            await DisplayAlert("Campos Incompletos", "Por favor llena todos los datos.", "OK");
            return;
        }

        var nuevoEstudiante = new Student
        {
            StudentId = TxtStudentId.Text,
            FullName = TxtFullName.Text,
            Email = TxtEmail.Text,
            Password = TxtPassword.Text
        };

        try
        {

            await _dbService.RegisterStudentAsync(nuevoEstudiante);

            await DisplayAlert("Éxito", "Estudiante registrado correctamente", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "No se pudo registrar. El ID podría estar duplicado.", "Aceptar");
        }
    }

    private async void OnBtnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}