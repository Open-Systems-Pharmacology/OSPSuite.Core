using OSPSuite.Assets;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public enum PKParameterSensitivityState
   {
      Success,
      FailedToCalculateDefaultPKValue
   }
   
   public class PKParameterSensitivity
   {
      private string _quantityPath;
      private string _id;
      private string _pkParameterName;
      private string _parameterName;

      /// <summary>
      ///    Path of underlying quantity for which pk-analyses were performed
      /// </summary>
      public virtual string QuantityPath
      {
         get => _quantityPath;
         set
         {
            _quantityPath = value;
            updateId();
         }
      }
      
      public PKParameterSensitivityState State { set; get; }

      /// <summary>
      ///    Name of PK Output (Cmax, Tmax etc...)
      /// </summary>
      public virtual string PKParameterName
      {
         get => _pkParameterName;
         set
         {
            _pkParameterName = value;
            updateId();
         }
      }

      /// <summary>
      ///    Unique name of parameter in sensitivity analysis
      /// </summary>
      public virtual string ParameterName
      {
         get => _parameterName;
         set
         {
            _parameterName = value;
            updateId();
         }
      }

      /// <summary>
      ///    Path of underlying parameter. Useful for scripting
      /// </summary>
      public virtual string ParameterPath { get; set; }

      /// <summary>
      ///    Value of sensitivity
      /// </summary>
      public virtual double Value { get; set; }

      public virtual string Id => _id;

      private void updateId()
      {
         _id = new[] {QuantityPath, PKParameterName, ParameterName}.ToPathString();
      }

      public override string ToString() => Id;

      public string PKParameterSelection() => Captions.SensitivityAnalysis.PkParameterOfOutput(PKParameterName, QuantityPath);
   }
}