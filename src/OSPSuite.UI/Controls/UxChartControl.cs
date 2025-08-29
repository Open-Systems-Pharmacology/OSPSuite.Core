using System;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraCharts;
using OSPSuite.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Controls
{
   public class UxChartControl : ChartControl
   {
      private const int TITLE_DEFAULT_FONT_SIZE = 16;
      private const int DESCRIPTION_DEFAULT_FONT_SIZE = 12;

      private readonly ChartTitle _title;
      private readonly ChartTitle _description;
      private readonly BarManager _barManager;
      public PopupMenu PopupMenu { get; }

      public UxChartControl() : this(true)
      {
      }

      public UxChartControl(bool useDefaultPopupMechanism = true)
      {
         Titles.Clear();

         _title = createTitle(TITLE_DEFAULT_FONT_SIZE, StringAlignment.Center, ChartTitleDockStyle.Top);
         _description = createTitle(DESCRIPTION_DEFAULT_FONT_SIZE, StringAlignment.Near, ChartTitleDockStyle.Bottom);

         Titles.Add(_title);
         Titles.Add(_description);

         _barManager = new BarManager { Form = this };
         PopupMenu = new PopupMenu(_barManager);

         if (useDefaultPopupMechanism)
            initializePopup();
      }

      private ChartTitle createTitle(int fontSize, StringAlignment alignment, ChartTitleDockStyle dockStyle)
      {
         return new ChartTitle { Text = string.Empty, Font = new Font("Arial", fontSize), Alignment = alignment, Dock = dockStyle, WordWrap = true };
      }

      public SvgImageCollection Images
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

      public BarItemLink AddExportToPngPopupMenu(ICanExportCharts canExportChartsToPng, bool beginGroup = false)
      {
         return AddPopupMenu(MenuNames.ExportToPng, canExportChartsToPng.ExportToPng, ApplicationIcons.ExportToPNG);
      }

      public BarItemLink AddCopyToClipboardPopupMenu(ICanExportCharts canCopyToClipboard, bool beginGroup = false)
      {
         return AddPopupMenu(Captions.CopyToClipboard, canCopyToClipboard.CopyToClipboard, ApplicationIcons.Copy, beginGroup);
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

      private void initializePopup()
      {
         _barManager.SetPopupContextMenu(this, PopupMenu);
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