using Microsoft.AspNetCore.Mvc.Rendering;
using Parcial1.Models;

namespace Parcial1.ViewModels;

public class EstudianteCreateViewModel
{
    public int Id { get; set; }
    public string NombreAlumno { get; set; }
    public string ApellidoAlumno { get; set; }
    public int Dni { get; set; }
    public int? CursoElegido { get; set; }
    public virtual List<CursoViewModel>? Cursos { get; set; }
    public List<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
    public List<int>? CursoIds { get; set; } 
    public string? NameFilter { get; set; }
    
}
