﻿using ITServiceApp.Business.Services.Email;
using ITServiceApp.Core.ComplexTypes;
using ITServiceApp.Core.Identity;
using ITServiceApp.Core.ViewModels;
using ITServiceApp.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ITServiceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,RoleManager<ApplicationRole>roleManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            Checkroles();
        }

        private void Checkroles()
        {
            foreach (var rolename in RoleModels.Roles)
            {
                if (!_roleManager.RoleExistsAsync(rolename).Result)
                {
                    var result = _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = rolename
                    }).Result;
                }

            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı daha önce kayıt edilmiştir");
                return View(model);
            }

            user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Bu email adresi daha önce kayıt edilmiştir");
                return View(model);
            }

            user = new ApplicationUser()
            {
                Email = model.Email,
                Name = model.Name,
                UserName = model.UserName,
                Surname = model.Surname
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var count = _userManager.Users.Count();
                result = await _userManager.AddToRoleAsync(user, count == 1 ? RoleModels.Admin : RoleModels.Passive);
                //email onay maili

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    Subject = "Confirm your email"
                };

                await _emailSender.SendAsync(emailMessage);

                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewBag.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            if (result.Succeeded && _userManager.IsInRoleAsync(user, RoleModels.Passive).Result)
            {
                await _userManager.RemoveFromRoleAsync(user, RoleModels.Passive);
                await _userManager.AddToRoleAsync(user, RoleModels.User);
            }

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

               

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Kullanıcı adı veya şifre hatalı");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            var model = new UserProfileViewModel()
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            };
            
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            user.Name = model.Name;
            user.Surname = model.Surname;
            if (user.Email != model.Email) 
            {
                await _userManager.RemoveFromRoleAsync(user, RoleModels.User);
                await _userManager.AddToRoleAsync(user, RoleModels.Passive);
                user.Email = model.Email;

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    Subject = "Confirm your email"
                };

                await _emailSender.SendAsync(emailMessage);
            }

            var result=await _userManager.UpdateAsync(user);
            if (!result.Succeeded) 
            {
                ModelState.AddModelError(string.Empty, ModelState.ToFullErrorString());
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(PasswordChangeViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded) 
            {
                ViewBag.Message = "Parola güncelleme işlemi başarılı";
                return RedirectToAction(nameof(Logout));
            }
            else 
            {
                ViewBag.Message = $"Bir hata oluştu:{ModelState.ToFullErrorString()}";
            }

            return RedirectToAction("Profile");
        }   
        
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user== null)
            {
                ViewBag.Message = "Girdiğiniz email bulunamadı";
            }
            else 
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmResetPassword", "Account",
                    new { userId = user.Id, code = code }, protocol: Request.Scheme);
                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                        $"Please reset your password by <a href='" +
                        $"{HtmlEncoder.Default.Encode(callbackUrl)}'>Click Here</a>", 
                          Subject = "Reset Passord"
                };
                await _emailSender.SendAsync(emailMessage);
                ViewBag.Message = "Mailinize Şifre güncelleme yönergemiz gönderilmiştir. ";
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult ConfirmResetPassword(string userId,string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code)) 
            {
                return BadRequest("Hatalı istek");
            }
            ViewBag.Code = code;
            ViewBag.UserId = userId;

            return View();
        }

        [AllowAnonymous,HttpPost]
        public async Task<IActionResult> ConfirmResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
                return View();
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));

            var result = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);

            if (result.Succeeded)
            {
                //email gönder
                TempData["Message"] = "Şifre değişikliğiniz gerçekleşmiştir.";
                return View();
            }
            else 
            {
                var message = string.Join("<br>", result.Errors.Select(x =>
                   x.Description));
                TempData["Message"] = message;
                return View();
            }
        }
        
    }
}

