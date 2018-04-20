using System;
using System.Collections.Generic;

namespace LiteMediator
{
    public sealed class SyncMediator
    {
        private readonly Dictionary<Type, List<Action<object>>> _messageHandlers = new Dictionary<Type, List<Action<object>>>();
        private readonly Dictionary<Type, Func<object, object>> _requestHandlers = new Dictionary<Type, Func<object, object>>();

        public void Publish<TMessage>(TMessage message)
        {
            var type = typeof(TMessage);
            if (_messageHandlers.ContainsKey(type))
                foreach (var handler in _messageHandlers[type])
                    handler(message);
        }

        public TResponse GetResponse<TRequest, TResponse>(TRequest request)
        {
            var type = typeof(TRequest);
            if (!_requestHandlers.ContainsKey(type))
                throw new KeyNotFoundException($"Missing required registered handler for type {type.FullName}");
            return (TResponse)_requestHandlers[type](request);
        }

        public void Register<TMessage>(Action<TMessage> onMessage)
        {
            var type = typeof(TMessage);
            if (!_messageHandlers.ContainsKey(type))
                _messageHandlers[type] = new List<Action<object>>();
            _messageHandlers[type].Add(x => onMessage((TMessage)x));
        }

        public void Register<TRequest, TResponse>(Func<TRequest, TResponse> getResponse)
        {
            var type = typeof(TRequest);
            if (_requestHandlers.ContainsKey(type))
                throw new InvalidOperationException($"Only one handler for type {type.FullName} may be registered.");
            _requestHandlers[type] = x => getResponse((TRequest)x);
        }
    }
}
