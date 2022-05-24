using System;

namespace Assets.Scripts.RuntimeChecks
{
    /// <summary>
    /// Performs runtime checks for fields, and throws exceptions of type <see cref="InvalidOperationException"/>.
    /// </summary>
    public static class Field
    {
        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the specified field is null.
        /// </summary>
        /// <param name="fieldValue">
        /// The field value
        /// </param>
        /// <param name="fieldName">
        /// The field name
        /// </param>
        public static void AssertNotNull(object fieldValue, string fieldName)
        {
            if (fieldValue == null)
            {
                throw new InvalidOperationException($"The field {fieldName} is null.");
            }
        }
    }
}
