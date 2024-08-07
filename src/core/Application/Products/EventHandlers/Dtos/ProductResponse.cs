using Domain.Enums;

namespace Application.Products.EventHandlers.Dtos;

public record ProductResponse(Guid Id, string Code, DateTime Date, StatusProductEnum Status, SellerResponse Seller, List<ItemProductResponse>? Items);
public record SellerResponse(Guid Id, string Document, string Name, string Email, string Phone);
public record ItemProductResponse(string Name, decimal Price, int Quantity);
