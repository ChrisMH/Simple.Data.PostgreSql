
CREATE OR REPLACE FUNCTION public.test_no_return()
RETURNS void AS
$$
BEGIN

END;
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.test_return(double_me integer)
RETURNS integer AS
$$
BEGIN
  return 2 * double_me;
END;
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.test_return_no_parameter_names(integer)
RETURNS integer AS
$$
BEGIN
  return 2 * $1;
END;
$$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION public.test_out(double_me integer, OUT doubled integer)
RETURNS integer AS
$$
BEGIN
  doubled = 2 * double_me;
END;
$$
LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION public.test_out_no_parameter_names(integer, OUT integer)
RETURNS integer AS
$$
BEGIN
  $2 = 2 * $1;
END;
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.test_inout(INOUT double_me integer)
RETURNS integer AS
$$
BEGIN
  double_me = 2 * double_me;
END;
$$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION public.test_inout_no_parameter_names(INOUT integer)
RETURNS integer AS
$$
BEGIN
  $1 = 2 * $1;
END;
$$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION public.test_multiple_out(starting_value integer, OUT doubled integer, OUT tripled integer, OUT quadrupled integer)
RETURNS record AS
$$
BEGIN
  doubled = 2 * starting_value;
  tripled = 3 * starting_value;
  quadrupled = 4 * starting_value;
END;
$$
LANGUAGE plpgsql;


