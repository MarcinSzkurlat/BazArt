using Application.Interfaces;
using MediatR;

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

                if (productToDelete == null) throw new Exception(); //TODO ErrorHandlingMiddleware (NotFound)

                _productRepository.DeleteProduct(productToDelete);
                var result = await _productRepository.SaveChangesAsync();

                if (result == 0) throw new Exception(); //TODO ErrorHandlingMiddleware (InternalError??)
            }
        }
    }
}
