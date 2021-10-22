//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the MainWindowViewModel class.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Windows.Input;


    /// <summary>
    /// ViewModel for demo application, where main interaction with SDK is implemented.
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {

        #region Private Fields

        /// <summary>
        /// The backing field for the PrintEngine property.
        /// </summary>
        private IPrintEngine printEngine;

        /// <summary>
        /// The backing field for the Label property.
        /// </summary>
        private ILabel label;

        /// <summary>
        /// The backing field for the SelectedVariable property.
        /// </summary>
        private IVariable selectedVariable;

        /// <summary>
        /// The backing field for the Printers property.
        /// </summary>
        private IList<IPrinter> printers;

        /// <summary>
        /// The backing field for the SelectedPrinter property.
        /// </summary>
        private IPrinter selectedPrinter;

        /// <summary>
        /// The backing field for the SessionPrintRequest property.
        /// </summary>
        private ISessionPrintRequest sessionPrintRequest;

        /// <summary>
        /// The backing field for the VariableValue property.
        /// </summary>
        private string variableValue;

        /// <summary>
        /// The backing field for the LabelFileName property.
        /// </summary>
        private string labelFileName;

        /// <summary>
        /// The backing field for the Preview property.
        /// </summary>
        private object preview;

        /// <summary>
        /// The backing field for the PreviewToFile property.
        /// </summary>
        private bool previewToFile;

        /// <summary>
        /// The backing field for the ShowSampleValues property.
        /// </summary>
        private bool showSampleValues;

        /// <summary>
        /// The backing field for the VariableName property.
        /// </summary>
        private string variableName;

        /// <summary>
        /// The backing field for the VariableDescription property.
        /// </summary>
        private string variableDescription;

        /// <summary>
        /// The backing field for the VariableSerialization property.
        /// </summary>
        private string variableSerialization;

        /// <summary>
        /// The backing field for the VariableFormat property.
        /// </summary>
        private string variableFormat;

        /// <summary>
        /// The backing field for the VariableIsUsed property.
        /// </summary>
        private string variableIsUsed;

        /// <summary>
        /// The backing field for the VariableLength property.
        /// </summary>
        private string variableLength;

        /// <summary>
        /// The backing field for the UseControlCenter property.
        /// </summary>
        private bool useControlCenter;

        /// <summary>
        /// The backing field for the BrowseForLabelCommand.
        /// </summary>
        private ICommand browseForLabelCommand;

        /// <summary>
        /// The backing field for the SetVariableCommand.
        /// </summary>
        private ICommand setVariableCommand;

        /// <summary>
        /// The backing field for the PrintLabelCommand.
        /// </summary>
        private ICommand printLabelCommand;

        /// <summary>
        /// The backing field for the PrintAsyncLabelCommand.
        /// </summary>
        private ICommand printAsyncLabelCommand;

        /// <summary>
        /// The backing field for the SessionPrintLabelCommand.
        /// </summary>
        private ICommand sessionPrintLabelCommand;

        /// <summary>
        /// The backing field for the SessionPrintStartLabelCommand.
        /// </summary>
        private ICommand sessionPrintStartLabelCommand;

        /// <summary>
        /// The backing field for the SessionPrintEndLabelCommand.
        /// </summary>
        private ICommand sessionPrintEndLabelCommand;

        /// <summary>
        /// The backing field for the OpenPrintToGraphicsWindowCommand.
        /// </summary>
        private ICommand openPrintToGraphicsWindowCommand;

        /// <summary>
        /// The backing field for the OpenLoadVariableValuesWindowCommand.
        /// </summary>
        private ICommand openLoadVariableValuesWindowCommand;

        /// <summary>
        /// The backing field for the ClearPrintRequestsCommand.
        /// </summary>
        private ICommand clearPrintRequestsCommand;

        /// <summary>
        /// The BrowseDocumentStorage ViewModel.
        /// </summary>
        private BrowseDocumentStorageViewModel browseEPMViewModel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.ViewModelName = "NiceLabel SDK .NET Demo Application";

            this.Quantity = "1";
            this.PrintRequests = new ObservableCollection<IPrintRequest>();

            // Accept non-trusted server certificates. Allows connecting to EPM with invalid ssl certificate.
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        #endregion

        #region Public Properties

        #region Commands

        /// <summary>
        /// Gets the BrowseForLabel command.  This command is bound to the "Open..." button UI element.
        /// </summary>
        /// <value>
        /// The BrowseForLabel command.
        /// </value>
        public ICommand BrowseForLabelCommand
        {
            get
            {
                if (this.browseForLabelCommand == null)
                {
                    this.browseForLabelCommand = new RelayCommand(p => this.BrowseForLabel());
                }

                return this.browseForLabelCommand;
            }
        }

        /// <summary>
        /// Gets the set variable command.  This command is bound to the "Set value" button UI element.
        /// </summary>
        /// <value>
        /// The set variable command.
        /// </value>
        public ICommand SetVariableCommand
        {
            get
            {
                if (this.setVariableCommand == null)
                {
                    this.setVariableCommand = new RelayCommand(p => this.SetVariable(), p => this.SetVariable_CanExecute());
                }

                return this.setVariableCommand;
            }
        }

        /// <summary>
        /// Gets the print label command.  This command is bound to the "Print" button UI element.
        /// </summary>
        /// <value>
        /// The print label command.
        /// </value>
        public ICommand PrintLabelCommand
        {
            get
            {
                if (this.printLabelCommand == null)
                {
                    this.printLabelCommand = new RelayCommand(p => this.PrintLabel(), p => this.LabelDependentCommand_CanExecute());
                }

                return this.printLabelCommand;
            }
        }

        /// <summary>
        /// Gets the print async label command.  This command is bound to the "Print async" button UI element.
        /// </summary>
        /// <value>
        /// The print async label command.
        /// </value>
        public ICommand PrintAsyncLabelCommand
        {
            get
            {
                if (this.printAsyncLabelCommand == null)
                {
                    this.printAsyncLabelCommand = new RelayCommand(p => this.PrintLabelAsync(), p => this.LabelDependentCommand_CanExecute());
                }

                return this.printAsyncLabelCommand;
            }
        }

        /// <summary>
        /// Gets the session print start label command.  This command is bound to the "Session start" button UI element.
        /// </summary>
        /// <value>
        /// The session print start label command.
        /// </value>
        public ICommand SessionPrintStartLabelCommand
        {
            get
            {
                if (this.sessionPrintStartLabelCommand == null)
                {
                    this.sessionPrintStartLabelCommand = new RelayCommand(p => this.StartSessionPrint(), p => this.StartSessionPrint_CanExecute());
                }

                return this.sessionPrintStartLabelCommand;
            }
        }

        /// <summary>
        /// Gets the session print label command.  This command is bound to the "Session print" button UI element.
        /// </summary>
        /// <value>
        /// The session print label command.
        /// </value>
        public ICommand SessionPrintLabelCommand
        {
            get
            {
                if (this.sessionPrintLabelCommand == null)
                {
                    this.sessionPrintLabelCommand = new RelayCommand(p => this.SessionPrint(), p => this.SessionPrint_CanExecute());
                }

                return this.sessionPrintLabelCommand;
            }
        }

        /// <summary>
        /// Gets the session print end label command.  This command is bound to the "End session" button UI element.
        /// </summary>
        /// <value>
        /// The session print end label command.
        /// </value>
        public ICommand SessionPrintEndLabelCommand
        {
            get
            {
                if (this.sessionPrintEndLabelCommand == null)
                {
                    this.sessionPrintEndLabelCommand = new RelayCommand(p => this.EndSessionPrint(), p => this.EndSessionPrint_CanExecute());
                }

                return this.sessionPrintEndLabelCommand;
            }
        }

        /// <summary>
        /// Gets the open print to graphics window command.  This command is bound to the "Print to graphics..." button UI element.
        /// </summary>
        /// <value>
        /// The open print to graphics window command.
        /// </value>
        public ICommand OpenPrintToGraphicsWindowCommand
        {
            get
            {
                if (this.openPrintToGraphicsWindowCommand == null)
                {
                    this.openPrintToGraphicsWindowCommand = new RelayCommand(p => this.OpenPrintToGraphicsWindow(), p => this.LabelDependentCommand_CanExecute());
                }

                return this.openPrintToGraphicsWindowCommand;
            }
        }

        /// <summary>
        /// Gets the open load variable values window command.  This command is bound to the "Load variable values..." button UI element.
        /// </summary>
        /// <value>
        /// The open load variable values window command.
        /// </value>
        public ICommand OpenLoadVariableValuesWindowCommand
        {
            get
            {
                if (this.openLoadVariableValuesWindowCommand == null)
                {
                    this.openLoadVariableValuesWindowCommand = new RelayCommand(p => this.OpenLoadVariableValuesWindow(), p => this.LabelDependentCommand_CanExecute());
                }

                return this.openLoadVariableValuesWindowCommand;
            }
        }

        /// <summary>
        /// Gets the clear print requests command.  This command is bound to the "Clear requests" button UI element.
        /// </summary>
        /// <value>
        /// The clear print requests command.
        /// </value>
        public ICommand ClearPrintRequestsCommand
        {
            get
            {
                if (this.clearPrintRequestsCommand == null)
                {
                    this.clearPrintRequestsCommand = new RelayCommand(p => this.ClearPrintRequests(), p => this.ClearPrintRequests_CanExecute());
                }

                return this.clearPrintRequestsCommand;
            }
        }

        #endregion

        #region SDK Properties

        /// <summary>
        /// Gets the list of printers.  This property is bound to the printer selection in UI.
        /// </summary>
        /// <value>
        /// The list of available printers.
        /// </value>
        public IList<IPrinter> Printers
        {
            get
            {
                if (this.printers == null)
                {
                    this.printers = this.PrintEngine.Printers;

                    if (this.printers.Count > 0)
                    {
                        this.SelectedPrinter = this.printers[0];
                    }
                }

                return this.printers;
            }
        }

        /// <summary>
        /// Gets or sets the selected printer. This property is bound to the selected item of the printer selection in UI.
        /// </summary>
        /// <value>
        /// The selected printer.
        /// </value>
        public IPrinter SelectedPrinter
        {
            get
            {
                return this.selectedPrinter;
            }

            set
            {
                if (this.selectedPrinter != value)
                {
                    this.selectedPrinter = value;

                    this.NotifyPropertyChanged("SelectedPrinter");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the label file. This property is bound to the edit control in UI, where label path is entered by the user.
        /// </summary>
        /// <value>
        /// The name of the label file.
        /// </value>
        public string LabelFileName
        {
            get
            {
                return this.labelFileName;
            }

            set
            {
                if (this.labelFileName != value)
                {
                    this.labelFileName = value;
                    this.NotifyPropertyChanged("LabelFileName");
                    this.NotifyPropertyChanged("LabelFileNameOnly");
                    this.NotifyPropertyChanged("LabelFileLocation");
                }
            }
        }

        /// <summary>
        /// Gets or sets the quantity. This property is bound to the edit control in UI, where the user enters desired quantity of labels for printing.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public string Quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether labels for all available data need to be printed. This property is bound to the edit control in UI, where the user enters desired quantity of labels for printing.
        /// </summary>
        /// <value>
        /// True if print job needs to contain labels for all records from the database.
        /// </value>
        public bool PrintAll
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label. When the label is successfully opened, this member is used to access its properties and execute actions on the label.
        /// </summary>
        /// <value>
        /// The interface to the opened label.
        /// </value>
        public ILabel Label
        {
            get
            {
                return this.label;
            }

            set
            {
                if (this.label != value)
                {
                    this.label = value;
                    this.label.PrintSettings.MonitorSpoolJobStatus = true;
                    this.SelectedVariable = null;

                    this.NotifyPropertyChanged("LabelSize");
                    this.NotifyPropertyChanged("OriginalPrinter");
                    this.NotifyPropertyChanged("PaperSize");
                    this.NotifyPropertyChanged("LabelOffset");
                    this.NotifyPropertyChanged("LabelsAcross");
                    this.NotifyPropertyChanged("Gap");
                    this.NotifyPropertyChanged("Variables");
                    this.NotifyPropertyChanged("Preview");
                    this.NotifyPropertyChanged("LabelFileName");
                }
            }
        }

        /// <summary>
        /// Gets the list of variables available in the opened label. This property is bound to the variable list UI element.
        /// </summary>
        /// <value>
        /// The variables list when the label is opened, null otherwise.
        /// </value>
        public IList<IVariable> Variables
        {
            get
            {
                if (this.label == null)
                {
                    return null;
                }

                return this.label.Variables;
            }
        }

        /// <summary>
        /// Gets or sets the selected variable. This property is bound to the selected item of the variable list UI element.
        /// </summary>
        /// <value>
        /// The selected variable.
        /// </value>
        public IVariable SelectedVariable
        {
            get
            {
                return this.selectedVariable;
            }

            set
            {
                if (this.selectedVariable != value)
                {
                    this.selectedVariable = value;

                    this.UpdateVariableValues();

                    this.NotifyPropertyChanged("SelectedVariable");
                }
            }
        }

        /// <summary>
        /// Gets the print requests.
        /// </summary>
        /// <value>
        /// The print requests.
        /// </value>
        public IList<IPrintRequest> PrintRequests { get; private set; }

        /// <summary>
        /// Gets or sets the session print request.
        /// </summary>
        /// <value>
        /// The session print request.
        /// </value>
        public ISessionPrintRequest SessionPrintRequest
        {
            get
            {
                return this.sessionPrintRequest;
            }

            set
            {
                this.sessionPrintRequest = value;
                this.NotifyPropertyChanged("SessionPrintRequest");
            }
        }

        /// <summary>
        /// Gets or sets the variable value. This member is bound to the UI control where the user can see and update variable value.
        /// </summary>
        /// <value>
        /// The variable value.
        /// </value>
        public string VariableValue
        {
            get
            {
                return this.variableValue;
            }

            set
            {
                if (this.variableValue != value)
                {
                    this.variableValue = value;
                    this.NotifyPropertyChanged("VariableValue");
                }

                this.NotifyPropertyChanged("VariableValueDescription");
            }
        }

        /// <summary>
        /// Gets the variable value description. This member is bound to the UI control where the user can see and update variable value description.
        /// </summary>
        /// <value>
        /// The variable value with additional information when the value is required.
        /// </value>
        public string VariableValueDescription
        {
            get
            {
                if (this.selectedVariable == null)
                {
                    return string.Empty;
                }

                return this.selectedVariable.CurrentValue + (this.selectedVariable.IsValueRequired ? " (required)" : string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the variable name. This member is bound to the UI control where the user can see and update variable name.
        /// </summary>
        /// <value>
        /// The variable name.
        /// </value>
        public string VariableName
        {
            get
            {
                return this.variableName;
            }

            set
            {
                if (this.variableName != value)
                {
                    this.variableName = value;

                    this.NotifyPropertyChanged("VariableName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the variable description. This member is bound to the UI control where the user can see and update variable description.
        /// </summary>
        /// <value>
        /// The variable description.
        /// </value>
        public string VariableDescription
        {
            get
            {
                return this.variableDescription;
            }

            set
            {
                if (this.variableDescription != value)
                {
                    this.variableDescription = value;

                    this.NotifyPropertyChanged("VariableDescription");
                }
            }
        }

        /// <summary>
        /// Gets or sets the variable length. This member is bound to the UI control where the user can see and update variable length.
        /// </summary>
        /// <value>
        /// The variable length.
        /// </value>
        public string VariableLength
        {
            get
            {
                return this.variableLength;
            }

            set
            {
                if (this.variableLength != value)
                {
                    this.variableLength = value;

                    this.NotifyPropertyChanged("VariableLength");
                }
            }
        }

        /// <summary>
        /// Gets or sets the variable serialization. This member is bound to the UI control where the user can see and update variable serialization.
        /// </summary>
        /// <value>
        /// The variable serialization.
        /// </value>
        public string VariableSerialization
        {
            get
            {
                return this.variableSerialization;
            }

            set
            {
                if (this.variableSerialization != value)
                {
                    this.variableSerialization = value;

                    this.NotifyPropertyChanged("VariableSerialization");
                }
            }
        }

        /// <summary>
        /// Gets or sets the variable format. This member is bound to the UI control where the user can see and update variable format.
        /// </summary>
        /// <value>
        /// The variable format.
        /// </value>
        public string VariableFormat
        {
            get
            {
                return this.variableFormat;
            }

            set
            {
                if (this.variableFormat != value)
                {
                    this.variableFormat = value;

                    this.NotifyPropertyChanged("VariableFormat");
                }
            }
        }

        /// <summary>
        /// Gets or sets the variable used indicator. This member is bound to the UI control where the user can see and update variable used indicator.
        /// </summary>
        /// <value>
        /// The variable used indicator.
        /// </value>
        public string VariableIsUsed
        {
            get
            {
                return this.variableIsUsed;
            }

            set
            {
                if (this.variableIsUsed != value)
                {
                    this.variableIsUsed = value;

                    this.NotifyPropertyChanged("VariableIsUsed");
                }
            }
        }

        /// <summary>
        /// Gets the size of the label.
        /// </summary>
        /// <value>
        /// The size of the label.
        /// </value>
        public string LabelSize
        {
            get
            {
                if (this.label != null)
                {
                    return this.label.LabelSettings.Width.ToString() + ',' + this.label.LabelSettings.Height.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the original printer name of the label.
        /// </summary>
        /// <value>
        /// The original printer name.
        /// </value>
        public string OriginalPrinter
        {
            get
            {
                if (this.label != null)
                {
                    return this.label.LabelSettings.OriginalPrinterName;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether label preview will be stored to a file.
        /// </summary>
        /// <value>
        /// <c>True</c> if label preview is stored to a file; otherwise, <c>false</c>.
        /// </value>
        public bool PreviewToFile
        {
            get
            {
                return this.previewToFile;
            }

            set
            {
                if (this.previewToFile != value)
                {
                    this.previewToFile = value;
                    this.NotifyPropertyChanged("PreviewToFile");
                    this.NotifyPropertyChanged("Preview");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether label preview shows sample values for data sources.
        /// </summary>
        /// <value>
        /// <c>True</c> if label preview shows sample values for data sources; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSampleValues
        {
            get
            {
                return this.showSampleValues;
            }

            set
            {
                if (this.showSampleValues != value)
                {
                    this.showSampleValues = value;
                    this.NotifyPropertyChanged("ShowSampleValues");
                    this.NotifyPropertyChanged("Preview");
                }
            }
        }

        /// <summary>
        /// Gets the selected label preview. 
        /// Label preview result is either image byte array or path to the stored label preview file, depending on the PreviewToFile property.
        /// </summary>
        /// <value>
        /// The preview path.
        /// </value>
        public object Preview
        {
            get
            {
                if (this.label != null)
                {
                    try
                    {
                        ILabelPreviewSettings settings = new LabelPreviewSettings();
                        settings.PreviewToFile = this.PreviewToFile;
                        settings.ImageFormat = "png";
                        settings.UseDefaultSize = true;
                        settings.ShowSampleValues = this.ShowSampleValues;

                        if (this.PreviewToFile)
                        {
                            string tempPath = Path.GetTempPath();
                            string file = Guid.NewGuid().ToString() + "." + settings.ImageFormat;

                            settings.Destination = Path.Combine(tempPath, file);
                        }

                        this.preview = this.Label.GetLabelPreview(settings);
                    }
                    catch (SDKException ex)
                    {
                        ErrorHandler.ReportError(ex);
                    }
                }

                return this.preview;
            }
        }

        /// <summary>
        /// Gets the size of the paper.
        /// </summary>
        /// <value>
        /// The size of the paper.
        /// </value>
        public string PaperSize
        {
            get
            {
                if (this.label != null)
                {
                    return this.label.LabelSettings.PageLayout.MediaWidth.ToString() + "," + this.label.LabelSettings.PageLayout.MediaHeight.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the label offset.
        /// </summary>
        /// <value>
        /// The label offset.
        /// </value>
        public string LabelOffset
        {
            get
            {
                if (this.label != null)
                {
                    return this.label.LabelSettings.PageLayout.HorizontalOffset.ToString() + "," + this.label.LabelSettings.PageLayout.VerticalOffset.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the labels across.
        /// </summary>
        /// <value>
        /// The labels across.
        /// </value>
        public string LabelsAcross
        {
            get
            {
                if (this.label != null)
                {
                    return this.label.LabelSettings.PageLayout.Columns.ToString() + "," + this.label.LabelSettings.PageLayout.Rows.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the gap.
        /// </summary>
        /// <value>
        /// The value of the horizontal and vertical gaps.
        /// </value>
        public string Gap
        {
            get
            {
                if (this.label != null)
                {
                    return this.label.LabelSettings.PageLayout.HorizontalGap.ToString() + "," + this.label.LabelSettings.PageLayout.VerticalGap.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the main Print engine object.
        /// </summary>
        /// <value>
        /// The PrintEngine object.
        /// </value>
        public IPrintEngine PrintEngine
        {
            get
            {
                if (this.printEngine == null)
                {
                    string sdkFilesPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\SDKFiles");
                    if (Directory.Exists(sdkFilesPath))
                    {
                        PrintEngineFactory.SDKFilesPath = sdkFilesPath;
                    }

                    this.printEngine = NiceLabel.SDK.PrintEngineFactory.PrintEngine;
                }

                return this.printEngine;
            }
        }

        #endregion

        /// <summary>
        /// Gets the file name only from the full file name.
        /// </summary>
        /// <value>
        /// The label file name only.
        /// </value>
        public string LabelFileNameOnly
        {
            get
            {
                return Path.GetFileName(this.labelFileName);
            }
        }

        /// <summary>
        /// Gets the file system path or the document storage location of the current label file.
        /// </summary>
        /// <value>
        /// The label file location.
        /// </value>
        public string LabelFileLocation
        {
            get
            {
                if (this.UseControlCenter)
                {
                    return this.browseEPMViewModel.ControlCenterUrl + this.browseEPMViewModel.CurrentDirectory;
                }
                else
                {
                    return Path.GetDirectoryName(this.labelFileName);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Control Center's Document Storage Server or the file system should be used for browsing labels.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use control center]; otherwise, <c>false</c>.
        /// </value>
        public bool UseControlCenter 
        {  
            get
            {
                return this.useControlCenter;
            }

            set
            {
                this.useControlCenter = value;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the variable values.
        /// </summary>
        internal void UpdateVariableValues()
        {
            if (this.selectedVariable != null)
            {
                this.VariableValue = this.selectedVariable.CurrentValue;
                this.VariableName = this.selectedVariable.Name;
                this.VariableDescription = this.selectedVariable.Description;

                if (this.selectedVariable.Length == 0)
                {
                    this.VariableLength = "Unlimited";
                }
                else
                {
                    this.VariableLength = this.selectedVariable.Length.ToString() + (this.selectedVariable.IsFixedLength ? " (fixed length)" : string.Empty);
                }

                string serialization;

                switch (this.selectedVariable.IncrementType)
                {
                    case 1:
                        serialization = "Increment";
                        break;
                    case 2:
                        serialization = "Decrement";
                        break;
                    default:
                        serialization = "No serialization";
                        break;
                }

                if (this.selectedVariable.IncrementType != 0)
                {
                    serialization += " " + this.selectedVariable.IncrementStep.ToString() + " every " + this.selectedVariable.IncrementCount.ToString();
                }

                this.VariableSerialization = serialization;

                this.VariableIsUsed = this.selectedVariable.IsUsed ? "Yes" : "No";

                switch (this.selectedVariable.Format)
                {
                    case 0:
                        this.VariableFormat = "All";
                        break;
                    case 1:
                        this.VariableFormat = "Numeric";
                        break;
                    case 2:
                        this.VariableFormat = "Alphanumeric";
                        break;
                    case 3:
                        this.VariableFormat = "Letters";
                        break;
                    case 4:
                        this.VariableFormat = "Seven bit";
                        break;
                    case 5:
                        this.VariableFormat = "Hexadecimal";
                        break;
                    default:
                        this.VariableFormat = "Other";
                        break;
                }
            }
            else
            {
                this.VariableValue = string.Empty;
                this.VariableName = string.Empty;
                this.VariableDescription = string.Empty;
                this.VariableLength = string.Empty;
                this.VariableSerialization = string.Empty;
                this.VariableIsUsed = string.Empty;
                this.VariableFormat = string.Empty;
            }
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Displays either the ControlCenterBrowser window or an OpenFileDialog control.  If user selects a file, PrintEngine opens the file.  This method is called when the BrowseForLabelCommand is executed.
        /// </summary>
        private void BrowseForLabel()
        {
            try
            {
                if (this.UseControlCenter)
                {
                    this.ShowControlCenterBrowser();
                }
                else
                {
                    this.ShowFileSystemBrowser();
                }
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Shows the Control Center browser for opening labels from Control Center's Document Storage Server.
        /// </summary>
        private void ShowControlCenterBrowser()
        {
            if (this.browseEPMViewModel == null)
            {
                this.browseEPMViewModel = new BrowseDocumentStorageViewModel(this);
            }

            BrowseDocumentStorageWindow browseEPMWindow = new BrowseDocumentStorageWindow();
            browseEPMWindow.Owner = System.Windows.Application.Current.MainWindow;
            browseEPMWindow.DataContext = this.browseEPMViewModel;

            if (browseEPMWindow.ShowDialog().Value)
            {
                this.LabelFileName = this.browseEPMViewModel.SelectedLabelFileName;
                this.Label = this.PrintEngine.OpenLabel(this.LabelFileName);
            }
        }

        /// <summary>
        /// Shows an OpenFileDialog control for opening labels from the file system.
        /// </summary>
        private void ShowFileSystemBrowser()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.CheckFileExists = true;
            if (Directory.Exists(Path.GetDirectoryName(this.LabelFileName)))
            {
                openDialog.InitialDirectory = Path.GetDirectoryName(this.LabelFileName);
            }

            openDialog.Filter = "NiceLabel Files (*.nlbl;*.lbl;*.xlbl)|*.nlbl;*.lbl;*.xlbl";

            DialogResult dialogResult = openDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.LabelFileName = openDialog.FileName;
                this.Label = this.PrintEngine.OpenLabel(this.LabelFileName);
                try
                {
                    this.Label.Variables["desc"].SetValue("Test");

                    this.UpdateVariableValues();
                    this.NotifyPropertyChanged("Preview");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Sets the variable's value.  This method is called when the SetVariableCommand is executed.
        /// </summary>
        private void SetVariable()
        {
            try
            {
                this.SelectedVariable.SetValue(this.VariableValue);
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
            finally
            {
                this.UpdateVariableValues();
                this.NotifyPropertyChanged("Preview");
            }
        }

        /// <summary>
        /// Controls when the SetVariableCommand can execute.
        /// </summary>
        /// <returns>True when there is a selected variable, otherwise false.</returns>
        private bool SetVariable_CanExecute()
        {
            return this.SelectedVariable != null;
        }

        /// <summary>
        /// Prints the label.  This method is called when the PrintLabelCommand is executed.
        /// </summary>
        private void PrintLabel()
        {
            try
            {
                ClearPrintRequests();

                this.UpdatePrinter();

                IPrintRequest printRequest;

                if (this.PrintAll)
                {
                    printRequest = this.Label.PrintAll();
                }
                else
                {
                    printRequest = this.Label.Print(int.Parse(this.Quantity));
                }

                this.PrintRequests.Add(printRequest);
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Prints the label asynchronously.  This method is called when the PrintLabelAsyncCommand is executed.
        /// </summary>
        private void PrintLabelAsync()
        {
            try
            {
                this.UpdatePrinter();

                IPrintRequest printRequest;

                if (this.PrintAll)
                {
                    printRequest = this.Label.PrintAllAsync();
                }
                else
                {
                    printRequest = this.Label.PrintAsync(int.Parse(this.Quantity));
                }

                printRequest.PrintJobStatusChanged += this.PrintRequest_PrintJobStatusChanged;
                this.PrintRequests.Add(printRequest);
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Handles the PrintJobStatusChanged event of the printRequest control.  If PrintJobStatus is an error, displays message to user.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PrintRequest_PrintJobStatusChanged(object sender, EventArgs e)
        {
            IPrintRequest printRequest = (IPrintRequest)sender;
            if (printRequest.PrintJobStatus == PrintJobStatus.Error)
            {
                string error = printRequest.PrintException.Message + Environment.NewLine + Environment.NewLine +
                    "Detailed message:" + Environment.NewLine + printRequest.PrintException.DetailedMessage + Environment.NewLine + Environment.NewLine +
                    "Detailed error code:" + Environment.NewLine + printRequest.PrintException.DetailedErrorCode;

                System.Windows.MessageBox.Show(error);
            }
        }

        /// <summary>
        /// Calls the label's StartSessionPrint method.  This method is called when the StartSessionPrintCommand is executed.
        /// </summary>
        private void StartSessionPrint()
        {
            try
            {
                this.UpdatePrinter();

                ISessionPrintRequest sessionPrintRequest = this.Label.StartSessionPrint();
                this.SessionPrintRequest = sessionPrintRequest;
                this.PrintRequests.Add(sessionPrintRequest);
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Controls when the StartSessionPrintCommand can execute.
        /// </summary>
        /// <returns>true when not already in a print session and there is an open label, otherwise false.</returns>
        private bool StartSessionPrint_CanExecute()
        {
            return (this.SessionPrintRequest == null) && (this.Label != null);
        }

        /// <summary>
        /// Calls the label's SessionPrint method.  This method is called when the SessionPrintCommand is executed.
        /// </summary>
        private void SessionPrint()
        {
            try
            {
                this.UpdatePrinter();
                this.Label.SessionPrint(int.Parse(this.Quantity), this.SessionPrintRequest);
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Controls when the SessionPrintCommand can execute.
        /// </summary>
        /// <returns>True when in a print session and there is an open label, otherwise false.</returns>
        private bool SessionPrint_CanExecute()
        {
            return (this.SessionPrintRequest != null) && (this.Label != null);
        }

        /// <summary>
        /// Ends the session print.  This method is called when the EndSessionPrintCommand is executed.
        /// </summary>
        private void EndSessionPrint()
        {
            try
            {
                this.UpdatePrinter();

                this.Label.EndSessionPrint(this.SessionPrintRequest);
                this.SessionPrintRequest = null;
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Controls when the EndSessionPrintCommand can execute.
        /// </summary>
        /// <returns>True when in a print session and there is an open label, otherwise false.</returns>
        private bool EndSessionPrint_CanExecute()
        {
            return (this.SessionPrintRequest != null) && (this.Label != null);
        }

        /// <summary>
        /// Opens the print to graphics window.  This method is called when the OpenPrintToGraphicsCommand is executed.
        /// </summary>
        private void OpenPrintToGraphicsWindow()
        {
            try
            {
                this.UpdatePrinter();

                PrintToGraphicsWindow resultWindow = new PrintToGraphicsWindow();
                resultWindow.DataContext = new PrintToGraphicsViewModel(this);
                resultWindow.Owner = System.Windows.Application.Current.MainWindow;
                resultWindow.ShowDialog();
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Opens the load variable values window.  This method is called when the OpenLoadVariableValuesWindowCommand is executed.
        /// </summary>
        private void OpenLoadVariableValuesWindow()
        {
            try
            {
                LoadVariableValuesWindow loadVariableValuesWindow = new LoadVariableValuesWindow();
                loadVariableValuesWindow.DataContext = new LoadVariableValuesViewModel(this);
                loadVariableValuesWindow.Owner = System.Windows.Application.Current.MainWindow;
                loadVariableValuesWindow.ShowDialog();
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Clears the print requests.  This method is called when the ClearPrintRequestsCommand is executed.
        /// </summary>
        private void ClearPrintRequests()
        {
            foreach (IPrintRequest printRequest in this.PrintRequests)
            {
                printRequest.Dispose();
            }

            this.PrintRequests.Clear();
        }

        /// <summary>
        /// Controls when the ClearPrintRequestsCommand can execute.
        /// </summary>
        /// <returns>True when not in a print session and there are existing print requests.</returns>
        private bool ClearPrintRequests_CanExecute()
        {
            return this.SessionPrintRequest == null && this.PrintRequests.Count > 0;
        }

        /// <summary>
        /// Controls when the Commands that require an open label can execute.
        /// </summary>
        /// <returns>true when there is an open label, otherwise false.</returns>
        private bool LabelDependentCommand_CanExecute()
        {
            return this.Label != null;
        }

        /// <summary>
        /// Updates the PrinterName property for the label.
        /// </summary>
        private void UpdatePrinter()
        {
            if (this.SelectedPrinter != null)
            {
                this.Label.PrintSettings.PrinterName = this.SelectedPrinter.Name;
            }
        }

        #endregion

        public static void BarcodeUP(string barcode)
        {
            
        }

        public static void BarcodeChoose(string name, string art, string barcode, int count,int mode, NiceLabel.SDK.MainWindowViewModel f)
        {
            f.DrawLabel(name, art, barcode, count, mode);
        }
        
        public void DrawLabel(string name, string art, string barcode, int count, int mode)
        {          
            try
            {
                bool flagToContinue = true;

                if (mode == 0 && Properties.Settings.Default.PathTransLabel!="")
                {
                    if (File.Exists(Properties.Settings.Default.PathTransLabel))
                    {
                        this.LabelFileName = Properties.Settings.Default.PathTransLabel;
                    }
                    else
                    {
                        flagToContinue = false;
                        MessageBox.Show("Проверьте путь к шаблону.", "Ошибка");
                    }
                }
                else if (mode == 1 && Properties.Settings.Default.PathIndividualLabel != "")
                {
                    if (File.Exists(Properties.Settings.Default.PathIndividualLabel))
                    {
                        this.LabelFileName = Properties.Settings.Default.PathIndividualLabel;
                    }
                    else 
                    {
                        flagToContinue = false;
                        MessageBox.Show("Проверьте путь к шаблону.", "Ошибка");
                    }
                }
                else if (Properties.Settings.Default.PathIndividualWYLabel != "")
                {
                    if (File.Exists(Properties.Settings.Default.PathIndividualWYLabel))
                    {
                        this.LabelFileName = Properties.Settings.Default.PathIndividualWYLabel;
                    }
                    else
                    {
                        flagToContinue = false;
                        MessageBox.Show("Проверьте путь к шаблону.", "Ошибка");
                    }
                }
                else { flagToContinue = false; }

                if (flagToContinue)
                {
                    this.Label = this.PrintEngine.OpenLabel(this.LabelFileName);

                    if (NiceLabel.SDK.DemoApp.MainWindow.artToLowFlag == false)
                    {
                        this.Label.Variables["art"].SetValue(art);
                    }
                    else 
                    {
                        art = art.ToLower();
                        this.Label.Variables["art"].SetValue(art);
                    }
                    this.Label.Variables["barcode"].SetValue(barcode);

                    if (mode == 0)
                    {
                        this.Label.Variables["count"].SetValue(Convert.ToString(count));
                    }

                    this.Label.Variables["name"].SetValue(name);

                    this.UpdateVariableValues();
                    this.NotifyPropertyChanged("Preview");
                }
                else 
                {
                    MessageBox.Show("Некорректные настройки");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Функция возвращаю переменные шаблона
        /// </summary>
        /// <param name="LabelPath"></param>
        /// <returns></returns>
        public List<string> GetLabelVariables(string LabelPath, out string ErrorDesc)
        {
            List<string> LabelsList = new List<string>();
            ErrorDesc = "";

            if (File.Exists(LabelPath))
            {
                try
                {
                    this.Label = this.PrintEngine.OpenLabel(LabelPath);

                    foreach (IVariable f in this.Label.Variables)
                    {
                        string c = f.Name.ToString();
                        LabelsList.Add(c);
                    }
                }
                catch
                {
                    ErrorDesc = "LabelError";
                }
            }
            else
            {
                ErrorDesc = "FileAccessError";
            }

            
            return LabelsList;
        }
    }
}