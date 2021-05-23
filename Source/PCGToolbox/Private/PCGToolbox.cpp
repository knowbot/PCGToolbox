#include "PCGToolboxPrivatePCH.h"
#include "Core.h"
#include "Modules/ModuleManager.h"
#include "PCGToolbox.h"

#define LOCTEXT_NAMESPACE "FPCGToolboxModule"

void FPCGToolboxModule::StartupModule()
{

	// This code will execute after your module is loaded into memory; the exact timing is specified in the .uplugin file per-module
	FString BaseDir = "PCGToolbox/Plugins/PCGToolbox";

	// Add on the relative location of the third party dll and load it
	FString LIBGMP, LIBMPFR;
#if PLATFORM_WINDOWS
	LIBGMP = FPaths::Combine(*BaseDir, TEXT("Binaries:Win64/libgmp-10.dll"));
	LIBMPFR = FPaths::Combine(*BaseDir, TEXT("Binaries:Win64/libmpfr-4.dll"));
#endif // PLATFORM_WINDOWS

	LibraryHandle = !LIBGMP.IsEmpty() ? FPlatformProcess::GetDllHandle(*LIBGMP) : nullptr;
	LibraryHandle = !LIBMPFR.IsEmpty() ? FPlatformProcess::GetDllHandle(*LIBMPFR) : nullptr;
}

void FPCGToolboxModule::ShutdownModule()
{
	// This function may be called during shutdown to clean up your module.  For modules that support dynamic reloading,
	// we call this function before unloading the module.

	FPlatformProcess::FreeDllHandle(LibraryHandle);
	LibraryHandle = nullptr;
}

#undef LOCTEXT_NAMESPACE

IMPLEMENT_MODULE(FPCGToolboxModule, PCGToolbox)
