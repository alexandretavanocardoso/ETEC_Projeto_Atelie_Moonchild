using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtelieMoonchild.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtelieMoonchild.Controllers
{
    [Authorize(Roles = "Administrador")] // Obrigatorio para nao entrar no admin pela URL
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
