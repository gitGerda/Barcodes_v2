namespace NiceLabel.SDK
{
    /// <summary>
    /// The command used to print the label asynchronously.
    /// </summary>
    public class PrintAsyncLabelCommand : AbstractCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintAsyncLabelCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public PrintAsyncLabelCommand(MainWindowViewModel viewModel)
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

                IPrintRequest printRequest = this.viewModel.Label.PrintAsync(int.Parse(this.viewModel.Quantity));
                this.viewModel.PrintRequests.Add(printRequest);
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }
    }
}
