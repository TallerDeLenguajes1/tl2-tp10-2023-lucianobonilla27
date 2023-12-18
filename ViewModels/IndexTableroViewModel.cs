using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class IndexTableroViewModel
    {
         public int Id { get; set; }
        [Display(Name = "Nombre de Tablero")]
        public string Nombre { get; set; }
        [Display(Name = "Nombre de Usuario")]
        public string Descripcion { get; set; }
        public int? Id_Usuario_Propietario { get; set; }
        public string? NombreUsuario {get; set;}



        public IndexTableroViewModel ()
        {
            Id=0;
            Nombre="";
            Descripcion="";
            Id_Usuario_Propietario=null;
            NombreUsuario=null;
        }

        public IndexTableroViewModel(Tablero tablero, string? Nombre)
        {
            this.Id=tablero.Id;
            this.Nombre=tablero.Nombre;
            this.Descripcion=tablero.Descripcion;
            this.Id_Usuario_Propietario=tablero.IdUsuarioPropietario;
            this.NombreUsuario=Nombre;
        }
    }
}