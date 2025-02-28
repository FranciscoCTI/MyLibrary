using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Interfaces
{
    /// <summary>
    /// Responsable for registering the exceptions thrown by the system
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Returns the singleton instance of the <see cref="ILogger"/>.
        /// </summary>
        public static ILogger? Instance;

        /// <summary>
        /// Creates a registry of the exception
        /// </summary>
        /// <param name="message">The text that will go on the registry entry</param>
        void Log(string message);
    }
}
