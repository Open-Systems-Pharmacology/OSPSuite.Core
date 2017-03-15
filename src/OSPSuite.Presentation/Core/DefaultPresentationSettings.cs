using System;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Core
{
   /// <summary>
   /// This class stores key-value pairs and is intended for use as presentation settings. Any presenter with settings
   /// can use this structure to create new settings and retrieve them later. Useful when a presenter is disposed, then loaded
   /// and the presentation should be re-applied.
   /// Examples
   ///   - tree nodes that are expanded
   ///   - tree nodes that are currently selected
   ///   - tabs that are currently visible
   /// </summary>
   public class DefaultPresentationSettings : IPresentationSettings
   {
      public virtual Cache<string, string> PresenterPropertyCache { get; private set; }

      public DefaultPresentationSettings()
      {
         PresenterPropertyCache = new Cache<string, string>();
      }

      /// <summary>
      /// Get a value for a property by the <paramref name="propertyName"/>
      /// </summary>
      /// <typeparam name="T">Must be a value-type</typeparam>
      /// <param name="propertyName">The name of the property being retrieved from settings</param>
      /// <param name="defaultValue">The value returned if the property is not initialized</param>
      /// <returns>The default value if the property is not initialized, otherwise the current value</returns>
      public virtual T GetSetting<T>(string propertyName, T defaultValue = default(T))
      {
         if (!PresenterPropertyCache.Contains(propertyName))
            PresenterPropertyCache.Add(propertyName, defaultValue.ConvertedTo<string>());

         if (typeof(T).IsEnum)
            return (T)Enum.Parse(typeof(T), PresenterPropertyCache[propertyName]);

         return PresenterPropertyCache[propertyName].ConvertedTo<T>();
      }

      /// <summary>
      /// Sets the value for a property by <paramref name="propertyName"/>
      /// </summary>
      /// <typeparam name="T">Must be a value-type</typeparam>
      /// <param name="propertyName">The name of the property being saved in the settings</param>
      /// <param name="propertyValue">The new value of the property being saved in the settings</param>
      public virtual void SetSetting<T>(string propertyName, T propertyValue)
      {
         PresenterPropertyCache[propertyName] = propertyValue.ToString();
      }

      public void SetTrue(string settingName)
      {
         SetSetting(settingName, true);
      }

      public void SetFalse(string settingName)
      {
         SetSetting(settingName, false);
      }

      public bool IsTrue(string settingName)
      {
         return GetSetting(settingName, default(bool));
      }

      public bool IsFalse(string settingName)
      {
         return !IsTrue(settingName);
      }

      public bool IsEqual<T>(string settingName, T value)
      {
         return Equals(GetSetting(settingName, default(T)), value);
      }
   }
}
