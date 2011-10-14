
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
	


/*
* basic_types
*/
CREATE TABLE public.basic_types (
  id                                 serial NOT NULL,
  
  smallint_field                     smallint,
  integer_field                      integer,
  bigint_field                       bigint,
  decimal_unlimited_field            decimal,
  decimal_10_2_field                 decimal(10,2),  
  numeric_unlimited_field            numeric,
  numeric_10_2_field                 numeric(10,2),                    
  real_field                         real,
  double_precision_field             double precision,  
  serial_field                       serial,
  bigserial_field                    bigserial,
  
  money_field                        money,

  character_varying_unlimited_field  character varying,
  character_varying_30_field         character varying(30),
  varchar_unlimited_field            varchar,
  varchar_30_field                   varchar(30),
  character_field                    character,
  character_10_field                 character(10),
  char_field                         char,
  char_10_field                      char(10),
  text_field                         text,
  
  bytea_field                        bytea,

  timestamp_field		     timestamp,
  timestamp_without_time_zone_field  timestamp without time zone,
  timestamptz_field                  timestamptz,
  timestamp_with_time_zone_field     timestamp with time zone,
  date_field                         date,
  time_field                         time,
  time_without_time_zone_field       time without time zone,
  timetz_field                       timetz,
  time_with_time_zone_field          time with time zone,
  interval_field                     interval,

  boolean_field                      boolean,

  point_field                        point,
  --line_field                         line, Not implemented in the database
  lseg_field                         lseg,
  box_field                          box,
  path_closed_field                  path,
  path_open_field                    path,
  polygon_field                      polygon,
  circle_field                       circle,

  cidr_field                         cidr,
  inet_field                         inet,
  macaddr_field                      macaddr,

  bit_field                          bit,
  bit_10_field                       bit(10),
  bit_varying_unlimited_field        bit varying,
  bit_varying_10_field               bit varying(10),

  tsvector_field                     tsvector,
  tsquery_field                      tsquery,

  uuid_field                         uuid,

  oid_field                          oid,
  
  CONSTRAINT basic_types_pkey
    PRIMARY KEY (id)
) WITH (
    OIDS = FALSE
  );
  
INSERT INTO public.basic_types
( smallint_field, 
  integer_field, 
  bigint_field, 
  decimal_unlimited_field,
  decimal_10_2_field,
  numeric_unlimited_field,
  numeric_10_2_field,
  real_field,
  double_precision_field,
  
  money_field,

  character_varying_unlimited_field,
  character_varying_30_field,   
  varchar_unlimited_field,          
  varchar_30_field,                 
  character_field,                  
  character_10_field,               
  char_field,                       
  char_10_field,                    
  text_field,

  bytea_field,
  
  timestamp_field,
  timestamp_without_time_zone_field,
  timestamptz_field,
  timestamp_with_time_zone_field,
  date_field,
  time_field,
  time_without_time_zone_field,
  timetz_field,
  time_with_time_zone_field,
  interval_field,

  boolean_field,

  point_field,
  --line_field, not implemented in the database
  lseg_field,
  box_field,
  path_closed_field,
  path_open_field,
  polygon_field,
  circle_field,

  cidr_field,
  inet_field,
  macaddr_field,
  bit_field,                  
  bit_10_field,               
  bit_varying_unlimited_field,
  bit_varying_10_field,
  
  tsvector_field,
  tsquery_field,

  uuid_field,

  oid_field

)
VALUES( 999,
        999999999,
        999999999999,
        1234567891234.12345,
        98765.43,
        1234567891234.12345,
        98765.43,
        999.88,
        999999999.9999999,
        
        '$123456.78',

        'String String String String String String String String String String String',
        'String String String String',
        'String String String String String String String String String String String',
        'String String String String',    
	'a',                  
        'abcdefghij',   
	'a',                  
        'abcdefghij',                
        'Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text',
  
        E'\\x0102030405066f5f4f3f2f1f0f',

        '2011-01-01 12:01:02',
        '2011-01-01 12:01:02',
        '2011-01-01 12:01:02 -05:00',
        '2011-01-01 12:01:02 -05:00',
        '2011-01-01',
        '12:01:02',
        '12:01:02',
        '12:01:02 -05:00',
        '12:01:02 -05:00',
        'P1Y2M3DT4H5M6S',

        TRUE,

        '(1,1)',
        --'((1,1),(2,2))',
        '((1,1),(2,2))',
        '(1,1),(10,10)',
        '((1,1),(2,2),(3,1))',
        '[(1,1),(2,2),(3,1)]',
        '((1,1),(2,2),(3,1))',
        '<(2,2),2>',
        
        '192.168.12',
        '127.0.0.1/32',
        '01:02:03:04:05:06',

        '1',
        '1100110011',
        '11110000111100001111000011110000',
        '11001100',

        'fat cat rat mat flat splat',
        'fat & rat & !cat',

        'aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee',

        1
);


/*
*
* array_types
*
*/
CREATE TABLE public.array_types
(
  id                           serial NOT NULL,
  integer_array_field          integer[],
  real_array_field             real[],
  double_precision_array_field double precision[],
  varchar_array_field          varchar[],

  integer_multi_array_field          integer[][],
  real_multi_array_field             real[][],
  double_precision_multi_array_field double precision[][],
  varchar_multi_array_field          varchar[][],
  
  CONSTRAINT array_types_pkey
    PRIMARY KEY (id)
) WITH(OIDS=FALSE);

INSERT INTO public.array_types
( integer_array_field,
  real_array_field,
  double_precision_array_field,
  varchar_array_field,
  integer_multi_array_field,         
  real_multi_array_field,            
  double_precision_multi_array_field,
  varchar_multi_array_field         
)
VALUES( '{1,2,3,4,5,6,7}',
        '{1.1,2.2,3.3,4.4,5.5,6.6,7.7}',    
        '{1.1,2.2,3.3,4.4,5.5,6.6,7.7}',
        '{one,two,three,four,five,six,seven}',
        '{{1,2,3,4,5,6,7},{8,9,1,2,3,4,5}}',
        '{{1.1,2.2,3.3,4.4,5.5,6.6,7.7},{8.8,9.9,1.1,2.2,3.3,4.4,5.5}}',    
        '{{1.1,2.2,3.3,4.4,5.5,6.6,7.7},{8.8,9.9,1.1,2.2,3.3,4.4,5.5}}',   
        '{{one,two,three,four,five,six,seven},{eight,nine,one,two,three,four,five}}'
);

/*
*
* Stored Procedures
*
*/

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






