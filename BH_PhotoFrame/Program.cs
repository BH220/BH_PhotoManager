namespace BH_PhotoFrame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var slideshow = new PhotoSlideshow(@"d:\a", 5000);
            slideshow.Run();
        }
    }
}
