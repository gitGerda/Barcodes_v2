using System;
using System.Windows.Input;

namespace NiceLabel.SDK
{
    /// <summary>
    /// Base abstract class for document storage commands.
    /// </summary>
    public abstract class AbstractDocumentStorageCommand : ICommand
    {
        protected IDocumentStorage documentStorage;
        protected BrowseDocumentStorageViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDocumentStorageCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public AbstractDocumentStorageCommand(BrowseDocumentStorageViewModel viewModel)
        {
            this.viewModel = viewModel;
            //this.documentStorage = viewModel.PrintEngine.GetDocumentStorage();
        }

#pragma warning disable 67
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;
#pragma warning restore  67

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Not used by commands in the application.</param>
        /// <returns>
        /// Always true.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
