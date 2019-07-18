using System.Drawing;

namespace OSPSuite.Assets
{
    public class ApplicationImage
    {
        private readonly Image _image;

        public ApplicationImage(Image image)
        {
            _image = image;
        }

        public static implicit operator Image(ApplicationImage applicationImage)
        {
            return applicationImage.toImage();
        }

        private Image toImage()
        {
            return _image;
        }
    }
}