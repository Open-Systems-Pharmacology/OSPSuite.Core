namespace OSPSuite.Core.Domain
{
   public static class FormulaUsablePathExtensions
   {
      public static TObjectPath WithAlias<TObjectPath>(this TObjectPath objectPath, string alias) where TObjectPath : FormulaUsablePath
      {
         objectPath.Alias = alias;
         return objectPath;
      }
   }
}