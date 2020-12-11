using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AtelieMoonchild.Models;
using AtelieMoonchild.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AtelieMoonchild.ViewModels;
using AtelieMoonchild.Services;

namespace AtelieMoonchild.Controllers
{
    /* [Authorize(Roles = "Administrador")]*/
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Manipula o banco
        private readonly AtelieMoonchildContext _context;

        public HomeController(ILogger<HomeController> logger, AtelieMoonchildContext context)
        {
            _logger = logger;
            /* Ejeta as listas*/
            _context = context;
        }

        // COMEÇO Ação HREF



        public IActionResult Index()
        {
            // Lista de conteudo
            List<CategoriaViewModel> categorias = new List<CategoriaViewModel>();
            var c = _context.Categorias.ToList();
            int delay = 100;
            foreach (var item in c)
            {
                CategoriaViewModel cvm = new CategoriaViewModel();
                cvm.Id = item.Id;
                cvm.Nome = item.Nome;
                cvm.Produtos = new List<ProdutoViewModel>();
                var p = _context.Produtos.Where(p => p.CategoriaId == item.Id).Take(2).ToList();
                foreach (var item2 in p)
                {
                    ProdutoViewModel pvm = new ProdutoViewModel()
                    {
                        Id = item2.Id,
                        Nome = item2.Nome,
                        Image = item2.Image,
                        Delay = delay
                    };
                    delay += 200;
                    cvm.Produtos.Add(pvm);
                }
                categorias.Add(cvm);
            }
            var model = new HomeViewModel();
            model.Categorias = categorias;
            model.Contato = new Contato();
            return View(model);
        }

        public IActionResult Sobre()
        {
            return View();
        }

        public IActionResult Contato()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Contatos(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = new EmailSender();
                string mensagem = string.Format("<img width='300' height='200' src='https://scontent-gru1-1.xx.fbcdn.net/v/t1.0-9/79137266_160356852014436_1308915322387955712_n.jpg?_nc_cat=106&ccb=2&_nc_sid=e3f864&_nc_ohc=4J4qpxDbbKkAX_yYJLc&_nc_ht=scontent-gru1-1.xx&oh=3e8720fa5e78ed93ddbe382a9a7d4bcb&oe=5FE19C25' /><br>" +
                    "<p style='color:#00008B;font-weight:800; font-size:18px; letter-spacing: 2px;'>Contato Atêlie Moonchild</p><br> " +
                    "<b>Nome:</b> {0}<br>" +
                    "<b>E-mail:</b> {1}<br>" +
                    "<b>Assunto</b> {2}<br>" +
                    "<b>Mensagem:</b> {3}", model.Contato.Nome, model.Contato.Email, model.Contato.Assunto, model.Contato.Mensagem);
                await email.Mail("ateliemoonchild@outlook.com", model.Contato.Email, "Contato Atêlie Moonchild", mensagem);
                // Gravar o contato no banco
                var contato = new Contato()
                {
                    Nome = model.Contato.Nome,
                    Email = model.Contato.Email,
                    Assunto = model.Contato.Assunto,
                    Mensagem = model.Contato.Mensagem,
                    DataContato = DateTime.Now
                };
                _context.Add(contato);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Enviado com Sucesso !" });
            }
            return Json(new { success = false, message = "Dados Incompleto !" });
        }

        public IActionResult Galeria()
        {

            // Lista de conteudo
            List<CategoriaViewModel> categorias = new List<CategoriaViewModel>();
            var c = _context.Categorias.ToList();
            int delay = 100;
            foreach (var item in c)
            {
                CategoriaViewModel cvm = new CategoriaViewModel();
                cvm.Id = item.Id;
                cvm.Nome = item.Nome;
                cvm.Produtos = new List<ProdutoViewModel>();
                var p = _context.Produtos.Where(p => p.CategoriaId == item.Id).Take(4).ToList();
                foreach (var item2 in p)
                {
                    ProdutoViewModel pvm = new ProdutoViewModel()
                    {
                        Id = item2.Id,
                        Nome = item2.Nome,
                        Image = item2.Image,
                        Delay = delay
                    };
                    delay += 200;
                    cvm.Produtos.Add(pvm);
                }
                categorias.Add(cvm);
            }
            return View(categorias);
        }

        public IActionResult Categoria(int? id)
        {
            CategoriaViewModel cat = new CategoriaViewModel(); // Cria o objeto
            var categoria = _context.Categorias.Find(id); // pesquisa categoria
            cat.Id = categoria.Id;
            cat.Nome = categoria.Nome;
            cat.Produtos = new List<ProdutoViewModel>(); // Cria lista dos produtos

            var produtos = _context.Produtos.Where(p => p.Categoria.Id == id).ToList();
            int delay = 100;
            foreach (var item in produtos)
            {
                var p = new ProdutoViewModel()
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Image = item.Image,
                    Delay = delay
                };
                delay += 200;
                cat.Produtos.Add(p);
            }

            return View(cat);
        }

        // FINAL Ação HREF

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
