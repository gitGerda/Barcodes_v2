namespace NiceLabel.SDK
{
    /// <summary>
    /// The command used to clear the print requests list.
    /// </summary>
    public class ClearPrintRequestsCommand : AbstractCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearPrintRequestsCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public ClearPrintRequestsCommand(MainWindowViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// The command execute method will clear the print requests list.
        /// </summary>
        /// <param name="parameter">Not used by this command.</param>
        public override void Execute(object parameter)
        {
            foreach (IPrintRequest printRequest in this.viewModel.PrintRequests)
            {
                printRequest.Dispose();
            }

            this.viewModel.PrintRequests.Clear();
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
            return this.viewModel.SessionPrintRequest == null;
        }
    }
}
