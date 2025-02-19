using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Mappers
{
   internal abstract class concern_for_TransportBuilderToTransportMapper : ContextSpecification<ITransportBuilderToTransportMapper>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected IFormulaBuilderToFormulaMapper _formulaBuilderToFormulaMapper;
      protected IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      protected SimulationConfiguration _simulationConfiguration;
      protected IProcessRateParameterCreator _processRateParameterCreator;
      protected SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _objectBaseFactory.Create<Transport>()).Returns(new Transport());
         _formulaBuilderToFormulaMapper = A.Fake<IFormulaBuilderToFormulaMapper>();
         _parameterMapper = A.Fake<IParameterBuilderCollectionToParameterCollectionMapper>();
         _processRateParameterCreator = new ProcessRateParameterCreator(_objectBaseFactory, _formulaBuilderToFormulaMapper);
         sut = new TransportBuilderToTransportMapper(_objectBaseFactory, _formulaBuilderToFormulaMapper, _parameterMapper, _processRateParameterCreator);
      }
   }

   internal class When_mapping_a_transport_builder_to_a_transport : concern_for_TransportBuilderToTransportMapper
   {
      private TransportBuilder _passiveTransportBuilder;
      private IFormula _kinetic;
      private Transport _transport;

      protected override void Context()
      {
         base.Context();
         _passiveTransportBuilder = new TransportBuilder {TransportType = TransportType.Efflux};
         _kinetic = A.Fake<IFormula>();
         A.CallTo(() => _formulaBuilderToFormulaMapper.MapFrom(_kinetic, _simulationBuilder)).Returns(_kinetic);
         _passiveTransportBuilder.Name = "PassiveTransport";
         _passiveTransportBuilder.Formula = _kinetic;
         A.CallTo(() => _parameterMapper.MapFrom(_passiveTransportBuilder.Parameters, _simulationBuilder)).Returns(new List<IParameter>());
      }

      protected override void Because()
      {
         _transport = sut.MapFrom(_passiveTransportBuilder, _simulationBuilder);
      }

      [Observation]
      public void should_have_returned_a_transport_with_name_property_set_to_builders_name()
      {
         _transport.Name.ShouldBeEqualTo(_passiveTransportBuilder.Name);
      }

      [Observation]
      public void should_have_returned_a_transport_with_formula_property_set_to_builders_kinetic()
      {
         _transport.Formula.ShouldBeEqualTo(_passiveTransportBuilder.Formula);
      }

      [Observation]
      public void should_have_added_the_transport_builder_as_reference_to_the_transport()
      {
         _simulationBuilder.BuilderFor(_transport).ShouldBeEqualTo(_passiveTransportBuilder);
      }
   }

   internal class When_mapping_an_active_influx_transport_builder_to_a_transport_for_which_a_parameter_rate_should_be_generated : concern_for_TransportBuilderToTransportMapper
   {
      private TransportBuilder _transportBuilder;
      private IFormula _kinetic;
      private Transport _transport;
      private IParameter _processRateParameter;

      protected override void Context()
      {
         base.Context();
         _transportBuilder = new TransportBuilder();
         _kinetic = A.Fake<IFormula>();
         _transportBuilder.CreateProcessRateParameter = true;
         A.CallTo(() => _formulaBuilderToFormulaMapper.MapFrom(_kinetic, _simulationBuilder)).ReturnsLazily(x => new ExplicitFormula("clone"));

         _transportBuilder.TransportType = TransportType.Influx;
         _transportBuilder.Name = "Active Transport";
         _transportBuilder.Formula = _kinetic;
         A.CallTo(() => _parameterMapper.MapFrom(_transportBuilder.Parameters, _simulationBuilder)).Returns(new List<IParameter>());
         _processRateParameter = new Parameter();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).Returns(_processRateParameter);
      }

      protected override void Because()
      {
         _transport = sut.MapFrom(_transportBuilder, _simulationBuilder);
         _processRateParameter = _transport.GetSingleChildByName<IParameter>(Constants.Parameters.PROCESS_RATE);
      }

      [Observation]
      public void should_have_created_a_parameter_process_rate_in_the_transport()
      {
         _processRateParameter.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_set_the_tag_in_the_parameter_process_rate_to_process_rate()
      {
         _processRateParameter.Tags.Contains(Constants.Parameters.PROCESS_RATE).ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_tag_neighborhood()
      {
         _processRateParameter.Tags.Contains(ObjectPathKeywords.NEIGHBORHOOD).ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_tag_drug()
      {
         _processRateParameter.Tags.Contains(ObjectPathKeywords.MOLECULE).ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_tag_active()
      {
         _processRateParameter.Tags.Contains(Constants.ACTIVE).ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_tag_influx()
      {
         _processRateParameter.Tags.Contains(Constants.INFLUX).ShouldBeTrue();
         _processRateParameter.Tags.Contains(Constants.NOT_INFLUX).ShouldBeFalse();
      }

      [Observation]
      public void the_formula_of_the_process_rate_parameter_should_have_been_set_to_a_copy_of_the_kinetic_formula()
      {
         _transport.Formula.IsExplicit().ShouldBeTrue();
         _transport.Formula.DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("clone");

         _processRateParameter.Formula.IsExplicit().ShouldBeTrue();
         _processRateParameter.Formula.DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("clone");

         _transport.Formula.ShouldNotBeEqualTo(_processRateParameter.Formula);
      }
   }

   internal class When_mapping_an_active_efflux_transport_builder_to_a_transport_for_which_a_parameter_rate_should_be_generated : concern_for_TransportBuilderToTransportMapper
   {
      private TransportBuilder _transportBuilder;
      private IFormula _kinetic;
      private Transport _transport;
      private IParameter _processRateParameter;

      protected override void Context()
      {
         base.Context();
         _transportBuilder = new TransportBuilder();
         _kinetic = A.Fake<IFormula>();
         _transportBuilder.CreateProcessRateParameter = true;
         A.CallTo(() => _formulaBuilderToFormulaMapper.MapFrom(_kinetic, _simulationBuilder)).ReturnsLazily(x => new ExplicitFormula("clone"));

         _transportBuilder.TransportType = TransportType.Efflux;
         _transportBuilder.Name = "Active Transport";
         _transportBuilder.Formula = _kinetic;
         A.CallTo(() => _parameterMapper.MapFrom(_transportBuilder.Parameters, _simulationBuilder)).Returns(new List<IParameter>());
         _processRateParameter = new Parameter();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).Returns(_processRateParameter);
      }

      protected override void Because()
      {
         _transport = sut.MapFrom(_transportBuilder, _simulationBuilder);
         _processRateParameter = _transport.GetSingleChildByName<IParameter>(Constants.Parameters.PROCESS_RATE);
      }

      [Observation]
      public void should_have_created_a_parameter_process_rate_in_the_transport()
      {
         _processRateParameter.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_set_the_tag_in_the_parameter_process_rate_to_process_rate()
      {
         _processRateParameter.Tags.Contains(Constants.Parameters.PROCESS_RATE).ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_tag_neighborhood()
      {
         _processRateParameter.Tags.Contains(ObjectPathKeywords.NEIGHBORHOOD).ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_tag_drug()
      {
         _processRateParameter.Tags.Contains(ObjectPathKeywords.MOLECULE).ShouldBeTrue();
      }

      [Observation]
      public void should_not_have_set_the_passive_tag()
      {
         _processRateParameter.Tags.Contains(Constants.PASSIVE).ShouldBeFalse();
      }

      [Observation]
      public void should_have_set_the_tag_active()
      {
         _processRateParameter.Tags.Contains(Constants.ACTIVE).ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_tag_not_influx()
      {
         _processRateParameter.Tags.Contains(Constants.INFLUX).ShouldBeFalse();
         _processRateParameter.Tags.Contains(Constants.NOT_INFLUX).ShouldBeTrue();
      }

      [Observation]
      public void the_formula_of_the_process_rate_parameter_should_have_been_set_to_a_copy_of_the_kinetic_formula()
      {
         _transport.Formula.IsExplicit().ShouldBeTrue();
         _transport.Formula.DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("clone");

         _processRateParameter.Formula.IsExplicit().ShouldBeTrue();
         _processRateParameter.Formula.DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("clone");

         _transport.Formula.ShouldNotBeEqualTo(_processRateParameter.Formula);
      }
   }

   internal class When_mapping_a_passive_builder_to_a_transport_for_which_a_parameter_rate_should_be_generated : concern_for_TransportBuilderToTransportMapper
   {
      private TransportBuilder _transportBuilder;
      private IFormula _kinetic;
      private Transport _transport;
      private IParameter _processRateParameter;

      protected override void Context()
      {
         base.Context();
         _transportBuilder = new TransportBuilder();
         _kinetic = A.Fake<IFormula>();
         _transportBuilder.CreateProcessRateParameter = true;
         A.CallTo(() => _formulaBuilderToFormulaMapper.MapFrom(_kinetic, _simulationBuilder)).ReturnsLazily(x => new ExplicitFormula("clone"));

         _transportBuilder.TransportType = TransportType.Diffusion;
         _transportBuilder.Name = "PassiveTransport";
         _transportBuilder.Formula = _kinetic;
         A.CallTo(() => _parameterMapper.MapFrom(_transportBuilder.Parameters, _simulationBuilder)).Returns(new List<IParameter>());
         _processRateParameter = new Parameter();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).Returns(_processRateParameter);
      }

      protected override void Because()
      {
         _transport = sut.MapFrom(_transportBuilder, _simulationBuilder);
         _processRateParameter = _transport.GetSingleChildByName<IParameter>(Constants.Parameters.PROCESS_RATE);
      }

      [Observation]
      public void should_have_set_the_tag_passive()
      {
         _processRateParameter.Tags.Contains(Constants.PASSIVE).ShouldBeTrue();
      }

      [Observation]
      public void should_not_have_set_the_tag_active()
      {
         _processRateParameter.Tags.Contains(Constants.ACTIVE).ShouldBeFalse();
      }
   }

   internal class When_mapping_a_passive_builder_to_a_transport_for_which_all_parameter_rates_should_be_generated : concern_for_TransportBuilderToTransportMapper
   {
      private TransportBuilder _transportBuilder;
      private IFormula _kinetic;
      private Transport _transport;
      private IParameter _processRateParameter;

      protected override void Context()
      {
         base.Context();
         _transportBuilder = new TransportBuilder();
         _kinetic = A.Fake<IFormula>();
         _transportBuilder.CreateProcessRateParameter = false;
         A.CallTo(() => _formulaBuilderToFormulaMapper.MapFrom(_kinetic, _simulationBuilder)).ReturnsLazily(x => new ExplicitFormula("clone"));

         _transportBuilder.TransportType = TransportType.Diffusion;
         _transportBuilder.Name = "PassiveTransport";
         _transportBuilder.Formula = _kinetic;
         A.CallTo(() => _parameterMapper.MapFrom(_transportBuilder.Parameters, _simulationBuilder)).Returns(new List<IParameter>());
         _processRateParameter = new Parameter();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).Returns(_processRateParameter);
         _simulationConfiguration.CreateAllProcessRateParameters = true;
      }

      protected override void Because()
      {
         _transport = sut.MapFrom(_transportBuilder, _simulationBuilder);
         _processRateParameter = _transport.GetSingleChildByName<IParameter>(Constants.Parameters.PROCESS_RATE);
      }

      [Observation]
      public void should_have_set_the_tag_passive()
      {
         _processRateParameter.Tags.Contains(Constants.PASSIVE).ShouldBeTrue();
      }

      [Observation]
      public void should_not_have_set_the_tag_active()
      {
         _processRateParameter.Tags.Contains(Constants.ACTIVE).ShouldBeFalse();
      }
   }
}