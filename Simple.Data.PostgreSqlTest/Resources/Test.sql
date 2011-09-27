
CREATE TABLE users (
  id serial NOT NULL,
  name varchar(100) NOT NULL,
  password varchar(100) NOT NULL,
  age integer NOT NULL
) WITH (OIDS = FALSE);

ALTER TABLE users
  ADD CONSTRAINT pk_users
  PRIMARY KEY (id);
  
INSERT INTO users (name, password, age) VALUES ('Bob', 'Bob', 32);
INSERT INTO users (name, password, age) VALUES ('Charlie', 'Charlie', 49);
INSERT INTO users (name, password, age) VALUES ('Dave', 'Dave', 12);



CREATE TABLE customers
(
  id serial NOT NULL,
  name varchar(100) NOT NULL,
  address varchar(200)
) WITH (OIDS=FALSE);

ALTER TABLE customers
  ADD CONSTRAINT pk_customers
  PRIMARY KEY (id);
  
INSERT INTO customers (name, address) VALUES ('Test', '100 Road');

	
	
CREATE TABLE orders (
  id serial NOT NULL,
  order_date timestamp NOT NULL,
  customer_id integer NOT NULL
) WITH (OIDS = FALSE);

ALTER TABLE orders
  ADD CONSTRAINT pk_orders
  PRIMARY KEY (id);

ALTER TABLE orders
  ADD CONSTRAINT fk_orders_customers_customer_id
  FOREIGN KEY (customer_id) REFERENCES customers (id)
  ON DELETE NO ACTION ON UPDATE NO ACTION;
  
INSERT INTO orders (order_date, customer_id) VALUES ('20101010 00:00:00.000', 1);

	
	
CREATE TABLE items (
  id serial NOT NULL,
  name varchar(100) NOT NULL,
  price money NOT NULL
) WITH (OIDS=FALSE);

ALTER TABLE items
  ADD CONSTRAINT pk_items
  PRIMARY KEY (id);


INSERT INTO items (name, price) VALUES ('Widget', '4.5000'::money);

	
	
CREATE TABLE order_items (
  id serial NOT NULL,
  order_id integer NOT NULL,
  item_id integer NOT NULL,
  quantity integer  NOT NULL
) WITH (OIDS = FALSE);

ALTER TABLE order_items
  ADD CONSTRAINT pk_order_items
  PRIMARY KEY (id);

ALTER TABLE order_items
  ADD CONSTRAINT fk_order_items_item_id
  FOREIGN KEY (item_id) REFERENCES items (id)
  ON DELETE NO ACTION ON UPDATE NO ACTION;
  
ALTER TABLE order_items
  ADD CONSTRAINT fk_order_items_order_id
  FOREIGN KEY (order_id) REFERENCES orders (id)
  ON DELETE NO ACTION ON UPDATE NO ACTION;
  		
INSERT INTO order_items (order_id, item_id, quantity) VALUES (1, 1, 10);
	
	
	
CREATE VIEW view_customers AS
  SELECT name, address, id
  FROM customers
  WHERE name LIKE '%e%';
	

CREATE OR REPLACE FUNCTION public.get_customers()
RETURNS SETOF public.customers AS
$$
SELECT * FROM customers ORDER BY id
$$
LANGUAGE 'sql'
VOLATILE;

CREATE OR REPLACE FUNCTION public.get_customer_orders
(
  IN integer
)
RETURNS SETOF public.orders AS
$$
SELECT * FROM public.orders WHERE id=$1
$$
LANGUAGE 'sql'
VOLATILE;

CREATE OR REPLACE FUNCTION public.get_customer_count()
RETURNS integer AS
$$
DECLARE
  c integer;
BEGIN
  SELECT INTO c COUNT(*) FROM public.customers;
  RETURN c;
END;
$$
LANGUAGE 'plpgsql'
VOLATILE;




CREATE OR REPLACE FUNCTION public.get_customers_and_orders (IN integer)
RETURNS SETOF refcursor AS
$$
DECLARE
  ref1 refcursor;
  ref2 refcursor;
BEGIN
  OPEN ref1 FOR SELECT * FROM public.customers WHERE id=$1;
  RETURN NEXT ref1;

  OPEN ref2 FOR SELECT * FROm public.orders WHERE customer_id=$1;
  RETURN NEXT ref2;
END;
$$
LANGUAGE plpgsql;




CREATE TABLE enum_test
(
  id serial NOT NULL,
  flag integer NOT NULL
) WITH (OIDS=FALSE);

ALTER TABLE enum_test
  ADD CONSTRAINT pk_enum_test
  PRIMARY KEY (id);


CREATE TABLE schema_table 
(
  id integer NOT NULL, 
  description varchar(100) NOT NULL
) WITH (OIDS=FALSE);

INSERT INTO schema_table VALUES (1, 'Pass');



CREATE SCHEMA test;

CREATE TABLE test.schema_table 
(
  id integer NOT NULL, 
  description varchar(100) NOT NULL
) WITH (OIDS=FALSE);

INSERT INTO test.schema_table VALUES (1, 'Pass');



CREATE TABLE paging_test
(
  id integer NOT NULL,
  dummy integer
) WITH (OIDS=FALSE);

 
CREATE TABLE no_primary_key_test
(
  id serial NOT NULL,
  dummy varchar NOT NULL
) WITH (OIDS=FALSE);


CREATE TABLE blobs
(
  id serial NOT NULL,
  data bytea
) WITH (OIDS=FALSE);

ALTER TABLE blobs
  ADD CONSTRAINT pk_blobs
  PRIMARY KEY (id);
	
	
CREATE OR REPLACE FUNCTION public.test_overload(IN integer)
RETURNS integer AS
$$
BEGIN
  RETURN $1;
END;
$$
LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION public.test_overload(IN integer, IN integer)
RETURNS integer AS
$$
BEGIN
  RETURN $1 + $2;
END;
$$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION public.test_return(doubleMe integer)
RETURNS integer AS
$$
BEGIN
  return 2 * doubleMe;
END;
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.test_out(doubleMe integer, OUT doubled integer )
RETURNS integer AS
$$
BEGIN
  doubled = 2 * doubleMe;
END;
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.test_inout(INOUT doubleMe integer)
RETURNS integer AS
$$
BEGIN
  doubleMe = 2 * doubleMe;
END;
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.test_multiple_out(startingValue integer, OUT doubled integer, OUT tripled integer, OUT quadrupled integer)
RETURNS record AS
$$
BEGIN
  doubled = 2 * startingValue;
  tripled = 3 * startingValue;
  quadrupled = 4 * startingValue;
END;
$$
LANGUAGE plpgsql;


/*
    CREATE TABLE [dbo].[Images](
	    [Id] [int] NOT NULL,
	    [TheImage] [image] NOT NULL
    );

    ALTER TABLE [dbo].[Images]
		ADD CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);




	BEGIN TRANSACTION
	
	DECLARE @PagingId AS INT
	SET @PagingId = 1
	WHILE @PagingId <= 100
	BEGIN
		INSERT INTO [dbo].[PagingTest] ([Id]) VALUES (@PagingId)
		SET @PagingId = @PagingId + 1
	END
	
	COMMIT TRANSACTION

END
GO

EXEC [dbo].[TestReset]
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






