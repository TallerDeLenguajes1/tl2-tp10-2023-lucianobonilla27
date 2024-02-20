using System;
using System.Collections.Generic;
using System.Data;
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
    public class TableroController : Controller
    {
        private ITableroRepository _repositorioTablero;
        private IUsuarioRepository _repositorioUsuario;
        private ITareaRepository _repositorioTarea;
        private readonly ILogger<TableroController> _logger;


        public TableroController(ILogger<TableroController> logger, ITableroRepository repositorioTablero, IUsuarioRepository repositorioUsuario, ITareaRepository reposirotioTarea)
        {
            _repositorioTablero = repositorioTablero;
            _repositorioUsuario = repositorioUsuario;
            _repositorioTarea = reposirotioTarea;
            _logger = logger;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
            {
                string rolUsuario = ObtenerRolUsuario();

                if (rolUsuario == "administrador")
                {
                    var viewModel = new TablerosViewModel
                    {
                        Tableros = ListarTableroViewModel(),
                        usuarioSesion = ObtenerIdUsuarioSesion()
                    };
                    return View(viewModel);
                }
                else
                {
                    var idUsuario = HttpContext.Session.GetInt32("Id");
                    var viewModel = new TablerosViewModel
                    {
                        Tableros = ListarTableroPorUsuarioViewModel(idUsuario.Value),
                        usuarioSesion = ObtenerIdUsuarioSesion()
                    };
                    return View(viewModel);
                }
            }

            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpGet]
        [Route("EditarTablero")]

        public IActionResult EditarTablero(int id)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
            {
                try
                {
                    var tablero = _repositorioTablero.ObtenerTableroPorId(id);
                    var tablerovm = new EditarTableroViewModel(tablero);
                    return View(tablerovm);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    return BadRequest();
                }


            }
            return (RedirectToRoute(new { Controller = "Home", action = "Index" }));

        }

        [HttpPost]
        [Route("EditarTablero")]
        public IActionResult EditarTablero(EditarTableroViewModel tablero)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
            {
                try
                {
                    var tableroMod = _repositorioTablero.ObtenerTableroPorId(tablero.Id);

                    tableroMod.IdUsuarioPropietario = tablero.Id_Usuario_Propietario;

                    tableroMod.Nombre = tablero.Nombre;
                    tableroMod.Descripcion = tablero.Descripcion;
                    _repositorioTablero.ModificarTablero(tablero.Id, tableroMod);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }

                return RedirectToAction("Index");
            }
            return (RedirectToRoute(new { Controller = "Home", action = "Index" }));

        }

        [HttpGet]
        [Route("CrearTablero")]
        public IActionResult CrearTablero()
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
            {
                return View(new CrearTableroViewModel());
            }
            return (RedirectToRoute(new { Controller = "Home", action = "Index" }));
        }

        [HttpPost]
        [Route("CrearTablero")]
        public IActionResult CrearTablero(CrearTableroViewModel t)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
            {
                var idUsuario = HttpContext.Session.GetInt32("Id");
                var tablero = new Tablero(0, idUsuario.Value, t.Nombre, t.Descripcion);
                try
                {
                    _repositorioTablero.CrearTablero(tablero);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
                return RedirectToAction("Index");
            }
            return (RedirectToRoute(new { Controller = "Home", action = "Index" }));
        }

        [HttpPost]
        [Route("EliminarTablero")]
        public IActionResult EliminarTablero(int id)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
            {
                try
                {
                    // Obtener el tablero antes de intentar eliminarlo
                    var tablero = _repositorioTablero.ObtenerTableroPorId(id);

                    // Verificar si el tablero tiene tareas asignadas
                    var tareasEnTablero = _repositorioTarea.ListarPorTablero(id);

                    if (tareasEnTablero.Any())
                    {
                        // Logear el error porque el tablero tiene tareas asignadas
                        _logger.LogError($"No se puede eliminar el tablero '{tablero.Nombre}' porque tiene tareas asignadas.");
                        return RedirectToAction("Index");
                    }

                    // Si no hay tareas en el tablero, proceder con la eliminación
                    _repositorioTablero.EliminarTablero(id);
                }
                catch (Exception e)
                {
                    // Logear cualquier otro error durante la eliminación del tablero
                    _logger.LogError(e.ToString());
                }

                return RedirectToAction("Index");
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

        private List<IndexTableroViewModel> ListarTableroViewModel()
        {
            var ListaTablerosViewModel = new List<IndexTableroViewModel>();
            var listaTablero = _repositorioTablero.ListarTableros();
            foreach (var item in listaTablero)
            {
                Usuario? Usu = _repositorioUsuario.ObtenerUsuarioPorId(item.IdUsuarioPropietario);
                string? nombre;
                if (Usu != null)
                {
                    nombre = Usu.NombreDeUsuario;
                }
                else
                {
                    nombre = null;
                }
                var TableroVM = new IndexTableroViewModel(item, nombre);
                ListaTablerosViewModel.Add(TableroVM);
            }
            return ListaTablerosViewModel;
        }


        private List<IndexTableroViewModel> ListarTableroPorUsuarioViewModel(int idUsuario)
        {
            var listaTablerosViewModel = new List<IndexTableroViewModel>();

            // Obtener los tableros del usuario propietario
            var tablerosPropios = _repositorioTablero.ListarTableroPorUsuario(idUsuario);

            // Obtener los tableros en los que el usuario tiene alguna tarea asignada
            var tablerosConTareasAsignadas = _repositorioTablero.ListarTablerosConTareasAsignadas(idUsuario);

            // Obtener los tableros con tareas no asignadas (con tareas donde IdUsuarioAsignado es null)
            var tablerosConTareasNoAsignadas = _repositorioTablero.ListarTablerosConTareasNoAsignadas();

            // Combinar ambas listas y eliminar duplicados usando GroupBy
            var tablerosUnicos = tablerosPropios
                .Concat(tablerosConTareasAsignadas)
                .Concat(tablerosConTareasNoAsignadas)
                .GroupBy(tablero => tablero.Id)
                .Select(group => group.First())
                .ToList();

            foreach (var tablero in tablerosUnicos)
            {
                Usuario? usuarioPropietario = _repositorioUsuario.ObtenerUsuarioPorId(tablero.IdUsuarioPropietario);
                string? nombreUsuarioPropietario = usuarioPropietario?.NombreDeUsuario;
                var tableroViewModel = new IndexTableroViewModel(tablero, nombreUsuarioPropietario);
                listaTablerosViewModel.Add(tableroViewModel);
            }

            return listaTablerosViewModel;
        }

        // Método para obtener el id del usuario de sesión
        private int ObtenerIdUsuarioSesion()
        {
            return HttpContext.Session.IsAvailable ? HttpContext.Session.GetInt32("Id") ?? 0 : 0;
        }

    }
}