using System.Windows;

namespace NiceLabel.SDK
{
    /// <summary>
    /// The command used to browse Document Storage for a label.
    /// </summary>
    public class BrowseDocumentStorageCommand : AbstractCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseDocumentStorageCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public BrowseDocumentStorageCommand(MainWindowViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// The command execute method will show a dialog that can be used to browse for a label file from Enterprise Print Manager.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Execute(object parameter)
        {
            BrowseDocumentStorageWindow browseEPMWindow = new BrowseDocumentStorageWindow();
            browseEPMWindow.Owner = Application.Current.MainWindow;
            BrowseDocumentStorageViewModel browseEPMViewModel = browseEPMWindow.DataContext as BrowseDocumentStorageViewModel;
            
            browseEPMViewModel.PrintEngine = this.viewModel.PrintEngine;

            if (string.IsNullOrEmpty(this.viewModel.PrintEngine.ControlCenterProperties.ControlCenterUrl))
            {
                browseEPMViewModel.ControlCenterUrl = "http://localhost/EPM/";
            }
            else
            {
                browseEPMViewModel.ControlCenterUrl = this.viewModel.PrintEngine.ControlCenterProperties.ControlCenterUrl;
            }

            if (browseEPMWindow.ShowDialog().Value)
            {
                this.viewModel.PrintEngine.ControlCenterProperties.ControlCenterUrl = browseEPMViewModel.ControlCenterUrl;

                this.viewModel.LabelFileName = browseEPMViewModel.SelectedLabelFileName;
            }            
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Not used by commands in the application.</param>
        /// <returns>
        /// True if command is enabled.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
