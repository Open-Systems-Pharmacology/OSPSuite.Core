namespace OSPSuite.Core.Journal
{
   public interface IRelatedItemDescriptionCreator
   {
      string DescriptionFor<T>(T relatedObject);
   }
}