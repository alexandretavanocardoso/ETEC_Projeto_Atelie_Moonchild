using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtelieMoonchild.Models;
using AtelieMoonchild.Services;
using AtelieMoonchild.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AtelieMoonchild.Controllers
{
    public class AccountController : Controller
    {
        // Criar os gerenciadores, Login, usuario, perfil
        private readonly UserManager<Usuario> userManager; // Usuario
        private readonly SignInManager<Usuario> signInManager; //Login
        private readonly RoleManager<IdentityRole> roleManager; // perfil

        // Injetar dependecias dos metodos acima
        public AccountController(UserManager<Usuario> user, SignInManager<Usuario> sign, RoleManager<IdentityRole> role) //ctor - metodo construtor
        {
            userManager = user;
            signInManager = sign;
            roleManager = role;

        }

        public IActionResult Login()
        {
            return View();
        }

        // Segurança Login
        [HttpPost]
        [ValidateAntiForgeryToken] // Valida as chaves criadas para evitar ataque
        public async Task<IActionResult> Login(LoginViewModel userModel, string returnUrl = null)
        {
            // Obrigatorio
            if (!ModelState.IsValid) // Vereifica se chegou os dados do login da page
            {
                return View(userModel);
            }
            // login    //lockoutOnFailure: true - se errar bloqueia o user com as option do startup                                                                                                        
            var result = await signInManager.PasswordSignInAsync(userModel.Email, userModel.Senha, userModel.Manter, lockoutOnFailure: true); // Pega a senha e o email e bate no banco
            if (result.Succeeded) // Se for um sucesso ele loga
            {
                // retorna o usuario pelo email
                var user = await userManager.FindByEmailAsync(userModel.Email);

                // retornas os perfis dos ususarios
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("Administrador"))
                    return RedirectToAction("Index", "Admin");
                return RedirectToAction(returnUrl);
            }
            if (result.IsLockedOut) // se foi bloqueado
                ModelState.AddModelError("", "Usuario bloqueado, aguarde liberação");
            else // se nao foi bloqueado
                ModelState.AddModelError("", "E-mail de acesso e/ou senha invalido");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() // Sair
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Localizar q o email é de um usuario
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);
            var email = new EmailSender();
            string mensagem = String.Format("Clique <a href='{0}'>AQUI</a> para resetar sua senha:", link);
            await email.Mail(model.Email, "Suporte@etecShop.com", "Recuperaççao de Senha", mensagem);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Localizar q o email é de um usuario
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Usuário Inválido", "E-mail não encontrado !! Entre em contato com o suporte");
                return View(model);
            }

            var reset = await userManager.ResetPasswordAsync(user, model.Token, model.Senha);
            if (!reset.Succeeded)
            {
                foreach (var error in reset.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous] // Anonimo, nao é necessario estar logado
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

        }

    }
}
