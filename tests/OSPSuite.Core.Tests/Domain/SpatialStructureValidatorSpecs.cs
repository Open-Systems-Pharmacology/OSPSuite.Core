using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_SpatialStructureValidator : ContextSpecification<SpatialStructureValidator>
   {
      protected ModelConfiguration _modelConfiguration;
      protected IModel _model;
      protected SimulationConfiguration _simulationConfiguration;
      protected SimulationBuilder _simulationBuilder;
      protected Neighborhood _neighborhood1;
      protected Neighborhood _neighborhood2;
      protected IContainer _liver;
      protected IContainer _kidney;
      protected IContainer _heart;
      protected ObjectPathFactoryForSpecs _objectPathFactory;

      protected override void Context()
      {
         _model = new Model();
         _model.Root = new Container().WithName("ROOT");
         _liver = new Container().WithName("Liver");
         _kidney = new Container().WithName("Kidney");
         _heart = new Container().WithName("Heart");
         _model.Root.Add(_liver);
         _model.Root.Add(_kidney);
         _model.Root.Add(_heart);
         _model.Neighborhoods = new Container();
         _neighborhood1 = new Neighborhood {Name = "Neighborhood1"};
         _neighborhood2 = new Neighborhood {Name = "Neighborhood2"};
         _model.Neighborhoods.Add(_neighborhood1);
         _model.Neighborhoods.Add(_neighborhood2);
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
         _modelConfiguration = new ModelConfiguration(_model, _simulationConfiguration, _simulationBuilder);
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         sut = new SpatialStructureValidator();
      }
   }

   internal class When_validating_a_model_for_which_all_neighborhoods_can_be_resolved : concern_for_SpatialStructureValidator
   {
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _neighborhood1.FirstNeighbor = _liver;
         _neighborhood1.SecondNeighbor = _kidney;

         _neighborhood1.FirstNeighbor.Mode = ContainerMode.Physical;
         _neighborhood1.SecondNeighbor.Mode = ContainerMode.Physical;

         _neighborhood2.FirstNeighbor = _kidney;
         _neighborhood2.SecondNeighbor = _heart;
         _neighborhood2.FirstNeighbor.Mode = ContainerMode.Physical;
         _neighborhood2.SecondNeighbor.Mode = ContainerMode.Physical;

      }

      protected override void Because()
      {
         _result = sut.Validate(_modelConfiguration);
      }

      [Observation]
      public void should_return_a_valid_validation_result()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   internal class When_validating_a_model_for_which_some_neighborhoods_cannot_be_resolved : concern_for_SpatialStructureValidator
   {
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _neighborhood1.FirstNeighbor = _liver;
         _neighborhood1.SecondNeighbor = _kidney;

         _neighborhood2.FirstNeighbor = _kidney;
         _neighborhood2.SecondNeighbor = null;
      }

      protected override void Because()
      {
         _result = sut.Validate(_modelConfiguration);
      }

      [Observation]
      public void should_return_an_valid_validation_result()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   internal class When_validating_a_model_for_which_some_neighborhoods_are_not_physical : concern_for_SpatialStructureValidator
   {
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _neighborhood1.FirstNeighbor = _liver;
         _neighborhood1.SecondNeighbor = _kidney;

         _neighborhood1.FirstNeighbor.Mode = ContainerMode.Logical;
         _neighborhood1.SecondNeighbor.Mode = ContainerMode.Physical;

         _neighborhood2.FirstNeighbor = _kidney;
         _neighborhood2.SecondNeighbor = _heart;
         _neighborhood2.FirstNeighbor.Mode = ContainerMode.Physical;
         _neighborhood2.SecondNeighbor.Mode = ContainerMode.Physical;

      }

      protected override void Because()
      {
         _result = sut.Validate(_modelConfiguration);
      }

      [Observation]
      public void should_return_an_valid_validation_result()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }
}