using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp09_2023_lucianobonilla27.Models;
using System.ComponentModel.DataAnnotations;


namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class CambiarEstadoTareaViewModel
    {
        [Required (ErrorMessage ="Este campo es necesario")]
        [Display (Name="Estado Tarea")]
        public Tarea.Estado EstadoT{get;set;}
        public int Id{get;set;}
    }
}