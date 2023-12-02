using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;
using tl2_tp09_2023_lucianobonilla27.Models;
using tl2_tp10_2023_lucianobonilla27.Models;
using tl2_tp10_2023_lucianobonilla27.Repository;

namespace tl2_tp10_2023_lucianobonilla27.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //private LoginRepository _repositorioLogin;
    private readonly ILoginRepository _repositorioLogin;
    public HomeController(ILoginRepository repositorioLogin)
    {
       _repositorioLogin=repositorioLogin;
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
        var usuarioLogeado = _repositorioLogin.ObtenerUsuario(usuario.Nombre,usuario.Contrasenia);
        
        // si el usuario no existe devuelvo al index
        if (usuarioLogeado == null) return RedirectToAction("Index");

         //Registro el usuario
        LogearUsuario(usuarioLogeado);
        
        //Devuelvo el usuario al Home
        return RedirectToRoute(new { controller = "Usuario", action = "Index" });
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
        return RedirectToRoute(new { controller = "Home", action = "Index" });
        
    }
}
