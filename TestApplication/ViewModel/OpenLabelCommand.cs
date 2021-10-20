using Microsoft.Win32;

namespace NiceLabel.SDK
{
    /// <summary>
    /// The command used to open a label.
    /// </summary>
    public class OpenLabelCommand : AbstractCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenLabelCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public OpenLabelCommand(MainWindowViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// The command execute method will open a new label via the PrintEngine interface.
        /// </summary>
        /// <param name="parameter">Not used by this command.</param>
        public override void Execute(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(this.viewModel.LabelFileName))
                {
                    OpenFileDialog openDialog = new OpenFileDialog();
                    openDialog.Multiselect = false;
                    openDialog.CheckFileExists = true;
                    openDialog.Filter = "NiceLabel Files (*.lbl)|*.lbl";

                    bool? dialogResult = openDialog.ShowDialog();

                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        this.viewModel.LabelFileName = openDialog.FileName;
                        this.viewModel.Label = this.viewModel.PrintEngine.OpenLabel(this.viewModel.LabelFileName);
                    }
                }
                else
                {
                    this.viewModel.Label = this.viewModel.PrintEngine.OpenLabel(this.viewModel.LabelFileName);
                }
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
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
