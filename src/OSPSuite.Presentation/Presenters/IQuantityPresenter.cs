using OSPSuite.Core.Domain;

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
      ///    Groups the columns by the given <paramref name="pathElementId" />
      /// </summary>
      void GroupBy(PathElementId pathElementId);

      /// <summary>
      ///    Path element column that should be sorted according to the sequence
      /// </summary>
      void SortColumn(PathElementId pathElementId);

      void Show(PathElementId pathElementId);

      void Hide(PathElementId pathElementId);

      void SetCaption(PathElementId pathElementId, string caption);

      void Show(QuantityColumn column);

      void Hide(QuantityColumn column);
   }
}