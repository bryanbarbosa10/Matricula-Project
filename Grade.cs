using SQLite;

namespace AMPS;

public class Grade
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int StudentDbId { get; set; }

    public string Materia { get; set; } = string.Empty;

    public int Creditos { get; set; }

    public string Calificacion { get; set; } = string.Empty;

    public double PuntosDeHonor => Calificacion?.ToUpper() switch
    {
        "A" => 4.0,
        "B" => 3.0,
        "C" => 2.0,
        "D" => 1.0,
        _ => 0.0
    };
}