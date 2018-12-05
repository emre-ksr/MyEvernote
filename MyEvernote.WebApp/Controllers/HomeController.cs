using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.Models;
using MyEvernote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();

        // GET: Home
        public ActionResult Index()
        {
            //BusinessLayer.Test t = new BusinessLayer.Test();
            //CategoryController üzerinden gelen view talebi ve model
            //if (TempData["mm"] != null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}

            return View(noteManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cat = categoryManager.Find(x=>x.Id == id.Value);
            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        { 
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult ShowProfile()
        { 
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errNotifyObj = new ViewModels.ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };

                return View("Error", errNotifyObj);
            }

            return View(res.Result);
        }
        public ActionResult EditProfile()
        { 
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errNotifyObj = new ViewModels.ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };

                return View("Error", errNotifyObj);
            }

            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                  (ProfileImage.ContentType == "image/jpeg" ||
                  ProfileImage.ContentType == "image/jpg" ||
                  ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                 
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }


                CurrentSession.Set<EvernoteUser>("login", res.Result);

                return RedirectToAction("/ShowProfile");
            }

            return View(model);
        }

        public ActionResult DeleteProfile()
        { 
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            { 
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                
                CurrentSession.Set<EvernoteUser>("login", res.Result);
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            { 
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                //if (model.Username == "aaa")
                //    ModelState.AddModelError("", "Kullanıcı adı kullanılıyor");
                //if (model.Email == "aa@aa.com")
                //    ModelState.AddModelError("", "Email kullanılıyor");

                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //        return View(model);
                //}

                OkViewModel notifyObj = new ViewModels.OkViewModel()
                {
                    Title = "Kayıt başarılı",
                    RedirectingUrl = "/Home/Register"
                };

                notifyObj.Items.Add("E-posta aktivasyonunu yapınız...");

                return View("Ok", notifyObj);
            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }


        public ActionResult UserActivate(Guid id)
        { 
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errNotifyObj = new ViewModels.ErrorViewModel()
                {
                    Title = "Geçersiz işlem",
                    Items = res.Errors
                };

                return View("Error", errNotifyObj);
                //TempData["errors"] = res.Errors;
                //return RedirectToAction("UserActivateCancel");
            }

            OkViewModel okNotifyObj = new ViewModels.OkViewModel()
            {
                Title = "Hesap aktifleştirildi",
                Header = "Hesabınız aktifleştirilmiştir. Artık not paylaşabilir ve beğeni yapabilirsiniz"
            };
            return View("Ok", okNotifyObj);
        }
        //public ActionResult UserActivateOK()
        //{
        //    return View();
        //}
        //public ActionResult UserActivateCancel()
        //{
        //    List<ErrorMessageObj> errors = null;
        //    if (TempData["errors"] != null)
        //    {
        //        errors = TempData["errors"] as List<ErrorMessageObj>;
        //    }
        //    return View(errors);
        //}
    }
}