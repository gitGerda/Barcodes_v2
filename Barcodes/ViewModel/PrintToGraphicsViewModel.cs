//-----------------------------------------------------------------------
// <copyright file="PrintToGraphicsViewModel.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the PrintToGraphicsViewModel class.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK
{
    using System.Windows.Input;

    /// <summary>
    /// This class handles the Print To Graphics feature.
    /// </summary>
    internal class PrintToGraphicsViewModel : BaseViewModel
    {
        #region Private Fields

        /// <summary>
        /// The backing field for the PrintToGraphicsSettings property.
        /// </summary>
        private IPrintToGraphicsSettings printToGraphicsSettings;

        /// <summary>
        /// The backing field for the PrintToGraphicsResult property.
        /// </summary>
        private IPrintToGraphicsResult printToGraphicsResult;

        /// <summary>
        /// The backing field for the BrowseForOutputFolderCommand property.
        /// </summary>
        private RelayCommand browseForOutputFolderCommand;

        /// <summary>
        /// The backing field for the GenerateGraphicsCommand property.
        /// </summary>
        private RelayCommand generateGraphicsCommand;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintToGraphicsViewModel" /> class.
        /// </summary>
        /// <param name="parentViewModel">The ViewModel of the parent.</param>
        public PrintToGraphicsViewModel(MainWindowViewModel parentViewModel)
            : base(parentViewModel)
        {
            this.ViewModelName = "Print To Graphics";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the print to graphics settings.
        /// </summary>
        /// <value>
        /// The interface to PrintToGraphicsSettings.
        /// </value>
        public IPrintToGraphicsSettings PrintToGraphicsSettings
        {
            get
            {
                if (this.printToGraphicsSettings == null)
                {
                    this.printToGraphicsSettings = new PrintToGraphicsSettings();
                    this.printToGraphicsSettings.Quantity = int.Parse(this.ParentViewModel.Quantity);
                    this.printToGraphicsSettings.PrintAll = this.ParentViewModel.PrintAll;
                    this.printToGraphicsSettings.ImageFormat = "PNG";
                    this.printToGraphicsSettings.PrintToFiles = true;
                }

                return this.printToGraphicsSettings;
            }

            set
            {
                this.printToGraphicsSettings = value;
                this.NotifyPropertyChanged("PrintToGraphicsSettings");
            }
        }

        /// <summary>
        /// Gets or sets the print to graphics result.
        /// </summary>
        /// <value>
        /// The interface to PrintToGraphicsResult.
        /// </value>
        public IPrintToGraphicsResult PrintToGraphicsResult
        {
            get
            {
                return this.printToGraphicsResult;
            }

            set
            {
                this.printToGraphicsResult = value;
                this.NotifyPropertyChanged("PrintToGraphicsResult");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where print to graphics method saves output to the destination folder.
        /// </summary>
        /// <value>
        /// <c>True</c> if print to graphics method saves output to the destination folder; otherwise,  <c>false</c>.
        /// </value>
        public string DestinationFolder
        {
            get
            {
                return this.PrintToGraphicsSettings.DestinationFolder;
            }

            set
            {
                this.PrintToGraphicsSettings.DestinationFolder = value;
                this.NotifyPropertyChanged("DestinationFolder");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether print to graphics returns list of image byte arrays or a list of file names.
        /// </summary>
        /// <value>
        /// <c>True</c> if print to graphics returns list of file names; otherwise, returns list of image byte arrays<c>false</c>.
        /// </value>
        public bool PrintToFiles
        {
            get
            {
                return this.PrintToGraphicsSettings.PrintToFiles;
            }

            set
            {
                this.PrintToGraphicsSettings.PrintToFiles = value;
                this.NotifyPropertyChanged("PrintToFiles");
            }
        }

        /// <summary>
        /// Gets the browse for output folder command.  This command is bound to the "..." button UI element.
        /// </summary>
        /// <value>
        /// The browse for output folder command.
        /// </value>
        public ICommand BrowseForOutputFolderCommand
        {
            get
            {
                if (this.browseForOutputFolderCommand == null)
                {
                    this.browseForOutputFolderCommand = new RelayCommand(p => this.BrowseForOutputFolder());
                }

                return this.browseForOutputFolderCommand;
            }
        }

        /// <summary>
        /// Gets the generate graphics command.  This command is bound to the "Generate graphics" button UI element.
        /// </summary>
        /// <value>
        /// The generate graphics command.
        /// </value>
        public ICommand GenerateGraphicsCommand
        {
            get
            {
                if (this.generateGraphicsCommand == null)
                {
                    this.generateGraphicsCommand = new RelayCommand(p => this.GenerateGraphics(), p => this.GenerateGraphicsCommand_CanExecute());
                }

                return this.generateGraphicsCommand;
            }
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Generates the graphics.  This method is called when the GenerateGraphicsCommand executes.
        /// </summary>
        private void GenerateGraphics()
        {
            try
            {
                this.PrintToGraphicsResult = this.ParentViewModel.Label.PrintToGraphics(this.PrintToGraphicsSettings);
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }

        /// <summary>
        /// Generates the graphics command_ can execute.
        /// </summary>
        /// <returns>True when not printing to files or the DestinationFolder is empty, otherwise false.</returns>
        private bool GenerateGraphicsCommand_CanExecute()
        {
            return !this.PrintToFiles || !string.IsNullOrEmpty(this.DestinationFolder);
        }

        /// <summary>
        /// Browses for output folder.  This method is called when the BrowseForOutputFolderCommand executes.
        /// </summary>
        private void BrowseForOutputFolder()
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.DestinationFolder = folderBrowserDialog.SelectedPath;
            }
        }

        #endregion
    }
}
