namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionParameter : PathAndValueEntity
   {
      public bool IsGlobalRelativeExpression()
      {
         return  this.HasGlobalExpressionName();
      }

      public bool IsRelativeExpression()
      {
         return this.HasExpressionName();
      }
   }
}