using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtelieMoonchild.Models
{
    [Table("Contatos")]
    public class Contato
    {
        [Key]
        public int Id { get; set; }

        [Display(Prompt = "Informe seu nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter apenas 50 caracteres")]
        public string Nome { get; set; }

        [Display(Prompt = "Informe seu email")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        [StringLength(100, ErrorMessage = "O nome deve ter apenas  100 caracteres")]
        public string Email { get; set; }

        [Display(Prompt = "Informe um assunto")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter apenas  50 caracteres")]
        public string Assunto { get; set; }

        [Display(Prompt = "Informe uma mensagem")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(500, ErrorMessage = "A Mensagem deve ter apenas  500 caracteres")]
        public string Mensagem { get; set; }

        [Display(Prompt = "Informe um retorno")]
        [StringLength(500, ErrorMessage = "O Retorno deve ter apenas  500 caracteres")]
        public string Retorno { get; set; }

        [Display(Name = "Data do Contato")]
        public DateTime DataContato { get; set; }

        [Display(Name = "Data de Retorno")]
        public DateTime? DataRetorno { get; set; }
    }
}
