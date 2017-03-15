using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

namespace OSPSuite.UI.Controls
{
   [UserRepositoryItem("RegisterIconTextEdit")]
   public class RepositoryItemIconTextEdit : RepositoryItemTextEdit
   {
      private object _imageList;
      private int _imageIndex;

      static RepositoryItemIconTextEdit()
      {
         RegisterIconTextEdit();
      }

      public RepositoryItemIconTextEdit()
      {
         _imageList = null;
         ImageIndex = -1;
      }

      public const string IconTextEditName = "IconTextEdit";

      public override string EditorTypeName => IconTextEditName;

      public static void RegisterIconTextEdit()
      {
         EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(IconTextEditName,
            typeof(IconTextEdit), typeof(RepositoryItemIconTextEdit),
            typeof(TextEditViewInfo), new TextEditPainter(), true));
      }

      public override void Assign(RepositoryItem item)
      {
         base.Assign(item);
         var source = item as RepositoryItemIconTextEdit;

         _imageList = source.ImageList;
         _imageIndex = source.ImageIndex;
      }

      [Description("Gets or sets the source of images to be displayed within the editor."), Category(CategoryName.Appearance), TypeConverter(typeof(ImageCollectionImagesConverter))]
      public virtual object ImageList
      {
         get { return _imageList; }
         set
         {
            if (ImageList == value) return;
            _imageList = value;
            OnPropertiesChanged();
         }
      }

      [Description("Gets or sets the index of the image displayed on the editor."), Category(CategoryName.Appearance), DefaultValue(-1), Editor(typeof(ImageIndexesEditor), typeof(UITypeEditor)), ImageList("ImageList")]
      public virtual int ImageIndex
      {
         get { return _imageIndex; }
         set
         {
            if (ImageIndex == value) return;
            _imageIndex = value;
            OnPropertiesChanged();
         }
      }

      public override BaseEditViewInfo CreateViewInfo()
      {
         return new IconTextEditViewInfo(this);
      }

      public override BaseEditPainter CreatePainter()
      {
         return new IconTextEditPainter();
      }
   }

   public class OnIconSelectionEventArgs : EventArgs
   {
      private int _imageIndex;

      public OnIconSelectionEventArgs(object iconCollection, int iconIndex)
      {
         ImageList = iconCollection;
         _imageIndex = iconIndex;
      }

      public virtual int ImageIndex
      {
         get { return _imageIndex; }
         set { _imageIndex = value; }
      }

      public virtual object ImageList
      {
         get;
      }
   }

   public class IconTextEdit : TextEdit
   {
      static IconTextEdit()
      {
         RepositoryItemIconTextEdit.RegisterIconTextEdit();
      }

      public override string EditorTypeName => RepositoryItemIconTextEdit.IconTextEditName;

      [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
      public new RepositoryItemIconTextEdit Properties => base.Properties as RepositoryItemIconTextEdit;

      protected override void OnMouseDown(MouseEventArgs e)
      {
         IconTextEditViewInfo vi = ViewInfo as IconTextEditViewInfo;
         if (vi.IsIconClick(e.Location))
         {
            int newX = e.X - vi.IconRect.Left;
            int newY = e.Y - vi.IconRect.Top;
            MouseEventArgs ee = new MouseEventArgs(e.Button, e.Clicks, newX, newY, e.Delta);
         }
         base.OnMouseDown(e);
      }
   }

   public class IconTextEditViewInfo : TextEditViewInfo
   {
      private Rectangle _fIconRect;
      private const int _separatorWidth = 3;

      public IconTextEditViewInfo(RepositoryItem item)
         : base(item)
      {
         _fIconRect = Rectangle.Empty;
      }

      protected internal virtual bool IsIconClick(Point p)
      {
         if (p.X > IconRect.Left && p.X < IconRect.Right && p.Y > IconRect.Top && p.Y < IconRect.Bottom) return true;
         else return false;
      }

      public object ImageList => Item.ImageList;

      public int ImageIndex => Item.ImageIndex;

      public new virtual RepositoryItemIconTextEdit Item => base.Item as RepositoryItemIconTextEdit;

      protected override void Assign(BaseControlViewInfo info)
      {
         base.Assign(info);
         IconTextEditViewInfo be = info as IconTextEditViewInfo;
         if (be == null) return;
         this._fIconRect = be._fIconRect;
      }

      public override Size CalcBestFit(Graphics g)
      {
         Size s = base.CalcBestFit(g);
         s.Width = s.Width + _separatorWidth * 2 + GetImageSize().Width;
         return s;
      }

      protected internal virtual Rectangle IconRect
      {
         get { return _fIconRect; }
         set { _fIconRect = value; }
      }

      public override void Offset(int x, int y)
      {
         base.Offset(x, y);
         if (!_fIconRect.IsEmpty) this._fIconRect.Offset(x, y);
      }

      protected override Rectangle CalcMaskBoxRect(Rectangle content, ref Rectangle contextImageBounds)
      {
         Rectangle r = base.CalcMaskBoxRect(content, ref contextImageBounds);
         r.Width = r.Width - GetImageSize().Width - _separatorWidth;
         return r;
      }

      protected override void CalcContentRect(Rectangle bounds)
      {
         base.CalcContentRect(bounds);
         _fIconRect = CalcIconRect(ContentRect);
      }

      protected virtual Rectangle CalcIconRect(Rectangle content)
      {
         Rectangle r = fMaskBoxRect;
         r.Size = GetImageSize();
         r.Location = new Point(fMaskBoxRect.Right + _separatorWidth, content.Top + content.Height / 2 - r.Height / 2);
         return r;
      }

      protected Size GetImageSize()
      {
         ImageCollection ic = ImageList as ImageCollection;
         if (ic != null) return ic.ImageSize;
         return new Size(0, 0);
      }

      protected override int CalcMinHeightCore(Graphics g)
      {
         int imageHeight = 0;
         if (ImageList != null)
         {
            imageHeight = GetImageSize().Height;
            if (AllowDrawFocusRect)
               imageHeight += (FocusRectThin + 1) * 2;
         }
         int fontHeight = base.CalcMinHeightCore(g);
         if (imageHeight > fontHeight) return imageHeight;
         else return fontHeight;
      }
   }

   public class IconTextEditPainter : TextEditPainter
   {
      protected override void DrawContent(ControlGraphicsInfoArgs info)
      {
         base.DrawContent(info);
         DrawIcon(info);
      }

      protected virtual void DrawIcon(ControlGraphicsInfoArgs info)
      {
         IconTextEditViewInfo vi = info.ViewInfo as IconTextEditViewInfo;
         OnIconSelectionEventArgs e = new OnIconSelectionEventArgs(vi.ImageList, vi.ImageIndex);

         if (e.ImageList != null && e.ImageIndex != -1)
            info.Cache.Paint.DrawImage(info.Cache, e.ImageList, e.ImageIndex, vi.IconRect, true);
         else
            info.Graphics.FillRectangle(info.Cache.GetSolidBrush(Color.White), vi.IconRect);
      }
   }
}