using SQLite;

namespace AMPS;

public class Course
{
    [PrimaryKey]
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int Creditos { get; set; }
    public bool IsCompleted { get; set; }
}
