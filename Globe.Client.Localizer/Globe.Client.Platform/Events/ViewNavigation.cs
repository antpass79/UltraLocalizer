namespace Globe.Client.Platofrm.Events
{
    public class ViewNavigation
    {
        public string ToView { get; }
        public string FromView { get; }

        public ViewNavigation(string toView)
        {
            this.ToView = toView;
        }

        public ViewNavigation(string toView, string fromView)
        {
            this.ToView = toView;
            this.FromView = fromView;
        }
    }
}