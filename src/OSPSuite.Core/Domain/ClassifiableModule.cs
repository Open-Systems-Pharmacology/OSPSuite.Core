namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    A wrapper for <see cref="Module"/> which can resolve a classification tree
   /// </summary>
   public class ClassifiableModule : Classifiable<Module>, IWithId
   {
      public ClassifiableModule() : base(ClassificationType.Module)
      {
      }

      public string Icon => Subject.Icon;
   }
}