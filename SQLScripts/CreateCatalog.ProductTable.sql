CREATE TABLE Catalog.Products (
    Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    Title VARCHAR(255),
    Price FLOAT(26),
    Description VARCHAR(255),
    Stock BIGINT,
    CategoryId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [Catalog].Categories(Id),
);
