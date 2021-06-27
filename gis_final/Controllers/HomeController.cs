using gis_final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Controllers
{
    public class HomeController : Controller
    {
        private readonly YasharDbContext _context;

        public HomeController(YasharDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }

        // GET: LoginController
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string Email, string Password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == Email && user.Password == Password);
            if (user != null)
            {
                var userRole = await _context.UserRoles.Include(p => p.Role).Where(x => x.UserId == user.Id).ToListAsync();
                if (userRole != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Email", user.Email);
                    if (userRole.Count() > 1)
                    {
                        int previousRolId = 0;
                        foreach (var role in userRole)
                        {
                            if (previousRolId > 0)
                            {
                                if (role.RoleId < previousRolId)
                                {
                                    HttpContext.Session.SetString("Role", role.Role.Title);
                                }
                            }
                            else
                            {
                                HttpContext.Session.SetString("Role", role.Role.Title);
                            }
                            previousRolId = role.RoleId;
                        }
                    }
                    else
                    {
                        foreach (var role in userRole)
                        {
                            HttpContext.Session.SetString("Role", role.Role.Title);
                        }
                    }
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return NotFound();
                }
            }

            ViewBag.msg = "Invalid credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("UserId");

            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
