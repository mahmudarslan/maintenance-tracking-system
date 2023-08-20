using System.ComponentModel;

namespace Arslan.Vms.VehicleService.Addresses
{
    public enum AddressKind
    {
        [Description("Commercial")]
        Commercial = 1,
        [Description("Residental")]
        Residental = 2
    }
}