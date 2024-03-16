namespace SharedData
{
    public class Review
    {
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public long Rating { get; set; }

        public int HardwareId { get; set; }

        public Hardware? Hardware { get; set; }
    }
}
