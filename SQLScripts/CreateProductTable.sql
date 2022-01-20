CREATE TABLE Product (
    Id BIGINT PRIMARY KEY,
    Title VARCHAR(255),
    Price FLOAT(26),
    Description VARCHAR(255),
    CategoryId BIGINT FOREIGN KEY REFERENCES [dbo].Category(Id),
);

drop table product