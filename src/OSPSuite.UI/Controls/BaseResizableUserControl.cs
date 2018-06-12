using System;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Controls
{
   public partial class BaseResizableUserControl : BaseUserControl, IResizableView
   {
      public event EventHandler<ViewResizedEventArgs> HeightChanged = delegate { };

      public BaseResizableUserControl()
      {
         InitializeComponent();
      }

      public virtual void AdjustHeight()
      {
         HeightChanged(this, new ViewResizedEventArgs(OptimalHeight));
      }

      /// <summary>
      ///    Returns the control optimal height so that it is displayed entirely without wasting space
      /// </summary>
      public virtual int OptimalHeight => DefaultSize.Height;

      public virtual void Repaint()
      {
         //this should trigger a repaint when required of all controls such as gridView
      }

      protected override void OnVisibleChanged(EventArgs e)
      {
         base.OnVisibleChanged(e);
         if (Visible)
            AdjustHeight();
      }
   }
}