using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_NeighborhoodBuilderToNeighborhoodMapper : ContextSpecification<INeighborhoodBuilderToNeighborhoodMapper>
   {
      protected IContainerBuilderToContainerMapper _containerMapper;
      protected IObjectBaseFactory _objectBaseFactory;
      protected IObjectPathFactory _objectPathFactory;
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IKeywordReplacerTask _keywordReplacerTask;
      protected IParameterBuilderToParameterMapper _parameterMapper;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _containerMapper = A.Fake< IContainerBuilderToContainerMapper>();
         _objectPathFactory =A.Fake<IObjectPathFactory>();
         _cloneManagerForModel =A.Fake<ICloneManagerForModel>();
         _keywordReplacerTask = A.Fake<IKeywordReplacerTask>();
         _parameterMapper = A.Fake<IParameterBuilderToParameterMapper>();
         sut = new NeighborhoodBuilderToNeighborhoodMapper(_objectBaseFactory, _containerMapper, _objectPathFactory,_keywordReplacerTask,_cloneManagerForModel,_parameterMapper);
      }
   }

   
   public class When_mapping_a_neighborhood_builder_to_a_neighborhood : concern_for_NeighborhoodBuilderToNeighborhoodMapper
   {
      private INeighborhoodBuilder _neighborhoodBuilder;
      private INeighborhood _neighborhood;
      private IEnumerable<string> _moleculeNames;
      private IContainer _rootContainer;
      private string _molecule1;
      private string _molecule2;
      private IParameter _clonePara1;
      private IParameter _clonePara2;
      private IModel _model;
      private IContainer _firstNeighborInModel;
      private IContainer _secondNeighborInModel;
      private IBuildConfiguration _buildConfiguration;
      private IContainer _moleculeContainer
     

   ;

      protected override void Context()
      {
         base.Context();
         _rootContainer = A.Fake<IContainer>().WithName("ROOT");
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _model = A.Fake<IModel>();
         _model.Root = _rootContainer;
         _neighborhoodBuilder = A.Fake<INeighborhoodBuilder>().WithName("tralala");
         _neighborhoodBuilder.FirstNeighbor = A.Fake<IContainer>();
         _neighborhoodBuilder.SecondNeighbor = A.Fake<IContainer>();
         A.CallTo(()=>_neighborhoodBuilder.MoleculeProperties).Returns(A.Fake<IContainer>());
         var para1 = A.Fake<IParameter>();
         var para2 = A.Fake<IParameter>();
         A.CallTo(()=>_neighborhoodBuilder.Parameters).Returns(new[] { para1, para2 });
         _clonePara1 =A.Fake<IParameter>();
         _clonePara2 =A.Fake<IParameter>();
         var firstNeighborBuilderPath = A.Fake<IObjectPath>();
         var secondNeighborBuilderPath = A.Fake<IObjectPath>();
         A.CallTo(()=>_objectPathFactory.CreateAbsoluteObjectPath(_neighborhoodBuilder.FirstNeighbor)).Returns(firstNeighborBuilderPath);
         A.CallTo(()=>_objectPathFactory.CreateAbsoluteObjectPath(_neighborhoodBuilder.SecondNeighbor)).Returns(secondNeighborBuilderPath);
         var firstNeighborModelPath = A.Fake<IObjectPath>();
         var secondNeighborModelPath = A.Fake<IObjectPath>();
         _firstNeighborInModel = A.Fake<IContainer>();
         _secondNeighborInModel = A.Fake<IContainer>();
         A.CallTo(()=>_keywordReplacerTask.CreateModelPathFor(firstNeighborBuilderPath, _model.Root)).Returns(firstNeighborModelPath);
         A.CallTo(()=>_keywordReplacerTask.CreateModelPathFor(secondNeighborBuilderPath, _model.Root)).Returns(secondNeighborModelPath);
         A.CallTo(()=>firstNeighborModelPath.Resolve<IContainer>(_rootContainer)).Returns(_firstNeighborInModel);
         A.CallTo(()=>secondNeighborModelPath.Resolve<IContainer>(_rootContainer)).Returns(_secondNeighborInModel);
          _moleculeContainer= A.Fake<IContainer>();
          A.CallTo(() => _containerMapper.MapFrom(_neighborhoodBuilder.MoleculeProperties, _buildConfiguration)).Returns(_moleculeContainer);
         _molecule1 = "molecule1";
         _molecule2 = "molecule2";
         _moleculeNames = new List<string> {_molecule1, _molecule2};
         A.CallTo(()=>_objectBaseFactory.Create<INeighborhood>()).Returns(A.Fake<INeighborhood>());
         A.CallTo(() => _parameterMapper.MapFrom(para1,_buildConfiguration)).Returns(_clonePara1);
         A.CallTo(() => _parameterMapper.MapFrom(para2,_buildConfiguration)).Returns(_clonePara2);
      }

      protected override void Because()
      {
         _neighborhood = sut.MapFrom(_neighborhoodBuilder, _model, _buildConfiguration, _moleculeNames, _moleculeNames);
      }
      [Observation]
      public void should_return_a_neighborhood_whose_name_was_set_to_the_name_of_the_neighborhood_builder()
      {
         A.CallTo(() => _neighborhood.UpdatePropertiesFrom(_neighborhoodBuilder, _cloneManagerForModel)).MustHaveHappened();  
      }

      [Observation]
      public void should_have_added_a_clone_of_the_neighborhood_parameters_to_the_created_neighborhood()
      {
         A.CallTo(() => _neighborhood.Add(_clonePara1)).MustHaveHappened();
         A.CallTo(() => _neighborhood.Add(_clonePara2)).MustHaveHappened();
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