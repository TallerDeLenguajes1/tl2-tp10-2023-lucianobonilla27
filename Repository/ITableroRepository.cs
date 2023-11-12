using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp09_2023_lucianobonilla27.Repository
{
    public interface ITableroRepository
    {
        void CrearTablero(Tablero t);
        void ModificarTablero(int id, Tablero tableroModificado);
        List<Tablero> ListarTableros();
        Tablero ObtenerTableroPorId(int id);
        void EliminarTablero(int id);
        List<Tablero> ListarTableroPorUsuario(int id);
    }
}