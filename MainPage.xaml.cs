namespace AMPS
{
    public partial class MainPage : ContentPage
    {

        private readonly DataBaseServices _dbService;

        public MainPage(DataBaseServices dbService)
        {
            InitializeComponent();
            _dbService = dbService;
        }
        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Registrar");
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string StudentId = StudentID.Text;
            string password = Password.Text;

            var estudiante = await _dbService.LoginStudentAsync(StudentId, password);

            if (estudiante != null)
            {
                await DisplayAlert("Bienvenido", $"Acceso concedido para: {estudiante.StudentId}", "Aceptar");
                
            }
            else
            {
                await DisplayAlert("Denegado", "Usuario o contraseña incorrectos", "Reintentar");
            }
        }
    }
}
