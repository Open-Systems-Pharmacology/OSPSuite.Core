using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serializers
{
   public abstract class concern_for_PopulationSimulationPKAnalyses : ModellingXmlSerializerBaseSpecs
   {
      private readonly int _numberOfIndividuals = 100000;
      private readonly Random _random = new Random();

      [Observation]
      public void TestSerializationPopulationSimulationPKAnalyses()
      {
         var x1 = new PopulationSimulationPKAnalyses();
         x1.AddPKAnalysis(createPKAnalyses("Path1"));
         x1.AddPKAnalysis(createPKAnalyses("Path2"));
         x1.AddPKAnalysis(createPKAnalyses("Path3"));
         x1.AddPKAnalysis(createPKAnalyses("Path4"));
         x1.AddPKAnalysis(createPKAnalyses("Path5"));


         var x2 = SerializeAndDeserialize(x1);

         x2.ShouldNotBeNull();
         x2.All().Count().ShouldBeEqualTo(5);
         x2.PKParameterFor("Path1", "AUC").Values.Length.ShouldBeEqualTo(_numberOfIndividuals);
      }

      private QuantityPKParameter createPKAnalyses(string path)
      {
         var pk = new QuantityPKParameter {QuantityPath = path};
         pk.Name = "AUC";

         pk.SetNumberOfIndividuals(_numberOfIndividuals);
         for (int i = 0; i < _numberOfIndividuals; i++)
         {
            pk.SetValue(i, (float) _random.NextDouble() * 100);
         }

         return pk;
      }
   }
}