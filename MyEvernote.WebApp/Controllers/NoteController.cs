﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Entities;
using MyEvernote.BusinessLayer;
using MyEvernote.WebApp.Models;

namespace MyEvernote.WebApp.Controllers
{
    public class NoteController : Controller
    {
        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        LikeManager likeManager = new LikeManager();
        public ActionResult Index()
        {
            var notes = noteManager.ListQueryable()
                .Include("Category")
                .Include("Owner")
                .Where(w => w.Owner.Id == CurrentSession.User.Id)
                .OrderByDescending(d => d.ModifiedOn);
            return View(notes.ToList());
        }

        public ActionResult MyLikedNotes() {
            var notes = likeManager.ListQueryable()
                .Include("LikedUser")
                .Include("Note")
                .Where(w => w.LikedUser.Id == CurrentSession.User.Id)
                .Select(s=>s.Note)
                .Include("Category")
                .Include("Owner")
                .OrderByDescending(d => d.ModifiedOn);
            return View("Index",notes.ToList());
        }
         
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x=>x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }
         
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                noteManager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }
         
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                Note db_note = noteManager.Find(x => x.Id == note.Id);
                db_note.IsDraft = note.IsDraft;
                db_note.CategoryId = note.CategoryId;
                db_note.Text = note.Text;
                db_note.Title = note.Title;

                noteManager.Update(db_note);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }
         
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }
         
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.Find(x => x.Id == id);
            noteManager.Delete(note);
            return RedirectToAction("Index");
        }
         
    }
}
