Create table ProductTable
(
	productId int primary key identity(1000,1),
	productName nvarchar(50) NOT NULL,
	productPrice money NOT NULL,
	
)

Alter table ProductTable
add productStocks int NULL

truncate table ProductTable

select * from ProductTable


Create table CategoryTable
(
	categoryId int primary key identity(1,1),
	categoryName nvarchar(50) NOT NULL
)

select * from CategoryTable;

Alter table ProductTable
add categoryId int null
REFERENCES CategoryTable(categoryId)

Insert into CategoryTable values('Electronics');
Insert into CategoryTable values('Food');
Insert into CategoryTable values('Clothes');
Insert into CategoryTable values('Beverages');

Insert into ProductTable values('Sony Speaker', 14000, 1,1000)
Insert into ProductTable values('Packaged ColdDrink', 50, 4,2000)
Insert into ProductTable values('Tshirt', 3000, 3,3000)
Insert into ProductTable values('Noodles', 200, 2,4000)
Insert into ProductTable values('Tv', 40000, 1,5000)