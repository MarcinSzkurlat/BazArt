using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Serilog;

namespace Application.Product.Command
{
    public class DeleteProductAsync
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }
            
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var productToDelete = await _productRepository.GetProductByIdAsync(request.Id);

                if (productToDelete == null) throw new NotFoundException("Product with this ID not exist");

                _productRepository.DeleteProduct(productToDelete);
                var result = await _productRepository.SaveChangesAsync();

                if (result == 0) throw new Exception();

                Log.Information($"Product ({request.Id}) deleted");
            }
        }
    }
}
