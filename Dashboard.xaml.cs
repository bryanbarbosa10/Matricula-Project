namespace AMPS
{
    public partial class Dashboard : ContentPage
    {
        private readonly DataBaseServices _dbService;

        public Dashboard(DataBaseServices dbService)
        {
            InitializeComponent();
            _dbService = dbService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            bool hasStudents = await _dbService.HasStudentsAsync();

            if (!hasStudents)
            {
                await Shell.Current.GoToAsync(nameof(ProfileCreation));
            }
        }
    }
}