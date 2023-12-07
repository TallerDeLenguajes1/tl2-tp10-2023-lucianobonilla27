using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            if (NoEstaLogeado()) return RedirectToRoute(new { controller = "Home", action = "Index"});
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
            if (NoEstaLogeado() || ObtenerRolUsuario() != "administrador") return RedirectToAction("Index");
            return View(new CrearUsuarioViewModel());
        }

        [HttpPost]
        [Route("CrearUsuario")]
        public IActionResult CrearUsuario(CrearUsuarioViewModel u)
        {
            if(!ModelState.IsValid) return RedirectToAction("Index");
            try
            {
                 var usuario = new Usuario(u.NombreDeUsuario,u.Contrasenia,u.RolUsuario);
                _repositorioUsuario.CrearUsuario(usuario);
               
            }
            catch (Exception e)
            {
               _logger.LogError(e.ToString());
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("EditarUsuario")]
        public IActionResult EditarUsuario(int id)
        {
            if(ObtenerRolUsuario() != "administrador") return RedirectToAction("Index");
            var u = _repositorioUsuario.ObtenerUsuarioPorId(id);
            var VmUsuario = new EditarUsuarioViewModel(u.Id,u.NombreDeUsuario,u.Contrasenia,u.RolUsuario);
            return View(VmUsuario);
        }

        [HttpPost]
        [Route("EditarUsuario")]
        public IActionResult EditarUsuario(EditarUsuarioViewModel usuario)
        {   
            if(!ModelState.IsValid) return RedirectToAction("Index");
            try
            {
                var usuarioMod = _repositorioUsuario.ObtenerUsuarioPorId(usuario.Id);
                usuarioMod.NombreDeUsuario = usuario.NombreDeUsuario;
                if (usuario.Contrasenia != null)
                {
                    usuarioMod.Contrasenia = usuario.Contrasenia;
                }
                usuarioMod.RolUsuario = usuario.RolUsuario;

                _repositorioUsuario.ModificarUsuario(usuario.Id,usuarioMod); 
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("EliminarUsuario")]
        public IActionResult EliminarUsuario(int id)
        {
            if(ObtenerRolUsuario() != "administrador") return RedirectToAction("Index");
            
            try
            {
                _repositorioUsuario.EliminarUsuario(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return RedirectToAction("Index");

        }

        private string ObtenerRolUsuario()
        {
            // Verifica si existe la sesión y si contiene el rol del usuario.
            if (HttpContext.Session.TryGetValue("NivelAcceso", out var rolBytes))
            {
                // Convierte los bytes almacenados en la sesión de nuevo a una cadena.
                var rol = Encoding.UTF8.GetString(rolBytes);
                return rol;
            }

            // Si no se encuentra el rol en la sesión, retorna una cadena vacía o nula.
            return string.Empty;
        }

        private bool NoEstaLogeado() => string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")); 

    }
}