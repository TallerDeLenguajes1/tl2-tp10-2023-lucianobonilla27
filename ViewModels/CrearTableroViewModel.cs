using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class CrearTableroViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Usuario Propietario")] 
        public int IdUsuarioPropietario{get;set;}
        
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre del Tablero")] 
        public string Nombre{get;set;}

        [Display(Name = "Descripcion")] 
        public string? Descripcion{get;set;}

        
    }
}