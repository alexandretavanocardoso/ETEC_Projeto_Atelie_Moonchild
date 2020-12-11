using System.ComponentModel.DataAnnotations;

namespace AtelieMoonchild.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "E-mail de Acesso", Prompt = "E-mail de acesso")]
        [Required(ErrorMessage = "Por favor, informe o e-mail de acesso")]
        [EmailAddress(ErrorMessage = "Por favor, informe o e-mail de acesso")] // Tipo email    
        public string Email { get; set; }

        [Display(Name = "Senha de Acesso", Prompt = "Senha de acesso")]
        [Required(ErrorMessage = "Por favor, informe o Senha de acesso")]
        [DataType(DataType.Password)] // Tipo senha    
        public string Senha { get; set; }

        [Display(Name = "Continuar conectado?")]
        public bool Manter { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Display(Name = "E-mail de Acesso", Prompt = "E-mail de Acesso")]
        [Required(ErrorMessage = "Por favor, Informe o E-mail de Acesso")]
        [EmailAddress(ErrorMessage = "Por favor, Informe um E-mail Válido!!")]
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        [Display(Name = "Informe a Nova Senha", Prompt = "Informe a Nova Senha")]
        [Required(ErrorMessage = "Por favor, Informe a Nova Senha de Acesso")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Confirmação de Senha", Prompt = "Confirmação de Senha")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "Nova senha e Confirmação não conferem!")]
        public string ConfirmarSenha { get; set; }


        public string Email { get; set; }

        public string Token { get; set; }
    }
}
