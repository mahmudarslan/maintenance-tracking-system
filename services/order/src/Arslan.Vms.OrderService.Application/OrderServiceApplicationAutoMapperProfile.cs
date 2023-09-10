using Arslan.Vms.OrderService.PurchaseOrders;
using Arslan.Vms.OrderService.SalesOrders;
using Arslan.Vms.OrderService.v1.PurchaseOrders.Dtos;
using Arslan.Vms.OrderService.v1.SalesOrders.Dtos;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Arslan.Vms.OrderService;

public class OrderServiceApplicationAutoMapperProfile : Profile
{
    public OrderServiceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        #region Purchase Order
        CreateMap<PurchaseOrder, PurchaseOrderDto>()
            .ForMember(s => s.ServiceLines, opt => opt.MapFrom(m => m.OrderLines))
            .ForMember(s => s.ProductLines, opt => opt.MapFrom(m => m.ReceiveLines))
            .Ignore(i => i.VendorName);

        CreateMap<PurchaseOrderLine, PurchaseOrderServiceLineDto>()
             .Ignore(i => i.ProductName)
             ;
        CreateMap<PurchaseOrderReceiveLine, PurchaseOrderProductLineDto>()
             .Ignore(i => i.OrderLineId)
             .Ignore(i => i.ReceviceLineId)
             .Ignore(i => i.UnitPrice)
             .Ignore(i => i.SubTotal)
             .Ignore(i => i.Discount);
        #endregion

        #region Sales Order
        CreateMap<SalesOrder, SalesOrderDto>()
            .ForMember(s => s.ServiceLines, opt => opt.MapFrom(m => m.OrderLines))
            .ForMember(s => s.ProductLines, opt => opt.MapFrom(m => m.PickLines))
            .Ignore(i => i.Files);

        CreateMap<SalesOrderLine, SalesOrderServiceLineDto>()
             .Ignore(i => i.ProductName)
             .Ignore(i => i.ProductIsDeleted);

        CreateMap<SalesOrderPickLine, SalesOrderProductLineDto>()
            .Ignore(i => i.OrderLineId)
            .Ignore(i => i.PickLineId)
            .Ignore(i => i.UnitPrice)
            .Ignore(i => i.SubTotal)
            .Ignore(i => i.Discount)
            .Ignore(i => i.TaxCodeId)
            .Ignore(i => i.DiscountIsPercent)
            ;
        #endregion
    }
}
