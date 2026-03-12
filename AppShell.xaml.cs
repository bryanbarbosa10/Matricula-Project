namespace AMPS
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Registrar", typeof(Registrar));
        }
    }
}
