using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtelieMoonchild.Models
{
    public class Usuario : IdentityUser
    {
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "Informe seu nome completo")]
        [StringLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Nome { get; set; }

        [Display(Name = "Apelido")]
        [StringLength(20, ErrorMessage = "Maximo 20 caracteres")]
        public string Apelido { get; set; }


        [Display(Name = "Data Nascimento")]
        [DataType(DataType.Date)] // Campo de data
        public DateTime DataNascimento { get; set; }

        [NotMapped] // Essa propriedade nao vai existir no banco
        public string Roles { get; set; } // pode ou nao acessar determinada pagina
    }
}
