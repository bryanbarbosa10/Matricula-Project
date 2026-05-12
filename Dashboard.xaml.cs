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

            if (!ActiveProfileService.HasActiveProfile)
            {
                var savedStudentId = await ActiveProfileService.GetSavedActiveStudentIdAsync();

                if (!string.IsNullOrWhiteSpace(savedStudentId))
                {
                    var student = await _dbService.GetStudentByStudentIdAsync(savedStudentId);

                    if (student != null)
                    {
                        await ActiveProfileService.SetActiveStudentAsync(student);
                    }
                }
            }

            if (ActiveProfileService.HasActiveProfile)
            {
                var student = ActiveProfileService.CurrentStudent;
                WelcomeLabel.Text = $"Welcome, {student.Name}";

                if (!string.IsNullOrWhiteSpace(student.Nickname))
                {
                    NicknameLabel.Text = student.Nickname;
                    NicknameLabel.IsVisible = true;
                }
                else
                {
                    NicknameLabel.Text = string.Empty;
                    NicknameLabel.IsVisible = false;
                }
            }
            else
            {
                WelcomeLabel.Text = "No hay perfil activo.";
                NicknameLabel.Text = string.Empty;
                NicknameLabel.IsVisible = false;
            }
        }
    }
}