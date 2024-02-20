using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;
using tl2_tp09_2023_lucianobonilla27.Models;
using tl2_tp10_2023_lucianobonilla27.Models;
using tl2_tp10_2023_lucianobonilla27.Repository;

namespace tl2_tp10_2023_lucianobonilla27.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<LoginRepository> _logger;

    private readonly ILoginRepository _repositorioLogin;
    public HomeController(ILogger<LoginRepository> logger,ILoginRepository repositorioLogin)
    {
       _repositorioLogin=repositorioLogin;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Login(LoginViewModel usuario)
    {
        try
        {
            var usuarioLogeado = _repositorioLogin.ObtenerUsuario(usuario.Nombre,usuario.Contrasenia);
            //Registro el usuario
            LogearUsuario(usuarioLogeado);
            _logger.LogInformation("Usuario " + usuarioLogeado.NombreDeUsuario + " Ingresó correctamente");
            return RedirectToRoute(new { controller = "Usuario", action = "Index" });
        }
        catch (Exception e)
        {
             _logger.LogError(e.ToString());
            _logger.LogWarning("Usuario invalido - Nombre de usuario:" + usuario.Nombre + "/Contraseña:" + usuario.Contrasenia);
        }
        // si el usuario no existe devuelvo al index
            return RedirectToAction("Index");
         
    }

     private void LogearUsuario(Usuario user)
    {
        HttpContext.Session.SetInt32("Id", user.Id);
        HttpContext.Session.SetString("Usuario", user.NombreDeUsuario);
        HttpContext.Session.SetString("Contrasenia", user.Contrasenia);
        HttpContext.Session.SetString("NivelAcceso", user.RolUsuario.ToString());
    }
    
    public IActionResult CerrarSesion()
{
    HttpContext.Session.Clear(); // Limpiar todas las variables de sesión
    return RedirectToRoute(new { controller = "Home", action = "Index" });
}
}
