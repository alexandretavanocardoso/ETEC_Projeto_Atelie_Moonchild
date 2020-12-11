using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtelieMoonchild.Models
{
    [Table("Produto")]
    public class Produto
    {
        [Key]
        public int Id { get; set; }
		
		
        [Display(Name = "Nome da Imagem", Prompt = "Nome")]
        [Required(ErrorMessage = "Por favor, informe o Nome do Produto")]
        [StringLength(70, ErrorMessage = "O Nome deve possuir no máximo 70 caracteres")]
        public string Nome { get; set; }

        [Display(Name = "Imagem", Prompt = "Imagem")]
        [StringLength(300)]
        public string Image { get; set; }


        [Display(Name = "Categoria da imagem", Prompt = "Categoria")]
        [Required(ErrorMessage = "Por favor, informe a Categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } // Foreign Key
    }
}
