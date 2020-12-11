using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AtelieMoonchild.Data;
using AtelieMoonchild.Models;
using X.PagedList;
using AtelieMoonchild.Services;
using AtelieMoonchild.ViewModels;

namespace AtelieMoonchild.Controllers
{
    public class ContatosController : Controller
    {
        private readonly AtelieMoonchildContext _context;

        public ContatosController(AtelieMoonchildContext context)
        {
            _context = context;
        }

        // GET: Contatos
        public async Task<IActionResult> Index(string filtro, string pesquisa, int? pagina)
        {
            /* Pesquisa*/
            if (pesquisa != null)
            {
                pagina = 1;
            }
            else
            {
                pesquisa = filtro;
            }
            ViewData["Filtro"] = pesquisa;

            var contatos = from c in _context.Contatos.OrderByDescending(p => p.DataContato) select c;

            if (!String.IsNullOrEmpty(pesquisa))
            {
                contatos = contatos.Where(p => p.Nome.Contains(pesquisa)).AsNoTracking();
            }

            int itensPorPagina = 3;
            return View(await contatos.ToPagedListAsync(pagina, itensPorPagina));
        }

        // GET: Contatos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contatos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // GET: Contatos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contatos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Assunto,Mensagem,Retorno,DataContato,DataRetorno")] Contato contato)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contato);
        }

        // GET: Contatos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return NotFound();
            }
            TempData["Retorno"] = contato.Retorno;
            return View(contato);
        }

        // POST: Contatos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Assunto,Mensagem,Retorno,DataContato,DataRetorno")] Contato contato)
        {
            if (id != contato.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var Enviar = ((TempData["Retorno"] == null) && (!string.IsNullOrEmpty(contato.Retorno)));

                if ((!Enviar) && (TempData["Retorno"] != null) )
                {
                    Enviar = TempData["Retorno"].ToString() != contato.Retorno;
                }

                if (Enviar)
                {
                    var email = new EmailSender();
                    string Retorno = string.Format("<img width='300' height='200' src='https://scontent-gru1-1.xx.fbcdn.net/v/t1.0-9/79137266_160356852014436_1308915322387955712_n.jpg?_nc_cat=106&ccb=2&_nc_sid=e3f864&_nc_ohc=4J4qpxDbbKkAX_yYJLc&_nc_ht=scontent-gru1-1.xx&oh=3e8720fa5e78ed93ddbe382a9a7d4bcb&oe=5FE19C25' /><br> " +
                        "<p style='font-weight:800; font-size:18px; letter-spacing: 2px;'>Olá, somos o Atêlie Moonchild </p> " +
                        "<p style='letter-spacing: 2px; font-weight:600; style='color:#000;'>{0}</p> " +
                        "<p style='letter-spacing: 2px; font-weight:600; style='color:#000;'>Obrigado pelo contato.<br> Atenciosamente, Equipe Atêlie Moonchild.</p> " +
                        "<p style='color:#000; font-weight:600; letter-spacing: 2px;'>Nosso e-mail: ateliemoonchild@outlook.com</p>", contato.Retorno);
                    await email.Mail(contato.Email, "ateliemoonchild@outlook.com", "Contato Atêlie", Retorno);
                    // Gravar o contato no banco
                    contato.DataRetorno = DateTime.Now;
                }

                try
                {
                    _context.Update(contato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContatoExists(contato.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(contato);
        }

        // GET: Contatos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contatos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // POST: Contatos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContatoExists(int id)
        {
            return _context.Contatos.Any(e => e.Id == id);
        }
    }
}
