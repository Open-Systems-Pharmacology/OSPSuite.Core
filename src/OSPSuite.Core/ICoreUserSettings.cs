namespace OSPSuite.Core
{
   public interface ICoreUserSettings
   {
      /// <summary>
      ///    Specifies how many core can be use by the application when performing parallel computations
      /// </summary>
      int MaximumNumberOfCoresToUse { get; set; }

      /// <summary>
      ///    Number of bin per plot
      /// </summary>
      int NumberOfBins { get; set; }

      /// <summary>
      /// Number of individuals per bin
      /// </summary>
      int NumberOfIndividualsPerBin { get; set; }
   }
}