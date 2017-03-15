using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.DTO
{
   /// <summary>
   ///    One path element
   /// </summary>
   public enum PathElement
   {
      Simulation,
      TopContainer,
      Container,
      BottomCompartment,
      Molecule,
      Name
   }

   /// <summary>
   ///    A cache returning for each path element the corresonding <see cref="PathElementDTO" />
   /// </summary>
   public class PathElements : Cache<PathElement, PathElementDTO>
   {
      /// <summary>
      /// Allows for categorization of <see cref="PathElements"/> in the UI.
      /// </summary>
      public string Category { get; set; }

      public PathElements() : base(x => new PathElementDTO())
      {
         Category = string.Empty;
      }
   }
}