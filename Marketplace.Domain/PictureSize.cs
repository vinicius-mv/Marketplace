using Marketplace.Framework;

namespace Marketplace.Domain;

public class PictureSize : Value<PictureSize>
{
    public int Width { get; set; }
    public int Height { get; set; }

    public PictureSize(int width, int height)
    {
        if (width <= 0)
            throw new InvalidEntityStateException(nameof(Width), "Picture width must be a positive number");

        if (height <= 0)
            throw new InvalidEntityStateException(nameof(Height), "Picture height must be a positive number");

        Width = width;
        Height = height;
    }

    internal PictureSize() { }
}
