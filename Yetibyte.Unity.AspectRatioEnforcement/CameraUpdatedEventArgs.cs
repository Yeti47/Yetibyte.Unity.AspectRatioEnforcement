using System;
using UnityEngine;

namespace Yetibyte.Unity.AspectRatioEnforcement {
    public class CameraUpdatedEventArgs : EventArgs {

        public Camera TargetCamera { get; private set; }

        public Rect OriginalPixelRect { get; private set; }
        public Rect NewPixelRect { get; private set; }

        public float OriginalAspectRatioValue { get; private set; }
        public AspectRatio NewAspectRatio { get; private set; }

        public CameraUpdatedEventArgs(Camera targetCamera, Rect originalPixelRect, Rect newPixelRect, float originalAspectRatioValue, AspectRatio newAspectRatio) {
            TargetCamera = targetCamera;
            OriginalPixelRect = originalPixelRect;
            NewPixelRect = newPixelRect;

            OriginalAspectRatioValue = originalAspectRatioValue;
            NewAspectRatio = newAspectRatio;
        }

        public override string ToString() => $"{nameof(OriginalAspectRatioValue)}: \"{OriginalAspectRatioValue}\" | {nameof(NewAspectRatio)}: \"{NewAspectRatio}\"";


    }
}