namespace SharedData
{
    public class Hardware
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public HardwareType Type { get; set; }

        public string Code { get; set; } = string.Empty;

        public long Stock { get; set; }

        public double Price { get; set; }
    }
}
