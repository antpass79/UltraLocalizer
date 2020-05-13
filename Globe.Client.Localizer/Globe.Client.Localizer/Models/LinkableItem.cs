namespace Globe.Client.Localizer.Models
{
    class LinkableItem
    {
        public LinkableItem(EditableStringItem editableStringItem, ConceptViewItem conceptViewItem)
        {
            this.EditableStringItem = editableStringItem;
            this.ConceptViewItem = conceptViewItem;
        }

        public EditableStringItem EditableStringItem { get; }
        public ConceptViewItem ConceptViewItem { get; }
    }
}
