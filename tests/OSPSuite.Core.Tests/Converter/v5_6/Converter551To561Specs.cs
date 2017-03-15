using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converter.v5_6;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Converter.v5_6
{
   public abstract class concern_for_Converter551To561 : ContextSpecification<Converter551To561>
   {
      protected IModelCoreSimulation _simulation;
      protected IBuildConfiguration _simulationConfiguration;
      protected IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         base.Context();
         sut = new Converter551To561();
      }
   }

   internal class When_converting_a_passiveTransportBuildingBlock : concern_for_Converter551To561
   {
      private IPassiveTransportBuildingBlock _passiveTransportBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _passiveTransportBuildingBlock = new PassiveTransportBuildingBlock();
         var pt1 = new TransportBuilder().WithName("T1");
         new Parameter().WithName("LP").WithMode(ParameterBuildMode.Local).WithParentContainer(pt1);
         new Parameter().WithName("PP").WithMode(ParameterBuildMode.Property).WithParentContainer(pt1);
         new Parameter().WithName("GP").WithMode(ParameterBuildMode.Global).WithParentContainer(pt1);
         _passiveTransportBuildingBlock.Add(pt1);
         var pt2 = new TransportBuilder().WithName("T2");
         new Parameter().WithName("LP").WithMode(ParameterBuildMode.Local).WithParentContainer(pt2);
         new Parameter().WithName("PP").WithMode(ParameterBuildMode.Property).WithParentContainer(pt2);
         new Parameter().WithName("GP").WithMode(ParameterBuildMode.Global).WithParentContainer(pt2);
         _passiveTransportBuildingBlock.Add(pt2);
      }

      protected override void Because()
      {
         base.Because();
         sut.Convert(_passiveTransportBuildingBlock);
      }

      [Observation]
      public void Should_change_all_non_local_passive_transports_parameters_to_local()
      {
         _passiveTransportBuildingBlock.SelectMany(
            pbt => pbt.GetAllChildren<IParameter>(p => !p.BuildMode.Equals(ParameterBuildMode.Local))).ShouldBeEmpty();
      }
   }
}