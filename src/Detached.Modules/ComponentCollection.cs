using System.Collections.ObjectModel;

namespace Detached.Modules
{
    public class ComponentCollection : Collection<Component>
    {
        readonly Module _module;

        public ComponentCollection(Module module)
        {
            _module = module;
        }

        protected override void InsertItem(int index, Component item)
        {
            base.InsertItem(index, item);

            item.Module = _module;
        }

        protected override void SetItem(int index, Component item)
        {
            base.SetItem(index, item);

            item.Module = _module;
        }
    }
}