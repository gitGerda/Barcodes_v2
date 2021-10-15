namespace NiceLabel.SDK
{
    public class GetDocumentsCommand : AbstractDocumentStorageCommand
    {
        public GetDocumentsCommand(BrowseDocumentStorageViewModel viewModel)
            : base(viewModel)
        {
        }

        public override void Execute(object parameter)
        {
            this.viewModel.Documents.Clear();
            PrintEngine.Instance.ControlCenterProperties.ControlCenterUrl = this.viewModel.ControlCenterUrl;

            try
            {
                foreach (IDocument document in this.documentStorage.GetDocuments(this.viewModel.CurrentDirectory, true))
                {
                    if (document.IsFolder || document.FileName.ToLower().EndsWith(".lbl"))
                    {
                        this.viewModel.Documents.Add(document);
                    }
                }
            }
            catch (SDKException ex)
            {
                ErrorHandler.ReportError(ex);
            }
        }
    }
}
