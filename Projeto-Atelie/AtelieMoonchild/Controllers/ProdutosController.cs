using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AtelieMoonchild.Data;
using AtelieMoonchild.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using X.PagedList;

namespace AtelieMoonchild.Controllers
{
    [Authorize(Roles = "Administrador")] // Obrigatorio para nao entrar no admin pela URL
    public class ProdutosController : Controller
    {
        private readonly AtelieMoonchildContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment; //Image

        public ProdutosController(AtelieMoonchildContext context, IWebHostEnvironment WebHost)
        {
            _context = context;
            WebHostEnvironment = WebHost;//Image
        }

        // GET: Produtos
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

            var produtos = from p in _context.Produtos.Include(p => p.Categoria) select p;

            if (!String.IsNullOrEmpty(pesquisa))
            {
                produtos = produtos.Where(p => p.Nome.Contains(pesquisa)).AsNoTracking();
            }
            int itensPorPagina = 6;
            ViewData["CaminhoImagem"] = WebHostEnvironment.WebRootPath;
            return View(await produtos.ToPagedListAsync(pagina, itensPorPagina));
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id, IFormFile Image)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            //Caminho da imagem
            ViewData["CaminhoImagem"] = WebHostEnvironment.WebRootPath;
            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Image,CategoriaId")] Produto produto, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)//Imagen
                {
                    // Definir pasta onde vai ser salvo
                    string pasta = Path.Combine(WebHostEnvironment.WebRootPath, "img\\Imagens");

                    //Nome unico
                    var NomeArquivo = Guid.NewGuid().ToString() + "_" + Image.FileName; // nome da imagem e extensão

                    //Caminho Arquivo
                    var CaminhoArquivo = Path.Combine(pasta, NomeArquivo);

                    //Biblioteca - Criar e salvar aqreuivos em HD
                    using (var stream = new FileStream(CaminhoArquivo, FileMode.Create)) // Cria o Arquivo e copia a imagem que chegou do form
                    {
                        await Image.CopyToAsync(stream);
                    }
                    // Localizaçao e nome imagem
                    produto.Image = "/img/Imagens/" + NomeArquivo;

                }
                // Fim Imagem no Create
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            //Caminho da imagem
            ViewData["CaminhoImagem"] = WebHostEnvironment.WebRootPath;
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Image,CategoriaId")] Produto produto, IFormFile NovaImage)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Nova imagem
                    if (NovaImage != null)
                    {
                        // Definir pasta onde vai ser salvo
                        string pasta = Path.Combine(WebHostEnvironment.WebRootPath, "img\\Imagens");

                        //Nome unico
                        var NomeArquivo = Guid.NewGuid().ToString() + "_" + NovaImage.FileName; // nome da imagem e extensão

                        //Caminho Arquivo
                        var CaminhoArquivo = Path.Combine(pasta, NomeArquivo);

                        //Biblioteca - Criar e salvar aqreuivos em HD
                        using (var stream = new FileStream(CaminhoArquivo, FileMode.Create)) // Cria o Arquivo e copia a imagem que chegou do form
                        {
                            await NovaImage.CopyToAsync(stream);
                        }
                        // Localizaçao e nome imagem
                        produto.Image = "/img/Imagens/" + NomeArquivo;
                    }
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
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
            //Caminho da imagem
            ViewData["CaminhoImagem"] = WebHostEnvironment.WebRootPath;
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id, IFormFile Image)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            //Caminho da imagem
            ViewData["CaminhoImagem"] = WebHostEnvironment.WebRootPath;
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
