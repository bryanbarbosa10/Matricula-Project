


using Microsoft.Extensions.Logging;

namespace AMPS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    //FONTS
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });



            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Student.db3");
            builder.Services.AddSingleton(s => new DataBaseServices(dbPath));

            // Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<Matricula>();
            builder.Services.AddTransient<Promedio>();
            builder.Services.AddTransient<Secuencial>();

#if DEBUG
            // Debug log
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}