namespace AMPS;

public partial class Promedio : ContentPage
{
    private readonly DataBaseServices _dbService;

    public List<Grade> MisNotas { get; set; } = new List<Grade>();

    public Promedio(DataBaseServices dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!ActiveProfileService.HasActiveProfile)
        {
            await DisplayAlert(
                "Perfil requerido",
                "Debes seleccionar un perfil antes de usar Promedio.",
                "OK"
            );

            await Shell.Current.GoToAsync("//ProfileManagement");
            return;
        }

        await CargarNotasAsync();
    }

    private async Task CargarNotasAsync()
    {
        MisNotas = await _dbService.GetGradesForActiveStudentAsync();

        CalcularGPA();
    }

    private async void OnCalcularGPAClicked(object sender, EventArgs e)
    {
        if (!ActiveProfileService.HasActiveProfile)
        {
            await DisplayAlert("Error", "No hay perfil activo.", "OK");
            return;
        }

        // Validate subject
        string materia = EntMateria.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(materia))
        {
            await DisplayAlert("Missing Data", "Debes escribir la materia.", "OK");
            return;
        }

        // Validate credits
        if (string.IsNullOrWhiteSpace(EntCreditos.Text))
        {
            await DisplayAlert("Missing Data", "Debes escribir los créditos.", "OK");
            return;
        }

        if (!int.TryParse(EntCreditos.Text, out int creditos) || creditos <= 0)
        {
            await DisplayAlert("Invalid Data", "Los créditos deben ser un número mayor de 0.", "OK");
            return;
        }

        // Validate grade
        if (PickNota.SelectedItem == null)
        {
            await DisplayAlert("Missing Data", "Debes seleccionar una calificación.", "OK");
            return;
        }

        string calificacion = PickNota.SelectedItem.ToString() ?? string.Empty;

        // Create new grade
        var nuevaNota = new Grade
        {
            Materia = materia,
            Creditos = creditos,
            Calificacion = calificacion
        };

        // Save grade to SQLite with active profile
        await _dbService.SaveGradeAsync(nuevaNota);

        // Reload grades from active profile
        await CargarNotasAsync();

        // Clear inputs
        EntMateria.Text = string.Empty;
        EntCreditos.Text = string.Empty;
        PickNota.SelectedItem = null;

        await DisplayAlert("AMPS", "Nota guardada correctamente.", "OK");
    }

    private void CalcularGPA()
    {
        if (MisNotas.Count == 0)
        {
            LblResultadoGPA.Text = "0.00";
            LblGpaTotal.Text = "0.00";
            return;
        }

        double honorPoints = MisNotas.Sum(n => n.PuntosDeHonor * n.Creditos);
        int totalCredits = MisNotas.Sum(n => n.Creditos);

        if (totalCredits == 0)
        {
            LblResultadoGPA.Text = "0.00";
            LblGpaTotal.Text = "0.00";
            return;
        }

        double gpa = honorPoints / totalCredits;

        LblResultadoGPA.Text = gpa.ToString("F2");
        LblGpaTotal.Text = gpa.ToString("F2");
    }
}