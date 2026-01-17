using System;
using UnityEngine;

namespace TwoSides.Utility.Coords2D
{
    /// <summary>
    /// Provides helpers to translate between screen, world, and viewport coordinates.
    /// </summary>
    public static class Coords2DTranslator
    {
        /// <summary>
        /// Converts a screen position into world coordinates.
        /// </summary>
        /// <param name="screenPos">Screen position in pixels.</param>
        /// <param name="worldCamera">Camera used for the conversion.</param>
        /// <returns>World space coordinates.</returns>
        public static Vector2 FromScreenToWorld(Vector2 screenPos, Camera worldCamera)
        {
            if (worldCamera == null)
                throw new ArgumentNullException(nameof(worldCamera));

            return (Vector2)worldCamera.ScreenToWorldPoint(screenPos);
        }

        /// <summary>
        /// Converts a world position into screen coordinates.
        /// </summary>
        /// <param name="worldPos">World space position.</param>
        /// <param name="worldCamera">Camera used for the conversion.</param>
        /// <returns>Screen coordinates in pixels.</returns>
        public static Vector2 FromWorldToScreen(Vector2 worldPos, Camera worldCamera)
        {
            if (worldCamera == null)
                throw new ArgumentNullException(nameof(worldCamera));

            return (Vector2)worldCamera.WorldToScreenPoint(worldPos);
        }

        /// <summary>
        /// Converts a screen position into viewport coordinates.
        /// </summary>
        /// <param name="screenPos">Screen position in pixels.</param>
        /// <param name="worldCamera">Camera used for the conversion.</param>
        /// <returns>Viewport coordinates in normalized space.</returns>
        public static Vector2 FromScreenToViewport(Vector2 screenPos, Camera worldCamera)
        {
            if (worldCamera == null)
                throw new ArgumentNullException(nameof(worldCamera));

            return (Vector2)worldCamera.ScreenToViewportPoint(screenPos);
        }

        /// <summary>
        /// Converts a world position into viewport coordinates.
        /// </summary>
        /// <param name="worldPos">World space position.</param>
        /// <param name="worldCamera">Camera used for the conversion.</param>
        /// <returns>Viewport coordinates in normalized space.</returns>
        public static Vector2 FromWorldToViewport(Vector2 worldPos, Camera worldCamera)
        {
            if (worldCamera == null)
                throw new ArgumentNullException(nameof(worldCamera));

            return (Vector2)worldCamera.WorldToViewportPoint(worldPos);
        }

        /// <summary>
        /// Converts a viewport position into world coordinates.
        /// </summary>
        /// <param name="viewportPos">Viewport coordinates in normalized space.</param>
        /// <param name="worldCamera">Camera used for the conversion.</param>
        /// <returns>World space coordinates.</returns>
        public static Vector2 FromViewportToWorld(Vector2 viewportPos, Camera worldCamera)
        {
            if (worldCamera == null)
                throw new ArgumentNullException(nameof(worldCamera));

            return (Vector2)worldCamera.ViewportToWorldPoint(viewportPos);
        }

        /// <summary>
        /// Converts a viewport position into screen coordinates.
        /// </summary>
        /// <param name="viewportPos">Viewport coordinates in normalized space.</param>
        /// <param name="worldCamera">Camera used for the conversion.</param>
        /// <returns>Screen coordinates in pixels.</returns>
        public static Vector2 FromViewportToScreen(Vector2 viewportPos, Camera worldCamera)
        {
            if (worldCamera == null)
                throw new ArgumentNullException(nameof(worldCamera));

            return (Vector2)worldCamera.ViewportToScreenPoint(viewportPos);
        }
    }
}