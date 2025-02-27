using System;
using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Mappers
{
   internal abstract class concern_for_NeighborhoodBuilderToNeighborhoodMapper : ContextSpecification<INeighborhoodBuilderToNeighborhoodMapper>
   {
      protected IContainerBuilderToContainerMapper _containerMapper;
      protected IObjectBaseFactory _objectBaseFactory;
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IKeywordReplacerTask _keywordReplacerTask;
      protected IParameterBuilderToParameterMapper _parameterMapper;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _containerMapper = A.Fake<IContainerBuilderToContainerMapper>();
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _keywordReplacerTask = A.Fake<IKeywordReplacerTask>();
         _parameterMapper = A.Fake<IParameterBuilderToParameterMapper>();
         sut = new NeighborhoodBuilderToNeighborhoodMapper(_objectBaseFactory, _containerMapper, _keywordReplacerTask, _cloneManagerForModel, _parameterMapper);
      }
   }

   internal class When_mapping_a_neighborhood_builder_to_a_neighborhood : concern_for_NeighborhoodBuilderToNeighborhoodMapper
   {
      private NeighborhoodBuilder _neighborhoodBuilder;
      private Neighborhood _neighborhood;
      private List<string> _moleculeNames;
      private IContainer _rootContainer;
      private string _molecule1;
      private string _molecule2;
      private IParameter _clonePara1;
      private IParameter _clonePara2;
      private IModel _model;
      private IContainer _firstNeighborInModel;
      private IContainer _secondNeighborInModel;
      private SimulationConfiguration _simulationConfiguration;
      private IContainer _moleculeContainer;
      private SimulationBuilder _simulationBuilder;
      private ModelConfiguration _modelConfiguration;

      protected override void Context()
      {
         base.Context();
         _rootContainer = A.Fake<IContainer>().WithName("ROOT");
         _simulationConfiguration = new SimulationConfiguration();
         _model = A.Fake<IModel>();
         _model.Root = _rootContainer;
         _neighborhoodBuilder = new NeighborhoodBuilder().WithName("tralala");
         _neighborhoodBuilder.Add(new Container().WithName(Constants.MOLECULE_PROPERTIES));

         var para1 = new Parameter().WithName("Para1");
         var para2 = new Parameter().WithName("Para2");
         _neighborhoodBuilder.AddParameter(para1);
         _neighborhoodBuilder.AddParameter(para2);
         _clonePara1 = new Parameter().WithName("Para1");
         _clonePara2 = new Parameter().WithName("Para2");
         _neighborhoodBuilder.FirstNeighborPath = A.Fake<ObjectPath>();
         _neighborhoodBuilder.SecondNeighborPath = A.Fake<ObjectPath>();
         var firstNeighborModelPath = A.Fake<ObjectPath>();
         var secondNeighborModelPath = A.Fake<ObjectPath>();
         _firstNeighborInModel = A.Fake<IContainer>();
         _secondNeighborInModel = A.Fake<IContainer>();

         A.CallTo(() => firstNeighborModelPath.Resolve<IContainer>(_rootContainer)).Returns(_firstNeighborInModel);
         A.CallTo(() => secondNeighborModelPath.Resolve<IContainer>(_rootContainer)).Returns(_secondNeighborInModel);

         _moleculeContainer = A.Fake<IContainer>();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
         A.CallTo(() => _containerMapper.MapFrom(_neighborhoodBuilder.MoleculeProperties, _simulationBuilder)).Returns(_moleculeContainer);
         _molecule1 = "molecule1";
         _molecule2 = "molecule2";
         _moleculeNames = new List<string> { _molecule1, _molecule2 };
         A.CallTo(() => _objectBaseFactory.Create<Neighborhood>()).Returns(new Neighborhood());

         A.CallTo(() => _parameterMapper.MapFrom(A<IParameter>._, A<SimulationBuilder>._)).ReturnsLazily((IParameter parameter, SimulationBuilder simulationBuilder) =>
         {
            if (ReferenceEquals(parameter, para1))
               return _clonePara1;
            if(ReferenceEquals(parameter, para2))
               return _clonePara2;

            throw new NotSupportedException();
         });

         _modelConfiguration = new ModelConfiguration(_model, _simulationConfiguration, _simulationBuilder);
         _modelConfiguration.UpdateReplacementContext();


         A.CallTo(() => _keywordReplacerTask.CreateModelPathFor(A<ObjectPath>._, A<ReplacementContext>._)).ReturnsLazily((ObjectPath path, ReplacementContext context) =>
         {
            if (ReferenceEquals(path, _neighborhoodBuilder.FirstNeighborPath))
               return firstNeighborModelPath;
            if (ReferenceEquals(path, _neighborhoodBuilder.SecondNeighborPath))
               return secondNeighborModelPath;

            throw new NotSupportedException();
         });
      }



      protected override void Because()
      {
         _neighborhood = sut.MapFrom(_neighborhoodBuilder, _moleculeNames, _moleculeNames, _modelConfiguration);
      }

      [Observation]
      public void should_return_a_neighborhood_whose_name_was_set_to_the_name_of_the_neighborhood_builder()
      {
         _neighborhood.Name.ShouldBeEqualTo(_neighborhoodBuilder.Name);
      }

      [Observation]
      public void should_have_added_a_clone_of_the_neighborhood_parameters_to_the_created_neighborhood()
      {
         _neighborhood.AllParameters().ShouldContain(_clonePara1, _clonePara2);
      }

      [Observation]
      public void should_have_resolved_the_first_and_second_neighbors_in_the_model()
      {
         _neighborhood.FirstNeighbor.ShouldBeEqualTo(_firstNeighborInModel);
         _neighborhood.SecondNeighbor.ShouldBeEqualTo(_secondNeighborInModel);
      }

      [Observation]
      public void should_have_updated_the_container_type_of_the_molecule_container_to_molecule()
      {
         _moleculeContainer.ContainerType.ShouldBeEqualTo(ContainerType.Molecule);
      }
   }
}