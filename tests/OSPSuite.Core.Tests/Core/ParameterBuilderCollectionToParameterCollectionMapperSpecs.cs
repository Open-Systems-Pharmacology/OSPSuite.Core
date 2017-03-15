using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterBuilderCollectionToParameterCollectionMapper : ContextSpecification<IParameterBuilderCollectionToParameterCollectionMapper>
   {
      protected IParameterBuilderToParameterMapper _parameterMapper;

      protected override void Context()
      {
         _parameterMapper = A.Fake<IParameterBuilderToParameterMapper>();
         sut = new ParameterBuilderCollectionToParameterCollectionMapper(_parameterMapper);
      }
   }

   
   public class When_mapping_a_collection_of_parameter_builder_to_a_collection_of_parameter:  concern_for_ParameterBuilderCollectionToParameterCollectionMapper
   {
      private IList<IParameter> _allParameterBuilders;
      private IEnumerable<IParameter> _results;
      private IParameter _para1;
      private IParameter _para2;
      private IParameter _para3;
      private IParameter _mappedPara1;
      private IParameter _mappedPara3;
      private IBuildConfiguration _buildConfiguration;

      protected override void Context()
      {
         base.Context();
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _para1 =A.Fake<IParameter>().WithMode(ParameterBuildMode.Local);
         _para2 = A.Fake<IParameter>().WithMode(ParameterBuildMode.Global);
         _para3 = A.Fake<IParameter>().WithMode(ParameterBuildMode.Property);
         _allParameterBuilders= new List<IParameter>{_para1,_para2,_para3};
         _mappedPara1 =A.Fake<IParameter>();
         _mappedPara3 = A.Fake<IParameter>();
         A.CallTo(() => _parameterMapper.MapFrom(_para1,_buildConfiguration)).Returns(_mappedPara1);
         A.CallTo(() => _parameterMapper.MapFrom(_para3, _buildConfiguration)).Returns(_mappedPara3);
      }
      protected override void Because()
      {
         _results = sut.MapFrom(_allParameterBuilders,_buildConfiguration, ParameterBuildMode.Local, ParameterBuildMode.Property);
      }
      [Observation]
      public void should_only_return_the_parameters_matching_the_given_mode()
      {
           _results.ShouldOnlyContain(_mappedPara1,_mappedPara3);
      }
   }
}	