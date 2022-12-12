using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.DAL;
using Pustok.Models;
using Pustok.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers
{
    public class AccountController:Controller
    {
        private readonly PustokDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(PustokDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
           _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterVm memberVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            if (await _userManager.FindByNameAsync(memberVm.Username) != null  )
            {
                ModelState.AddModelError("Username", "User already exists");
                return RedirectToAction("Login");
            }
            if(await _userManager.FindByEmailAsync(memberVm.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
            }


            AppUser appUser = new AppUser
            {
                Email = memberVm.Email,
                Fullname = memberVm.Fullname,
                UserName = memberVm.Username
            };

           var result =  await _userManager.CreateAsync(appUser,memberVm.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }   

           


            return RedirectToAction("Login","Account");
        }
    }
}
