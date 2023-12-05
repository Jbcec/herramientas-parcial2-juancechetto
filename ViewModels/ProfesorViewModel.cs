using Microsoft.AspNetCore.Mvc.Rendering;
using Parcial1.Models;

namespace Parcial1.ViewModels;

public class ProfesorCreateViewModel
{
    public int Id { get; set; }
    public string NombreProfesor { get; set; }
    public string ApellidoProfesor { get; set; }
    public int Dni { get; set; }

    public List<Profesor> Profesores { get; set; } = new List<Profesor>();
    public int? CursoDictado { get; set; }

    public List<int>? CursoIds { get; set; }
    public string? NameFilter { get; set; }
}
