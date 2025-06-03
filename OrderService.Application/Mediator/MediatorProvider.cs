
namespace OrderService.Application.Mediator
{
    public class MediatorProvider : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public MediatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(IRequestHandler<,>)
                .MakeGenericType(request.GetType(), typeof(TResponse));

            dynamic handler = _serviceProvider.GetService(handlerType)
                ?? throw new InvalidOperationException($"Handler not found for {request.GetType().Name}");

            return await handler.Handle((dynamic)request);
        }
    }
}
