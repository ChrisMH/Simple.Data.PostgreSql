INSERT INTO public.users (name, password, age) VALUES ('Bob', 'Bob', 32);
INSERT INTO public.users (name, password, age) VALUES ('Charlie', 'Charlie', 49);
INSERT INTO public.users (name, password, age) VALUES ('Dave', 'Dave', 12);

INSERT INTO public.customers (name, address) VALUES ('Test', '100 Road');

INSERT INTO public.orders (order_date, customer_id) VALUES ('20101010 00:00:00.000', currval('public.customers_id_seq'));

INSERT INTO public.items (name, price) VALUES ('Widget', '4.5000'::money);

INSERT INTO public.order_items (order_id, item_id, quantity) VALUES (currval('public.orders_id_seq'), currval('public.items_id_seq'), 10);



INSERT INTO public.schema_table VALUES (1, 'Pass');
INSERT INTO test.schema_table VALUES (1, 'Pass');




/*
*
* basic_types
*
*/
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



