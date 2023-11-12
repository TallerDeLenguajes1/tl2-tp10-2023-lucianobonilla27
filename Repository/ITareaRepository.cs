using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp09_2023_lucianobonilla27.Repository
{
    public interface ITareaRepository
    {
        Tarea CrearTareaEnTablero(int idTablero,Tarea t);
        void ModificarNombreTarea(int id,string nombreNuevo);
        void ModificarEstadoTarea(int id,int estadoNuevo);
        Tarea ObtenerTareaPorId(int id);
        List<Tarea> ListarPorUsuario(int idUsuario);
        void EliminarTarea(int id);
        void AsignarTareaAUsuario(int idTarea,int idUsuario);

        List<Tarea> ListarPorTablero(int id);
    }
}