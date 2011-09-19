CREATE TABLE users (
  id serial NOT NULL,
  name varchar(100) NOT NULL,
  password varchar(100) NOT NULL,
  age          integer NOT NULL,
  CONSTRAINT users_pkey
    PRIMARY KEY (id)
) WITH (OIDS = FALSE);

INSERT INTO users (name, password, age) VALUES ('Bob', 'Bob', 32);
INSERT INTO users (name, password, age) VALUES ('Charlie', 'Charlie', 49);
INSERT INTO users (name, password, age) VALUES ('Dave', 'Dave', 12);
