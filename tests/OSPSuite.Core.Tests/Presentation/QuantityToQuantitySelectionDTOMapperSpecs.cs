using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Mappers;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_QuantityToQuantitySelectionDTOMapper : ContextSpecification<IQuantityToQuantitySelectionDTOMapper>
   {
      private IPathToPathElementsMapper _pathToPathElementsMapper;
      private IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _pathToPathElementsMapper= A.Fake<IPathToPathElementsMapper>();   
         _entityPathResolver= A.Fake<IEntityPathResolver>();   
         sut = new QuantityToQuantitySelectionDTOMapper(_entityPathResolver,_pathToPathElementsMapper);
      }
   }

   public class When_mapping_a_null_quantity_to_a_quantity_selection : concern_for_QuantityToQuantitySelectionDTOMapper
   {
      [Observation]
      public void should_return_null()
      {
         sut.MapFrom(null).ShouldBeNull();
      }
   }
}	