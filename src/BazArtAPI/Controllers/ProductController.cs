﻿using Application.Dtos;
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
            [FromQuery]string categoryName, [FromQuery]int pageNumber = 1)
        {
            int.TryParse(_config["PageSize"], out int pageSize);

            var results = await _mediator.Send(new GetProductsByCategoryAsync.Query
            {
                CategoryName = categoryName,
                PageNumber = pageNumber,
                PageSize = pageSize
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

        [HttpPost("{id:guid}/photo")]
        public async Task<IActionResult> AddPhotoAsync([FromRoute]Guid id, [FromForm] IFormFile file)
        {
            int.TryParse(_config["Images:Settings:Image:Height"], out int photoHeight);
            int.TryParse(_config["Images:Settings:Image:Width"], out int photoWidth);

            await _mediator.Send(new AddProductPhotoAsync.Command
            {
                ProductId = id,
                File = file,
                PhotoHeight = photoHeight,
                PhotoWidth = photoWidth
            });

            return Ok();
        }

        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhotoAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteProductPhotoAsync.Command{ProductId = id});

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
