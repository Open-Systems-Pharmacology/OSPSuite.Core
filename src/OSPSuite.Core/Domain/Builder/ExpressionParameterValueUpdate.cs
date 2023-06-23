using System.Linq;

namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionParameterValueUpdate
   {
      public ExpressionParameterValueUpdate(ObjectPath path)
      {
         Path = path;
      }

      public double UpdatedValue { get; set; }
      public ObjectPath Path { get; }
      public string Name => Path.Last();
   }
}