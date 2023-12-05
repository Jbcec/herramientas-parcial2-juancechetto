using Parcial1.Data;
using Parcial1.Models;
using Microsoft.EntityFrameworkCore;

namespace Parcial1.Services;

public class ProfesorService : IProfesorServices
{
    private readonly CursoContext _context;

    public ProfesorService(CursoContext context)
    {
        _context = context;
    }
    public void Create(Profesor obj)
    {
        _context.Add(obj);
        _context.SaveChanges();
    }

    public void Delete(Profesor obj)
    {
        _context.Remove(obj);
        _context.SaveChanges();
    }

    public List<Profesor> GetAll()
    {
        return _context.Profesor.Include(r => r.Cursos).ToList();
    }
    
    public List<Profesor> GetAll(string filter)
    {
        var query = GetQuery();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(x => x.NombreProfesor.ToLower().Contains(filter.ToLower())
                || x.ApellidoProfesor.ToLower().Contains(filter.ToLower()) 
                || x.Dni.ToString().Contains(filter));
        }

        return query.ToList();
    }

    public Profesor? GetById(int id)
    {
        var Profesor = _context.Profesor
                .Include(r => r.Cursos)
                .FirstOrDefault(m => m.Id == id);

        return Profesor;
    }

    public void Update(Profesor obj)
    {
        _context.Update(obj);
        _context.SaveChanges();
    }

    private IQueryable<Profesor> GetQuery()
    {
        return from Profesor in _context.Profesor select Profesor;
    }
}