﻿namespace CatalogAPI.Data
{
    public class Products
    {
        public long Id { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
    }
}
