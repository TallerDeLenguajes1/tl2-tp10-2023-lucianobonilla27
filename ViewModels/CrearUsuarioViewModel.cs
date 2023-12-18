using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using tl2_tp09_2023_lucianobonilla27.Models;


namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class CrearUsuarioViewModel
{
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id Usuario")] 
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre de Usuario")] 
    public string NombreDeUsuario { get; set; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")] 
    public string Contrasenia { get; set; }

    [DataType(DataType.Password)]
    [Compare("Contrasenia", ErrorMessage = "Las contraseñas no coinciden")]
    public string ContraseniaRep { get; set; }

    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Rol usuario")] 
    public Usuario.Rol RolUsuario { get; set; }
}

}