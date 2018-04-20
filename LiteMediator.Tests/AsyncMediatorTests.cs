using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteMediator.Tests
{
    [TestClass]
    public class AsyncMediatorTests
    {
        private readonly SampleRequest _sampleRequest = new SampleRequest { Content = "Which color conveys the most personality?" };
        private readonly SampleResponse _sampleResponse = new SampleResponse { Content = "Orange" };
        private readonly SampleMessage _sampleMessage = new SampleMessage { Content = "Come read the new treatise on Life, Colors, and Code" };
        private readonly AsyncMediator _mediator = new AsyncMediator();

        [TestMethod]
        public async Task AsyncMediator_NoMessageHandlers_NoExceptions()
        {
            await _mediator.Publish(_sampleMessage);
        }

        [TestMethod]
        public async Task AsyncMediator_OneMessageHandler_HandlerInvokedOnce()
        {
            var output = new List<string>();
            _mediator.Register<SampleMessage>(x => output.Add(x.Content));

            await _mediator.Publish(_sampleMessage);

            Assert.AreEqual(1, output.Count);
            Assert.AreEqual(_sampleMessage.Content, output.First());
        }

        [TestMethod]
        public async Task AsyncMediator_MultipleMessageHandlers_AllHandlersInvoked()
        {
            var output = new List<string>();
            _mediator.Register<SampleMessage>(x => output.Add("1"));
            _mediator.Register<SampleMessage>(x => output.Add("2"));
            _mediator.Register<SampleMessage>(x => output.Add("3"));

            await _mediator.Publish(_sampleMessage);

            CollectionAssert.AreEquivalent(new[] { "1", "2", "3" }, output);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public async Task AsyncMediator_NoRequestHandler_ThrowsKeyNotFoundException()
        {
            await _mediator.GetResponse<SampleRequest, SampleResponse>(_sampleRequest);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void AsyncMediator_RegisterTwoRequestHandlers_ThrowsInvalidOperationException()
        {
            _mediator.Register<SampleRequest, SampleResponse>(x => _sampleResponse);
            _mediator.Register<SampleRequest, SampleResponse>(x => _sampleResponse);
        }

        [TestMethod]
        public async Task AsyncMediator_GetResponse_ResponseCorrect()
        {
            _mediator.Register<SampleRequest, SampleResponse>(x => _sampleResponse);

            var resp = await _mediator.GetResponse<SampleRequest, SampleResponse>(_sampleRequest);

            Assert.AreEqual(_sampleResponse.Content, resp.Content);
        }

        private class SampleMessage
        {
            public string Content { get; set; }
        }

        private class SampleRequest
        {
            public string Content { get; set; }
        }

        private class SampleResponse
        {
            public string Content { get; set; }
        }
    }
}
