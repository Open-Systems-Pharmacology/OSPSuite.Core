using System;
using System.Collections;
using System.Reflection;

namespace OSPSuite.UI.Views.Importer
{
   static class CleanUpHelper
   {

      /// <summary>
      /// This generic methods clears recursively all controls.
      /// </summary>
      public static void ReleaseControls(IEnumerable controls)
      {
         if (controls == null) return;
         foreach (var control in controls)
         {

            var properties = control.GetType().GetProperties();
            foreach (var property in properties)
            {
               if (property.Name != "Controls") continue;
               var childcontrols = property.GetValue(control, null);
               if (childcontrols == null) continue;
               ReleaseControls((IEnumerable)childcontrols);
            }
            ReleaseDataSource(control);
            ReleaseEvents(control);
            var disposableObject = control as IDisposable;
            if (disposableObject != null) 
// ReSharper disable EmptyGeneralCatchClause
               try {disposableObject.Dispose(); }catch
// ReSharper restore EmptyGeneralCatchClause
               {}
         }
      }

      /// <summary>
      /// This generic method set the DataSource property to null for given object.
      /// </summary>
      /// <remarks><para>All eventually occuring error are catched.</para></remarks>
      /// <param name="obj"></param>
      private static void ReleaseDataSource(object obj)
      {
         try
         {
            var propertyInfo = obj.GetType().GetProperty("DataSource");
            if (propertyInfo != null && propertyInfo.CanWrite)
               propertyInfo.SetValue(obj, null, null);
// ReSharper disable EmptyGeneralCatchClause
         } catch{}
// ReSharper restore EmptyGeneralCatchClause
      }

      private const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

      /// <summary>
      /// This generic method releases all events of an object.
      /// </summary>
      /// <param name="obj"></param>
      public static void ReleaseEvents(object obj)
      {
         if (obj == null) return;

         //Get all of the events and then use the DeclaringType property to get the instance of the fields
         var events = obj.GetType().GetEvents(flags);
         if (events == null)
            return;
         if (events.Length < 1)
            return;

         //Store all the FieldInfo objects in a HashTable
         var fieldInfos = new Hashtable();

         for (var i = 0; i < events.Length; i++)
         {
            //Get all of the fields for the selected declared type
            var fields = events[i].DeclaringType.GetFields(flags);
            foreach (var field in fields)
            {
               if (events[i].Name.Equals(field.Name) && !fieldInfos.Contains(field.Name))
                  fieldInfos.Add(field.Name, field);
            }
         }

         foreach (FieldInfo fieldInfo in fieldInfos.Values)
         {
            if (fieldInfo == null) continue;
            var multicastDelegate = fieldInfo.GetValue(obj) as MulticastDelegate;
            if (multicastDelegate == null) continue;
            foreach (var del in multicastDelegate.GetInvocationList())
            {
               var eventVar = getEvent(fieldInfo.Name, fieldInfo.DeclaringType);
               if (eventVar == null) continue;
               var removeMethod = eventVar.GetRemoveMethod();
               if (removeMethod == null) continue;
               removeMethod.Invoke(obj, new object[] { del });
            }
         }
      }

      private static EventInfo getEvent(string name, Type t)
      {
         if (name == null)
            return null;
         return t == null ? null : t.GetEvent(name, flags);
      }

   }
}
