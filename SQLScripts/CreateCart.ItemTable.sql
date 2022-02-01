CREATE TABLE Cart.Items (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(255),
    Price FLOAT(26),
    Quantity BIGINT,
    CartId BIGINT FOREIGN KEY REFERENCES [Cart].Carts(Id),
    ExternalId BIGINT
);