using System;
using System.Text;

namespace Globe.Shared.Extensions
{
    public static class ExceptionExtensions
    {
        #region Public Functions

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="exception">Exception to dump.</param>
        /// <returns>Dumped exception.</returns>
        public static string Dump(this Exception exception)
        {
            StringBuilder dumping = new StringBuilder();
            Dump(exception, dumping, string.Empty, "\t");

            return dumping.ToString();
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Dumps the exception.
        /// </summary>
        /// <param name="exception">Exception to dump.</param>
        /// <param name="dumping">Partial dumped exception.</param>
        /// <param name="headerTab">Tab char for header.</param>
        /// <param name="BodyTab">Tab char for body.</param>
        private static void Dump(Exception exception, StringBuilder dumping, string headerTab, string BodyTab)
        {
            if (exception == null)
                return;

            dumping.Append(headerTab);
            dumping.AppendLine("Type:");

            dumping.Append(BodyTab);
            dumping.AppendLine(exception.GetType().Name);

            dumping.Append(headerTab);
            dumping.AppendLine("Message:");

            dumping.Append(BodyTab);
            dumping.AppendLine(exception.Message);

            dumping.Append(headerTab);
            dumping.AppendLine("Source:");

            dumping.Append(headerTab);
            dumping.AppendLine("Stack Trace:");

            dumping.Append(BodyTab);
            dumping.AppendLine(exception.StackTrace);

            dumping.Append(headerTab);
            dumping.AppendLine("Inner Exception:");

            Dump(exception.InnerException, dumping, headerTab + "\t", BodyTab + "\t");
        }

        #endregion
    }
}
