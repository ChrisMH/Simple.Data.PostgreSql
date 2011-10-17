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
