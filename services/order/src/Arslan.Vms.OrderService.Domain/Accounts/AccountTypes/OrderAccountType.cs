namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public enum OrderAccountType
    {
        InvPurchased = 1101,
        InvServicesReceived = 1105,
        InvNonstockedReceived = 1106,
        InvOnOrder = 1201,
        InvPicked = 1203,
        InvPending = 1300,
        InvSold = 1400,
        InvServicesPerformed = 1407,
        InvNonstockedUsed = 1408,

        MnyServiceCosts = 2305,
        MnyNonstockedCosts = 2306,
        MnyInventoryValue = 2201,
        MnyCOGS = 2401,


    }
}
