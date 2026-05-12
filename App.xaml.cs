namespace AMPS
{
    public partial class App : Application
    {
        private readonly DataBaseServices _dbService;

        public App(DataBaseServices dbService)
        {
            InitializeComponent();
            _dbService = dbService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_dbService));
        }
    }
}