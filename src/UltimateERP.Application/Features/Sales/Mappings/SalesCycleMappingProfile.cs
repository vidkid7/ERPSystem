using AutoMapper;
using UltimateERP.Application.Features.Sales.DTOs;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Sales.Mappings;

public class SalesCycleMappingProfile : Profile
{
    public SalesCycleMappingProfile()
    {
        // Sales Quotation
        CreateMap<SalesQuotation, SalesQuotationDto>()
            .ForMember(d => d.QuotationNo, o => o.MapFrom(s => s.QuotationNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.QuotationDate))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Details))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<SalesQuotationDetail, SalesQuotationItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.Qty, o => o.MapFrom(s => s.Quantity));

        // Sales Order
        CreateMap<SalesOrder, SalesOrderDto>()
            .ForMember(d => d.OrderNo, o => o.MapFrom(s => s.OrderNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.OrderDate))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.QuotationRef, o => o.MapFrom(s => s.SalesQuotationId))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Details))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<SalesOrderDetail, SalesOrderItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.Qty, o => o.MapFrom(s => s.Quantity))
            .ForMember(d => d.DeliveredQty, o => o.MapFrom(s => s.DeliveredQuantity));

        // Sales Allotment
        CreateMap<SalesAllotment, SalesAllotmentDto>()
            .ForMember(d => d.AllotmentNo, o => o.MapFrom(s => s.AllotmentNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.AllotmentDate))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.AllocatedQty, o => o.MapFrom(s => s.AllottedQuantity))
            .ForMember(d => d.DeliveredQty, o => o.MapFrom(s => s.DeliveredQuantity))
            .ForMember(d => d.DeliveryDate, o => o.MapFrom(s => s.AllotmentDate.ToString("yyyy-MM-dd")))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        // Sales Delivery Note (DispatchOrder)
        CreateMap<DispatchOrder, SalesDeliveryNoteDto>()
            .ForMember(d => d.DeliveryNoteNo, o => o.MapFrom(s => s.DispatchNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.DispatchDate))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Sections))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<DispatchSection, SalesDeliveryItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.DeliveredQty, o => o.MapFrom(s => s.Quantity));

        // Sales Return
        CreateMap<SalesReturn, SalesReturnDto>()
            .ForMember(d => d.ReturnNo, o => o.MapFrom(s => s.ReturnNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.ReturnDate))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.InvoiceRef, o => o.MapFrom(s => s.SalesInvoiceId))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Details))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<SalesReturnDetail, SalesReturnItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.ReturnQty, o => o.MapFrom(s => s.Quantity));

        // Sales Debit Note
        CreateMap<SalesDebitNote, SalesDebitNoteDto>()
            .ForMember(d => d.NoteNo, o => o.MapFrom(s => s.NoteNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.NoteDate))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.InvoiceRef, o => o.MapFrom(s => s.SalesInvoiceId))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        // Sales Credit Note
        CreateMap<SalesCreditNote, SalesCreditNoteDto>()
            .ForMember(d => d.NoteNo, o => o.MapFrom(s => s.NoteNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.NoteDate))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.InvoiceRef, o => o.MapFrom(s => s.SalesInvoiceId))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
