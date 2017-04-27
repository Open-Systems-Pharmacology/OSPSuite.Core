using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Presenters
{
   public enum QuantityColumn
   {
      Dimension,
      QuantityType,
      Selection
   }

   public interface IQuantityPresenter : IPresenter
   {
      /// <summary>
      ///    Groups the columns by the given <paramref name="pathElement" />
      /// </summary>
      void GroupBy(PathElement pathElement);

      /// <summary>
      ///    Path element column that should be sorted according to the sequence
      /// </summary>
      void SortColumn(PathElement pathElement);

      void Show(PathElement pathElement);

      void Hide(PathElement pathElement);

      void SetCaption(PathElement pathElement, string caption);

      void Show(QuantityColumn column);

      void Hide(QuantityColumn column);
   }
}