using System;

namespace OSPSuite.CLI.Core.Services
{
   [Flags]
   public enum SimulationExportMode
   {
      /// <summary>
      ///    Results as json
      /// </summary>
      Json = 1 << 0,

      /// <summary>
      ///    Results as CSV
      /// </summary>
      Csv = 1 << 1,

      /// <summary>
      ///    Sim Model file (simulation used by sim model for Matlab and R)
      /// </summary>
      Xml = 1 << 2,

      /// <summary>
      ///    pkml Model (simulation used by mobi and pksim)
      /// </summary>
      Pkml = 1 << 3,

      /// <summary>
      ///    Export simulation results to excel (Individual simulations only)
      /// </summary>
      Xlsx = 1 << 4,

      All = Json | Csv | Xml | Pkml | Xlsx
   }
}