CREATE TABLE Items (
    Id BIGINT PRIMARY KEY,
    Title VARCHAR(255),
    Price FLOAT(26),
    Quantity BIGINT,
    CartId BIGINT FOREIGN KEY REFERENCES [dbo].Carts(Id),
);