using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Emre",
                Surname = "Kiser",
                Username = "emre",
                Password = "123456",
                ProfileImageFilename = "user.jpg",
                ActivateGuid = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "emre",
                Email = "emrekiser@gmail.com",
                IsActive = true,
                IsAdmin = true
            };
            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Yasemin",
                Surname = "Yakupoğlu",
                Username = "yasemin",
                Password = "123456",
                ProfileImageFilename = "user.jpg",
                ActivateGuid = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "emre",
                Email = "yasemin@gmail.com",
                IsActive = true,
                IsAdmin = false
            };

            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            for (int i = 0; i < 10; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Username = $"user{i.ToString()}",
                    Password = "123",
                    ProfileImageFilename = "user.jpg",
                    ActivateGuid = Guid.NewGuid(), 
                    ModifiedUsername = $"user{i.ToString()}",
                    Email = FakeData.NetworkData.GetEmail(),
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now), 
                    IsActive = true,
                    IsAdmin = false
                };
                context.EvernoteUsers.Add(user);
            }
            context.SaveChanges();

            List<EvernoteUser> users = context.EvernoteUsers.ToList();

            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "emre"
                };
                context.Categories.Add(cat);
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    EvernoteUser euser = users[FakeData.NumberData.GetNumber(0, users.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 10),
                        Owner = euser,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = euser.Username
                    };
                    cat.Notes.Add(note);

                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 5); j++)
                    {
                        EvernoteUser commentOwner = users[FakeData.NumberData.GetNumber(0, users.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = commentOwner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = commentOwner.Username
                        };
                        note.Comments.Add(comment);
                    }


                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked like = new Liked() {
                            LikedUser = users[m]
                        };

                        note.Likes.Add(like);

                    }
                }
            }

            context.SaveChanges();
        }
    }
}
