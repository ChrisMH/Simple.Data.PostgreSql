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
  
