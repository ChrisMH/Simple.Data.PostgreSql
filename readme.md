# Simple.Data.PostgreSql
An adapter to PostgreSql databases for Simple.Data

# Testing
The default tests assume a database cluster installed at localhost:5432.
The superuser account is used to create and delete a test database during testing.  It assume user id 'postgres' and password 'postgres', which is how I have my development database configured.  If yours is different, change the appropriate settings in app.config to run the unit tests.

