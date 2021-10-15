using System;
using System.Windows;

namespace NiceLabel.SDK
{
    /// <summary>
    /// The command used to set a variable value.
    /// </summary>
    public class SetVariableCommand : AbstractCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetVariableCommand"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public SetVariableCommand(MainWindowViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// The command execute method will set a new variable value to the selected variable.
        /// </summary>
        /// <param name="parameter">Not used by this command.</param>
        public override void Execute(object parameter)
        {
            if (this.viewModel.SelectedVariable == null)
            {
                return;
            }

            try
            {
                this.viewModel.SelectedVariable.SetValue(this.viewModel.VariableValue);
                this.viewModel.UpdateVariableValues();
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
                this.viewModel.UpdateVariableValues();
            }
        }
    }
}
