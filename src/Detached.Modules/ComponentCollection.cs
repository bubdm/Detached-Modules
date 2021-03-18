using System.Collections.ObjectModel;

namespace Detached.Modules
{
    public class ComponentCollection : Collection<IComponent>
    {
        readonly IModule _module;

        public ComponentCollection(IModule module)
        {
            _module = module;
        }

        protected override void InsertItem(int index, IComponent item)
        {
            base.InsertItem(index, item);

            item.Module = _module;
        }

        protected override void SetItem(int index, IComponent item)
        {
            base.SetItem(index, item);

            item.Module = _module;
        }
    }
}