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

