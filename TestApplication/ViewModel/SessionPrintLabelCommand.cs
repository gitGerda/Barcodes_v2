using System.Windows.Input;

namespace NiceLabel.SDK
{
    /// <summary>
    /// The command used to print the label.
    /// </summary>
    public class SessionPrintLabelCommand : AbstractCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionPrintLabelCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public SessionPrintLabelCommand(MainWindowViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// The command execute method will print the label via the PrintEngine interface.
        /// </summary>
        /// <param name="parameter">Not used by this command.</param>
        public override void Execute(object parameter)
        {
            if (this.viewModel.Label == null)
            {
                return;
            }

            try
            {
                if (this.viewModel.SelectedPrinter != null)
                {
                    this.viewModel.Label.PrintSettings.PrinterName = this.viewModel.SelectedPrinter.Name;
                }

                this.viewModel.Label.SessionPrint(int.Parse(this.viewModel.Quantity), this.viewModel.SessionPrintRequest);
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
        /// True if no session is already printing.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return (this.viewModel.SessionPrintRequest != null) && (this.viewModel.Label != null);
        }
    }
}
