using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tl2_tp09_2023_lucianobonilla27.Models
{
    public class Usuario
    {
        public enum Rol
        {
            administrador,
            operador
        }
        public int Id{get;set;}
        public string NombreDeUsuario{get;set;}
        public string Contrasenia{get;set;}
        public Rol RolUsuario{get;set;}

        public Usuario(string nombre,string contrasenia,Rol rol){
            this.Id = 0;
            this.NombreDeUsuario = nombre;
            this.Contrasenia = contrasenia;
            this.RolUsuario = rol;
        }

        public Usuario(){
            this.Id = 0;
            this.Contrasenia = "";
            this.NombreDeUsuario = "";
            this.RolUsuario = 0;
        }
    }
}