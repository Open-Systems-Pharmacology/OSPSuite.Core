using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.MinimalImplementations
{
    public class LazyLoadTask : ILazyLoadTask
    {
        public void Load<TObject>(TObject objectToLoad) where TObject : class, ILazyLoadable
        {
            //nothing to do
        }
    }
}