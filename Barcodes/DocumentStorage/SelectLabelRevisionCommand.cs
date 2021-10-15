namespace NiceLabel.SDK
{
    public class SelectLabelRevisionCommand : AbstractDocumentStorageCommand
    {
        public SelectLabelRevisionCommand(BrowseDocumentStorageViewModel viewModel)
            : base(viewModel)
        {
        }

        public override void Execute(object parameter)
        {
            if (this.viewModel.SelectedRevision == null)
            {
                return;
            }

            this.viewModel.SelectedLabelFileName = this.viewModel.SelectedRevision.FilePath;
            this.viewModel.CloseWindow();
        }
    }
}
