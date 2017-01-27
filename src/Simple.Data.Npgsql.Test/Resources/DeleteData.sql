
DELETE FROM public.order_items;
DELETE FROM public.orders;
DELETE FROM public.items;
DELETE FROM public.customers;
DELETE FROM public.users;

DELETE FROM public.schema_table;
DELETE FROM test.schema_table;

DELETE FROM public.basic_types;
DELETE FROM public.array_types;


ALTER SEQUENCE public.array_types_id_seq RESTART;
ALTER SEQUENCE public.basic_types_bigserial_field_seq RESTART;
ALTER SEQUENCE public.basic_types_id_seq RESTART;
ALTER SEQUENCE public.basic_types_serial_field_seq RESTART;
ALTER SEQUENCE public.blobs_id_seq RESTART;
ALTER SEQUENCE public.customers_id_seq RESTART;
ALTER SEQUENCE public.enum_test_id_seq RESTART;
ALTER SEQUENCE public.items_id_seq RESTART;
ALTER SEQUENCE public.no_primary_key_test_id_seq RESTART;
ALTER SEQUENCE public.order_items_id_seq RESTART;
ALTER SEQUENCE public.orders_id_seq RESTART;
ALTER SEQUENCE public.users_id_seq RESTART;

