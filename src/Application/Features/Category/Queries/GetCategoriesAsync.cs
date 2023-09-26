using Application.Dtos.Category;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Category.Queries
{
    public class GetCategoriesAsync
    {
        public class Query : IRequest<IEnumerable<CategoryDto>>
        {

        }

        public class Handler : IRequestHandler<Query, IEnumerable<CategoryDto>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;

            public Handler(ICategoryRepository categoryRepository, IMapper mapper)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var categories = await _categoryRepository.GetCategoriesAsync();

                var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                return categoriesDto;
            }
        }
    }
}
