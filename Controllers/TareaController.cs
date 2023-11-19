using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp10_2023_lucianobonilla27.Controllers
{
    [Route("[controller]")]
    public class TareaController : Controller
    {
        private readonly ILogger<TareaController> _logger;
        private TareaRepository manejo;

        public TareaController(ILogger<TareaController> logger)
        {
            _logger = logger;
            manejo = new TareaRepository();
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var tareas = manejo.ListarPorUsuario(5);
            return View(tareas);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        [HttpGet]
        [Route("EditarTarea")]
        public IActionResult EditarTarea(int id)
        {
            var tarea = manejo.ObtenerTareaPorId(id);
            return View(tarea);
        }

        [HttpPost]
        [Route("EditarTarea")]
        public IActionResult EditarTarea(int id,Tarea modificada)
        {
            manejo.ModificarTarea(id,modificada);
             return RedirectToAction("Index");

        }

        [HttpGet]
        [Route("CrearTarea")]
        public IActionResult CrearTarea()
        {
            return View(new Tarea());
        }

        [HttpPost]
        [Route("CrearTarea")]
        public IActionResult CrearTarea(Tarea nueva)
        {
            manejo.CrearTareaEnTablero(2,nueva);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("EliminarTarea")]
        public IActionResult EliminarTarea(int id)
        {
            manejo.EliminarTarea(id);
            return RedirectToAction("Index");

        }
    }
}