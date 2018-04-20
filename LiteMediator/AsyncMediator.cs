using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiteMediator
{
    public sealed class AsyncMediator
    {
        private readonly Dictionary<Type, List<Func<object, Task>>> _messageHandlers = new Dictionary<Type, List<Func<object, Task>>>();
        private readonly Dictionary<Type, Func<object, Task<object>>> _requestHandlers = new Dictionary<Type, Func<object, Task<object>>>();

        public async Task Publish<TMessage>(TMessage message)
        {
            var type = typeof(TMessage);
            if (_messageHandlers.ContainsKey(type))
                foreach (var handler in _messageHandlers[type])
                    await handler(message);
        }

        public async Task<TResponse> GetResponse<TRequest, TResponse>(TRequest request)
        {
            var type = typeof(TRequest);
            if (!_requestHandlers.ContainsKey(type))
                throw new KeyNotFoundException($"Missing required registered handler for type {type.FullName}");
            return (TResponse)await _requestHandlers[type](request);
        }

        public void Register<TMessage>(Action<TMessage> onMessage)
        {
            Register<TMessage>(x =>
            {
                onMessage(x);
                return Task.CompletedTask;
            });
        }

        public void Register<TMessage>(Func<TMessage, Task> onMessage)
        {
            var type = typeof(TMessage);
            if (!_messageHandlers.ContainsKey(type))
                _messageHandlers[type] = new List<Func<object, Task>>();
            _messageHandlers[type].Add(async x => await onMessage((TMessage)x));
        }

        public void Register<TRequest, TResponse>(Func<TRequest, TResponse> getResponse)
        {
            Register<TRequest, TResponse>(x => Task.FromResult(getResponse(x)));
        }

        public void Register<TRequest, TResponse>(Func<TRequest, Task<TResponse>> getResponse)
        {
            var type = typeof(TRequest);
            if (_requestHandlers.ContainsKey(type))
                throw new InvalidOperationException($"Only one handler for type {type.FullName} may be registered.");
            _requestHandlers[type] = (async x => await getResponse((TRequest)x));
        }
    }
}
