using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapturaCognitiva.App_Tools;
using CapturaCognitiva.Data;
using CapturaCognitiva.Models.ViewModels;
using CapturaCognitiva.WebServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CapturaCognitiva.Controllers
{
    public class ManageImagesController : CapturaCognitivaController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ManageImagesController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _db = context;
            _env = env;
        }
        [Authorize(Roles = "Administrador,OperadorDigitalizador")]
        public ActionResult Index()
        {
            try
            {
                ViewBag.IdImageAnterior = 0;
                ViewBag.IdImageSiguiente = 0;
                var imageActual = _db.Guides.FirstOrDefault(c => !c.IsCompleted);
                if (imageActual == null)
                {
                    ViewBag.isImagen = false;
                    Message("No tiene imagenes pendientes", MessageType.Info);
                    return View();
                }
                else
                {
                    ViewBag.isImagen = true;
                    var imageSiguiente = _db.Guides.FirstOrDefault(c => !c.IsCompleted && c.Id != imageActual.Id && c.Id > imageActual.Id && !c.IsUpload);
                    if (imageSiguiente != null)
                    {
                        ViewBag.IdImageSiguiente = imageSiguiente.Id;
                        ViewBag.IdImageAnterior = 0;
                    }
                    else
                    {
                        ViewBag.IdImageAnterior = 0;
                        ViewBag.IdImageSiguiente = 0;
                    }
                }
                if (string.IsNullOrEmpty(imageActual.Image.Uuid))
                {
                    ViewBag.isImagen = false;
                    Message("Ocurrio un error con la imagen comuniquese con el adminsitrador", MessageType.Danger);
                    return View();
                }
                else
                {
                    WSImage wSImage = new WSImage();
                    var responseWsImage = wSImage.GetImage(imageActual.Image.Uuid);
                    if (responseWsImage.Success)
                    {
                        return View(new ImageFormViewModels
                        {
                            Id = imageActual.Id,
                            ImageBase64 = responseWsImage.ImageBase64,
                            AddressReceiver = imageActual.Receiver.Address,
                            CellReceiver = imageActual.Receiver.Cell,
                            NameReceiver = imageActual.Receiver.Name,
                            StateReceiver = imageActual.Receiver.State,
                            AddressSender = imageActual.Sender.Address,
                            CellSender = imageActual.Sender.Cell,
                            NameSender = imageActual.Sender.Name,
                            StateSender = imageActual.Sender.State,
                        });
                    }
                    else
                    {
                        ViewBag.isImagen = false;
                        Message("Ocurrio un error con la imagen comuniquese con el adminsitrador", MessageType.Danger);
                        return View();
                    }
                }
            }
            catch
            {
                ViewBag.isImagen = false;
                Message("Ocurrio un error comuniquese con el adminsitrador", MessageType.Danger);
                return View();
                throw;
            }
        }
        [Authorize(Roles = "Administrador,OperadorDigitalizador")]
        [HttpGet]
        public ActionResult Find(int id)
        {
            try
            {
                ViewBag.IdImageAnterior = 0;
                ViewBag.IdImageSiguiente = 0;
                if (id == 0)
                {
                    var imageActual = _db.Guides.FirstOrDefault(c => !c.IsCompleted);
                    if (imageActual == null)
                    {
                        ViewBag.isImagen = false;
                        Message("No tiene imagenes pendientes", MessageType.Info);
                        return View();
                    }
                    else
                    {
                        ViewBag.isImagen = true;
                        var imageSiguiente = _db.Guides.FirstOrDefault(c => !c.IsCompleted && c.Id != imageActual.Id && c.Id > imageActual.Id);
                        if (imageSiguiente != null)
                        {
                            ViewBag.IdImageSiguiente = imageSiguiente.Id;
                            ViewBag.IdImageAnterior = 0;
                        }
                        else
                        {
                            ViewBag.IdImageAnterior = 0;
                            ViewBag.IdImageSiguiente = 0;
                        }
                    }
                    if (string.IsNullOrEmpty(imageActual.Image.Uuid))
                    {
                        ViewBag.isImagen = false;
                        Message("Ocurrio un error con la imagen comuniquese con el adminsitrador", MessageType.Danger);
                        return View();
                    }
                    else
                    {
                        WSImage wSImage = new WSImage();
                        var responseWsImage = wSImage.GetImage(imageActual.Image.Uuid);
                        if (responseWsImage.Success)
                        {
                            return View(new ImageFormViewModels
                            {
                                Id = imageActual.Id,
                                ImageBase64 = responseWsImage.ImageBase64,
                                AddressReceiver = imageActual.Receiver.Address,
                                CellReceiver = imageActual.Receiver.Cell,
                                NameReceiver = imageActual.Receiver.Name,
                                StateReceiver = imageActual.Receiver.State,
                                AddressSender = imageActual.Sender.Address,
                                CellSender = imageActual.Sender.Cell,
                                NameSender = imageActual.Sender.Name,
                                StateSender = imageActual.Sender.State,
                            });
                        }
                        else
                        {
                            ViewBag.isImagen = false;
                            Message("Ocurrio un error con la imagen comuniquese con el adminsitrador", MessageType.Danger);
                            return View();
                        }
                    }
                }
                else
                {
                    var imageActual = _db.Guides.FirstOrDefault(c => !c.IsCompleted && c.Id == id);
                    if (imageActual == null)
                    {
                        ViewBag.isImagen = false;
                        Message("No tiene imagenes pendientes", MessageType.Info);
                        return View();
                    }
                    else
                    {
                        ViewBag.isImagen = true;
                        var imageSiguiente = _db.Guides.FirstOrDefault(c => !c.IsCompleted && c.Id != imageActual.Id && c.Id > imageActual.Id);
                        if (imageSiguiente != null)
                        {
                            ViewBag.IdImageSiguiente = imageSiguiente.Id;
                        }
                        else
                        {
                            ViewBag.IdImageSiguiente = 0;
                        }
                        var imageAnterior = _db.Guides.FirstOrDefault(c => !c.IsCompleted && c.Id != imageActual.Id && c.Id < imageActual.Id);
                        if (imageAnterior != null)
                        {
                            ViewBag.IdImageAnterior = imageAnterior.Id;
                        }
                        else
                        {
                            ViewBag.IdImageAnterior = 0;
                        }
                    }
                    if (string.IsNullOrEmpty(imageActual.Image.Uuid))
                    {
                        ViewBag.isImagen = false;
                        Message("Ocurrio un error con la imagen comuniquese con el adminsitrador", MessageType.Danger);
                        return View();
                    }
                    else
                    {
                        WSImage wSImage = new WSImage();
                        var responseWsImage = wSImage.GetImage(imageActual.Image.Uuid);
                        if (responseWsImage.Success)
                        {
                            return View(new ImageFormViewModels
                            {
                                Id = imageActual.Id,
                                ImageBase64 = responseWsImage.ImageBase64,
                                AddressReceiver = imageActual.Receiver.Address,
                                CellReceiver = imageActual.Receiver.Cell,
                                NameReceiver = imageActual.Receiver.Name,
                                StateReceiver = imageActual.Receiver.State,
                                AddressSender = imageActual.Sender.Address,
                                CellSender = imageActual.Sender.Cell,
                                NameSender = imageActual.Sender.Name,
                                StateSender = imageActual.Sender.State,
                            });
                        }
                        else
                        {
                            ViewBag.isImagen = false;
                            Message("Ocurrio un error con la imagen comuniquese con el adminsitrador", MessageType.Danger);
                            return View();
                        }
                    }
                }
            }
            catch
            {
                ViewBag.isImagen = false;
                Message("Ocurrio un error comuniquese con el adminsitrador", MessageType.Danger);
                return View();
                throw;
            }
        }

        [Authorize(Roles = "Administrador,OperadorDigitalizador")]
        [HttpPost]
        public ActionResult Edit(ImageFormViewModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                else
                {
                    var imageActual = _db.Guides.FirstOrDefault(c => !c.IsCompleted && c.Id == model.Id);
                    imageActual.FechaUpload = DateTime.Now;
                    imageActual.IsUpload = true;
                    _db.Entry(imageActual).State = EntityState.Modified;                   
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ViewBag.isImagen = false;
                Message("Ocurrio un error comuniquese con el adminsitrador", MessageType.Danger);
                return View();
                throw;
            }
        }




    }
}
