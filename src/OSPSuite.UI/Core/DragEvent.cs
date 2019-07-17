using System;
using System.Windows.Forms;
using OSPSuite.Presentation.Core;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility;

namespace OSPSuite.UI.Core
{
   public class DragEvent : IDragEvent
   {
      private readonly DragEventArgs _dragEventArgs;

      public DragEvent(DragEventArgs dragEventArgs)
      {
         _dragEventArgs = dragEventArgs;
      }

      public bool TypeBeingDraggedIs<T>() => _dragEventArgs.TypeBeingDraggedIs<T>();

      public bool TypeBeingDraggedIs(Type type) => _dragEventArgs.TypeBeingDraggedIs(type);

    

      public void SetEffectForType<T>(DragEffect matchEffect = DragEffect.Move, DragEffect notMatchEffect = DragEffect.None)
      {
         Effect = TypeBeingDraggedIs<string>() ? DragEffect.Move : DragEffect.None;
      }

      public DragEffect Effect
      {
         get => EnumHelper.ParseValue<DragEffect>(_dragEventArgs.Effect.ToString());
         set => _dragEventArgs.Effect = EnumHelper.ParseValue<DragDropEffects>(value.ToString());
      }

      public T Data<T>() => _dragEventArgs.Data<T>();
   }
}