/* See License.md in the solution root for license information.
 * File: AssemblyInfo.cs
*/
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Resources;

[assembly: System.CLSCompliant(false)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ComVisible(false)]

#if !PCL
[assembly: AssemblyTitle("Geodesy")]
#else
[assembly: AssemblyTitle("Geodesy.PCL")]
#endif
[assembly: AssemblyDescription("Class library with some geodesic algorithms.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug Version")]
#else
[assembly: AssemblyConfiguration("Release Version")]
#endif
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("Geodesic")]
[assembly: AssemblyCopyright("Copyright © Microsoft. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]		

[assembly: AssemblyVersion("1.0.6.0")]
[assembly: AssemblyFileVersion("1.0.6.0")]

[assembly: AssemblyDelaySign(false)]
[assembly: NeutralResourcesLanguageAttribute("en")]
