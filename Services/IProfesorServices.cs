using Parcial1.Models;

namespace Parcial1.Services;

public interface IProfesorServices
{
    void Create (Profesor obj);
    List<Profesor> GetAll();
    List<Profesor> GetAll(string filter);
    void Update (Profesor obj);
    void Delete (Profesor obj);
    Profesor? GetById(int id);
}