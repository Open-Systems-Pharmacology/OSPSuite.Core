using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serializers
{
   public class concern_for_PopulationSimulationPKAnalyses : ModelingXmlSerializerBaseSpecs
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
         x2.PKParameterFor("Path1", "AUC").Count.ShouldBeEqualTo(_numberOfIndividuals);
      }

      [Observation]
      public void should_return_the_expected_path_values()
      {
         var sut = new PopulationSimulationPKAnalyses();
         sut.AddPKAnalysis(createPKAnalyses("Path1"));
         sut.AddPKAnalysis(createPKAnalyses("Path2"));

         sut.AllQuantityPaths.ShouldOnlyContain("Path1", "Path2");
      }

      private QuantityPKParameter createPKAnalyses(string path)
      {
         var pk = new QuantityPKParameter {QuantityPath = path};
         pk.Name = "AUC";

         for (int i = 0; i < _numberOfIndividuals; i++)
         {
            pk.SetValue(i, (float) _random.NextDouble() * 100);
         }

         return pk;
      }
   }
}