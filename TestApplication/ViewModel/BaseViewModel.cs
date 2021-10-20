//-----------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the BaseViewModel class.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Base class for view models used in this test application. The base class is used for sending a property changed event to the user interface.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the BaseViewModel class.
        /// </summary>
        protected BaseViewModel()
        {
            this.ParentViewModel = null;
        }

        /// <summary>
        /// Initializes a new instance of the BaseViewModel class.
        /// </summary>
        /// <param name="parentViewModel">The ViewModel of the parent.</param>
        protected BaseViewModel(MainWindowViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name of the ViewModel used for display purposes.
        /// </summary>
        /// <value>The name of the ViewModel.</value>
        public virtual string ViewModelName { get; protected set; }

        /// <summary>
        /// Gets or sets the Parent ViewModel.
        /// </summary>
        internal MainWindowViewModel ParentViewModel { get; set; }

        /// <summary>
        /// Gets or sets the CloseWindow Action.
        /// </summary>
        internal virtual Action CloseWindow { get; set; }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}