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
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private UsuarioRepository manejo;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
            manejo = new UsuarioRepository();

        }

        [Route("Index")]

        public IActionResult Index()
        {
            var usuarios = manejo.ListarUsuarios();
            return View(usuarios);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpGet]
        [Route("CrearUsuario")]
        public IActionResult CrearUsuario() 
        {
            return View(new Usuario());
        }

        [HttpPost]
        [Route("CrearUsuario")]
        public IActionResult CrearUsuario(Usuario u)
        {
            manejo.CrearUsuario(u);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("EditarUsuario")]
        public IActionResult EditarUsuario(int id)
        {
            var usuario = manejo.ObtenerUsuarioPorId(id);
            return View(usuario);
        }

        [HttpPost]
        [Route("EditarUsuario")]
        public IActionResult EditarUsuario(Usuario usuario)
        {   
            var usuarioMod = manejo.ObtenerUsuarioPorId(usuario.Id);
            usuarioMod.NombreDeUsuario = usuario.NombreDeUsuario;

            manejo.ModificarUsuario(usuario.Id,usuario);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("EliminarUsuario")]
        public IActionResult EliminarUsuario(int id)
        {
            manejo.EliminarUsuario(id);
            return RedirectToAction("Index");

        }

    }
}