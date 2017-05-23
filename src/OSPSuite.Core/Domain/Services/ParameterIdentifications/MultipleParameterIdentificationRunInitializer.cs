using System;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Maths.Random;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class MultipleParameterIdentificationRunInitializer : ParameterIdentifcationRunInitializer
   {
      private RandomGenerator _randomGenerator;

      public MultipleParameterIdentificationRunInitializer(ICloneManagerForModel cloneManager, IParameterIdentificationRun parameterIdentificationRun) : base(cloneManager, parameterIdentificationRun)
      {
      }

      public void Initialize(ParameterIdentification parameterIdentification, int runIndex, RandomGenerator randomGenerator)
      {
         Initialize(parameterIdentification, runIndex);
         _randomGenerator = randomGenerator;
      }

      public override Task<ParameterIdentification> InitializeRun()
      {
         return Task.Run(() =>
         {
            var newParameterIdentification = _cloneManager.Clone(ParameterIdentification);
            //Index is zero based
            newParameterIdentification.Description = Captions.ParameterIdentification.RandomStartValueRunNameFor(RunIndex + 1);
            randomizeStartValuesFor(newParameterIdentification);
            return newParameterIdentification;
         });
      }

      private void randomizeStartValuesFor(ParameterIdentification parameterIdentification)
      {
         parameterIdentification.AllVariableIdentificationParameters.Each(x =>
         {
            x.StartValueParameter.Value = generatedRandomStartValueFor(x);
         });
      }

      private double generatedRandomStartValueFor(IdentificationParameter identificationParameter)
      {
         double minValue = identificationParameter.MinValueParameter.Value;
         double maxValue = identificationParameter.MaxValueParameter.Value;

         if (identificationParameter.Scaling == Scalings.Log)
         {
            minValue = Math.Log10(minValue);
            maxValue = Math.Log10(maxValue);
         }

         var value = _randomGenerator.UniformDeviate(minValue, maxValue);

         if (identificationParameter.Scaling == Scalings.Log)
            value = Math.Pow(10, value);

         return value;
      }
   }
}