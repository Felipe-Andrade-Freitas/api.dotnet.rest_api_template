using Bogus;
using ProductService;
using System.Text;
using Application.Common.Controllers;
using Application.Common.Persistence;
using Application.Products.EventHandlers.Requests.CreateProduct;
using Application.Products.EventHandlers.Validators;
using Bogus.Extensions.Brazil;
using Moq;
using Newtonsoft.Json;
using Shared.Exceptions;
using Shared.Extensions;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RestApiTemplateTest.Product;

public class CreateProductRequestTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CreateProductRequestTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact(DisplayName = "CreateProductRequest_StatusCode200OK")]
    public async Task CreateProductRequest_WhenRequestFilledRight_200OK()
    {
        // Arrange
        var requestFake = new Faker<CreateProductRequest>()
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
                .GenerateLazy(5).ToList());
        
        var request = requestFake.Generate();
        var repositoryMock = new Mock<IRepository<Domain.Entities.Product>>();
        repositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new CreateProductRequestHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact(DisplayName = "CreateProductRequest_StatusCode400BadRequest")]
    public async Task CreateProductRequest_StatusCode400BadRequest()
    {
        // Arrange
        var request = new CreateProductRequest();
        var validator = new CreateProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(ErrorsMessages.Validate.ValidateProduct.DateRequired, result.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ErrorsMessages.Validate.ValidateProduct.SellerRequired, result.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ErrorsMessages.Validate.ValidateProduct.MinItemsRequired, result.Errors.Select(e => e.ErrorMessage));
    }

    [Fact(DisplayName = "CreateItemToProductRequest_StatusCode400BadRequest")]
    public async Task CreateItemToProductRequest_StatusCode400BadRequest()
    {
        // Arrange
        var requestFake = new Faker<CreateProductRequest>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Seller, _ => new Faker<CreateSellerToProductRequest>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf())
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<CreateItemToProductRequest>()
                .GenerateLazy(5).ToList());

        var request = requestFake.Generate();
        var validator = new CreateProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(ErrorsMessages.Validate.ValidateProduct.ValidateItem.NameRequired, result.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ErrorsMessages.Validate.ValidateProduct.ValidateItem.PriceRequired, result.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ErrorsMessages.Validate.ValidateProduct.ValidateItem.QuantityRequired, result.Errors.Select(e => e.ErrorMessage));
    }

    [Fact(DisplayName = "CreateSellerToProductRequest_StatusCode400BadRequest")]
    public async Task CreateSellerToProductRequest_StatusCode400BadRequest()
    {
        // Arrange
        var requestFake = new Faker<CreateProductRequest>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Seller, _ => new Faker<CreateSellerToProductRequest>())
            .RuleFor(s => s.Items, _ => new Faker<CreateItemToProductRequest>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList());

        var request = requestFake.Generate();
        var validator = new CreateProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(ErrorsMessages.Validate.ValidateSeller.NameRequired, result.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ErrorsMessages.Validate.ValidateSeller.DocumentRequired, result.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ErrorsMessages.Validate.ValidateSeller.EmailRequired, result.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(ErrorsMessages.Validate.ValidateSeller.PhoneRequired, result.Errors.Select(e => e.ErrorMessage));
    }

    [Fact(DisplayName = "CreateSellerToProductRequest_EmailInvalid_StatusCode400BadRequest")]
    public async Task CreateSellerToProductRequest_EmailInvalid_StatusCode400BadRequest()
    {
        // Arrange
        var requestFake = new Faker<CreateProductRequest>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Seller, _ => new Faker<CreateSellerToProductRequest>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Person.Cpf())
                .RuleFor(s => s.Email, _ => "email inválido")
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<CreateItemToProductRequest>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList());

        var request = requestFake.Generate();
        var validator = new CreateProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(ErrorsMessages.Validate.ValidateSeller.EmailInvalid, result.Errors.Select(e => e.ErrorMessage));
    }
    
    [Fact(DisplayName = "CreateSellerToProductRequest_EmailInvalid_StatusCode400BadRequest")]
    public async Task CreateSellerToProductRequest_DocumentInvalid_StatusCode400BadRequest()
    {
        // Arrange
        var requestFake = new Faker<CreateProductRequest>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Seller, _ => new Faker<CreateSellerToProductRequest>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, _ => "123")
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<CreateItemToProductRequest>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList());

        var request = requestFake.Generate();
        var validator = new CreateProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(ErrorsMessages.Validate.ValidateDocument.InvalidDocument, result.Errors.Select(e => e.ErrorMessage));
    }

    [Fact(DisplayName = "CreateSellerToProductRequest_EmailInvalid_StatusCode400BadRequest")]
    public async Task CreateSellerToProductRequest_ValidCnpj_StatusCode400BadRequest()
    {
        // Arrange
        var requestFake = new Faker<CreateProductRequest>()
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Seller, _ => new Faker<CreateSellerToProductRequest>()
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.Document, f => f.Company.Cnpj(false))
                .RuleFor(s => s.Email, f => f.Person.Email)
                .RuleFor(s => s.Phone, f => f.Person.Phone))
            .RuleFor(s => s.Items, _ => new Faker<CreateItemToProductRequest>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(s => s.Quantity, f => f.Random.Int(0, 10))
                .GenerateLazy(5).ToList());

        var request = requestFake.Generate();
        var validator = new CreateProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "CreateSellerToProductRequest_EmailInvalid_StatusCode400BadRequest")]
    public async Task CreateSellerToProductRequest_ValidCpf_StatusCode400BadRequest()
    {
        // Arrange
        var requestFake = new Faker<CreateProductRequest>()
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
                .GenerateLazy(5).ToList());

        var request = requestFake.Generate();
        var validator = new CreateProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        // Assert
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "CreateProductRequest_WhenIntegrationRequestFilledRight_200OK")]
    public async Task CreateProductRequest_WhenIntegrationRequestFilledRight_200OK()
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
            .Generate()
            .ToJson();

        // Act
        var response = await _client.PostAsync($"/api/sale", new StringContent(request, Encoding.UTF8, "application/json"));

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "CreateProductRequest_WhenIntegrationRequest_400BadRequest")]
    public async Task CreateProductRequest_WhenIntegrationRequest_400BadRequest()
    {
        // Arrange
        var request = new Faker<CreateProductRequest>()
            .RuleFor(s => s.Seller, _ => new Faker<CreateSellerToProductRequest>())
            .RuleFor(s => s.Items, _ => new Faker<CreateItemToProductRequest>()
                .GenerateLazy(5).ToList())
            .Generate()
            .ToJson();

        // Act
        var response = await _client.PostAsync($"/api/sale", new StringContent(request, Encoding.UTF8, "application/json"));
        var result = JsonConvert.DeserializeObject<BaseResult>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.Status);
        Assert.NotNull(result.ErrorMessages);
        Assert.NotEmpty(result.ErrorMessages);
    }
}