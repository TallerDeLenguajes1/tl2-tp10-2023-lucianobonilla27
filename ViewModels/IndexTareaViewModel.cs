using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class IndexTareaViewModel
    {
         public int Id{get;set;}
        public int IdTablero{get;set;}
        public string NombreTablero{get;set;}
        public string Nombre{get;set;}

        public Tarea.Estado EstadoT{get;set;}
        public string? Descripcion{get;set;}
        public string? Color{get;set;}
        public int? IdUsuarioAsignado{get;set;}
        public string? NombreUsuarioAsignado{get;set;}

         public IndexTareaViewModel()
         {
        Id=0;
        this.IdTablero=0;
        this.Nombre="";
        this.EstadoT=0;
        this.Descripcion="";
        this.Color="";
        this.IdUsuarioAsignado=0;
        this.NombreUsuarioAsignado =null;
        this.NombreTablero=null;
        }

        public IndexTareaViewModel(Tarea tar, string? nombreUsu, string? nombreTab)
        {
            this.Id=tar.Id;
            this.IdTablero=tar.IdTablero;
            this.Nombre=tar.Nombre;
            this.EstadoT=tar.EstadoT;
            this.Descripcion=tar.Descripcion;
            this.Color=tar.Color;
            this.IdUsuarioAsignado=tar.IdUsuarioAsignado;
            this.NombreUsuarioAsignado=nombreUsu;
            this.NombreTablero=nombreTab;
        }
    }
}