using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyEvernote.WebApp.Models
{
    public class CacheHelper
    {
        public const string categoryCacheKey = "category-cache";

        public static List<Category> GetCategoriesFromCache() {
            var result = WebCache.Get(categoryCacheKey);
            if (result == null)
            {
                CategoryManager cm = new CategoryManager();
                result = cm.List();
                WebCache.Set(categoryCacheKey, result, 20, true);
            }

            return result;
        }

        public static void RemoveCategoriesCache()
        {
            Remove(categoryCacheKey);
        }

        public static void Remove(string key) {
            WebCache.Remove(key);
        }
    }
}