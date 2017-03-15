using System.Drawing;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.UI.Mappers
{
   public interface ISkinManagerToSkinGalleryMapper
   {
      RibbonGalleryBarItem MapFrom(RibbonBarManager barManager, ISkinManager skinManager);
   }

   public class SkinManagerToSkinGalleryMapper : ISkinManagerToSkinGalleryMapper
   {
      private readonly IContainer _container;

      public SkinManagerToSkinGalleryMapper(IContainer container)
      {
         _container = container;
      }

      public RibbonGalleryBarItem MapFrom(RibbonBarManager barManager, ISkinManager skinManager)
      {
         var rgbiSkins = createSkinGallery(barManager);
         foreach (var skin in skinManager.All())
         {
            var command = _container.Resolve<ActivateSkinCommand>();
            command.SkinName = skin;
            addSkinToGallery(rgbiSkins, CreateMenuButton.WithCaption(skin).WithCommand(command));
         }
         return rgbiSkins;
      }

      private RibbonGalleryBarItem createSkinGallery(RibbonBarManager barManager)
      {
         var rgbiSkins = new RibbonGalleryBarItem(barManager);
         rgbiSkins.Caption = "Skins";
         rgbiSkins.Gallery.AllowHoverImages = true;
         rgbiSkins.Gallery.Appearance.ItemCaptionAppearance.Normal.Options.UseFont = true;
         rgbiSkins.Gallery.Appearance.ItemCaptionAppearance.Normal.Options.UseTextOptions = true;
         rgbiSkins.Gallery.Appearance.ItemCaptionAppearance.Normal.TextOptions.HAlignment = HorzAlignment.Center;
         rgbiSkins.Gallery.ColumnCount = 4;
         rgbiSkins.Gallery.FixedHoverImageSize = false;
         rgbiSkins.Gallery.Groups.AddRange(new[] { new GalleryItemGroup { Caption = "Skins" } });
         rgbiSkins.Gallery.ImageSize = new Size(32, 17);
         rgbiSkins.Gallery.ItemImageLocation = Locations.Top;
         rgbiSkins.Gallery.RowCount = 4;
         rgbiSkins.Gallery.InitDropDownGallery += (o, e) => rgbiSkins_Gallery_InitDropDownGallery(rgbiSkins, e);
         rgbiSkins.Gallery.ItemClick += (o, e) => e.Item.Tag.DowncastTo<IMenuBarButton>().Click();
         return rgbiSkins;
      }

      private void addSkinToGallery(RibbonGalleryBarItem rgbiSkins, IMenuBarButton menuBarItem)
      {
         SimpleButton imageButton = new SimpleButton();
         imageButton.LookAndFeel.SetSkinStyle(menuBarItem.Caption);
         GalleryItem gItem = new GalleryItem();
         gItem.Tag = menuBarItem;
         gItem.Caption = menuBarItem.Caption;
         rgbiSkins.Gallery.Groups[0].Items.Add(gItem);

         gItem.Image = getSkinImage(imageButton, 32, 17, 2);
         gItem.HoverImage = getSkinImage(imageButton, 70, 36, 5);
         gItem.Caption = menuBarItem.Caption;
         gItem.Hint = menuBarItem.Caption;
      }

      private Bitmap getSkinImage(SimpleButton button, int width, int height, int indent)
      {
         Bitmap image = new Bitmap(width, height);
         using (Graphics g = Graphics.FromImage(image))
         {
            StyleObjectInfoArgs info = new StyleObjectInfoArgs(new GraphicsCache(g));
            info.Bounds = new Rectangle(0, 0, width, height);
            button.LookAndFeel.Painter.GroupPanel.DrawObject(info);
            button.LookAndFeel.Painter.Border.DrawObject(info);
            info.Bounds = new Rectangle(indent, indent, width - indent * 2, height - indent * 2);
            button.LookAndFeel.Painter.Button.DrawObject(info);
         }
         return image;
      }

      private void rgbiSkins_Gallery_InitDropDownGallery(RibbonGalleryBarItem rgbiSkins, InplaceGalleryEventArgs e)
      {
         e.PopupGallery.CreateFrom(rgbiSkins.Gallery);
         e.PopupGallery.AllowFilter = false;
         e.PopupGallery.ShowItemText = true;
         e.PopupGallery.ShowGroupCaption = true;
         e.PopupGallery.AllowHoverImages = false;
         foreach (GalleryItemGroup galleryGroup in e.PopupGallery.Groups)
            foreach (GalleryItem item in galleryGroup.Items)
               item.Image = item.HoverImage;
         e.PopupGallery.ColumnCount = 2;
         e.PopupGallery.ImageSize = new Size(70, 36);
      }
   }
}