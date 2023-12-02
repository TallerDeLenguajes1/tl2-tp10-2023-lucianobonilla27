using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tl2_tp09_2023_lucianobonilla27.Models;
using tl2_tp10_2023_lucianobonilla27.ViewModels;

namespace tl2_tp10_2023_lucianobonilla27.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _repositorioUsuario;
        private readonly ILogger<UsuarioController> _logger;
        
        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository repositorioUsuario)
        {
            _repositorioUsuario=repositorioUsuario;
            _logger = logger;
        }

        [Route("Index")]

        public IActionResult Index()
        {
            var usuarios = _repositorioUsuario.ListarUsuarios();
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
            return View(new CrearUsuarioViewModel());
        }

        [HttpPost]
        [Route("CrearUsuario")]
        public IActionResult CrearUsuario(CrearUsuarioViewModel u)
        {
            var usuario = new Usuario(u.NombreDeUsuario,u.Contrasenia,u.RolUsuario);
            _repositorioUsuario.CrearUsuario(usuario);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("EditarUsuario")]
        public IActionResult EditarUsuario(int id)
        {
            var u = _repositorioUsuario.ObtenerUsuarioPorId(id);
            var VmUsuario = new EditarUsuarioViewModel(u.Id,u.NombreDeUsuario,u.Contrasenia,u.RolUsuario);
            return View(VmUsuario);
        }

        [HttpPost]
        [Route("EditarUsuario")]
        public IActionResult EditarUsuario(EditarUsuarioViewModel usuario)
        {   
            var usuarioMod = _repositorioUsuario.ObtenerUsuarioPorId(usuario.Id);
            usuarioMod.NombreDeUsuario = usuario.NombreDeUsuario;
            if (usuario.Contrasenia != null)
            {
                usuarioMod.Contrasenia = usuario.Contrasenia;
            }
            usuarioMod.RolUsuario = usuario.RolUsuario;

            _repositorioUsuario.ModificarUsuario(usuario.Id,usuarioMod);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("EliminarUsuario")]
        public IActionResult EliminarUsuario(int id)
        {
            _repositorioUsuario.EliminarUsuario(id);
            return RedirectToAction("Index");

        }

    }
}