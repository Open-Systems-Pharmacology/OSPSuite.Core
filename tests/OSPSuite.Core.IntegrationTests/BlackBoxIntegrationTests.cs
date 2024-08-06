using Castle.Components.DictionaryAdapter.Xml;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility.Container;
using System.IO;
using System;

namespace OSPSuite.Core
{


   public class When_loading_the_black_box_simulation : ContextForIntegration<IPKMLPersistor>
   {
      protected SpatialStructure _spatialStructure;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var persistor = IoC.Resolve<IPKMLPersistor>();
         var projectFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data\\BlackBox.pkml");
         _spatialStructure = persistor.Load<SpatialStructure>(projectFile);
      }

      [Observation]
      public void should_have_assigned_the_expected_formula_to_the_black_box_parameters()
      {
         var parameter = _spatialStructure.Neighborhoods.FindByName("Breasts_int_Breasts_cell").MoleculeProperties.Parameter("Partition coefficient (intracellular/plasma)");
         parameter.Formula.Name.ShouldBeEqualTo("PARTITION COEFFICIENT FORMULA");

      }
   }
}