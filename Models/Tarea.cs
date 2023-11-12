using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tl2_tp09_2023_lucianobonilla27.Models
{
    public class Tarea
    {
        public enum Estado 
        {
            ToDo,
            Doing,
            Review,
            Done

        }
        public int Id{get;set;}
        public int IdTablero{get;set;}
        public string Nombre{get;set;}

        public Estado EstadoT{get;set;}
        public string? Descripcion{get;set;}
        public string? Color{get;set;}
        public int? IdUsuarioAsignado{get;set;}

    // public Tarea(int id, int idTablero, string nombre, Estado estado, string? descripcion, string? color, int idUsuarioAsignado)
    // {
    //     Id = id;
    //     IdTablero = idTablero;
    //     Nombre = nombre;
    //     EstadoT = estado;
    //     Descripcion = descripcion;
    //     Color = color;
    //     IdUsuarioAsignado = idUsuarioAsignado;
    // }
        
    }
}