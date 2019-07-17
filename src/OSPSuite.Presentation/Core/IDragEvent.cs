using System;

namespace OSPSuite.Presentation.Core
{
   [Flags]
   public enum DragEffect
   {
      /// <summary>The drop target does not accept the data.</summary>
      None = 0,

      /// <summary>The data from the drag source is copied to the drop target.</summary>
      Copy = 1 << 0,

      /// <summary>The data from the drag source is moved to the drop target.</summary>
      Move = 1 << 1,

      /// <summary>The data from the drag source is linked to the drop target.</summary>
      Link = 1 << 2,

      /// <summary>
      ///    The target can be scrolled while dragging to locate a drop position that is not currently visible in the
      ///    target.
      /// </summary>
      Scroll = 1 << 3,

      /// <summary>
      ///    The combination of the <see cref="DragEffect.Copy" />, <see cref="DragEffect.Move" />, and
      ///    <see cref="DragEffect.Scroll" /> effects.
      /// </summary>
      All = Scroll | Move | Copy
   }

   public interface IDragEvent
   {
      /// <summary>
      ///    Returns <c>true</c> if the type of the object being dragged is <typeparamref name="T" /> otherwise <c>false</c>
      /// </summary>
      bool TypeBeingDraggedIs<T>();

      bool TypeBeingDraggedIs(Type type);

      /// <summary>
      ///    Set the effect to <paramref name="matchEffect" /> if the type being dragged is <typeparamref name="T" /> otherwise
      ///    to <paramref name="notMatchEffect" />
      /// </summary>
      void SetEffectForType<T>(DragEffect matchEffect = DragEffect.Move, DragEffect notMatchEffect = DragEffect.None);

      DragEffect Effect { get; set; }

      T Data<T>();
   }
}