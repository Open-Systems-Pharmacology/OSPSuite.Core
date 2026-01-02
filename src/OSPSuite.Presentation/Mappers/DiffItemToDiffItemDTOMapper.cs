using System;
using OSPSuite.Assets;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Presentation.Mappers
{
   public interface IDiffItemToDiffItemDTOMapper : IMapper<DiffItem, DiffItemDTO>
   {
   }

   public class DiffItemToDiffItemDTOMapper : IDiffItemToDiffItemDTOMapper,
      IVisitor<MissingDiffItem>,
      IVisitor<PropertyValueDiffItem>,
      IVisitor<MismatchDiffItem>,
      IStrictVisitor
   {
      private readonly IPathToPathElementsMapper _pathElementsMapper;
      private readonly IDisplayNameProvider _displayNameProvider;
      private DiffItemDTO _diffItemDTO;
      private readonly IPathAndValueEntityToPathElementsMapper _pathAndValueEntityToPathElementsMapper;

      public DiffItemToDiffItemDTOMapper(IPathToPathElementsMapper pathElementsMapper, IDisplayNameProvider displayNameProvider, IPathAndValueEntityToPathElementsMapper pathAndValueEntityToPathElementsMapper)
      {
         _pathElementsMapper = pathElementsMapper;
         _displayNameProvider = displayNameProvider;
         _pathAndValueEntityToPathElementsMapper = pathAndValueEntityToPathElementsMapper;
      }

      public DiffItemDTO MapFrom(DiffItem diffItem)
      {
         _diffItemDTO = new DiffItemDTO { Description = diffItem.Description };
         try
         {
            this.Visit(diffItem);
            return _diffItemDTO;
         }
         finally
         {
            _diffItemDTO = null;
         }
      }

      private void updateDiffItem(string value1, string value2, string propertyName, string objectName, bool itemIsMissing, Func<PathElements> mapPathElements)
      {
         _diffItemDTO.Value1 = value1;
         _diffItemDTO.Value2 = value2;
         _diffItemDTO.Property = propertyName;
         _diffItemDTO.ObjectName = objectName;
         _diffItemDTO.ItemIsMissing = itemIsMissing;

         _diffItemDTO.PathElements = mapPathElements();
      }

      public void Visit(MissingDiffItem missingDiffItem)
      {
         string value1 = string.Empty;
         string value2 = string.Empty;

         if (missingDiffItem.MissingObject2 != null)
         {
            value1 = Captions.Comparisons.Absent;
            value2 = presentWithDetails(missingDiffItem.PresentObjectDetails);
         }
         else if (missingDiffItem.MissingObject1 != null)
         {
            value1 = presentWithDetails(missingDiffItem.PresentObjectDetails);
            value2 = Captions.Comparisons.Absent;
         }

         updateDiffItem(value1, value2, missingDiffItem.MissingObjectType, missingDiffItem.MissingObjectName, itemIsMissing: true, () => mapPathElementsAsIEntity(missingDiffItem.CommonAncestor));
      }

      private string presentWithDetails(string presentDetails)
      {
         if (string.IsNullOrEmpty(presentDetails))
            return Captions.Comparisons.Present;

         return $"{Captions.Comparisons.Present} ({presentDetails})";
      }

      public void Visit(PropertyValueDiffItem propertyDiffItem)
      {
         Func<PathElements> pathElementsMapper = () => mapPathElementsAsIEntity(propertyDiffItem.CommonAncestor);

         if (propertyDiffItem.Object1.IsAnImplementationOf<PathAndValueEntity>())
            pathElementsMapper = () => _pathAndValueEntityToPathElementsMapper.MapFrom(propertyDiffItem.Object1.DowncastTo<PathAndValueEntity>());

         updateDiffItem(propertyDiffItem.FormattedValue1, propertyDiffItem.FormattedValue2, propertyDiffItem.PropertyName, objectNameFrom(propertyDiffItem), itemIsMissing: false, pathElementsMapper);
      }

      private string objectNameFrom(DiffItem diffItem)
      {
         return
            displayIf<SimulationConfiguration>(diffItem, x => ancestorDisplayName(diffItem)) ??
            displayIf<IFormula>(diffItem, x => ancestorDisplayName(diffItem)) ??
            displayIf<ReactionPartner>(diffItem, x => x.Partner.Name) ??
            displayIf<ReactionPartnerBuilder>(diffItem, x => x.MoleculeName) ??
            displayIf<UsedCalculationMethod>(diffItem, x => x.Category) ??
            displayIf<CalculationMethod>(diffItem, x => displayNameFor(new Category<CalculationMethod> { Name = x.Category })) ??
            displayIf<ObjectPath>(diffItem, x => ancestorDisplayName(diffItem)) ??
            displayIf<ValuePoint>(diffItem, x => ancestorDisplayName(diffItem)) ??
            displayIf<PathAndValueEntity>(diffItem, x => x.Name) ??
            displayIf<ExtendedProperty<string>>(diffItem, x => x.DisplayName) ??
            _displayNameProvider.DisplayNameFor(diffItem.Object1);
      }

      private string displayIf<T>(DiffItem propertyDiffItem, Func<T, string> displayFunc) where T : class
      {
         var castObject = propertyDiffItem.Object1 as T;
         return castObject == null ? null : displayFunc(castObject);
      }

      private string ancestorDisplayName(DiffItem propertyDiffItem) => displayNameFor(propertyDiffItem.CommonAncestor);

      private string displayNameFor(object objectToDisplay) => _displayNameProvider.DisplayNameFor(objectToDisplay);

      public void Visit(MismatchDiffItem mismatchDiffItem)
      {
         var ancestor = mismatchDiffItem.CommonAncestor;
         var ancestorName = displayNameFor(ancestor);
         updateDiffItem(displayNameFor(mismatchDiffItem.Object1), displayNameFor(mismatchDiffItem.Object2), mismatchDiffItem.Description, ancestorName, itemIsMissing: false, () => mapPathElementsAsIEntity(ancestor));
      }

      private PathElements mapPathElementsAsIEntity(object ancestor)
      {
         var entity = ancestor as IEntity;
         return entity == null ? new PathElements() : _pathElementsMapper.MapFrom(entity);
      }
   }
}