namespace OSPSuite.Core.Domain.UnitSystem
{
   public interface IDimensionMergingInformation
   {
      IDimension SourceDimension { get; }
      IDimension TargetDimension { get; }
   }

   public class SimpleDimensionMergingInformation : IDimensionMergingInformation
   {
      public SimpleDimensionMergingInformation(IDimension source, IDimension target)
      {
         SourceDimension = source;
         TargetDimension = target;
      }

      public IDimension SourceDimension { get; private set; }
      public IDimension TargetDimension { get; private set; }
   }
}