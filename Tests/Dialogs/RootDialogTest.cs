using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using TestBot.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace Blog.Tests.Dialogs
{
    [TestClass]
    public class RootDialogTests
    {
        [TestMethod]
        public async Task Bot_Test_Generico()
        {
            foreach (var item in RootDialog.data)
            {
                var preg = item.Key;
                var resp = item.Value;

                using (ShimsContext.Create())
                {
                    // Arrange
                    var waitCalled = false;
                    var message = string.Empty;

                    var target = new RootDialog();

                    var activity = new Activity(ActivityTypes.Message)
                    {
                        Text = preg as string
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
                    Assert.AreEqual(resp, message);
                }
            }
        }

        //[TestMethod]
        //public async Task Bot_Test_Attachments()
        //{
        //    foreach (var item in RootDialog.dataAttachments)
        //    {
        //        var preg = item.Key;
        //        var att = item.Value as Attachment;

        //        using (ShimsContext.Create())
        //        {
        //            // Arrange
        //            var waitCalled = false;
        //            var message = string.Empty;

        //            var target = new RootDialog();

        //            var activity = new Activity(ActivityTypes.Message)
        //            {
        //                Text = preg
        //            };

        //            var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
        //            {
        //                IsCompletedGet = () => true,
        //                GetResult = () => activity
        //            };

        //            var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
        //            {
        //                GetAwaiter = () => awaiter
        //            };

        //            var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();

        //            Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.PostAsyncIBotToUserStringStringCancellationToken = (user, s1, s2, token) =>
        //            {
        //                message = s1;
        //                return Task.CompletedTask;
        //            };

        //            Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
        //            {
        //                if (waitCalled) return;

        //                waitCalled = true;

        //                // The callback is what is being tested.
        //                callback(context, awaitable);
        //            };

        //            // Act
        //            await target.StartAsync(context);

        //            // Assert
        //            Assert.AreEqual(resp, message);
        //        }
        //    }
        //}
    }
}
