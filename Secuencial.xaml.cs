using System.Collections.ObjectModel;

namespace AMPS;

public partial class Secuencial : ContentPage
{
    private readonly DataBaseServices _dbService;
    public ObservableCollection<Course> MiSecuencial { get; set; } = new();

    public Secuencial(DataBaseServices dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatos();
    }

    private async void OnUploadDocumentClicked(object sender, EventArgs e)
    {
        try
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.iOS, new[] { "com.adobe.pdf", "public.image" } },
                { DevicePlatform.Android, new[] { "application/pdf", "image/*" } },
                { DevicePlatform.WinUI, new[] { ".pdf", ".jpg", ".png" } },
                });

            PickOptions options = new()
            {
                PickerTitle = "Selecciona tu documento secuencial",
                FileTypes = customFileType,
            };

            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                FileNameLabel.Text = $"Archivo: {result.FileName}";

               
                await DisplayAlert("Éxito", "Documento cargado. Ahora puedes marcar tus cursos.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo cargar el archivo: {ex.Message}", "OK");
        }
    }

    private async Task CargarDatos()
    {
        var lista = await _dbService.GetCoursesAsync();
        if (lista.Count == 0)
        {
            // Datos iniciales si la DB está vacía
            var iniciales = new List<Course> {
                new Course { Codigo="COMP2800", Nombre="Programación I", Creditos=3, IsCompleted=false },
                new Course { Codigo="MATE1000", Nombre="Precálculo", Creditos=4, IsCompleted=false }
            };
            foreach (var c in iniciales) await _dbService.SaveCourseAsync(c);
            lista = iniciales;
        }

        MiSecuencial.Clear();
        foreach (var c in lista) MiSecuencial.Add(c);
    }

    private async void OnGuardarProgresoClicked(object sender, EventArgs e)
    {
        foreach (var curso in MiSecuencial)
            await _dbService.SaveCourseAsync(curso);

        await DisplayAlert("AMPS", "Progreso guardado correctamente", "OK");
    }
}