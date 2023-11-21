using Application.Dtos;
using Application.Dtos.Product;
using Application.Features.Product.Command;
using Application.Features.Product.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BazArtAPI.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public ProductController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config = config;
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDetailDto>> GetProductByIdAsync([FromRoute]Guid id)
        {
            var productDto = await _mediator.Send(new GetProductByIdAsync.Query { Id = id });

            return Ok(productDto);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginatedItems<IEnumerable<ProductDto>>>> GetProductsByCategoryAsync(
            [FromQuery]string categoryName, [FromQuery]int pageNumber)
        {
            var results = await _mediator.Send(new GetProductsByCategoryAsync.Query
            {
                CategoryName = categoryName,
                PageNumber = pageNumber,
                PageSize = int.Parse(_config["PageSize"]!)
            });

            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDetailDto>> CreateProductAsync([FromBody]CreateProductDto productToCreate)
        {
            var productDto = await _mediator.Send(new CreateProductAsync.Command { ProductToCreate = productToCreate });

            return Created($"api/product/{productToCreate.Id}", productDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductDetailDto>> EditProductAsync([FromRoute]Guid id, [FromBody]EditProductDto productToEdit)
        {
            var productDto = await _mediator.Send(new EditProductAsync.Command { Id = id, ProductToEdit = productToEdit });

            return Ok(productDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProductAsync([FromRoute]Guid id)
        {
            await _mediator.Send(new DeleteProductAsync.Command{Id = id});

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCreatedDate()
        {
            var products = await _mediator.Send(new GetProductsByCreatedDate.Query());
            
            return Ok(products);
        }
    }
}
