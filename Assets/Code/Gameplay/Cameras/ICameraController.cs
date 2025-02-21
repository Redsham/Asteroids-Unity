using System;
using Other;
using UnityEngine;

namespace Gameplay.Cameras
{
    public interface ICameraController
    {
        public static ICameraController Active { get; private set; }
        internal static void SetActive(ICameraController controller)
        {
            if (Active != null && controller != null)
                throw new Exception("Only one camera controller can be active at a time.");
            
            Active = controller;
        }
        
        
        Camera   Camera   { get; }
        Vector3  Position { get; set; }
        Bounds2D View     { get; }


        void     Shake(CameraShakeBase shake);
    }
}