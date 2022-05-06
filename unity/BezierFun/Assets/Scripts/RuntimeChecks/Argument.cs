using System;

namespace Assets.Scripts.RuntimeChecks
{
    /// <summary>
    /// Performs runtime checks for arguments, and throws exceptions of type <see cref="ArgumentException"/>.
    /// </summary>
    public static class Argument
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified argument is null.
        /// </summary>
        /// <param name="argument">
        /// The argument
        /// </param>
        /// <param name="argumentName">
        /// The argument name
        /// </param>
        public static void AssertNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
