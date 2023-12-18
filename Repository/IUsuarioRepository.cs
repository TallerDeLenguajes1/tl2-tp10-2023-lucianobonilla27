using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tl2_tp09_2023_lucianobonilla27.Models
{
   public interface IUsuarioRepository
    {
        void CrearUsuario(Usuario usuario);
        void ModificarUsuario(int id, Usuario usuarioModificado);
        List<Usuario> ListarUsuarios();
        Usuario ObtenerUsuarioPorId(int? id);
       Usuario ObtenerUsuarioPorNombre(string nombre);

        void EliminarUsuario(int id);
    }
}