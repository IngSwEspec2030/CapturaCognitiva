
using CapturaCognitiva.App_Tools;
using CapturaCognitiva.Data;
using CapturaCognitiva.Data.Entities;
using CapturaCognitiva.Models.AccountViewModels;
using CapturaCognitiva.Models.Response;
using CapturaCognitiva.Models.ViewModelsApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CapturaCognitiva.Controllers
{
    public class AccountsController : CapturaCognitivaController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModels> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AccountsController(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginViewModels> logger,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _db = context;
            _env = env;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModels { Email = "", Password = "", RememberMe = false });
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModels model)
        {

            if (ModelState.IsValid)
            {
                var user = _db.ApplicationUsers.FirstOrDefault(c => c.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Informacion invalida.");
                    return View(model);
                }
                if (user.IsRemoved)
                {
                    ModelState.AddModelError(string.Empty, "Intento invalido");
                    return View(model);
                }
                if (!user.IsEnabled)
                {
                    ModelState.AddModelError(string.Empty, "Cuenta bloqueada, comuniquese con el administrador");
                    return View(model);
                }
                var roles = SessionData.GetNameRole(_db, user.Id);
                if (roles != "Administrador" && roles != "OperadorDigitalizador")
                {
                    ModelState.AddModelError(string.Empty, "Acceso invalido, comuniquese con el adminsitrador");
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Intento invalido");
                    return View(model);
                }
            }
            return View();
        }
        [Authorize(Roles = "Administrador,OperadorDigitalizador")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotViewModels { Email = "" });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotViewModels model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Informacion invalida");
                return View(model);
            }
            else
            {
                var userForgot = _db.ApplicationUsers.FirstOrDefault(c => c.Email == model.Email);
                if (userForgot == null)
                {
                    ModelState.AddModelError("", "Informacion invalida.");
                }
                var codigoForgotPassword = _db.CodigoForgotPasswords.FirstOrDefault(c => c.ApplicationUser.Email == model.Email && !c.IsUsed);
                if (codigoForgotPassword == null)
                {
                    CodigoForgotPassword codigoUserForgot = new CodigoForgotPassword
                    {
                        ApplicationUserId = userForgot.Id,
                        Code = Encryptor.GeneratePassword(),
                        FechaCreacion = DateTime.Now,
                        FechaUso = null,
                        IsUsed = false
                    };
                    _db.Add(codigoUserForgot);
                    _db.SaveChanges();
                    EmailHelper emailHelper = new EmailHelper(_env);
                    var apiKey = _db.Roles.FirstOrDefault(c => c.Id == "2").Name;
                    if (await emailHelper.SendPasswordRecovery(userForgot.Nombres, userForgot.Email, codigoUserForgot.Code, apiKey))
                    {
                        ModelState.AddModelError("", "Revise su correo por favor");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Reintente nuevamente");
                        return View(model);
                    }
                }
                else
                {
                    EmailHelper emailHelper = new EmailHelper(_env);
                    var apiKey = _db.Roles.FirstOrDefault(c => c.Id == "2").Name;
                    if (await emailHelper.SendPasswordRecovery(userForgot.Nombres, userForgot.Email, codigoForgotPassword.Code,apiKey))
                    {
                        ModelState.AddModelError("", "Revise su correo por favor");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Reintente nuevamente");
                        return View(model);
                    }
                }
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Usuarios(string message = "", int error = 0)
        {
            try
            {
                var users = _db.ApplicationUsers.Where(c => !c.IsRemoved).ToList();
                List<UsersViewModels> Users = new List<UsersViewModels>();

                foreach (var item in users)
                {
                    var roles = _db.UserRoles.FirstOrDefault(c => c.UserId == item.Id);
                    if (roles != null)
                    {
                        string rolename = _db.Roles.FirstOrDefault(c => c.Id == roles.RoleId).Name;
                        Users.Add(new UsersViewModels
                        {
                            Rol = rolename,
                            Cedula = item.Cedula,
                            Email = item.Email,
                            Nombre = item.Nombres,
                            Id = item.Id,
                            IsEnable = item.IsEnabled
                        });
                    }
                }
                if (error == 1)
                {
                    Message(message, MessageType.Danger);
                    return View(Users);
                }
                if (error == -1)
                {
                    Message(message, MessageType.Success);
                    return View(Users);
                }
                if (Users.Count > 0)
                {
                    Message("Se encontraron registros", MessageType.Success);
                }
                else
                {
                    Message("No se encontraron registros", MessageType.Warning);
                }
                return View(Users);
            }
            catch
            {
                throw;
            }

        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Register()
        {
            try
            {
                var roles = _db.Roles.ToList();
                ViewBag.RoleTypeList = new SelectList(roles, "Id", "Name");
                return View(new RegisterViewModel { });
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                if (!ModelState.IsValid)
                {
                    var roles = _db.Roles.ToList();
                    ViewBag.RoleTypeList = new SelectList(roles, "Id", "Name", model.Rol);
                    return View(model);
                }
                else
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        EmailConfirmed = false,
                        IsEnabled = true,
                        DateEnabled = DateTime.Now,
                        PhoneNumber = "",
                        LockoutEnabled = false,
                        Attemps = 0,
                        Nombres = model.Nombres,
                        DateCreated = DateTime.Now,
                        Cedula = model.Cedula
                    };
                    var result = await _userManager.CreateAsync(user, model.Cedula.ToString());

                    if (result.Succeeded)
                    {
                        var roles = _db.Roles.FirstOrDefault(c => c.Id == model.Rol);
                        if (roles == null)
                        {
                            return RedirectToAction("Usuarios", new { message = $"Ocurrio un error comuniquese con el administrador", error = 1 });
                        }
                        var roleAdd = await _userManager.AddToRoleAsync(user, roles.Name);
                        scope.Complete();
                        return RedirectToAction("Usuarios", new { message = $"Registrado correctamente: {model.Nombres}", error = -1 });
                    }
                    else
                    {
                        scope.Dispose();
                        return RedirectToAction("Usuarios", new { message = "Ocurrio un error comuniquese con el administrador", error = 1 });
                    }
                }
            }
            catch
            {
                scope.Dispose();
                return RedirectToAction("Usuarios", new { message = "Ocurrio un error comuniquese con el administrador", error = 1 });
                throw;
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(string id)
        {
            try
            {
                var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
                if (user == null)
                {
                    return RedirectToAction("Usuarios", new { message = $"No existe el usuario", error = -1 });
                }
                else
                {
                    var userRole = _db.UserRoles.FirstOrDefault(c => c.UserId == id);
                    if (userRole == null)
                    {
                        return RedirectToAction("Usuarios", new { message = $"Ocurrio un error comuniquese con el administrador", error = -1 });
                    }
                    var rolesUser = _db.Roles.FirstOrDefault(c => c.Id == userRole.RoleId);
                    if (rolesUser == null)
                    {
                        return RedirectToAction("Usuarios", new { message = $"Ocurrio un error comuniquese con el administrador", error = -1 });
                    }
                    var roles = _db.Roles.ToList();
                    ViewBag.RoleTypeList = new SelectList(roles, "Id", "Name", rolesUser.Id);

                    return View(new EditViewModels
                    {
                        Cedula = user.Cedula,
                        Id = user.Id,
                        Nombres = user.Nombres
                    });
                }
            }
            catch
            {
                return RedirectToAction("Usuarios", new { message = $"Ocurrio un error comuniquese con el administrador", error = -1 });
                throw;

            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(EditViewModels model)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                if (!ModelState.IsValid)
                {
                    var roles = _db.Roles.ToList();
                    ViewBag.RoleTypeList = new SelectList(roles, "Id", "Name", model.Rol);
                    return View(model);
                }
                else
                {
                    var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == model.Id);
                    if (user == null)
                    {
                        return RedirectToAction("Usuarios", new { message = $"El usuario no existe", error = 1 });
                    }
                    else
                    {

                        var roles = _db.Roles.FirstOrDefault(c => c.Id == model.Rol);
                        if (roles == null)
                        {
                            return RedirectToAction("Usuarios", new { message = $"Ocurrio un error comuniquese con el administrador", error = 1 });
                        }
                        var userRoles = _db.UserRoles.FirstOrDefault(c => c.UserId == model.Id);
                        if (userRoles == null)
                        {
                            return RedirectToAction("Usuarios", new { message = $"El Rol no existe", error = 1 });
                        }
                        user.Cedula = model.Cedula;
                        user.Nombres = model.Nombres;
                        userRoles.RoleId = roles.Id;
                        _db.Entry(user).State = EntityState.Modified;
                        _db.Entry(userRoles).State = EntityState.Modified;
                        _db.SaveChanges();
                        scope.Complete();
                        return RedirectToAction("Usuarios", new { message = $"Usuario modificado correctamente {user.Nombres}", error = -1 });
                    }

                }
            }
            catch
            {
                scope.Dispose();
                return RedirectToAction("Usuarios", new { message = "Ocurrio un error comuniquese con el administrador", error = 1 });
                throw;
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Activate(string id)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
                if (user == null)
                {
                    return RedirectToAction("Usuarios", new { message = $"El usuario no existe", error = 1 });
                }
                else
                {
                    if (user.IsEnabled)
                    {
                        user.IsEnabled = false;
                    }
                    else
                    {
                        user.IsEnabled = true;
                    }

                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("Usuarios", new { message = $"Usuario modificado correctamente {user.Nombres}", error = -1 });
                }

            }
            catch
            {
                scope.Dispose();
                return RedirectToAction("Usuarios", new { message = "Ocurrio un error comuniquese con el administrador", error = 1 });
                throw;
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(string id)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
                if (user == null)
                {
                    return RedirectToAction("Usuarios", new { message = $"El usuario no existe", error = 1 });
                }
                else
                {
                    if (user.IsRemoved)
                    {
                        return RedirectToAction("Usuarios", new { message = $"Este usario ya fue eliminado {user.Nombres}", error = 1 });
                    }
                    else
                    {
                        user.IsRemoved = true;
                        _db.Entry(user).State = EntityState.Modified;
                        _db.SaveChanges();
                        scope.Complete();
                        return RedirectToAction("Usuarios", new { message = $"Usuario modificado correctamente {user.Nombres}", error = -1 });
                    }
                }
            }
            catch
            {
                scope.Dispose();
                return RedirectToAction("Usuarios", new { message = "Ocurrio un error comuniquese con el administrador", error = 1 });
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult RecoveryPassword(string code)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                var codeVerify = _db.CodigoForgotPasswords.FirstOrDefault(c => c.Code == code);
                if (codeVerify != null)
                {
                    if (codeVerify.IsUsed)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        return View(new RecoveryPasswordViewModels { Id = codeVerify.Id });
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch
            {
                scope.Dispose();
                return RedirectToAction("Usuarios", new { message = "Ocurrio un error comuniquese con el administrador", error = 1 });
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RecoveryPasswordAsync(RecoveryPasswordViewModels model)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                else
                {
                    var codeVerify = _db.CodigoForgotPasswords.FirstOrDefault(c => c.Id == model.Id);
                    if (codeVerify != null)
                    {
                        var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == codeVerify.ApplicationUserId);
                        if (user == null)
                        {
                            ModelState.AddModelError("", "Informacion invalida");
                            return View(model);
                        }
                        else
                        {
                            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                            var res = await _userManager.UpdateAsync(user);
                            if (res.Succeeded)
                            {
                                codeVerify.IsUsed = true;
                                codeVerify.FechaUso = DateTime.Now;
                                _db.Entry(codeVerify).State = EntityState.Modified;
                                _db.Entry(codeVerify).State = EntityState.Modified;
                                _db.SaveChanges();
                                scope.Complete();
                                return RedirectToAction("Login");
                            }
                            else
                            {
                                scope.Dispose();
                                ModelState.AddModelError("", "Informacion invalida");
                                return View(model);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Informacion invalida");
                        return View(model);
                    }
                }
            }
            catch
            {
                scope.Dispose();
                return RedirectToAction("Usuarios", new { message = "Ocurrio un error comuniquese con el administrador", error = 1 });
                throw;
            }
        }
    }
}
