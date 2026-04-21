namespace AMPS
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Registrar", typeof(Registrar));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
        }

        public void SetUserContext(string studentId)
        {
            LblEstudianteId.Text = $"ID: {studentId}";
        }
    }
}