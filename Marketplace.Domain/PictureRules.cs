namespace Marketplace.Domain;

public static class PictureRules
{
    public static bool HasCorrectSize(this Picture picture)
    {
        return picture.Size.Width >= 800 && 
            picture.Size.Height >= 600;
    }
}
