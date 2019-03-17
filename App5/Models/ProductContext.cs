using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;

namespace App5.Models
{
    class ProductContext: DbContext
    {
        
        public DbSet<Product> Products
        {
            get;
            set;
        }
        private static readonly string source = "products.db";
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("DataSource =" + Path.Combine(storageFolder.Path, source));
            Debug.WriteLine(storageFolder.Path);
        }
        
    }
}
