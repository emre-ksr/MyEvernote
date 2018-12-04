using MyEvernote.BusinessLayer.Abstract; 
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        //NoteManager nm = new NoteManager();
        //LikeManager lm = new LikeManager();
        //CommentManager cm = new CommentManager();
        //public override int Delete(Category category)
        //{
        //    foreach (Note note in category.Notes.ToList())
        //    {
        //        foreach (Liked like in note.Likes.ToList())
        //        {
        //            lm.Delete(like);
        //        }
        //        foreach (Comment comment in note.Comments.ToList())
        //        {
        //            cm.Delete(comment);
        //        }
        //        nm.Delete(note);
        //    }
        //    return base.Delete(category);
        //}
    }
}
