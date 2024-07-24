using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Utils : MonoBehaviour
    {
        /// <summary>
        /// Calculate the direction vector from origin to target
        /// </summary>
        /// <returns>A normalized direction vector 2D</returns>
        public static Vector2 GetDirectionVector2(Vector2 origin, Vector2 target)
        {
            return (target - origin).normalized;
        }
        
        /// <summary>
        /// Calculate the direction vector from origin to target
        /// </summary>
        /// <returns>A normalized direction vector 3D</returns>
        public static Vector3 GetDirectionVector3s(Vector3 origin, Vector3 target)
        {
            return (target - origin).normalized;
        }
    }
}