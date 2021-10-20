namespace NiceLabel.SDK
{
    /// <summary>
    /// The command used to print the label.
    /// </summary>
    public class SessionPrintEndLabelCommand : AbstractCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionPrintEndLabelCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public SessionPrintEndLabelCommand(MainWindowViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// The command execute method will print the label via the PrintEngine interface.
        /// </summary>
        /// <param name="parameter">Not used by this command.</param>
        public override void Execute(object parameter)
        {
            if ((this.viewModel.Label == null) || (this.viewModel.SessionPrintRequest == null))
            {
                return;
            }

            try
            {
                if (this.viewModel.SelectedPrinter != null)
                {
                    this.viewModel.Label.PrintSettings.PrinterName = this.viewModel.SelectedPrinter.Name;
                }

                this.viewModel.Label.EndSessionPrint(this.viewModel.SessionPrintRequest);
                this.viewModel.SessionPrintRequest = null;
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
