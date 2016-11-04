using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Avalonia.OSX
{
    class PlatformIconLoader : IPlatformIconLoader
    {
        public IWindowIconImpl LoadIcon(IBitmapImpl bitmap)
        {
            return null;
        }

        public IWindowIconImpl LoadIcon(Stream stream)
        {
            return null;
        }

        public IWindowIconImpl LoadIcon(string fileName)
        {
            return null;
        }
    }
}
