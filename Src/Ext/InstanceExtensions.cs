﻿using System;

namespace VulkanCore.Ext
{
    /// <summary>
    /// Provides extension methods for the <see cref="Instance"/> class.
    /// </summary>
    public static unsafe class InstanceExtensions
    {
        /// <summary>
        /// Create a debug report callback object.
        /// </summary>
        /// <param name="instance">The instance the callback will be logged on.</param>
        /// <param name="createInfo">
        /// The structure which defines the conditions under which this callback will be called.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <returns>A <see cref="DebugReportCallbackExt"/> handle.</returns>
        /// <exception cref="InvalidOperationException">Vulkan command not found.</exception>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static DebugReportCallbackExt CreateDebugReportCallbackExt(this Instance instance,
            DebugReportCallbackCreateInfoExt createInfo, AllocationCallbacks? allocator = null)
        {
            return new DebugReportCallbackExt(instance, ref createInfo, ref allocator);
        }

        /// <summary>
        /// To inject it's own messages into the debug stream an application uses this method.
        /// </summary>
        /// <param name="instance">The instance the callback will be logged on.</param>
        /// <param name="flags">
        /// Indicates the <see cref="DebugReportFlagsExt"/> that triggered this callback.
        /// </param>
        /// <param name="objectType">
        /// The type of object being used / created at the time the event was triggered.
        /// </param>
        /// <param name="object">
        /// Gives the object where the issue was detected. Object may be 0 if there is no object
        /// associated with the event.
        /// </param>
        /// <param name="location">
        /// A component (layer, driver, loader) defined value that indicates the "location" of the
        /// trigger. This is an optional value.
        /// </param>
        /// <param name="messageCode">
        /// A layer defined value indicating what test triggered this callback.
        /// </param>
        /// <param name="layerPrefix">Abbreviation of the component making the callback.</param>
        /// <param name="message">Unicode string detailing the trigger conditions.</param>
        /// <exception cref="InvalidOperationException">Vulkan command not found.</exception>
        public static void DebugReportMessageExt(this Instance instance, DebugReportFlagsExt flags,
            DebugReportObjectTypeExt objectType, long @object, int location, int messageCode, string layerPrefix,
            string message)
        {
            int byteCount = Interop.GetMaxByteCount(layerPrefix);
            var layerPrefixBytes = stackalloc byte[byteCount];
            Interop.StringToPtr(layerPrefix, layerPrefixBytes, byteCount);

            byteCount = Interop.GetMaxByteCount(message);
            var messageBytes = stackalloc byte[byteCount];
            Interop.StringToPtr(message, messageBytes, byteCount);

            var debugReportMessageExt = instance.GetProc<DebugReportMessageExtDelegate>("vkDebugReportMessageEXT");
            debugReportMessageExt(instance, 
                flags, objectType, @object, location, messageCode, layerPrefixBytes, messageBytes);
        }

        private delegate void DebugReportMessageExtDelegate(IntPtr instance, DebugReportFlagsExt flags, 
            DebugReportObjectTypeExt objectType, long @object, int location, int messageCode, byte* layerPrefix, byte* message);
    }
}