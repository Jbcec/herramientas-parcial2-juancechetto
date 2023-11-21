using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parcial1.Data;
using Parcial1.Models;
using Parcial1.ViewModels;
using Parcial1.Services;
using Microsoft.AspNetCore.Authorization;

namespace Parcial1.Controllers
{
    public class EstudianteController : Controller
    {
        private IEstudianteServices _estudianteService;  
        private ICursoServices _cursoServices;

        public EstudianteController(IEstudianteServices estudianteService, ICursoServices cursoService)
        {
            _estudianteService = estudianteService;
            _cursoServices = cursoService;
        }
      
        // GET: Estudiante
        public IActionResult Index(string nameFilter)
        {
            var model = new EstudianteCreateViewModel();
            model.Estudiantes = _estudianteService.GetAll(nameFilter);
            return View(model);
        }

        // GET: Estudiante/Details/5
        [Authorize(Roles = "Administrador, Profesor")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = _estudianteService.GetById(id.Value);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // GET: Estudiante/Create
        [Authorize(Roles = "Estudiante")]
        public IActionResult Create()
        {
            var cursosList  = _cursoServices.GetAll();
            ViewData["Cursos"] = new SelectList(cursosList, "Id", "Nombre");
            return View();
        }

        // POST: Estudiante/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        
        public IActionResult Create([Bind("Id,NombreAlumno,ApellidoAlumno,Dni,CursoIds")] EstudianteCreateViewModel estudianteView)
        {
            if (ModelState.IsValid)
            {
                var cursos = _cursoServices.GetAll().Where(x => estudianteView.CursoIds.Contains(x.Id)).ToList();

                foreach (var curso in cursos)
                {
                    if (curso.Capacidad <= 0)
                    {
                        ModelState.AddModelError("", $"El curso '{curso.Nombre}' está lleno. No se puede inscribir más estudiantes.");
                        return View(estudianteView);
                    }

                    curso.Capacidad--;

                     _cursoServices.Update(curso);
                }

                var estudiante = new Estudiante
                {
                    NombreAlumno = estudianteView.NombreAlumno,
                    ApellidoAlumno = estudianteView.ApellidoAlumno,
                    Dni = estudianteView.Dni,
                    Cursos = cursos
                };

                _estudianteService.Create(estudiante);

                return RedirectToAction(nameof(Index));
            }

            return View(estudianteView);
        }


        // GET: Estudiante/Edit/5
        [Authorize(Roles = "Administrador, Estudiante")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = _estudianteService.GetById(id.Value);
            if (estudiante == null)
            {
                return NotFound();
            }
            
            return View(estudiante);
        }

        // POST: Estudiante/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CursoId,NombreAlumno,ApellidoAlumno,Dni,CursoElegido")] Estudiante estudiante)
        {
              if (id != estudiante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _estudianteService.Update(estudiante);
                return RedirectToAction(nameof(Index));
            }
            
            return View(estudiante);
        }

        // GET: Estudiante/Delete/5
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = _estudianteService.GetById(id.Value);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = _estudianteService.GetById(id);
            if (estudiante != null)
            {
                _estudianteService.Delete(estudiante);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
          return _estudianteService.GetById(id) != null;
        }
    }
 }

