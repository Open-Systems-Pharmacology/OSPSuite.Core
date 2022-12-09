using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IStartValue : IUsingFormula, IWithDisplayUnit, IWithPath, IWithValueOrigin, IWithNullableValue
   {
      /// <summary>
      ///    Obsolete for serialization only
      /// </summary>
      double? StartValue { get; set; }

      IObjectPath ContainerPath { get; set; }
   }

   public abstract class StartValueBase : PathAndValueEntity, IStartValue
   {
      public ValueOrigin ValueOrigin { get; }
 
      protected StartValueBase()
      {
         ValueOrigin = new ValueOrigin();
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceStartValue = source as StartValueBase;
         if (sourceStartValue == null) return;


         ValueOrigin.UpdateAllFrom(sourceStartValue.ValueOrigin);
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         if(Equals(ValueOrigin, sourceValueOrigin))
            return;

         ValueOrigin.UpdateFrom(sourceValueOrigin);
         OnPropertyChanged(() => ValueOrigin);
      }

      public override string ToString()
      {
         return $"Path={ContainerPath}, Name={Name}";
      }

      public double? StartValue { 
         get => Value;
         set => Value = value;
      }
   }
}