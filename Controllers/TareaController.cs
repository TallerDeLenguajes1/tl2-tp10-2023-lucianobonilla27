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
    public class TareaController : Controller
    {
        private readonly ITareaRepository _repositorioTarea;
        private readonly ITableroRepository _repositorioTablero;
        private IUsuarioRepository _repositorioUsuario;
        private readonly ILogger<TareaController> _logger;

        public TareaController(ILogger<TareaController> logger, ITareaRepository repositorioTarea, ITableroRepository repositorioTablero, IUsuarioRepository repositorioUsuario)
        {
            _repositorioTarea = repositorioTarea;
            _repositorioTablero = repositorioTablero;
            _repositorioUsuario = repositorioUsuario;
            _logger = logger;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null && HttpContext.Session.GetString("NivelAcceso") != "administrador"){
                    return View(ListarTareasIndex());
            }
            else if(HttpContext.Session.IsAvailable && HttpContext.Session.GetString("NivelAcceso") == "administrador")
            {
                var tareas = _repositorioTarea.ListarTareas();
                List<IndexTareaViewModel> tareasvm = new List<IndexTareaViewModel>();
                foreach (var tarea in tareas)
                {
                    var tablero = _repositorioTablero.ObtenerTableroPorId(tarea.IdTablero);
                    var usuarioAsignado = _repositorioUsuario.ObtenerUsuarioPorId(tarea.IdUsuarioAsignado);

                    string? nombreUsuarioAsignado = usuarioAsignado?.NombreDeUsuario;
                    string? nombreTablero = tablero?.Nombre;

                    var tarvm = new IndexTareaViewModel(tarea, nombreUsuarioAsignado, nombreTablero);
                    tareasvm.Add(tarvm);
                }
                var tareasViewModel = new TareasViewModel()
                {
                    Tareas = tareasvm,
                    UsuarioSesion = ObtenerIdUsuarioSesion(),
                    TablerosDelUsuarioSesion = _repositorioTablero.ListarTableroPorUsuario(ObtenerIdUsuarioSesion()),
                    NivelAcceso = "administrador"
                };
                return View(tareasViewModel);
            }
             return (RedirectToRoute(new { Controller = "Home", action = "Index" }));
             
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
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
                {
                    var tarea = _repositorioTarea.ObtenerTareaPorId(id);
                    var tareaVm = new EditarTareaViewModel();
                    tareaVm.Id = tarea.Id;
                    tareaVm.Descripcion = tarea.Descripcion;
                    tareaVm.Color = tarea.Color;
                    tareaVm.EstadoT = tarea.EstadoT;
                    tareaVm.IdTablero = tarea.IdTablero;
                    tareaVm.IdUsuarioAsignado = tarea.IdUsuarioAsignado;
                    tareaVm.Nombre = tarea.Nombre;
                    tareaVm.Usuarios = _repositorioUsuario.ListarUsuarios();
                    tareaVm.Tableros = _repositorioTablero.ListarTableroPorUsuario(ObtenerIdUsuarioSesion());
                    var tab = _repositorioTablero.ObtenerTableroPorId(tarea.IdTablero);
                    tareaVm.NombreTablero = tab.Nombre;
                    if (tarea.IdUsuarioAsignado != null)
                    {
                        var usu = _repositorioUsuario.ObtenerUsuarioPorId(tarea.IdUsuarioAsignado);
                        tareaVm.NombreUsuarioAsignado = usu.NombreDeUsuario;
                    }
                    
                    return View(tareaVm);
                }
                return RedirectToAction("Index", "Home");
            }

            [HttpPost]
            [Route("EditarTarea")]
            public IActionResult EditarTarea(int id, EditarTareaViewModel modificada)
            {
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
                {
                    try
                    {
                        var tarea = _repositorioTarea.ObtenerTareaPorId(id);
                        var tablero = _repositorioTablero.ObtenerTableroPorNombre(modificada.NombreTablero);
                        
                        if (modificada.IdUsuarioAsignado.HasValue && modificada.IdUsuarioAsignado == -1)
                        {
                            // Ningún usuario asignado
                            modificada.IdUsuarioAsignado = null;
                        }

                        if (tablero != null)
                        {
                            modificada.IdTablero = tablero.Id;
                        }
                        else
                        {
                            modificada.IdTablero = tarea.IdTablero;
                        }

                        _repositorioTarea.ModificarTarea(id, new Tarea(modificada.Id, modificada.IdTablero, modificada.Nombre, modificada.EstadoT, modificada.Descripcion, modificada.Color, modificada.IdUsuarioAsignado));
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                    }

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", "Home");
            }


         [HttpGet]
        [Route("CrearTarea")]
        public IActionResult CrearTarea()
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
            {
                var CrearTareaVm = new CrearTareaViewModel();
                CrearTareaVm.Usuarios = _repositorioUsuario.ListarUsuarios();
                CrearTareaVm.Tableros = _repositorioTablero.ListarTableroPorUsuario(ObtenerIdUsuarioSesion());
                return View(CrearTareaVm);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("CrearTarea")]
        public IActionResult CrearTarea(CrearTareaViewModel nueva)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
            {
                try
                {
                    var tarea = new Tarea();
                    tarea.Id = nueva.Id;
                    tarea.Color = nueva.Color;
                    tarea.Descripcion = nueva.Descripcion;
                    tarea.Nombre = nueva.Nombre;
                    tarea.EstadoT = nueva.EstadoT;

                    if (nueva.IdUsuarioAsignado.HasValue)
                    {
                        tarea.IdUsuarioAsignado = nueva.IdUsuarioAsignado;
                    }
                    else
                    {
                        tarea.IdUsuarioAsignado = null;
                    }

                    var tab = _repositorioTablero.ObtenerTableroPorNombre(nueva.NombreTablero);
                    _repositorioTarea.CrearTareaEnTablero(tab.Id, tarea);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }



            [HttpPost]
            [Route("EliminarTarea")]
            public IActionResult EliminarTarea(int id)
            {
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                    try
                    {
                        _repositorioTarea.EliminarTarea(id);
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
            [Route("CambiarEstadoTarea")]

            public IActionResult CambiarEstadoTarea(int id)
            {
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
                {

                    var tarea = _repositorioTarea.ObtenerTareaPorId(id);
                    var cambiarEstadoVm = new CambiarEstadoTareaViewModel(){
                        Id = tarea.Id,
                        EstadoT = tarea.EstadoT
                    };
                    
                    return View(cambiarEstadoVm);
                }
                return RedirectToAction("Index", "Home");
                
            }

              [HttpPost]
            [Route("CambiarEstadoTarea")]
            public IActionResult CambiarEstadoTarea(CambiarEstadoTareaViewModel tareaVm)
            {
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null)
                {
                    try
                    {
                       _repositorioTarea.ModificarEstadoTarea(tareaVm.Id,tareaVm.EstadoT);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                    }

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", "Home");
            }

           

            private TareasViewModel ListarTareasIndex()
            {
                var listaTareasIndex = new List<IndexTareaViewModel>();

                // Obtener el id del usuario de sesión
                var idUsuarioSesion = ObtenerIdUsuarioSesion();

                // Obtener las tareas asignadas al usuario de sesión
                var tareasAsignadas = _repositorioTarea.ListarPorUsuario(idUsuarioSesion);

                // Obtener las tareas no asignadas (con IdUsuarioAsignado igual a null)
                var tareasNoAsignadas = _repositorioTarea.ListarTareasNoAsignadas();

                // Obtener los tableros del usuario de sesión
                var tablerosUsuario = _repositorioTablero.ListarTableroPorUsuario(idUsuarioSesion);

                // Obtener las tareas de los tableros del usuario de sesión
                var tareasPropias = _repositorioTarea.ListarTareasPorTableros(tablerosUsuario.Select(t => t.Id).ToList());

                // Combinar y eliminar duplicados
                var todasLasTareas = tareasAsignadas.Union(tareasPropias).Union(tareasNoAsignadas).GroupBy(t => t.Id).Select(group => group.First()).ToList();


                foreach (var tarea in todasLasTareas)
                {
                    var tablero = _repositorioTablero.ObtenerTableroPorId(tarea.IdTablero);

                    var usuarioAsignado = _repositorioUsuario.ObtenerUsuarioPorId(tarea.IdUsuarioAsignado);

                    string? nombreUsuarioAsignado = usuarioAsignado?.NombreDeUsuario;
                    string? nombreTablero = tablero?.Nombre;

                    var tarvm = new IndexTareaViewModel(tarea, nombreUsuarioAsignado, nombreTablero);
                    listaTareasIndex.Add(tarvm);
                }
                var tareasViewModel = new TareasViewModel()
                {
                    Tareas = listaTareasIndex,
                    TablerosDelUsuarioSesion = tablerosUsuario,
                    UsuarioSesion = ObtenerIdUsuarioSesion(),
                    NivelAcceso = "operador"
                };

                return tareasViewModel;
            }



            // Método para obtener el id del usuario de sesión
          private int ObtenerIdUsuarioSesion()
            {
                return HttpContext.Session.IsAvailable ? HttpContext.Session.GetInt32("Id") ?? 0 : 0;
            }



        }
    }
