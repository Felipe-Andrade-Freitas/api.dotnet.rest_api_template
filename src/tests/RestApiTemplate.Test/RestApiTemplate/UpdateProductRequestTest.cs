using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductService;
using System.Text;
using Application.Common.Controllers;
using Application.Common.Persistence;
using Application.Products.EventHandlers.Dtos;
using Application.Products.EventHandlers.Requests.CreateProduct;
using Application.Products.EventHandlers.Requests.UpdateProduct;
using Bogus.Extensions.Brazil;
using Domain.Entities;
using Domain.Enums;
using Moq;
using Newtonsoft.Json;
using Shared.Exceptions;
using Shared.Extensions;

namespace RestApiTemplateTest.Product;

public class UpdateProductRequestTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UpdateProductRequestTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact(DisplayName = "UpdateProductRequest_WhenRequestFilledRight_200OK")]
    public async Task UpdateProductRequest_WhenRequestFilledRight_200OK()
    {
        // Arrange
        var response = new Faker<Domain.Entities.Product>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Seller, _ => new Faker<Seller>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf(false))
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<ItemProduct>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList())
            .Generate();

        var requestFake = new Faker<UpdateProductRequest>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.Status, _ => StatusProductEnum.PaymentApproved);

        var request = requestFake.Generate();

        var repositoryMock = new Mock<IRepository<Domain.Entities.Product>>();
        repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(response);

        repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductRequestHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "UpdateProductRequest_WhenRequestFilledRight_AwaitingPayment_Canceled_200OK")]
    public async Task UpdateProductRequest_WhenRequestFilledRight_AwaitingPayment_Canceled_200OK()
    {
        // Arrange
        var response = new Faker<Domain.Entities.Product>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Status, _ => StatusProductEnum.AwaitingPayment)
            .RuleFor(s => s.Seller, _ => new Faker<Seller>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf(false))
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<ItemProduct>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList())
            .Generate();

        var requestFake = new Faker<UpdateProductRequest>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.Status, _ => StatusProductEnum.Canceled);

        var request = requestFake.Generate();

        var repositoryMock = new Mock<IRepository<Domain.Entities.Product>>();
        repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(response);

        repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductRequestHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "UpdateProductRequest_WhenRequestFilledRight_PaymentApproved_Canceled_200OK")]
    public async Task UpdateProductRequest_WhenRequestFilledRight_PaymentApproved_Canceled_200OK()
    {
        // Arrange
        var response = new Faker<Domain.Entities.Product>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Status, _ => StatusProductEnum.PaymentApproved)
            .RuleFor(s => s.Seller, _ => new Faker<Seller>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf(false))
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<ItemProduct>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList())
            .Generate();

        var requestFake = new Faker<UpdateProductRequest>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.Status, _ => StatusProductEnum.Canceled);

        var request = requestFake.Generate();

        var repositoryMock = new Mock<IRepository<Domain.Entities.Product>>();
        repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(response);

        repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductRequestHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "UpdateProductRequest_WhenRequestFilledRight_PaymentApproved_SentToCarrier_200OK")]
    public async Task UpdateProductRequest_WhenRequestFilledRight_PaymentApproved_SentToCarrier_200OK()
    {
        // Arrange
        var response = new Faker<Domain.Entities.Product>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Status, _ => StatusProductEnum.PaymentApproved)
            .RuleFor(s => s.Seller, _ => new Faker<Seller>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf(false))
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<ItemProduct>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList())
            .Generate();

        var requestFake = new Faker<UpdateProductRequest>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.Status, _ => StatusProductEnum.SentToCarrier);

        var request = requestFake.Generate();

        var repositoryMock = new Mock<IRepository<Domain.Entities.Product>>();
        repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(response);

        repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductRequestHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "UpdateProductRequest_WhenRequestFilledRight_SentToCarrier_Delivered_200OK")]
    public async Task UpdateProductRequest_WhenRequestFilledRight_SentToCarrier_Delivered_200OK()
    {
        // Arrange
        var response = new Faker<Domain.Entities.Product>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Status, _ => StatusProductEnum.SentToCarrier)
            .RuleFor(s => s.Seller, _ => new Faker<Seller>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf(false))
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<ItemProduct>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList())
            .Generate();

        var requestFake = new Faker<UpdateProductRequest>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.Status, _ => StatusProductEnum.Delivered);

        var request = requestFake.Generate();

        var repositoryMock = new Mock<IRepository<Domain.Entities.Product>>();
        repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(response);

        repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductRequestHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "UpdateProductRequest_StatusCode422OK")]
    public async Task UpdateProductRequest_StatusCode422OK()
    {
        // Arrange
        var response = new Faker<Domain.Entities.Product>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Status, _ => StatusProductEnum.Delivered)
            .RuleFor(s => s.Seller, _ => new Faker<Seller>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf(false))
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<ItemProduct>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList())
            .Generate();

        var requestFake = new Faker<UpdateProductRequest>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.Status, _ => StatusProductEnum.PaymentApproved);

        var request = requestFake.Generate();

        var repositoryMock = new Mock<IRepository<Domain.Entities.Product>>();
        repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(response);

        repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductRequestHandler(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(request, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(string.Format(ErrorsMessages.Product.UnableToUpdateProductStatus, StatusProductEnum.Delivered, StatusProductEnum.PaymentApproved), exception.Message);
    }

    [Fact(DisplayName = "UpdateProductRequest_WhenIntegrationRequestFilledRight_200OK")]
    public async Task UpdateProductRequest_WhenIntegrationRequestFilledRight_200OK()
    {
        // Arrange
        var request = new Faker<CreateProductRequest>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Seller, _ => new Faker<CreateSellerToProductRequest>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf(false))
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<CreateItemToProductRequest>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList())
            .Generate();

        var responseCreate = await _client.PostAsync($"/api/sale", new StringContent(request.ToJson(), Encoding.UTF8, "application/json"));
        responseCreate.EnsureSuccessStatusCode();

        var result = JsonConvert.DeserializeObject<BaseResult<Guid>>(await responseCreate.Content.ReadAsStringAsync());
        Assert.NotNull(result);

        var id = result.Data;

        // Act
        var requestUpdate = new Faker<UpdateProductRequest>()
            .RuleFor(s => s.Status, _ => StatusProductEnum.PaymentApproved)
            .Generate();

        var responseUpdate = await _client.PatchAsync($"/api/sale/{id}/status", new StringContent(requestUpdate.ToJson(), Encoding.UTF8, "application/json"));
        responseUpdate.EnsureSuccessStatusCode();

        var resultUpdate = JsonConvert.DeserializeObject<BaseResult<Guid>>(await responseUpdate.Content.ReadAsStringAsync());
        Assert.NotNull(resultUpdate);

        var response = await _client.GetAsync($"/api/sale/{id}");
        var getResult =
            JsonConvert.DeserializeObject<BaseResult<ProductResponse>>(await response.Content.ReadAsStringAsync());

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(getResult);
        Assert.NotNull(getResult.Data);
        Assert.Equal(request.Date, getResult.Data.Date);
        Assert.Equal(StatusProductEnum.PaymentApproved, getResult.Data.Status);
        Assert.NotNull(getResult.Data.Seller);
        Assert.Equal(request.Seller.Document, getResult.Data.Seller.Document);
        Assert.Equal(request.Seller.Email, getResult.Data.Seller.Email);
        Assert.Equal(request.Seller.Name, getResult.Data.Seller.Name);
        Assert.Equal(request.Seller.Phone, getResult.Data.Seller.Phone);
        Assert.NotNull(getResult.Data.Items);
        Assert.All(request.Items, saleRequest =>
        {
            var actualItem = getResult.Data.Items.FirstOrDefault(item => item.Name == saleRequest.Name);
            Assert.NotNull(actualItem);
            Assert.Equal(saleRequest.Name, actualItem.Name);
            Assert.Equal(saleRequest.Quantity, actualItem.Quantity);
            Assert.Equal(saleRequest.Price, actualItem.Price);
        });
    }
}