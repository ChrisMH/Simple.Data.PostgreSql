# Simple.Data.PostgreSql
An adapter to PostgreSql databases for Simple.Data

# Use
The adapter makes use of the Npgsql open source database adapter. 

To use this adapter, you will need to add the provider name to your app.config or web.config file:

<pre>&lt;system.data&gt;
  &lt;DbProviderFactories&gt;
    &lt;add name="Npgsql Data Provider"
         invariant="Npgsql"
         support="FF"
         description=".Net Framework Data Provider for Postgresql Server"
         type="Npgsql.NpgsqlFactory, Npgsql" /&gt;
  &lt;/DbProviderFactories&gt;
&lt;/system.data&gt;
</pre>
  
# Testing
The default tests assume a database cluster installed at localhost:5432.
Tests assume that the superuser account is named 'postgres' with password 'postgres' and that there is a default database named 'postgres'.  If your system is not set up like that, all these settings can be changed in the app.config file.

