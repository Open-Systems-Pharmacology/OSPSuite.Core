using OSPSuite.Assets;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class PKParameterSensitivity
   {
      /// <summary>
      ///    Path of underlying quantity for which pkanalyses were performed
      /// </summary>
      public virtual string QuantityPath { get; set; }

      /// <summary>
      ///    Name of PK Output (Cmax, Tmax etc...)
      /// </summary>
      public virtual string PKParameterName { get; set; }

      /// <summary>
      /// Unique name of parameter in sensitivity analysis
      /// </summary>
      public virtual string ParameterName { get; set; }

      /// <summary>
      /// Value of sensitivity
      /// </summary>
      public virtual double Value { get; set; }

      public override string ToString()
      {
         return new[] { QuantityPath, PKParameterName, ParameterName }.ToPathString();
      }

      public string PKParameterSelection() => Captions.SensitivityAnalysis.PkParameterOfOutput(PKParameterName, QuantityPath);
   }
}