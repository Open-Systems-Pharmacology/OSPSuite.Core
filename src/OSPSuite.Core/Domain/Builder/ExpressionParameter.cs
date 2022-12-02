using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionParameter : PathAndValueEntity, IStartValue
   {
      /// <summary>
      /// Do not use! When refactoring on promotion to core, this should be removed
      /// </summary>
      public double? StartValue { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         if (!(source is ExpressionParameter sourceExpressionParameter)) 
            return;

         StartValue = sourceExpressionParameter.StartValue;
      }
   }
}
