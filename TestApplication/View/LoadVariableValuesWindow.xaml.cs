//-----------------------------------------------------------------------
// <copyright file="LoadVariableValuesWindow.xaml.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the LoadVariableValuesWindow Window.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for LoadVariableValuesWindow Window.
    /// </summary>
    public partial class LoadVariableValuesWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadVariableValuesWindow"/> class.
        /// </summary>
        public LoadVariableValuesWindow()
        {
            this.InitializeComponent();
            this.DataContextChanged += this.LoadVariableValuesWindow_DataContextChanged;
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void CloseWindow()
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Handles the DataContextChanged event of the LoadVariableValuesWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void LoadVariableValuesWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext is LoadVariableValuesViewModel)
            {
                ((LoadVariableValuesViewModel)this.DataContext).CloseWindow = this.CloseWindow;
            }
        }
    }
}
