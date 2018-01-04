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

[assembly: AssemblyTitle("Geodesy")]
[assembly: AssemblyDescription("A library with basic geodesic algorithms to compute distances, directions and coordinate transformations.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug Version")]
#else
[assembly: AssemblyConfiguration("Release Version")]
#endif
[assembly: AssemblyCompany("Pfeifers-Software")]
[assembly: AssemblyProduct("Geodesic")]
[assembly: AssemblyCopyright("Copyright © 2014-2018 Jürgen Pfeifer. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]		

[assembly: AssemblyVersion("2.0.2.0")]
[assembly: AssemblyFileVersion("2.0.2.0")]

[assembly: AssemblyDelaySign(false)]
[assembly: NeutralResourcesLanguageAttribute("en")]
