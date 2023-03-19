using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   public interface IEmptyStartValueCreator<out T>
   {
      T CreateEmptyStartValue(IDimension dimension);
   }
}