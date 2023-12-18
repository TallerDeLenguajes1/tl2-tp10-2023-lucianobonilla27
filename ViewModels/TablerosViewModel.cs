using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tl2_tp10_2023_lucianobonilla27.ViewModels
{
    public class TablerosViewModel
    {
        public List<IndexTableroViewModel> Tableros { get; set; }
        public int usuarioSesion { get; set; }
    }
}