using System;
using System.Text;

namespace PGK.Extensions
{
    /// <summary>
    /// Extensions for StringBuilder
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// AppendLine version with format string parameters.
        /// </summary>
        public static void AppendLine(this StringBuilder builder, string value, params Object[] parameters)
        {
            builder.AppendLine(string.Format(value, parameters));
        }

        /// <summary>
        /// Appends the value of the object's System.Object.ToString() method followed by the default line terminator to the end of the current
        /// System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition">The conditional expression to evaluate.</param>
        /// <param name="value"></param>
        public static StringBuilder AppendLineIf(this StringBuilder @this, bool condition, object value)
        {
            if (condition) @this.AppendLine(value.ToString());
            return @this;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains zero or more format items, followed by the default
        /// line terminator to the end of the current System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition">The conditional expression to evaluate.</param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static StringBuilder AppendLineIf(this StringBuilder @this, bool condition, string format, params object[] args)
        {
            if (condition) @this.AppendFormat(format, args).AppendLine();
            return @this;
        }

        /// <summary>
        /// Appends the value of the object's System.Object.ToString() method to the end of the current
        /// System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        public static StringBuilder AppendIf(this StringBuilder @this, bool condition, object value)
        {
            if (condition) @this.Append(value.ToString());
            return @this;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains zero or more format items, 
        /// to the end of the current System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="this"></param>
        /// <param name="condition"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static StringBuilder AppendFormatIf(this StringBuilder @this, bool condition, string format, params object[] args)
        {
            if (condition) @this.AppendFormat(format, args);
            return @this;
        }
    }
}
