﻿using System;
using System.Windows.Forms;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Extensions
{
   public static class DragEventArgsExtensions
   {
      public static bool TypeBeingDraggedIs<T>(this DragEventArgs e)
      {
         return TypeBeingDraggedIs(e, typeof(T));
      }

      public static bool TypeBeingDraggedIs(this DragEventArgs e, Type type)
      {
         var dataBeingDragged = data(e, type);
         return dataBeingDragged != null && dataBeingDragged.IsAnImplementationOf(type);
      }

      // Convert DragDropKeyState as integer to Enum DragDropKeyFlags 
      // This is because the DragEventArgs.KeyState is an integer
      // but on the events which are using it, it is being converted to DragDropKeyState for better readability
      public static DragDropKeyFlags AsKeyFlags(this DragEventArgs dragEventArgs) =>
         (DragDropKeyFlags)dragEventArgs.KeyState;

      public static T Data<T>(this DragEventArgs e)
      {
         try
         {
            return data(e, typeof(T)).DowncastTo<T>();
         }
         catch (InvalidCastException)
         {
            return default(T);
         }
      }

      private static object data(DragEventArgs e, Type type)
      {
         var dragInfo = getData<DragDropInfo>(e);
         if (dragInfo != null)
            return dragInfo.Subject;

         return getData(e, type);
      }

      private static T getData<T>(DragEventArgs e) where T : class
      {
         return getData(e, typeof(T)) as T;
      }

      private static object getData(DragEventArgs e, Type type)
      {
         return e.Data.GetData(type);
      }
   }
}