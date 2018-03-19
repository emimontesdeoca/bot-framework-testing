using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestBot.Dialogs;
using System.IO;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Autofac;
using Microsoft.QualityTools.Testing.Fakes;
using TestBot;
using System.Threading.Tasks;

namespace BotUnitTesting
{
    [TestClass]
    public class PruebaPreguntas
    {
        [TestMethod]
        public async Task Prueba_deberia_devolver_respuestaAsync()
        {

            using (ShimsContext.Create())
            {
                // Arrange
                var waitCalled = false;
                var message = string.Empty;
                var postAsyncCalled = false;

                var target = new RootDialog();

                var activity = new Activity(ActivityTypes.Message)
                {
                    Text = "Hello World"
                };

                var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
                {
                    IsCompletedGet = () => true,
                    GetResult = () => activity
                };

                var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
                {
                    GetAwaiter = () => awaiter
                };

                var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();

                Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.PostAsyncIBotToUserStringStringCancellationToken = (user, s1, s2, token) =>
                {
                    message = s1;
                    postAsyncCalled = true;
                    return Task.CompletedTask;
                };

                Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
                {
                    if (waitCalled) return;

                    waitCalled = true;

                    // The callback is what is being tested.
                    callback(context, awaitable);
                };

                // Act
                await target.StartAsync(context);

                // Assert
                Assert.AreEqual("You sent Hello World which was 11 characters", message, "Message is wrong");
                Assert.IsTrue(postAsyncCalled, "PostAsync was not called");
            }

        }

    }
}
