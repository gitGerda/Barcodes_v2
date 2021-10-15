//-----------------------------------------------------------------------
// <copyright file="LoadVariableValuesViewModel.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the LoadVariableValuesViewModel class.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    /// <summary>
    /// The window used to Load Variable Values for a label.
    /// </summary>
    public class LoadVariableValuesViewModel : BaseViewModel
    {
        #region Private Fields

        /// <summary>
        /// The backing field for the IsFromValuePairs property.
        /// </summary>
        private bool isFromValuePairs;

        /// <summary>
        /// The backing field for the VariableValues property.
        /// </summary>
        private string variableValues;
       
        /// <summary>
        /// The backing field for the IgnoreMissingVariables property.
        /// </summary>
        private bool ignoreMissingVariables;
        
        /// <summary>
        /// The backing field for the UpdateVariableValuesCommand property.
        /// </summary>
        private RelayCommand updateVariableValuesCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadVariableValuesViewModel"/> class.
        /// </summary>
        /// <param name="parentViewModel">The ViewModel of the parent.</param>
        public LoadVariableValuesViewModel(MainWindowViewModel parentViewModel)
            : base(parentViewModel)
        {
            this.ViewModelName = "Load Variable Values";
            this.IsFromValuePairs = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is from value pairs.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is from value pairs; otherwise, <c>false</c>.
        /// </value>
        public bool IsFromValuePairs
        {
            get
            {
                return this.isFromValuePairs;
            }

            set
            {
                this.isFromValuePairs = value;
                this.NotifyPropertyChanged("IsFromValuePairs");
                this.UpdateVariableValuesDisplay();
            }
        }

        /// <summary>
        /// Gets or sets the variable values.
        /// </summary>
        /// <value>
        /// The variable values.
        /// </value>
        public string VariableValues
        {
            get
            {
                return this.variableValues;
            }

            set
            {
                this.variableValues = value;
                this.NotifyPropertyChanged("VariableValues");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the exception is to be thrown when the variable is missing in the label and the user tries to set its value.
        /// </summary>
        /// <value>
        /// <c>true</c> if ignore missing variables; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreMissingVariables
        {
            get
            {
                return this.ignoreMissingVariables;
            }

            set
            {
                this.ignoreMissingVariables = value;
                this.NotifyPropertyChanged("IgnoreMissingVariables");
            }
        }

        /// <summary>
        /// Gets the update variable values command.
        /// </summary>
        /// <value>
        /// The update variable values command.
        /// </value>
        public ICommand UpdateVariableValuesCommand
        {
            get
            {
                if (this.updateVariableValuesCommand == null)
                {
                    this.updateVariableValuesCommand = new RelayCommand(p => this.LoadValues());
                }

                return this.updateVariableValuesCommand;
            }
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Loads the variable values.
        /// </summary>
        private void LoadValues()
        {
            try
            {
                if (this.IsFromValuePairs)
                {
                    this.ParentViewModel.Label.SetVariableValues(this.VariableValues, this.IgnoreMissingVariables);
                }
                else
                {
                    this.ParentViewModel.Label.SetVariableValuesXml(this.VariableValues, this.IgnoreMissingVariables);
                }

                this.ParentViewModel.UpdateVariableValues();
                this.CloseWindow();
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Updates the variable values display.
        /// </summary>
        private void UpdateVariableValuesDisplay()
        {
            this.VariableValues = this.IsFromValuePairs ? this.CreateValuePairStructure() : this.CreateXMLStructure();
        }

        /// <summary>
        /// Creates the XML structure.
        /// </summary>
        /// <returns>XML structured variable information.</returns>
        private string CreateXMLStructure()
        {
            StringBuilder sb = new StringBuilder("<variables>" + Environment.NewLine);
            this.ParentViewModel.Label.Variables.ToList().ForEach(v => sb.Append(this.AddVariableNode(v)));
            sb.Append("</variables>");

            return sb.ToString();
        }

        /// <summary>
        /// Creates the value pair structure.
        /// </summary>
        /// <returns>Value-Pair structured variable information.</returns>
        private string CreateValuePairStructure()
        {
            StringBuilder sb = new StringBuilder();
            this.ParentViewModel.Label.Variables.ToList().ForEach(v => sb.Append(this.AddVariableValuePair(v)));
            return sb.ToString();
        }

        /// <summary>
        /// Adds the variable node.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns>XML node containing variable's name and value.</returns>
        private string AddVariableNode(IVariable var)
        {
            return "\t<variable name=\"" + var.Name + "\">" + var.CurrentValue + "</variable>" + Environment.NewLine;
        }

        /// <summary>
        /// Adds the variable value pair.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns>Value Pair statement for variable.</returns>
        private string AddVariableValuePair(IVariable var)
        {
            return var.Name + "=" + var.CurrentValue + Environment.NewLine;
        }

        #endregion
    }
}
