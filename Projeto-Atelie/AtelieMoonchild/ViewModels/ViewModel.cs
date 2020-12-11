using AtelieMoonchild.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtelieMoonchild.ViewModels
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public List<ProdutoViewModel> Produtos { get; set; }
    }

    public class ProdutoViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Image { get; set; }

        public int Delay { get; set; }
    }

    public class HomeViewModel
    {
        public List<CategoriaViewModel> Categorias { get; set; }
        public Contato Contato { get; set; }
    }


}
