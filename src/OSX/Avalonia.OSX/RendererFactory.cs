using System;
using Avalonia.Rendering;

namespace Avalonia.OSX
{
	public class RendererFactory : IRendererFactory
	{
		public IRenderer CreateRenderer(IRenderRoot root, IRenderLoop renderLoop)
		{
			return new Renderer(root, renderLoop);
		}
	}
}
