using System.Collections.ObjectModel;

namespace Detached.Modules
{
    public class DetachedModuleCollection : Collection<DetachedModule>
    {
        readonly DetachedApplication _app;

        public DetachedModuleCollection(DetachedApplication app)
        {
            _app = app;
        }

        protected override void InsertItem(int index, DetachedModule item)
        {
            base.InsertItem(index, item);
            item.Application = _app;
        }

        protected override void SetItem(int index, DetachedModule item)
        {
            base.SetItem(index, item);
            item.Application = _app;
        }
    }
}