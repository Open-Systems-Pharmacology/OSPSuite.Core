using System.Collections.Generic;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    keywords used in object paths to
   /// </summary>
   public static class ObjectPathKeywords
   {
      //list of all keywords defined
      private static readonly List<string> _allKeywords = new List<string>();

      /// <summary>
      ///    String representing a reference to local Molecule
      /// </summary>
      public static readonly string MOLECULE = addKeyword("MOLECULE");

      /// <summary>
      ///    String representing a reference to the Source Amount of a Transport
      /// </summary>
      public static readonly string SOURCE = addKeyword("SOURCE");

      /// <summary>
      ///    String representing a reference to the Target Amount of a Transport
      /// </summary>
      public static readonly string TARGET = addKeyword("TARGET");

      /// <summary>
      ///    String representing a reference to the Neighborhood where the transport is created
      /// </summary>
      public static readonly string NEIGHBORHOOD = addKeyword("NEIGHBORHOOD");

      /// <summary>
      ///    String representing a reference to  the localized realization of a transport process
      /// </summary>
      public static readonly string REALIZATION = addKeyword("REALIZATION");

      /// <summary>
      ///    String representing a reference to  the transporter molecule in a transport process
      /// </summary>
      public static readonly string TRANSPORTER = addKeyword("TRANSPORTER");

      /// <summary>
      ///    String representing a reference to  the transporter molecule
      /// </summary>
      public static readonly string FIRST_NEIGHBOR = addKeyword("FIRST_NEIGHBOR");

      /// <summary>
      ///    String representing a reference to  the transporter molecule
      /// </summary>
      public static readonly string SECOND_NEIGHBOR = addKeyword("SECOND_NEIGHBOR");

      /// <summary>
      ///    String representing a reference to the global transport process
      /// </summary>
      public static readonly string TRANSPORT = addKeyword("TRANSPORT");

      /// <summary>
      ///    String representing a reference to all floating molecules. The entry will be duplicated.
      ///    This is typically used in event assignment to change all floating molecules at once
      /// </summary>
      public static readonly string ALL_FLOATING_MOLECULES = addKeyword("ALL_FLOATING_MOLECULES");

      /// <summary>
      ///    Represents the keywords allowing us to identify a neighborhood between two containers
      ///    For example CONTAINER1|/<NBH />CONTAINER2/<NBH /> means the neighborhood between CONTAINER1 and CONTAINER2
      /// </summary>
      public static readonly string NBH = addKeyword("<NBH>");

      /// <summary>
      ///    Search for a compartment with the same name as the last part preceding the keyword under Lumen
      /// </summary>
      /// <example>
      ///    <code>
      ///      ..|..|LUMEN_SEGMENT|MOLECULE => Organism|Lumen|Duodenum|MOLECULE
      ///   </code>
      /// </example>
      public static readonly string LUMEN_SEGMENT = addKeyword("LUMEN_SEGMENT");

      /// <summary>
      ///    Search for the next segment in lumen
      /// </summary>
      /// <example>
      ///    Assuming the formula is used in a parameter in Caecum
      ///    <code>
      ///      ..|LUMEN_NEXT_SEGMENT|P1.. => Organism|Lumen|ColonAscendens|P1
      ///    </code>
      ///    Also works with absolute paths:
      ///    <code>
      ///      Organism|Lumen|Stomach|LUMEN_NEXT_SEGMENT|P1 => Organism|Lumen|Duodenum|P1
      ///    </code>
      /// </example>
      public static readonly string LUMEN_NEXT_SEGMENT = addKeyword("LUMEN_NEXT_SEGMENT");


      /// <summary>
      ///    Search for the previous segment in lumen
      /// </summary>
      /// <example>
      ///    Assuming the formula is used in a parameter in ColonAscendens
      ///    <code>
      ///      ..|LUMEN_PREVIOUS_SEGMENT|P1.. => Organism|Lumen|Caecum|P1
      ///    </code>
      ///    Also works with absolute paths:
      ///    <code>
      ///      Organism|Lumen|ColonAscendens|LUMEN_PREVIOUS_SEGMENT|P1 => Organism|Lumen|Caecum|P1
      ///    </code>
      /// </example>
      public static readonly string LUMEN_PREVIOUS_SEGMENT = addKeyword("LUMEN_PREVIOUS_SEGMENT");

      private static string addKeyword(string keyword)
      {
         _allKeywords.Add(keyword);
         return keyword;
      }

      public static IReadOnlyList<string> All => _allKeywords;
   }
}