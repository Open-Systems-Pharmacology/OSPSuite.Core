using DevExpress.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation;
using OSPSuite.UI.Mappers;

namespace OSPSuite.UI.Services
{
   public class ImageListRetriever : IImageListRetriever
   {
      private readonly IApplicationIconsToImageCollectionMapper _mapper;
      private readonly IPresentationUserSettings _userSettings;
      private bool _initialized;

      private SvgImageCollection _allImages16x16;
      private SvgImageCollection _allImages32x32;
      private SvgImageCollection _allImages24x24;
      private SvgImageCollection _allImages48x48;

      public ImageListRetriever(IApplicationIconsToImageCollectionMapper mapper, IPresentationUserSettings userSettings)
      {
         _mapper = mapper;
         _userSettings = userSettings;
      }

      public SvgImageCollection AllImages16x16
      {
         get
         {
            Start();
            return _allImages16x16;
         }
      }

      public SvgImageCollection AllImages24x24
      {
         get
         {
            Start();
            return _allImages24x24;
         }
      }

      public SvgImageCollection AllImages32x32
      {
         get
         {
            Start();
            return _allImages32x32;
         }
      }


      private SvgImageCollection allImageForSize(IconSize iconSize)
      {
         if (iconSize == IconSizes.Size16x16)
            return _allImages16x16;

         if (iconSize == IconSizes.Size24x24)
            return _allImages24x24;

         if (iconSize == IconSizes.Size32x32)
            return _allImages32x32;

         if (iconSize == IconSizes.Size48x48)
            return _allImages48x48;

         return _allImages24x24;
      }

      public int ImageIndex(string imageName)
      {
         Start();
         return ApplicationIcons.IconIndex(imageName);
      }

      public int ImageIndex(ApplicationIcon icon)
      {
         return ImageIndex(icon.IconName);
      }

      public void Start()
      {
         if (_initialized) return;
         _allImages16x16 = _mapper.MapFrom(ApplicationIcons.All(), IconSizes.Size16x16);
         _allImages24x24 = _mapper.MapFrom(ApplicationIcons.All(), IconSizes.Size24x24);
         _allImages32x32 = _mapper.MapFrom(ApplicationIcons.All(), IconSizes.Size32x32);
         _allImages48x48 = _mapper.MapFrom(ApplicationIcons.All(), IconSizes.Size48x48);
         _initialized = true;
      }

      public SvgImageCollection AllImagesForTreeView
      {
         get { return allImageForSize(_userSettings.IconSizeTreeView); }
      }

      public SvgImageCollection AllImagesForContextMenu
      {
         get { return allImageForSize(_userSettings.IconSizeContextMenu); }
      }

      public SvgImageCollection AllImagesForTabs
      {
         get { return allImageForSize(_userSettings.IconSizeTab); }
      }
   }
}