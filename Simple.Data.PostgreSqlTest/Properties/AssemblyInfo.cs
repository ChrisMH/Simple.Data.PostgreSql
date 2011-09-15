using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if(DEBUG)
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyTitle("Simple.Data.PostgreSqlTest")]
[assembly: AssemblyDescription("PostgreSql adapter for Simple.Data Tests")]
[assembly: AssemblyCompany("Chris Hogan")]
[assembly: AssemblyProduct("Simple.Data.PostgreSqlTest")]
[assembly: AssemblyCopyright("Copyright © Chris Hogan 2010-2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("0.9.0.0")]
[assembly: AssemblyFileVersion("0.9.0.0")]
