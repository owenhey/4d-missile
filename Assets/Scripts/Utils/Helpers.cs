using UnityEngine;

namespace Scripts.Utils {
    public static class Helpers {
        /// <summary>
        /// Remaps a value from one range into another using linear interpolation. Clamps the starting values
        /// </summary>
        public static float RemapClamp(float value, float startLow, float startHigh, float endLow, float endHigh){
            value = Mathf.Clamp(value, startLow, startHigh);
            return endLow + ((endHigh - endLow) / (startHigh - startLow)) * (value - startLow);
        }

        /// <summary>
        /// Remaps a value from one range into another using linear interpolation
        /// </summary>
        public static float RemapNoClamp(float value, float startLow, float startHigh, float endLow, float endHigh){
            return endLow + ((endHigh - endLow) / (startHigh - startLow)) * (value - startLow);
        }
    }
}