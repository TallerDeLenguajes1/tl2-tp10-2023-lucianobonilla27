using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class TareasViewModel
    {
        public List<IndexTareaViewModel> Tareas {get;set;}
        public List<Tablero> TablerosDelUsuarioSesion {get;set;}
        public int UsuarioSesion { get; set; }
        public string NivelAcceso{ get; set; }
        public int IdUsuarioPropietario { get; set; }


    }
}