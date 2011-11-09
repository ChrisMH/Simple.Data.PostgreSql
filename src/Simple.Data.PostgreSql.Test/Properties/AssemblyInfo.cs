using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if(DEBUG)
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyTitle("Simple.Data.PostgreSql.Test")]
[assembly: AssemblyDescription("PostgreSql adapter for Simple.Data Tests")]
[assembly: AssemblyCompany("Chris Hogan")]
[assembly: AssemblyProduct("Simple.Data.PostgreSql.Test")]
[assembly: AssemblyCopyright("Copyright © Chris Hogan 2010-2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("0.9.6.1")]
[assembly: AssemblyFileVersion("0.9.6.1")]
