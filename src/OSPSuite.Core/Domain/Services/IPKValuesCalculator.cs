using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain.Services
{
   public interface IPKValuesCalculator
   {
      /// <summary>
      ///    Calculates and returns the pk values for the given [time, concentration] profile. If the dose is defined
      ///    global pk values such as Vss, CL, VD will be availble
      ///    InfusionTime should only be set for a single dosing iv application
      /// </summary>
      /// <param name="time">Time array</param>
      /// <param name="concentration">Concentation array (should have same length as time</param>
      /// <param name="options"></param>
      PKValues CalculatePK(IReadOnlyList<float> time, IReadOnlyList<float> concentration, PKCalculationOptions options = null);

      /// <summary>
      ///    Calculates and returns the pk values for the given [time, concentration] profile. If the dose is defined
      ///    global pk values such as Vss, CL, VD will be availble
      ///    InfusionTime should only be set for a single dosing iv application
      /// </summary>
      PKValues CalculatePK(DataColumn dataColumn, PKCalculationOptions options = null);
   }


}