//-----------------------------------------------------------------------
// <copyright file="ErrorHandler.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the ErrorHandler class.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK
{
    using System;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Static class for displaying a MessageBox with error details to user.
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Static method used for displaying a MessageBox with SDKException details.
        /// </summary>
        /// <param name="ex">SDKException that contains the details of the current exception to display.</param>
        public static void ReportError(SDKException ex)
        {
            string error = ex.Message + Environment.NewLine + Environment.NewLine +
                "Detailed message:" + Environment.NewLine + ex.DetailedMessage + Environment.NewLine + Environment.NewLine +
                "Detailed error code:" + Environment.NewLine + ex.DetailedErrorCode;
                
            MessageBox.Show(Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.IsActive), error);
        }
    }
}
