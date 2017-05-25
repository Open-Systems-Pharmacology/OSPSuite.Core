using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraCharts;
using OSPSuite.Assets;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Controls
{
   public class UxChartControl : ChartControl
   {
      private readonly ChartTitle _title;
      private readonly ChartTitle _description;
      private readonly ClipboardTask _clipboardTask;
      private readonly BarManager _barManager;
      public PopupMenu PopupMenu { get; }

      public UxChartControl(bool useDefaultPopupMechanism = true, bool addCopyToClipboardMenu = true)
      {
         Titles.Clear();

         _title = createTitle(16, StringAlignment.Center, ChartTitleDockStyle.Top);
         _description = createTitle(12, StringAlignment.Near, ChartTitleDockStyle.Bottom);

         Titles.Add(_title);
         Titles.Add(_description);

         _clipboardTask = new ClipboardTask();
         _barManager = new BarManager {Form = this};
         PopupMenu = new PopupMenu(_barManager);

         if (useDefaultPopupMechanism)
            initializePopup(addCopyToClipboardMenu);
      }

      /// <summary>
      ///    This is to increase performance of the chart control.
      /// </summary>
      /// <param name="collection">Objects which gets updated. Could be Series, Panes, Annotations, SecondaryAxesX, etc.</param>
      /// <param name="actionToPerform">Action that will be performed between <c>BeginUpdate</c> and <c>EndUpdate</c></param>
      public void DoUpdateOf(ChartCollectionBase collection, Action actionToPerform)
      {
         try
         {
            collection.BeginUpdate();
            actionToPerform();
         }
         finally
         {
            collection.EndUpdate();
         }
      }

      private ChartTitle createTitle(int fontSize, StringAlignment alignment, ChartTitleDockStyle dockStyle)
      {
         return new ChartTitle {Text = string.Empty, Font = new Font("Arial", fontSize), Alignment = alignment, Dock = dockStyle, WordWrap = true};
      }

      public ImageCollection Images
      {
         set => _barManager.Images = value;
      }

      public BarItemLink AddPopupMenu(string caption, Action action, ApplicationIcon icon, bool beginGroup = false)
      {
         var button = new BarButtonItem(_barManager, caption, ApplicationIcons.IconIndex(icon));
         button.ItemClick += (o, e) => this.DoWithinExceptionHandler(action);
         var link = PopupMenu.AddItem(button);
         link.BeginGroup = beginGroup;
         return link;
      }

      public virtual string Title
      {
         get => _title.Text;
         set => _title.Text = value;
      }

      public virtual string Description
      {
         get => _description.Text;
         set => _description.Text = value;
      }

      /// <summary>
      ///    Copy the chart as emf into the clipboard
      /// </summary>
      public virtual void CopyToClipboard()
      {
         CopyChartToClipboard(this);
      }

      public virtual void CopyChartToClipboard(ChartControl chartControl)
      {
         using (var ms = new MemoryStream())
         {
            chartControl.ExportToImage(ms, ImageFormat.Emf);
            ms.Seek(0, SeekOrigin.Begin);

            _clipboardTask.PutEnhMetafileOnClipboard(this, new Metafile(ms));
         }
      }

      private void initializePopup(bool addCopyToClipboardMenu)
      {
         _barManager.SetPopupContextMenu(this, PopupMenu);

         if (addCopyToClipboardMenu)
            AddCopyToCliboardMenu();
      }

      public void AddCopyToCliboardMenu()
      {
         AddPopupMenu(MenuNames.CopyToClipboard, CopyToClipboard, ApplicationIcons.Copy);
      }

      public DiagramCoordinates DiagramCoordinatesAt(HotTrackEventArgs e)
      {
         return XYDiagram.PointToDiagram(e.HitInfo.HitPoint);
      }

      public XYDiagram XYDiagram
      {
         get
         {
            try
            {
               return Diagram as XYDiagram;
            }
            catch
            {
               // DevExpress throws NullReferenceException, if no diagram exists;
               return null;
            }
         }
      }

      public AxisX AxisX => XYDiagram.AxisX;

      public AxisY AxisY => XYDiagram.AxisY;

      public Color DiagramBackColor
      {
         get
         {
            if (XYDiagram == null) return Color.Empty;
            return XYDiagram.DefaultPane.BackColor;
         }
         set
         {
            if (XYDiagram == null) return;
            XYDiagram.DefaultPane.FillStyle.FillMode = FillMode.Solid;
            XYDiagram.DefaultPane.BackColor = value;
         }
      }

      public void InitializeColor()
      {
         PaletteName = "Pastel Kit";
         DiagramBackColor = Colors.ChartDiagramBack;
         BackColor = Colors.ChartBack;
      }
   }
}