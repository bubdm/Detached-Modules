using System.Collections.ObjectModel;

namespace Detached.Modules
{
    public class DetachedComponentCollection : Collection<DetachedComponent>
    {
        readonly DetachedModule _module;

        public DetachedComponentCollection(DetachedModule module)
        {
            _module = module;
        }

        protected override void InsertItem(int index, DetachedComponent item)
        {
            base.InsertItem(index, item);

            item.Module = _module;
        }

        protected override void SetItem(int index, DetachedComponent item)
        {
            base.SetItem(index, item);

            item.Module = _module;
        }
    }
}