namespace NiceLabel.SDK
{
    public class SelectLabelCommand : AbstractDocumentStorageCommand
    {
        public SelectLabelCommand(BrowseDocumentStorageViewModel viewModel)
            : base(viewModel)
        {
        }

        public override void Execute(object parameter)
        {
            if (this.viewModel.SelectedDocument == null || this.viewModel.SelectedDocument.IsFolder)
            {
                return;
            }

            this.viewModel.SelectedLabelFileName = this.viewModel.SelectedDocument.FilePath;
            this.viewModel.CloseWindow();
        }
    }
}
