using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tl2_tp09_2023_lucianobonilla27.Models
{
    public class Tablero
    {
        public int Id{get;set;}
        public int IdUsuarioPropietario{get;set;}
        public string Nombre{get;set;}
        public string? Descripcion{get;set;}

        // public Tablero(int id,int idUsuario,string nombre,string descripcion){
        //     this.Id = id;
        //     this.IdUsuarioPropietario = idUsuario;
        //     this.Nombre = nombre;
        //     this.Descripcion = descripcion;

        // }

        // public Tablero(int id,int idUsuario,string nombre){
        //     this.Id = id;
        //     this.IdUsuarioPropietario = idUsuario;
        //     this.Nombre = nombre;
        // }

    }
}