using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Unity.AspectRatioEnforcement {
    public class ScreenSizeChangedEventArgs : EventArgs {

        public float OriginalWidth { get; private set; }
        public float OriginalHeight { get; private set; }

        public float NewWidth { get; private set; }
        public float NewHeight { get; private set; }

        public ScreenSizeChangedEventArgs(float originalWidth, float originalHeight, float newWidth, float newHeight) {
            OriginalWidth = originalWidth;
            OriginalHeight = originalHeight;
            NewWidth = newWidth;
            NewHeight = newHeight;
        }



    }
}
