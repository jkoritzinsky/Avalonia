using AppKit;
using Foundation;
using Avalonia;
using Avalonia.OSX;

namespace ControlCatalog.OSX
{
	[Register("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		public override void DidFinishLaunching(NSNotification notification)
		{
			AppBuilder.Configure<App>()
					  .UseAppKit()
					  .UseRenderingSubsystem(() => { }, "NONE")
			          .Start<Avalonia.Controls.Window>();
		}
	}
}
