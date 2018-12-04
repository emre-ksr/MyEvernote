using MyEvernote.DataAccessLayer.EntityFramework; 
using MyEvernote.Entities;
using System;
using System.Collections.Generic;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        Repository<Category> repo_category = new Repository<Category>();
        Repository<Note> repo_note = new Repository<Note>();
        Repository<Comment> repo_comment = new Repository<Comment>();
        public Test()
        {
            //DatabaseContext db = new DatabaseContext();
            ////db.Database.CreateIfNotExists();

            //db.EvernoteUsers.ToList();

            //List<Category> liste = repo_category.List();
            //List<Category> listFilter = repo_category.List(x => x.Id > 3);
            //this.InsertTest();
            //this.UpdateTest();
            //this.DeleteTest();
            //this.CommentTest();
        }

        public void InsertTest()
        {
            EvernoteUser user = new EvernoteUser()
            {
                Name = "test",
                Surname = "test",
                Username = "test",
                Password = "123456",
                ActivateGuid = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "test",
                Email = "emrekiser@gmail.com",
                IsActive = true,
                IsAdmin = false
            };

            int result = repo_user.Insert(user);
        }

        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "Test");
            if (user != null)
            {
                user.Name = "yyyyy";
                int result = repo_user.Update(user);
            }
        }

        public void DeleteTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "test");
            if (user != null)
            {
                int result = repo_user.Delete(user);
            }
        }

        public void CommentTest() {
            EvernoteUser user = repo_user.Find(x => x.Id == 3);
            Note note = repo_note.Find(x => x.Id == 2);
            Comment cmt = new Comment() {
                Text = "test comment",
                Owner = user,
                Note = note,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "emre",
                CreatedOn = DateTime.Now
            };
            repo_comment.Insert(cmt);
        }
    }
}
