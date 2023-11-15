﻿using Microsoft.EntityFrameworkCore;
using devpro_project.Models;

namespace devpro_project.Models
{
    public class MyDbContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            //đọc tag MyConnectionString tại file appsettings.json
            string strConnectionString = config.GetConnectionString("MyConnectionString").ToString();
            optionsBuilder.UseSqlServer(strConnectionString);
        }
        public DbSet<ItemUser> Users { get; set; }
        public DbSet<ItemCategory> Categories { get; set; }
        public DbSet<ItemAdv> Adv { get; set; }
        public DbSet<ItemCustomer> Customers { get; set; }
        public DbSet<ItemNews> News { get; set; }
        public DbSet<ItemOrder> Orders { get; set; }
        public DbSet<ItemOrderDetail> OrdersDetail { get; set; }
        public DbSet<ItemProduct> Products { get; set; }
        public DbSet<ItemRating> Rating { get; set; }
        public DbSet<ItemSlide> Slides { get; set; }
        public DbSet<ItemTag> Tags { get; set; }
        public DbSet<ItemTagProduct> TagsProducts { get; set; }
        public DbSet<ItemCategoryProduct> CategoriesProducts { get; set; }
    }
}
