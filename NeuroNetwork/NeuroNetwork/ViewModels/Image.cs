namespace NeuroNetwork.ViewModels
{
    public class Image
    {
        public string ShortFileName { get; set; }
        public string Uri { get; set; }
        public int[] Pixels { get; set; }
        public int Answer { get; set; }
    }
}