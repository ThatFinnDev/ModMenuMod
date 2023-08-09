using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(ModMenuMod.BuildInfo.Description)]
[assembly: AssemblyDescription(ModMenuMod.BuildInfo.Description)]
[assembly: AssemblyCompany(ModMenuMod.BuildInfo.Company)]
[assembly: AssemblyProduct(ModMenuMod.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + ModMenuMod.BuildInfo.Author)]
[assembly: AssemblyTrademark(ModMenuMod.BuildInfo.Company)]
[assembly: AssemblyVersion(ModMenuMod.BuildInfo.Version)]
[assembly: AssemblyFileVersion(ModMenuMod.BuildInfo.Version)]
[assembly: MelonInfo(typeof(ModMenuMod.ModMenuMod), ModMenuMod.BuildInfo.Name, ModMenuMod.BuildInfo.Version, ModMenuMod.BuildInfo.Author, ModMenuMod.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("MonomiPark", "SlimeRancher2")]