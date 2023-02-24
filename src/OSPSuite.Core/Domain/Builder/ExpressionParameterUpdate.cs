namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionParameterUpdate : IWithName
   {
      public ExpressionParameterUpdate(ExpressionParameter expressionParameter)
      {
         Path = expressionParameter.Path;
         OriginalValue = expressionParameter.Value;
         UpdatedValue = expressionParameter.Value;
         Name = expressionParameter.Name;
         ContainerPath = expressionParameter.ContainerPath;
      }

      public double? UpdatedValue { get; set; }
      public ObjectPath Path { get; }
      public double? OriginalValue { get; }
      public string Name { get; set; }
      public string ContainerPath { get; }
   }
}