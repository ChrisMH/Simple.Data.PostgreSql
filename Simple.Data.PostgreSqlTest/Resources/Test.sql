CREATE TABLE users (
  id serial NOT NULL,
  name varchar(100) NOT NULL,
  password varchar(100) NOT NULL,
  age integer NOT NULL,
  CONSTRAINT users_pkey
    PRIMARY KEY (id)
) WITH (OIDS = FALSE);

INSERT INTO users (name, password, age) VALUES ('Bob', 'Bob', 32);
INSERT INTO users (name, password, age) VALUES ('Charlie', 'Charlie', 49);
INSERT INTO users (name, password, age) VALUES ('Dave', 'Dave', 12);

CREATE TABLE orders (
  order_id serial NOT NULL,
  order_date timestamp NOT NULL,
  customer_id integer NOT NULL,
  CONSTRAINT orders_pkey
    PRIMARY KEY (order_id)
) WITH (OIDS = FALSE);

CREATE TABLE order_items (
  order_item_id serial NOT NULL,
  order_id integer NOT NULL,
  item_id integer NOT NULL,
  quantity integer  NOT NULL,
  CONSTRAINT order_items_pkey
    PRIMARY KEY (order_item_id)
) WITH (OIDS = FALSE);

/*
    CREATE TABLE [dbo].[Images](
	    [Id] [int] NOT NULL,
	    [TheImage] [image] NOT NULL
    );

    ALTER TABLE [dbo].[Images]
		ADD CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

	CREATE TABLE [dbo].[Items] (
		[ItemId] INT            IDENTITY (1, 1) NOT NULL,
		[Name]   NVARCHAR (100) NOT NULL,
		[Price]  MONEY          NOT NULL
	);

	ALTER TABLE [dbo].[Items]
		ADD CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED ([ItemId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

	CREATE TABLE [dbo].[Customers] (
		[CustomerId] INT            IDENTITY (1, 1) NOT NULL,
		[Name]       NVARCHAR (100) NOT NULL,
		[Address]    NVARCHAR (200) NULL
	);

	ALTER TABLE [dbo].[Customers]
		ADD CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
	    
	CREATE TABLE [dbo].[PagingTest] ([Id] int not null, [Dummy] int);

	CREATE TABLE [dbo].[Blobs](
		[Id] [int] NOT NULL,
		[Data] [varbinary](max) NULL
	)

	ALTER TABLE [dbo].[Blobs]
		ADD CONSTRAINT [PK_Blobs] PRIMARY KEY CLUSTERED ([Id] ASC)	

	CREATE TABLE [dbo].[EnumTest](
		[Id] [int] IDENTITY (1, 1) NOT NULL,
		[Flag] [int] NOT NULL
	)

	ALTER TABLE [dbo].[EnumTest]
		ADD CONSTRAINT [PK_EnumTest] PRIMARY KEY CLUSTERED ([Id] ASC)	

	BEGIN TRANSACTION
	SET IDENTITY_INSERT [dbo].[Customers] ON
	INSERT INTO [dbo].[Customers] ([CustomerId], [Name], [Address]) VALUES (1, N'Test', N'100 Road')
	SET IDENTITY_INSERT [dbo].[Customers] OFF
	SET IDENTITY_INSERT [dbo].[Orders] ON
	INSERT INTO [dbo].[Orders] ([OrderId], [OrderDate], [CustomerId]) VALUES (1, '20101010 00:00:00.000', 1)
	SET IDENTITY_INSERT [dbo].[Orders] OFF
	SET IDENTITY_INSERT [dbo].[Items] ON
	INSERT INTO [dbo].[Items] ([ItemId], [Name], [Price]) VALUES (1, N'Widget', 4.5000)
	SET IDENTITY_INSERT [dbo].[Items] OFF
	SET IDENTITY_INSERT [dbo].[OrderItems] ON
	INSERT INTO [dbo].[OrderItems] ([OrderItemId], [OrderId], [ItemId], [Quantity]) VALUES (1, 1, 1, 10)
	SET IDENTITY_INSERT [dbo].[OrderItems] OFF
	SET IDENTITY_INSERT [dbo].[Users] ON
	INSERT INTO [dbo].[Users] ([Id], [Name], [Password], [Age]) VALUES (1,'Bob','Bob',32)
	INSERT INTO [dbo].[Users] ([Id], [Name], [Password], [Age]) VALUES (2,'Charlie','Charlie',49)
	INSERT INTO [dbo].[Users] ([Id], [Name], [Password], [Age]) VALUES (3,'Dave','Dave',12)
	SET IDENTITY_INSERT [dbo].[Users] OFF
	
	DECLARE @PagingId AS INT
	SET @PagingId = 1
	WHILE @PagingId <= 100
	BEGIN
		INSERT INTO [dbo].[PagingTest] ([Id]) VALUES (@PagingId)
		SET @PagingId = @PagingId + 1
	END
	
	COMMIT TRANSACTION

	ALTER TABLE [dbo].[Orders] WITH NOCHECK
		ADD CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([CustomerId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

	ALTER TABLE [dbo].[OrderItems] WITH NOCHECK
		ADD CONSTRAINT [FK_OrderItems_Items] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([ItemId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

	ALTER TABLE [dbo].[OrderItems] WITH NOCHECK
		ADD CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([OrderId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
END
GO

EXEC [dbo].[TestReset]
GO

CREATE VIEW [dbo].[VwCustomers]
    AS
        SELECT     Name, Address, CustomerId
        FROM         dbo.Customers
        WHERE     (Name LIKE '%e%')
    
GO

CREATE TABLE [dbo].[SchemaTable] ([Id] int not null, [Description] nvarchar(100) not null);
GO
INSERT INTO [dbo].[SchemaTable] VALUES (1, 'Pass')
GO

CREATE SCHEMA [test] AUTHORIZATION [dbo]
GO

CREATE TABLE [test].[SchemaTable] ([Id] int not null, [Description] nvarchar(100) not null);
GO

CREATE PROCEDURE [dbo].[GetCustomers]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Customers
    ORDER BY CustomerId
END
GO
CREATE PROCEDURE [dbo].[GetCustomerAndOrders] (@CustomerId int)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Customers WHERE CustomerId = @CustomerId;
    SELECT * FROM Orders WHERE CustomerId = @CustomerId;
END
GO
CREATE PROCEDURE [dbo].[GetCustomerCount]
AS
BEGIN
    SET NOCOUNT ON;
    RETURN 1
END
GO
CREATE PROCEDURE ReturnStrings(@Strings AS [dbo].[StringList] READONLY)
AS
SELECT Value FROM @Strings
GO
CREATE FUNCTION [dbo].[VarcharAndReturnInt] (@AValue varchar(50)) RETURNS INT AS BEGIN
  IF ISNUMERIC(@AValue) = 1
  BEGIN
    RETURN cast (@AValue  as int)
  END
  RETURN 42
END
GO
CREATE TABLE [dbo].[GroupTestMaster] (
		[Id]       INT            IDENTITY (1, 1) NOT NULL,
		[Name]     NVARCHAR (100) NOT NULL
	);

ALTER TABLE [dbo].[GroupTestMaster]
	ADD CONSTRAINT [PK_GroupTestMaster] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
GO
CREATE TABLE [dbo].[GroupTestDetail] (
		[Id]    INT      IDENTITY (1, 1) NOT NULL,
		[Date]  DATETIME NOT NULL,
		[Number] INT NOT NULL,
		[MasterId] INT      NOT NULL
	);

ALTER TABLE [dbo].[GroupTestDetail]
	ADD CONSTRAINT [PK_GroupTestDetail] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

ALTER TABLE [dbo].[GroupTestDetail] WITH NOCHECK
	ADD CONSTRAINT [FK_GroupTestDetail_GroupTestMaster] FOREIGN KEY ([MasterId]) REFERENCES [dbo].[GroupTestMaster] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
INSERT INTO [dbo].[GroupTestMaster] VALUES ('One')
INSERT INTO [dbo].[GroupTestMaster] VALUES ('Two')
INSERT INTO [dbo].[GroupTestDetail] VALUES ('1999-1-1',1,1)
INSERT INTO [dbo].[GroupTestDetail] VALUES ('2000-1-1',2,1)
INSERT INTO [dbo].[GroupTestDetail] VALUES ('2001-1-1',3,1)
INSERT INTO [dbo].[GroupTestDetail] VALUES ('2010-1-1',2,2)
INSERT INTO [dbo].[GroupTestDetail] VALUES ('2011-1-1',3,2)
*/

