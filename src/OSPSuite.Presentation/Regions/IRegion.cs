namespace OSPSuite.Presentation.Regions
{
   public interface IRegion
   {
      /// <summary>
      ///    Name of the underlying region
      /// </summary>
      string Name { get; }

      /// <summary>
      ///    Add a view to the region
      /// </summary>
      /// <param name="view"></param>
      void Add(object view);

      /// <summary>
      ///    Switch visiblity
      /// </summary>
      void ToggleVisibility();

      /// <summary>
      ///    Returns <c>true</c> if the region is visible otherwise <c>false</c>
      /// </summary>
      bool IsVisible { get; }

      /// <summary>
      /// Shows the dock panel even (make if visible if it was hidden)
      /// </summary>
      void Show();
   }
}