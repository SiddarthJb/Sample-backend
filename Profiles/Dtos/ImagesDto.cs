namespace Z1.Profiles.Dtos
{
    public class ImagesDto
    {
        public string Image { get; set; }
        public int Order { get; set; }
    }

    public class UpdateImageOrderDto
    {
        public List<int> NewOrder { get; set; }
    }
}
