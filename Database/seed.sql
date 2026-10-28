USE QUANLYDONHANG;
Go

INSERT INTO Categories (Name)
VALUES (N'Cà phê'), (N'Trà'), (N'Bánh ngọt');

INSERT INTO Products (CategoryId, Name, Price, StockQuantity)
VALUES 
	((SELECT Id FROM Categories WHERE NAME = N'Cà phê'), N'Cà phê đen', 25000, 100),
	((SELECT Id FROM Categories WHERE Name = N'Cà phê'),   N'Cà phê sữa',     29000, 100),
    ((SELECT Id FROM Categories WHERE Name = N'Cà phê'),   N'Bạc xỉu',        32000,  80),
    ((SELECT Id FROM Categories WHERE Name = N'Trà'),      N'Trà đào',        35000,  60),
    ((SELECT Id FROM Categories WHERE Name = N'Trà'),      N'Trà vải',        35000,  60),
    ((SELECT Id FROM Categories WHERE Name = N'Trà'),      N'Trà sữa',        38000,  50),
    ((SELECT Id FROM Categories WHERE Name = N'Bánh ngọt'), N'Bánh tiramisu',  45000,  20),
    ((SELECT Id FROM Categories WHERE Name = N'Bánh ngọt'), N'Bánh croissant', 30000,  25);
GO


SELECT p.Id, p.Name, p.Price, p.StockQuantity, c.Name AS Category
FROM Products AS p
JOIN Categories c ON c.Id = p.CategoryId;