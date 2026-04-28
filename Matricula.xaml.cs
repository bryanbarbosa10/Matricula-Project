using System.Collections.ObjectModel;

namespace AMPS;

public partial class Matricula : ContentPage
{
    private readonly DataBaseServices _dbService;

    // Usamos una colección observable para que la UI se actualice automáticamente
    public ObservableCollection<string> Semestres { get; set; } = new();

    public Matricula(DataBaseServices dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        BindingContext = this;

        // Datos de ejemplo iniciales
        CargarSemestresDefault();
    }

    private void CargarSemestresDefault()
    {
        Semestres.Clear();
        Semestres.Add("Semestre 1 - 2024");
        Semestres.Add("Semestre 2 - 2024");
    }

    // Lógica para el botón "+" que tienes en el XAML
    private async void OnAddMatriculaClicked(object sender, EventArgs e)
    {
        string resultado = await DisplayActionSheet("Nueva Matrícula", "Cancelar", null, "Verano 1", "Verano 2", "Próximo Semestre");
        
        if (resultado != "Cancelar" && resultado != null)
        {
            Semestres.Add(resultado);
            await DisplayAlert("Éxito", $"Se ha preparado el espacio para: {resultado}", "OK");
        }
    }

    // Este método se asegura de que si navegas a la página, los datos se refresquen
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Aquí podrías cargar las matrículas reales desde _dbService en el futuro
    }
}