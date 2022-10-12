using System.ComponentModel;

namespace StaticIP.Models
{
    public class IPAddressModel
    {
        public int Id { get; set; }

        [DisplayName("IPv4 Address")]
        public string VFourAddress { get; set; }

        public bool InUse { get; set; }
    }
}
