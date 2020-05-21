namespace Globe.Client.Localizer.Models
{
    class LinkableItem
    {
        public LinkableItem(EditableContext editableStringItem, StringView conceptViewItem)
        {
            this.EditableStringItem = editableStringItem;
            this.ConceptViewItem = conceptViewItem;
        }

        public EditableContext EditableStringItem { get; }
        public StringView ConceptViewItem { get; }
    }
}
