using System;

namespace UI.Utility.RichTextBox
{
    #region EventArgs extension and event delegates

    /// <summary>
    /// Class to extend EventArgs with a cancel parameter.
    /// </summary>
    public class RTFEditorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RTFEditorEventArgs"/> is cancel.
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
        private bool cancel;
        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }

    #endregion
}