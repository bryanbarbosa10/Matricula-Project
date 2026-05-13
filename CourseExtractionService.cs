using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;
using System.Text;
using System.Text.RegularExpressions;

namespace AMPS;

public class CourseExtractionService
{
    public async Task<List<ExtractedCourse>> ExtractCoursesAsync(FileResult file)
    {
        string extension = Path.GetExtension(file.FileName).ToLower();

        string text = extension switch
        {
            ".pdf" => await ExtractTextFromPdfAsync(file),
            ".docx" => await ExtractTextFromWordAsync(file),
            _ => string.Empty
        };

        return ParseCoursesFromText(text);
    }

    private async Task<string> ExtractTextFromPdfAsync(FileResult file)
    {
        string tempPath = await CopyFileToTempAsync(file);

        var builder = new StringBuilder();

        using var document = PdfDocument.Open(tempPath);

        foreach (var page in document.GetPages())
        {
            builder.AppendLine(page.Text);
            builder.AppendLine();
        }

        return builder.ToString();
    }

    private async Task<string> ExtractTextFromWordAsync(FileResult file)
    {
        string tempPath = await CopyFileToTempAsync(file);

        var builder = new StringBuilder();

        using var wordDoc = WordprocessingDocument.Open(tempPath, false);

        var body = wordDoc.MainDocumentPart?.Document.Body;

        if (body != null)
        {
            builder.AppendLine(body.InnerText);
        }

        return builder.ToString();
    }

    private async Task<string> CopyFileToTempAsync(FileResult file)
    {
        string tempPath = Path.Combine(FileSystem.CacheDirectory, file.FileName);

        using var sourceStream = await file.OpenReadAsync();
        using var destinationStream = File.Create(tempPath);

        await sourceStream.CopyToAsync(destinationStream);

        return tempPath;
    }

    private List<ExtractedCourse> ParseCoursesFromText(string text)
    {
        var courses = new List<ExtractedCourse>();

        if (string.IsNullOrWhiteSpace(text))
            return courses;

        // Limpieza básica del texto extraído
        text = text.Replace("\r", "\n");
        text = Regex.Replace(text, @"[ \t]+", " ");
        text = Regex.Replace(text, @"\n+", "\n");

        /*
         Detecta patrones como:
         COMP 2051 – Desarrollo Web Lado – Cliente (“Front – End”) 3
         COMP 2120 – Lógica de programación 3
         MATH 2251- Cálculo I 5
         GESP 1101 – Español I 3
         GEIC 1010 – Tecnología de la Información y Computadoras 3
         *GEEN ___02 – Inglés II 3
        */

        var pattern =
            @"(?<code>\*?[A-Z]{4}\s?(?:\d{4}|_{2,}\d{0,2}))\s*[–—-]\s*(?<name>.*?)(?=\s+(?<credits>[1-5])(?:\s|$))";

        var matches = Regex.Matches(
            text,
            pattern,
            RegexOptions.Singleline
        );

        foreach (Match match in matches)
        {
            string codigo = match.Groups["code"].Value
                .Replace("*", "")
                .Trim();

            string nombre = match.Groups["name"].Value.Trim();

            string creditsText = match.Groups["credits"].Value.Trim();

            if (!int.TryParse(creditsText, out int creditos))
                continue;

            // Evita nombres demasiado largos por culpa de texto pegado del PDF
            nombre = CleanCourseName(nombre);

            if (string.IsNullOrWhiteSpace(codigo) ||
                string.IsNullOrWhiteSpace(nombre))
                continue;

            // Evita duplicados simples
            bool alreadyExists = courses.Any(c =>
                c.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase) &&
                c.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            if (alreadyExists)
                continue;

            courses.Add(new ExtractedCourse
            {
                Codigo = codigo,
                Nombre = nombre,
                Creditos = creditos,
                IsSelected = true
            });
        }

        return courses;
    }

    private string CleanCourseName(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return string.Empty;

        nombre = nombre.Replace("\n", " ");
        nombre = Regex.Replace(nombre, @"\s+", " ").Trim();

        // Corta texto basura común que puede aparecer después del nombre
        string[] stopWords =
        {
            "Firma del estudiante",
            "Firma del profesor",
            "TOTAL",
            "PROMEDIO",
            "FECHA",
            "NOTAS",
            "Cursos en Progreso",
            "Cursos a Matricular",
            "Comentarios",
            "(R)Requisito",
            "(C) Concurrente"
        };

        foreach (string stopWord in stopWords)
        {
            int index = nombre.IndexOf(stopWord, StringComparison.OrdinalIgnoreCase);

            if (index >= 0)
            {
                nombre = nombre.Substring(0, index).Trim();
            }
        }

        return nombre;
    }
}