using System.Collections.ObjectModel;

namespace Detached.Modules
{
    public class ModuleCollection : Collection<Module>
    {
        readonly Application _app;

        public ModuleCollection(Application app)
        {
            _app = app;
        }

        protected override void InsertItem(int index, Module item)
        {
            base.InsertItem(index, item);
            item.Application = _app;
        }

        protected override void SetItem(int index, Module item)
        {
            base.SetItem(index, item);
            item.Application = _app;
        }
    }
}