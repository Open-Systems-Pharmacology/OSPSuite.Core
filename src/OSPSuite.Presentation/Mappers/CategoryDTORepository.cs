using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
{
   public interface ICategoryDTORepository : IRepository<CategoryDTO>
   {
      
   }
   public class CategoryDTORepository : ICategoryDTORepository
   {
      private readonly ICompoundCalculationMethodRepository _compoundCalculationMethodRepository;
      private readonly IDisplayNameProvider _displayNameProvider;

      public CategoryDTORepository(ICompoundCalculationMethodRepository compoundCalculationMethodRepository, IDisplayNameProvider displayNameProvider)
      {
         _compoundCalculationMethodRepository = compoundCalculationMethodRepository;
         _displayNameProvider = displayNameProvider;
      }

      public IEnumerable<CategoryDTO> All()
      {
         return _compoundCalculationMethodRepository.All().GroupBy(x => x.Category).Where(hasAlternatives).Select(mapFrom);
      }

      public IReadOnlyList<CategoryDTO> MapFrom(IEnumerable<CalculationMethod> calculationMethods)
      {
         return calculationMethods.GroupBy(x => x.Category).Where(hasAlternatives).Select(mapFrom).ToList();
      }

      private bool hasAlternatives(IGrouping<string, CalculationMethod> group)
      {
         return group.Count() > 1;
      }

      private CategoryDTO mapFrom(IGrouping<string, CalculationMethod> group)
      {
         var categoryName = group.Key;
         var category = new Category<CalculationMethod> {Name = categoryName};
         return new CategoryDTO
         {
            Name = categoryName,
            DisplayName = _displayNameProvider.DisplayNameFor(category),
            Methods = group.All().ToList()
         };
      }
   }
}