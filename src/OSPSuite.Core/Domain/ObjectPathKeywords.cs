using System.Collections.Generic;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    keywords used in object paths to
   /// </summary>
   public static class ObjectPathKeywords
   {
      //list of all keywords defined
      private static readonly IList<string> _allKeywords = new List<string>();

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
      ///    String representing a reference to the Neighborhood there the transport is created
      /// </summary>
      public static readonly string NEIGHBORHOOD = addKeyword("NEIGHBORHOOD");

      /// <summary>
      ///    String representing a reference to  the localized realization of a transport process
      /// </summary>
      public static readonly string REALIZATION = addKeyword("REALIZATION");

      /// <summary>
      ///    String representing a reference to  the transporter molecule in a transpot process
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

      private static string addKeyword(string keyword)
      {
         _allKeywords.Add(keyword);
         return keyword;
      }

      public static IEnumerable<string> All => _allKeywords;
   }
}