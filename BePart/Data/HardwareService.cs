using SharedData;

namespace BePart.Data
{
    public interface IHardwareService
    {
        List<Hardware> GetAllHardware();
        Hardware GetHardwareById(int id);
        void AddHardware(Hardware hardware);
        void UpdateHardware(Hardware hardware);
        void DeleteHardware(int id);
    }

    public class HardwareService : IHardwareService
    {
        private readonly List<Hardware> _hardwares = new List<Hardware>();

        public HardwareService()
        {
            _hardwares.Add(new Hardware { Id = 1, Name = "Asus TUF RTX 3080", Type = HardwareType.GPU, Code = "1234561", Stock = 0, Price = 1599.00 });
            _hardwares.Add(new Hardware { Id = 2, Name = "EVGA XC3 RTX 3070 Ti", Type = HardwareType.GPU, Code = "1234562", Stock = 0, Price = 1299.00 });
            _hardwares.Add(new Hardware { Id = 3, Name = "AMD Ryzen 5950X", Type = HardwareType.CPU, Code = "1234563", Stock = 0, Price = 899.00 });
            _hardwares.Add(new Hardware { Id = 4, Name = "Samsung 980 PRO SSD 1TB", Type = HardwareType.STORAGE, Code = "1234564", Stock = 0, Price = 299.00 });
            _hardwares.Add(new Hardware { Id = 5, Name = "Kingston FURY Beast DDR5 32GB", Type = HardwareType.RAM, Code = "1234565", Stock = 0, Price = 699.00 });
        }

        public List<Hardware> GetAllHardware() => _hardwares;

        public Hardware GetHardwareById(int id) => _hardwares.SingleOrDefault(c => c.Id == id);

        public void AddHardware(Hardware hardware)
        {
            var lastHardware = _hardwares.Count() == 0 ? 0 : _hardwares.Last().Id;
            lastHardware += 1;
            hardware.Id = lastHardware;
            _hardwares.Add(hardware);
        }

        public void UpdateHardware(Hardware hardware)
        {
            var index = _hardwares.FindIndex(c => c.Id == hardware.Id);
            if (index != -1)
            {
                _hardwares[index] = hardware;
            }
        }

        public void DeleteHardware(int id)
        {
            _hardwares.RemoveAll(c => c.Id == id);
        }
    }
}
