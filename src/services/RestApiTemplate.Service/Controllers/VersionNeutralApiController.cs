using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiVersionNeutral]
public class VersionNeutralApiController : BaseApiController
{
}