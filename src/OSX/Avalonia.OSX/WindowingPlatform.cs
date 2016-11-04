using System;
using Avalonia.Platform;

namespace Avalonia.OSX
{
	public class WindowingPlatform : IWindowingPlatform
	{
		public IEmbeddableWindowImpl CreateEmbeddableWindow()
		{
			throw new NotImplementedException();
		}

		public IPopupImpl CreatePopup()
		{
			throw new NotImplementedException();
		}

		public IWindowImpl CreateWindow()
		{
			return new WindowImpl();
		}
	}
}

