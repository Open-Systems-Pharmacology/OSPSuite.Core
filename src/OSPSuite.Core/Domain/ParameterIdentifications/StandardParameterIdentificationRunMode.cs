using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class StandardParameterIdentificationRunMode : ParameterIdentificationRunMode
   {
      public StandardParameterIdentificationRunMode() : base(Captions.ParameterIdentification.RunModes.Standard, isSingleRun: true)
      {
      }

      protected override ParameterIdentificationRunMode CreateClone()
      {
         return new StandardParameterIdentificationRunMode();
      }
   }
}