using Arslan.Vms.OrderService.PurchaseOrders;
using Arslan.Vms.OrderService.PurchaseOrders.Versions;
using Arslan.Vms.OrderService.SalesOrders;
using Arslan.Vms.OrderService.SalesOrders.Versions;
using System;

namespace Arslan.Vms.OrderService
{
    //public class OrdersEntityFrameworkCoreAutoMapperProfile : Profile
    //{
    //    public OrdersEntityFrameworkCoreAutoMapperProfile()
    //    {
    //        CreateMap<PurchaseOrder, PurchaseOrderVersion>()
    //            .ForMember(s => s.OrderLines, opt => opt.MapFrom(m => m.OrderLines))
    //            .ForMember(s => s.ReceiveLines, opt => opt.MapFrom(m => m.ReceiveLines))
    //            .ForMember(f => f.Id, opt => opt.MapFrom(src => Guid.Empty))
    //            .ForMember(f => f.PurchaseOrderId, opt => opt.MapFrom(src => src.Id));

    //        CreateMap<PurchaseOrderLine, PurchaseOrderLineVersion>()
    //            .ForMember(f => f.Id, opt => opt.MapFrom(src => Guid.Empty))
    //            .ForMember(f => f.PurchaseOrderLineId, opt => opt.MapFrom(src => src.Id));

    //        CreateMap<PurchaseOrderReceiveLine, PurchaseOrderReceiveLineVersion>()
    //            .ForMember(f => f.Id, opt => opt.MapFrom(src => Guid.Empty))
    //            .ForMember(f => f.PurchaseOrderReceiveLineId, opt => opt.MapFrom(src => src.Id));


    //        CreateMap<SalesOrder, SalesOrderVersion>()
    //            .ForMember(s => s.Lines, opt => opt.MapFrom(m => m.OrderLines))
    //            .ForMember(s => s.PickLines, opt => opt.MapFrom(m => m.PickLines))
    //            .ForMember(f => f.Id, opt => opt.MapFrom(src => Guid.Empty))
    //            .ForMember(f => f.SalesOrderId, opt => opt.MapFrom(src => src.Id));

    //        CreateMap<SalesOrderLine, SalesOrderLineVersion>()
    //            .ForMember(f => f.Id, opt => opt.MapFrom(src => Guid.Empty))
    //            .ForMember(f => f.SalesOrderLineId, opt => opt.MapFrom(src => src.Id));

    //        CreateMap<SalesOrderPickLine, SalesOrderPickLineVersion>()
    //            .ForMember(f => f.Id, opt => opt.MapFrom(src => Guid.Empty))
    //            .ForMember(f => f.SalesOrderPickLineId, opt => opt.MapFrom(src => src.Id));
    //    }
    //}
}