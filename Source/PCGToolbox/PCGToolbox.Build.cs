using System.IO;
using UnrealBuildTool;
namespace UnrealBuildTool.Rules {
    public class PCGToolbox : ModuleRules {
        private string ModulePath {
            get { return ModuleDirectory; }
        }
        private string ThirdPartyPath {
            get { return Path.GetFullPath (Path.Combine (ModulePath, "../ThirdParty/")); }
        }

        public PCGToolbox (ReadOnlyTargetRules Target) : base (Target) {
            PublicDefinitions.Add ("WIN32");
            PublicDefinitions.Add ("_WINDOWS");
            PublicDefinitions.Add ("_CRT_SECURE_NO_DEPRECATE");
            PublicDefinitions.Add ("_SCL_SECURE_NO_DEPRECATE");
            PublicDefinitions.Add ("_CRT_SECURE_NO_WARNINGS");
            PublicDefinitions.Add ("_SCL_SECURE_NO_WARNINGS");
            PublicDefinitions.Add ("CGAL_USE_MPFR");
            PublicDefinitions.Add ("CGAL_USE_GMP");

            // Startard Module Dependencies
            PublicDependencyModuleNames.AddRange (new string[] { "Core" });
            PrivateDependencyModuleNames.AddRange (new string[] { "CoreUObject", "Engine", "Slate", "SlateCore" });

            // Start CGAL linking here!
            bool isCGALSupported = false;

            // Create CGAL Path
            string CGALPath = Path.Combine (ThirdPartyPath, "CGAL");

            // Get Library Path
            string LibPath = "";
            if (Target.Platform == UnrealTargetPlatform.Win64) {
                LibPath = Path.Combine (CGALPath, "lib");
                isCGALSupported = true;
            } else {
                string Err = string.Format ("{0} dedicated server is made to depend on {1}. We want to avoid this, please correct module dependencies.", Target.Platform.ToString (), this.ToString ());
                System.Console.WriteLine (Err);
            }

            if (isCGALSupported) {
                //Add Include path
                PublicIncludePaths.AddRange (new string[] { Path.Combine (CGALPath, "include") });
                PublicIncludePaths.AddRange (new string[] { Path.Combine (CGALPath, "include", "auxiliary") }); //Dependencies

                // Add Library Path
                // PublicAdditionalLibraries.Add (LibPath);

                //Add Static Libraries
                PublicAdditionalLibraries.Add (Path.Combine (LibPath, "libgmp-10.lib"));
                PublicAdditionalLibraries.Add (Path.Combine (LibPath, "libmpfr-4.lib"));
                PublicDelayLoadDLLs.Add ("libgmp-10.dll");
                PublicDelayLoadDLLs.Add ("libmpfr-4.dll");
                RuntimeDependencies.Add (Path.Combine (LibPath, "libgmp-10.dll"));
                RuntimeDependencies.Add (Path.Combine (LibPath, "libmpfr-4.dll"));
            }

            PublicDefinitions.Add (string.Format ("WITH_CGAL_BINDING={0}", isCGALSupported ? 1 : 0));

            // Start CGAL linking here!
            bool isFastNoiseSupported = false;

            // Create CGAL Path
            string FastNoisePath = Path.Combine (ThirdPartyPath, "FastNoise2");

            // Get Library Path
            string BinPath = "";
            if (Target.Platform == UnrealTargetPlatform.Win64) {
                LibPath = Path.Combine (FastNoisePath, "lib");
                isFastNoiseSupported = true;
            } else {
                string Err = string.Format ("{0} dedicated server is made to depend on {1}. We want to avoid this, please correct module dependencies.", Target.Platform.ToString (), this.ToString ());
                System.Console.WriteLine (Err);
            }

            if (isCGALSupported) {
                //Add Include path
                PublicIncludePaths.AddRange (new string[] { Path.Combine (FastNoisePath, "include") });

                // Add Library Path
                // PublicAdditionalLibraries.Add (LibPath);

                //Add Static Libraries
                PublicAdditionalLibraries.Add (Path.Combine (LibPath, "FastNoise.lib"));
                PublicDelayLoadDLLs.Add ("FastNoise.dll");
                RuntimeDependencies.Add (Path.Combine (LibPath, "FastNoise.dll"));
            }

            PublicDefinitions.Add (string.Format ("WITH_FASTNOISE_BINDING={0}", isFastNoiseSupported ? 1 : 0));
        }
    }
}