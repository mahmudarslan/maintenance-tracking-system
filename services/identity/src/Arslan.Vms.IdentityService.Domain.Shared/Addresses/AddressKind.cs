using System.ComponentModel;

namespace Arslan.Vms.IdentityService.Addresses
{
    public enum AddressKind
    {
        [Description("Commercial")]
        Commercial = 1,
        [Description("Residental")]
        Residental = 2
    }
}