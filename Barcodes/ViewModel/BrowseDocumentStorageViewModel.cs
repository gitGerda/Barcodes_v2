//-----------------------------------------------------------------------
// <copyright file="BrowseDocumentStorageViewModel.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the BrowseDocumentStorageViewModel class.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// The window used to browse Document Storage for a label.
    /// </summary>
    public class BrowseDocumentStorageViewModel : BaseViewModel
    {
        #region Private Fields

        /// <summary>
        /// Backing field for CurrentDirectory property.
        /// </summary>
        private string currentDirectory;
        
        /// <summary>
        /// Backing field for SelectedDocument property.
        /// </summary>
        private IDocument selectedDocument;

        /// <summary>
        /// Backing field for SelectedRevision property.
        /// </summary>
        private IDocumentRevision selectedRevision;

        /// <summary>
        /// Backing field for GetDocumentsCommand property.
        /// </summary>
        private ICommand getDocumentsCommand;

        /// <summary>
        /// Backing field for SelectLabelCommand property.
        /// </summary>
        private ICommand selectLabelCommand;

        /// <summary>
        /// Backing field for SelectLabelRevisionCommand property.
        /// </summary>
        private ICommand selectLabelRevisionCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BrowseDocumentStorageViewModel class.
        /// </summary>
        /// <param name="parentViewModel">The parent ViewModel.</param>
        public BrowseDocumentStorageViewModel(MainWindowViewModel parentViewModel)
            : base(parentViewModel)
        {
            this.ViewModelName = "Browse Document Storage";

            this.CurrentDirectory = "/";
            this.Documents = new ObservableCollection<IDocument>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Control Center URL.
        /// </summary>
        /// <value>
        /// The Control Center URL.
        /// </value>
        public string ControlCenterUrl
        {
            get
            {
                return this.ParentViewModel.PrintEngine.ControlCenterProperties.ControlCenterUrl;
            }

            set
            {
                if (this.ParentViewModel.PrintEngine.ControlCenterProperties.ControlCenterUrl != value)
                {
                    this.ParentViewModel.PrintEngine.ControlCenterProperties.ControlCenterUrl = value;
                    this.NotifyPropertyChanged("ControlCenterUrl");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Selected Label File Name.
        /// </summary>
        /// <value>The name of the current label file.</value>
        public string SelectedLabelFileName { get; set; }

        /// <summary>
        /// Gets or sets the Current Directory.
        /// </summary>
        /// <value>The current directory name.</value>
        public string CurrentDirectory
        {
            get
            {
                return this.currentDirectory;
            }

            set
            {
                if (this.currentDirectory != value)
                {
                    this.currentDirectory = value;
                    this.NotifyPropertyChanged("CurrentDirectory");
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected document.
        /// </summary>
        /// <value>
        /// The selected document.
        /// </value>
        public IDocument SelectedDocument
        {
            get
            {
                return this.selectedDocument;
            }

            set
            {
                if (this.selectedDocument != value)
                {
                    this.selectedDocument = value;
                    this.NotifyPropertyChanged("SelectedDocument");
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected revision.
        /// </summary>
        /// <value>
        /// The selected revision.
        /// </value>
        public IDocumentRevision SelectedRevision
        {
            get
            {
                return this.selectedRevision;
            }

            set
            {
                if (this.selectedRevision != value)
                {
                    this.selectedRevision = value;
                    this.NotifyPropertyChanged("SelectedRevision");
                }
            }
        }

        /// <summary>
        /// Gets the documents.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        public ObservableCollection<IDocument> Documents { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is print logging enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is print logging enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrintLoggingEnabled
        {
            get
            {
                return this.ParentViewModel.PrintEngine.ControlCenterProperties.IsPrintLoggingEnabled;
            }

            set
            {
                if (this.ParentViewModel.PrintEngine.ControlCenterProperties.IsPrintLoggingEnabled != value)
                {
                    this.ParentViewModel.PrintEngine.ControlCenterProperties.IsPrintLoggingEnabled = value;
                    this.NotifyPropertyChanged("IsPrintLoggingEnabled");
                }
            }
        }

        /// <summary>
        /// Gets the get documents command.
        /// </summary>
        /// <value>
        /// The get documents command.
        /// </value>
        public ICommand GetDocumentsCommand
        {
            get
            {
                if (this.getDocumentsCommand == null)
                {
                    this.getDocumentsCommand = new RelayCommand(p => this.GetDocuments());
                }

                return this.getDocumentsCommand;
            }
        }

        /// <summary>
        /// Gets the select label command.
        /// </summary>
        /// <value>
        /// The select label command.
        /// </value>
        public ICommand SelectLabelCommand
        {
            get
            {
                if (this.selectLabelCommand == null)
                {
                    this.selectLabelCommand = new RelayCommand(p => this.SelectLabel(), p => this.SelectLabel_CanExecute());
                }

                return this.selectLabelCommand;
            }
        }

        /// <summary>
        /// Gets the select label revision command.
        /// </summary>
        /// <value>
        /// The select label revision command.
        /// </value>
        public ICommand SelectLabelRevisionCommand
        {
            get
            {
                if (this.selectLabelRevisionCommand == null)
                {
                    this.selectLabelRevisionCommand = new RelayCommand(p => this.SelectLabelRevision(), p => this.SelectLabelRevision_CanExecute());
                }

                return this.selectLabelRevisionCommand;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the documents from Control Center's Document Storage.
        /// </summary>
        private void GetDocuments()
        {
            this.Documents.Clear();

            try
            {
                IDocumentStorage documentStorage = this.ParentViewModel.PrintEngine.GetDocumentStorage();
                foreach (IDocument document in documentStorage.GetDocuments(this.CurrentDirectory, true))
                {
                    if (document.IsFolder || document.FileName.ToLower().EndsWith(".nlbl") || document.FileName.ToLower().EndsWith(".lbl"))
                    {
                        this.Documents.Add(document);
                    }
                }
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Selects the label.
        /// </summary>
        private void SelectLabel()
        {
            this.SelectedLabelFileName = this.SelectedDocument.FilePath;
            this.CloseWindow();
        }

        /// <summary>
        /// Selects the label_ can execute.
        /// </summary>
        /// <returns>True if the SelectLabelCommand can be executed, otherwise False.</returns>
        private bool SelectLabel_CanExecute()
        {
            return this.SelectedDocument != null && !this.SelectedDocument.IsFolder;
        }

        /// <summary>
        /// Selects the label revision.
        /// </summary>
        private void SelectLabelRevision()
        {
            this.SelectedLabelFileName = this.SelectedRevision.FilePath;
            this.CloseWindow();
        }

        /// <summary>
        /// Selects the label revision_ can execute.
        /// </summary>
        /// <returns>True if the SelectLabelRevisionCommand can be executed, otherwise False.</returns>
        private bool SelectLabelRevision_CanExecute()
        {
            return this.SelectedRevision != null;
        }

        #endregion
    }
}