using System;
using Avalonia.Platform;

namespace Avalonia.Shared.PlatformSupport
{
	partial class StandardRuntimePlatform : IRuntimePlatform
	{
		private static Lazy<RuntimePlatformInfo> info = new Lazy<RuntimePlatformInfo>(
			() =>
			  new RuntimePlatformInfo
			  {
				  IsMono = true,
				  IsUnix = true,
				  IsMobile = false,
				  IsCoreClr = false,
				  IsDesktop = true,
				  IsDotNetFramework = false,
				  OperatingSystem = OperatingSystemType.OSX,

			   });

		public RuntimePlatformInfo GetRuntimeInfo() => info.Value;
	}
}
