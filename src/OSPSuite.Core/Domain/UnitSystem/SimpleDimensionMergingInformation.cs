namespace OSPSuite.Core.Domain.UnitSystem
{
   public interface IDimensionMergingInformation
   {
      IDimension SourceDimension { get; }
      IDimension TargetDimension { get; }
   }

   public class SimpleDimensionMergingInformation : IDimensionMergingInformation
   {
      public IDimension SourceDimension { get; }
      public IDimension TargetDimension { get; }

      public SimpleDimensionMergingInformation(IDimension source, IDimension target)
      {
         SourceDimension = source;
         TargetDimension = target;
      }
   }
}