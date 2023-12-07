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
using tl2_tp09_2023_lucianobonilla27.Repository;
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
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                    return View(ListarTareasIndex());
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
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){

                    var tarea = _repositorioTarea.ObtenerTareaPorId(id);
                    var tareaVm = new EditarTareaViewModel();
                    tareaVm.Id = tarea.Id;
                    tareaVm.Descripcion = tarea.Descripcion;
                    tareaVm.Color = tarea.Color;
                    tareaVm.EstadoT = tarea.EstadoT;
                    tareaVm.IdTablero = tarea.IdTablero;
                    tareaVm.IdUsuarioAsignado = tarea.IdUsuarioAsignado;
                    tareaVm.Nombre = tarea.Nombre;
                    var usu = _repositorioUsuario.ObtenerUsuarioPorId(tarea.IdUsuarioAsignado);
                    tareaVm.NombreUsuarioAsignado = usu.NombreDeUsuario;
                    var tab = _repositorioTablero.ObtenerTableroPorId(tarea.IdTablero);
                    tareaVm.NombreTablero = tab.Nombre;
                    return View(tareaVm);
                }
                return (RedirectToRoute(new { Controller = "Home", action = "Index" }));

            }

            [HttpPost]
            [Route("EditarTarea")]
            public IActionResult EditarTarea(int id, EditarTareaViewModel modificada)
            {
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                    try
                    {
                        var tarea = _repositorioTarea.ObtenerTareaPorId(id);
                        var usuario = _repositorioUsuario.ObtenerUsuarioPorNombre(modificada.NombreUsuarioAsignado);
                        var tablero = _repositorioTablero.ObtenerTableroPorNombre(modificada.NombreTablero);
                        int usuarioAsignado; // creo esta variable ya que en el constructor de tarea no puedo manda modificada.IdUsuarioAsignado
                        if (usuario != null)
                        {
                            usuarioAsignado = usuario.Id;
                        }else
                        {
                            usuarioAsignado = tarea.IdUsuarioAsignado;
                        }
                         
                        if (tablero != null)
                        {
                            modificada.IdTablero = tablero.Id;
                        }else
                        {
                            modificada.IdTablero = tarea.IdTablero;
                        }
                        _repositorioTarea.ModificarTarea(id, new Tarea(modificada.Id,modificada.IdTablero,modificada.Nombre,modificada.EstadoT,modificada.Descripcion,modificada.Color,usuarioAsignado));
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
            [Route("CrearTarea")]
            public IActionResult CrearTarea()
            {
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                     return View(new CrearTareaViewModel());
                }
                return (RedirectToRoute(new { Controller = "Home", action = "Index" }));

            }

            [HttpPost]
            [Route("CrearTarea")]
            public IActionResult CrearTarea(CrearTareaViewModel nueva)
            {
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                
                   try
                   {
                        var tarea = new Tarea();
                        tarea.Id = nueva.Id;
                        tarea.Color = nueva.Color;
                        tarea.Descripcion = nueva.Descripcion;
                        tarea.Nombre = nueva.Nombre;
                        tarea.EstadoT = nueva.EstadoT;
                        var usu = _repositorioUsuario.ObtenerUsuarioPorNombre(nueva.NombreUsuarioAsignado);
                        tarea.IdUsuarioAsignado = usu.Id;
                        var tab = _repositorioTablero.ObtenerTableroPorNombre(nueva.NombreTablero);
                        _repositorioTarea.CrearTareaEnTablero(tab.Id,tarea);
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

           

            private List<IndexTareaViewModel> ListarTareasIndex(){
                var listaTareasIndex = new List<IndexTareaViewModel>();
                var listaTareas = _repositorioTarea.ListarTareas();
                foreach (var item in listaTareas)
                {
                    Tablero? tab = _repositorioTablero.ObtenerTableroPorId(item.IdTablero);
                    Usuario? usu = _repositorioUsuario.ObtenerUsuarioPorId(item.IdUsuarioAsignado);
                    string? nombreUsuario;
                    string? nombreTablero;
                    if (usu != null)
                    {
                        nombreUsuario = usu.NombreDeUsuario;
                    }
                    else
                    {
                        nombreUsuario = null;
                    }
                    if (tab != null)
                    {
                        nombreTablero = tab.Nombre;
                    }
                    else
                    {
                        nombreTablero = null;
                    }
                    var tarvm = new IndexTareaViewModel(item, nombreUsuario, nombreTablero);
                    listaTareasIndex.Add(tarvm);
                }
                return listaTareasIndex;
            }

        }
    }
