namespace AMPS
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Routes
            Routing.RegisterRoute(nameof(ProfileCreation), typeof(ProfileCreation));
            Routing.RegisterRoute(nameof(Dashboard), typeof(Dashboard));
            Routing.RegisterRoute(nameof(Matricula), typeof(Matricula));
            Routing.RegisterRoute(nameof(Promedio), typeof(Promedio));
            Routing.RegisterRoute(nameof(Secuencial), typeof(Secuencial));
            Routing.RegisterRoute(nameof(ProfileManagement), typeof(ProfileManagement));
        }
    }
}