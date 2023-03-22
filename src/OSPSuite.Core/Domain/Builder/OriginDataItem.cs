namespace OSPSuite.Core.Domain.Builder
{
   public class OriginDataItem : IWithName, IWithDescription
   {
      public string Name { get; set; }
      public string DisplayName { get; set; }
      public string Description { get; set; }
      public string Icon { get; set; }
      public string Value { get; set; }

      public override string ToString() => Name;
   }
}