
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
