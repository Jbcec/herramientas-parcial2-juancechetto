using System.ComponentModel.DataAnnotations;

namespace Parcial1.Models;

public class Profesor
{
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    public string NombreProfesor { get; set; }

    [Display(Name = "Apellido")]
    public string ApellidoProfesor { get; set; }
    public int Dni { get; set; }

    [Display(Name = "Curso Elegido")]
    public string? CursoDictado { get; set; }


}