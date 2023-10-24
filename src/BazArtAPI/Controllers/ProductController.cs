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

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
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
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCategoryAsync(
            [FromQuery]string categoryName)
        {
            var productsDto = await _mediator.Send(new GetProductsByCategoryAsync.Query { CategoryName = categoryName });

            return Ok(productsDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductAsync([FromBody]CreateProductDto productToCreate)
        {
            await _mediator.Send(new CreateProductAsync.Command { ProductToCreate = productToCreate });

            return Created($"api/product/{productToCreate.Id}", null);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> EditProductAsync([FromRoute]Guid id, [FromBody]EditProductDto productToEdit)
        {
            await _mediator.Send(new EditProductAsync.Command { Id = id, ProductToEdit = productToEdit });

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProductAsync([FromRoute]Guid id)
        {
            await _mediator.Send(new DeleteProductAsync.Command{Id = id});

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("latest")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCreatedDate()
        {
            var products = await _mediator.Send(new GetProductsByCreatedDate.Query());
            
            return Ok(products);
        }
    }
}
