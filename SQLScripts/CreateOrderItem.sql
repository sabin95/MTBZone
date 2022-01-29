CREATE TABLE OrderItems (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(255),
    Price FLOAT(26),
    Quantity BIGINT,
    OrderId BIGINT FOREIGN KEY REFERENCES [dbo].Orders(Id),
);