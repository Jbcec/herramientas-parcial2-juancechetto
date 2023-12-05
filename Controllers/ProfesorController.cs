using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parcial1.Data;
using Parcial1.Models;
using Parcial1.Services;
using Parcial1.ViewModels;

namespace Parcial1.Controllers
{
    public class ProfesorController : Controller
    {
        private IProfesorServices _profesorService;
        private ICursoServices _cursoServices;

        public ProfesorController(IProfesorServices profesorService, ICursoServices cursoService)
        {
            _profesorService = profesorService;
            _cursoServices = cursoService;
        }

        // GET: Profesor
        public IActionResult Index(string nameFilter)
        {
            var model = new ProfesorCreateViewModel();
            model.Profesores = _profesorService.GetAll(nameFilter);
            return View(model);
        }

        // GET: Profesor/Details/5
        [Authorize(Roles = "Administrador, Profesor")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesor = _profesorService.GetById(id.Value);
            if (profesor == null)
            {
                return NotFound();
            }

            return View(profesor);
        }

        // GET: Profesor/Create
        [Authorize(Roles = "Profesor")]
        public IActionResult Create()
        {
            var cursosList = _cursoServices.GetAll();
            ViewData["Cursos"] = new SelectList(cursosList, "Id", "Nombre");
            return View();
        }

        // POST: Profesor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("Id,NombreProfesor,ApellidoProfesor,Dni,CursoIds")]
                ProfesorCreateViewModel profesorView
        )
        {
            if (ModelState.IsValid)
            {
                var cursos = _cursoServices
                    .GetAll()
                    .Where(x => profesorView.CursoIds.Contains(x.Id))
                    .ToList();

                foreach (var curso in cursos)
                {
                    if (curso.Capacidad <= 0)
                    {
                        ModelState.AddModelError(
                            "",
                            $"El curso '{curso.Nombre}' está lleno. No se puede inscribir más ProfesorEs."
                        );
                        return View(profesorView);
                    }

                    curso.Capacidad--;

                    _cursoServices.Update(curso);
                }

                var profesor = new Profesor
                {
                    NombreProfesor = profesorView.NombreProfesor,
                    ApellidoProfesor = profesorView.ApellidoProfesor,
                    Dni = profesorView.Dni,
                };

                _profesorService.Create(profesor);

                return RedirectToAction(nameof(Index));
            }

            return View(profesorView);
        }

        // GET: Profesor/Edit/5
        [Authorize(Roles = "Administrador, Profesor")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesor = _profesorService.GetById(id.Value);
            if (profesor == null)
            {
                return NotFound();
            }

            return View(profesor);
        }

        // POST: Profesor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,CursoId,NombreProfesor,ApellidoProfesor,Dni,CursoElegido")] Profesor profesor
        )
        {
            if (id != profesor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _profesorService.Update(profesor);
                return RedirectToAction(nameof(Index));
            }

            return View(profesor);
        }

        // GET: Profesor/Delete/5
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesor = _profesorService.GetById(id.Value);
            if (profesor == null)
            {
                return NotFound();
            }

            return View(profesor);
        }

        // POST: Profesor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profesor = _profesorService.GetById(id);
            if (profesor != null)
            {
                _profesorService.Delete(profesor);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProfesorExists(int id)
        {
            return _profesorService.GetById(id) != null;
        }
    }
}
