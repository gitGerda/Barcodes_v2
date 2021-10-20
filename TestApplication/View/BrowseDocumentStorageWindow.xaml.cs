//-----------------------------------------------------------------------
// <copyright file="BrowseDocumentStorageWindow.xaml.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the BrowseDocumentStorageWindow Window.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for the BrowseDocumentStorageWindow Window.
    /// </summary>
    public partial class BrowseDocumentStorageWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseDocumentStorageWindow"/> class.
        /// </summary>
        public BrowseDocumentStorageWindow()
        {
            this.InitializeComponent();
            this.DataContextChanged += this.BrowseDocumentStorageWindow_DataContextChanged;
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
        /// Handles the DataContextChanged event of the BrowseDocumentStorageWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void BrowseDocumentStorageWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext is BrowseDocumentStorageViewModel)
            {
                ((BrowseDocumentStorageViewModel)this.DataContext).CloseWindow = this.CloseWindow;
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the ListBoxItem control.  If ListBoxItem is a folder, this fires the ViewModel's GetDocumentsCommand to retrieve the documents from that folder.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ListBoxItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            BrowseDocumentStorageViewModel viewModel = (BrowseDocumentStorageViewModel)this.DataContext;
            if (viewModel.SelectedDocument.IsFolder)
            {
                // Add path separator to end of current directory, if necessary. 
                if (!viewModel.CurrentDirectory.EndsWith("/"))
                {
                    viewModel.CurrentDirectory += "/";
                }

                viewModel.CurrentDirectory = viewModel.CurrentDirectory + viewModel.SelectedDocument.FileName;
                viewModel.GetDocumentsCommand.Execute(this);
            }
        }
    }
}
