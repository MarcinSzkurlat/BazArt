using Application.Dtos.Category;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Category.Queries
{
    public class GetCategoryByNameAsync
    {
        public class Query : IRequest<CategoryDto>
        {
            public string Name { get; set; }
        }
    }

    public class Handler : IRequestHandler<GetCategoryByNameAsync.Query, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public Handler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryByNameAsync.Query request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(request.Name);

            if (category == null) throw new NotFoundException("Category not found");

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return categoryDto;
        }
    }
}
