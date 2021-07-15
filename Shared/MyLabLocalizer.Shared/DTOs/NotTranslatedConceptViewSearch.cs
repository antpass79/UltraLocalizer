namespace MyLabLocalizer.Shared.DTOs
{
    public class NotTranslatedConceptViewSearch<TComponent, TInternal>
        where TComponent : ComponentNamespace
        where TInternal : InternalNamespace
    {
        public TComponent ComponentNamespace { get; set; }
        public TInternal InternalNamespace { get; set; }
        public Language Language { get; set; }
    }

    public class NotTranslatedConceptViewSearch : NotTranslatedConceptViewSearch<ComponentNamespace, InternalNamespace>
    {
    }
}
