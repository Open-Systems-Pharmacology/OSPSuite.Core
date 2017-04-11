using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraCharts;
using OSPSuite.Assets;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Controls
{
   public class UxChartControl : ChartControl
   {
      private readonly ChartTitle _title;
      private readonly ChartTitle _description;
      private readonly ClipboardTask _clipboardTask;
      private readonly BarManager _barManager;
      private readonly PopupMenu _popupMenu;

      public UxChartControl(bool addDefaultPopup = true)
      {
         Titles.Clear();

         _title = new ChartTitle {Text = string.Empty, Font = new Font("Arial", 16), Alignment = StringAlignment.Center, Dock = ChartTitleDockStyle.Top, WordWrap = true};
         _description = new ChartTitle { Text = string.Empty, Font = new Font("Arial", 12), Alignment = StringAlignment.Near, Dock = ChartTitleDockStyle.Bottom, WordWrap = true };

         Titles.Add(_title);
         Titles.Add(_description);

         _clipboardTask = new ClipboardTask();
         _barManager = new BarManager {Form = this};
         _popupMenu = new PopupMenu(_barManager);

         if (!addDefaultPopup) return;

         _barManager.SetPopupContextMenu(this, _popupMenu);
         initializePopup();
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

      public ImageCollection Images
      {
         set { _barManager.Images = value; }
      }

      public BarItemLink AddPopupMenu(string caption, Action action, ApplicationIcon icon, bool beginGroup = false)
      {
         var button = new BarButtonItem(_barManager, caption, ApplicationIcons.IconIndex(icon));
         button.ItemClick += (o, e) => this.DoWithinExceptionHandler(action);
         var link = _popupMenu.AddItem(button);
         link.BeginGroup = beginGroup;
         return link;
      }

      public virtual string Title
      {
         get { return _title.Text; }
         set { _title.Text = value; }
      }

      public virtual string Description
      {
         get { return _description.Text; }
         set { _description.Text = value; }
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

      private void initializePopup()
      {
         AddPopupMenu(Captions.CopyAsImage, CopyToClipboard, ApplicationIcons.Paste);
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