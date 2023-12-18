using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using tl2_tp09_2023_lucianobonilla27.Models;


namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class EditarTableroViewModel
    {
      public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    public string Nombre { get; set; }

    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name = "Propietario")]
    public int Id_Usuario_Propietario { get; set; }


    
   
    

    public EditarTableroViewModel(Tablero t){
        this.Id = t.Id;
        this.Nombre = t.Nombre;
        this.Descripcion = t.Descripcion;
        this.Id_Usuario_Propietario = t.IdUsuarioPropietario;
   
    }
     public EditarTableroViewModel()
    {
        Id=0;
        Nombre="";
        Descripcion="";
        Id_Usuario_Propietario=0;
    
    }
    }
}