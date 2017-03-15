using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   public interface IEmptyStartValueCreator<T>
   {
      T CreateEmptyStartValue(IDimension dimension);
   }
}