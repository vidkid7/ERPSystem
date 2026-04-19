using AutoMapper;
using UltimateERP.Application.Features.Purchase.DTOs;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Purchase.Mappings;

public class PurchaseCycleMappingProfile : Profile
{
    public PurchaseCycleMappingProfile()
    {
        // Purchase Quotation
        CreateMap<PurchaseQuotation, PurchaseQuotationDto>()
            .ForMember(d => d.QuotationNo, o => o.MapFrom(s => s.QuotationNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.QuotationDate))
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Remarks, o => o.MapFrom(s => s.Remarks))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Details));

        CreateMap<PurchaseQuotationDetail, PurchaseQuotationItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.Qty, o => o.MapFrom(s => s.Quantity));

        // Purchase Order
        CreateMap<PurchaseOrder, PurchaseOrderDto>()
            .ForMember(d => d.OrderNo, o => o.MapFrom(s => s.OrderNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.OrderDate))
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null))
            .ForMember(d => d.QuotationRef, o => o.MapFrom(s => s.PurchaseQuotationId))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Remarks, o => o.MapFrom(s => s.Remarks))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Details));

        CreateMap<PurchaseOrderDetail, PurchaseOrderItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.Qty, o => o.MapFrom(s => s.Quantity))
            .ForMember(d => d.ReceivedQty, o => o.MapFrom(s => s.ReceivedQuantity));

        // Receipt Note (GRN)
        CreateMap<ReceiptNote, ReceiptNoteDto>()
            .ForMember(d => d.GRNNo, o => o.MapFrom(s => s.GRNNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.ReceiptDate))
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null))
            .ForMember(d => d.PurchaseOrderRef, o => o.MapFrom(s => s.PurchaseOrderId))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Remarks, o => o.MapFrom(s => s.Remarks))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Details));

        CreateMap<ReceiptNoteDetail, ReceiptNoteItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.ReceivedQty, o => o.MapFrom(s => s.ReceivedQuantity))
            .ForMember(d => d.AcceptedQty, o => o.MapFrom(s => s.AcceptedQuantity))
            .ForMember(d => d.RejectedQty, o => o.MapFrom(s => s.RejectedQuantity));

        // Purchase Return
        CreateMap<PurchaseReturn, PurchaseReturnDto>()
            .ForMember(d => d.ReturnNo, o => o.MapFrom(s => s.ReturnNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.ReturnDate))
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null))
            .ForMember(d => d.InvoiceRef, o => o.MapFrom(s => s.InvoiceReference))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Remarks, o => o.MapFrom(s => s.Remarks))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Details));

        CreateMap<PurchaseReturnDetail, PurchaseReturnItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
            .ForMember(d => d.ReturnQty, o => o.MapFrom(s => s.ReturnQuantity));

        // Purchase Debit Note
        CreateMap<PurchaseDebitNote, PurchaseDebitNoteDto>()
            .ForMember(d => d.NoteNo, o => o.MapFrom(s => s.NoteNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.NoteDate))
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null))
            .ForMember(d => d.InvoiceRef, o => o.MapFrom(s => s.InvoiceReference))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        // Purchase Credit Note
        CreateMap<PurchaseCreditNote, PurchaseCreditNoteDto>()
            .ForMember(d => d.NoteNo, o => o.MapFrom(s => s.NoteNumber))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.NoteDate))
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.Name : null))
            .ForMember(d => d.InvoiceRef, o => o.MapFrom(s => s.InvoiceReference))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
