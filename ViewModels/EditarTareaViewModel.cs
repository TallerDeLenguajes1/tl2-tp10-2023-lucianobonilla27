using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class EditarTareaViewModel
    {
        public int Id {get;set;}
        [Required (ErrorMessage ="Este campo es necesario")]
        [Display (Name="Nombre Tarea")]
        public string Nombre{get;set;}
        public int? IdTablero {get; set;}
        [Required (ErrorMessage ="Este campo es necesario")]
        [Display (Name="Nombre Tablero")]
        public string NombreTablero {get; set;}
        [Required (ErrorMessage ="Este campo es necesario")]
        [Display (Name="Estado Tarea")]
        public Tarea.Estado EstadoT {get; set;}
        [Required (ErrorMessage ="Este campo es necesario")]
        [Display (Name="Descripcion Tarea")]
        public string Descripcion {get; set;}
        [Display (Name="Color Tarea")]
        public string? Color {get; set;}
        [Display (Name="Nombre Usuario asignado")]
        public string? NombreUsuarioAsignado {get; set;}
        public int? IdUsuarioAsignado {get; set;}


        public EditarTareaViewModel()
        {
            this.Id=0;
            this.IdTablero=null;
            this.NombreTablero="";
            this.NombreUsuarioAsignado = "";
            this.EstadoT=0;
            this.Descripcion="";
            this.Color="";
            this.IdUsuarioAsignado=null;
        }

        
    }
}