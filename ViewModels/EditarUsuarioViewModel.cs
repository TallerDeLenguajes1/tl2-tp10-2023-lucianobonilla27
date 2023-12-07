using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class EditarUsuarioViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Usuario")] 
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre de Usuario")] 
        public string NombreDeUsuario { get; set; }
        

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")] 
        public string? Contrasenia { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Rol usuario")] 
        public Usuario.Rol RolUsuario { get; set; }

        public EditarUsuarioViewModel(int id,string nombre,string contrasenia,Usuario.Rol rol){
            this.Id = id;
            this.NombreDeUsuario = nombre;
            this.Contrasenia = contrasenia;
            this.RolUsuario = rol;
        }

        public EditarUsuarioViewModel(){
            this.Id = 0;
            this.NombreDeUsuario = "";
            this.Contrasenia = "";
            this.RolUsuario = 0;
        }
    }
}