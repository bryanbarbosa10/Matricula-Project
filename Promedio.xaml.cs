namespace AMPS;

public partial class Promedio : ContentPage
{
    public List<Grade> MisNotas { get; set; } = new List<Grade>();

    public Promedio()
    {
        InitializeComponent();
    }

    private void OnCalcularGPAClicked(object sender, EventArgs e)
    {
        // 1. Validar entradas
        if (string.IsNullOrWhiteSpace(EntCreditos.Text) || PickNota.SelectedItem == null)
            return;

        // 2. Crear nueva nota y añadirla a la lista
        var nuevaNota = new Grade
        {
            Materia = EntMateria.Text,
            Creditos = int.Parse(EntCreditos.Text),
            Calificacion = PickNota.SelectedItem.ToString()
        };

        MisNotas.Add(nuevaNota);

        // 3. Llamar al método de cálculo
        CalcularGPA();
    }

    private void CalcularGPA()
    {
        if (MisNotas.Count == 0) return;

         
        double honorPoints = MisNotas.Sum(n => n.PuntosDeHonor * n.Creditos);
        int totalCredits = MisNotas.Sum(n => n.Creditos);

        double gpa = honorPoints / totalCredits;

        // Actualización de la UI
        LblResultadoGPA.Text = gpa.ToString("F2");
        LblGpaTotal.Text = gpa.ToString("F2");
    }
}