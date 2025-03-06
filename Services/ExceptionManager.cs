using Library.Core.Interfaces;

namespace Library.Services
{
    /// <summary>
    /// Handle the exceptions on the system. Requires a Logger to store them
    /// in a file or DB.
    /// </summary>
    public class ExceptionManager
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Main constructor for the <see cref="ExceptionManager"/>
        /// </summary>
        public ExceptionManager()
        {
            _logger = Logger.Instance;
        }

        /// <summary>
        /// Log the exception in the location and settings defined by the internal
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="ex">The exception being thrown</param>
        /// <param name="context">A definition of the environment where this exception
        /// is thrown</param>
        public void HandleException(Exception ex, string context = "General")
        {
            string logMessage = $"[{DateTime.Now}] [{context}] {ex.Message}\n{ex.StackTrace}\n";
            _logger.Log(logMessage);
        }
    }
}
