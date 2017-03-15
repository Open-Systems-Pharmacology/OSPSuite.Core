namespace OSPSuite.Presentation.Core
{
   /// <summary>
   ///    interface representing a generic presentation settings where settings can be retrieved or stored by key
   /// </summary>
   public interface IPresentationSettings
   {
      /// <summary>
      ///    Get a value for a property by the <paramref name="propertyName" />
      /// </summary>
      /// <typeparam name="T">Must be a value-type</typeparam>
      /// <param name="propertyName">The name of the property being retrieved from settings</param>
      /// <param name="defaultValue">The value returned if the property is not initialized</param>
      /// <returns>The default value if the property is not initialized, otherwise the current value</returns>
      T GetSetting<T>(string propertyName, T defaultValue = default(T));

      /// <summary>
      ///    Sets the value for a property by <paramref name="propertyName" />
      /// </summary>
      /// <typeparam name="T">Must be a value-type</typeparam>
      /// <param name="propertyName">The name of the property being saved in the settings</param>
      /// <param name="propertyValue">The new value of the property being saved in the settings</param>
      void SetSetting<T>(string propertyName, T propertyValue);
   }
}