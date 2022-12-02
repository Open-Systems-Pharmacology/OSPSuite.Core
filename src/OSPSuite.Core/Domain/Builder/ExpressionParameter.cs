using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionParameter : PathAndValueEntity, IStartValue
   {
      /// <summary>
      /// Do not use! When refactoring on promotion to core, this should be removed
      /// </summary>
      public double? StartValue { get; set; }

      public ExpressionParameter(IParameter parameter)
      {
         Name = parameter.Name;

         //not sure we need to be checking hte if else here
         if (parameter.Formula == null)
            StartValue = parameter.Value;
         else
            Formula = parameter.Formula;
         
         Path = new ObjectPath(parameter.EntityPath().ToPathArray());
         Dimension = parameter.Dimension;
         DisplayUnit = parameter.DisplayUnit;
      }

      public ExpressionParameter()
      {
      }
   }
}
