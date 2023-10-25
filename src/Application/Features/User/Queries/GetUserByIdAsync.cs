using Application.Dtos.User;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.User.Queries
{
    public class GetUserByIdAsync
    {
        public class Query : IRequest<UserDetailDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, UserDetailDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public Handler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<UserDetailDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserByIdAsync(request.Id);

                if (user == null) throw new NotFoundException("User not found");

                var userDto = _mapper.Map<UserDetailDto>(user);

                return userDto;
            }
        }
    }
}
