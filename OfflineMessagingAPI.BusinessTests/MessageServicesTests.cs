using System;
using Xunit;
using Moq;
using OfflineMessagingAPI.Business.Concrete;
using OfflineMessagingAPI.Business.Abstract;
using OfflineMessagingAPI.DataAccess.Abstract;
using OfflineMessagingAPI.Entities.Helpers;
using OfflineMessagingAPI.Entities.Entities;

namespace OfflineMessagingAPI.BusinessTests
{

    public class MessageServicesTests
    {
        private readonly MessageServices _messageServicesUnderTest;
        Mock<IBlockServices> _mockBlockServices = new Mock<IBlockServices>();
        Mock<IUserServices> _mockUserServices = new Mock<IUserServices>();
        Mock<IMessageRepository> _mockMessageRepository = new Mock<IMessageRepository>();

        public MessageServicesTests()
        {
            _messageServicesUnderTest = new MessageServices(_mockMessageRepository.Object, _mockBlockServices.Object, _mockUserServices.Object);
        }


        [Fact]
        public void Send_ShouldNotSendMessage_WhenTheTargetUserNotExist()
        {
            //Arrange
            var sendMessageHelper = new SendMessageHelper()
            {
                SenderId = 1,
                ReceiverUsername = "Test",
                MessageContent = "TestContent"
            };

            _mockUserServices.Setup(x => x.GetUserByUsername(sendMessageHelper.ReceiverUsername)).Returns((User)null);

            //Act
            var result = _messageServicesUnderTest.Send(sendMessageHelper);
            var actualMessage = "Böyle bir kullanýcý sistemde kayýtlý deðildir.";
            //Assert
            Assert.False(result.Success);
            Assert.Equal(actualMessage, result.Message);
        }

        [Fact]
        public void Send_ShouldNotSendMessage_WhenUserIsBlocked()
        {
            //Arrange
            var sendMessageHelper = new SendMessageHelper()
            {
                SenderId = 1,
                ReceiverUsername = "Test",
                MessageContent = "TestContent"
            };

            _mockUserServices.Setup(x => x.GetUserByUsername(sendMessageHelper.ReceiverUsername)).Returns(new User());
            _mockBlockServices.Setup(x => x.CheckIfUserBlocked(It.IsAny<int>(), sendMessageHelper.SenderId)).Returns(true);

            //Act
            var result = _messageServicesUnderTest.Send(sendMessageHelper);
            var actualMessage = "Bu kullanýcýya mesaj gönderme yetkiniz yoktur.";
            //Assert
            Assert.False(result.Success);
            Assert.Equal(actualMessage, result.Message);
        }

        [Fact]
        public void Send_ShouldNotSendMessage_WhenMessageIsNotCreated()
        {
            //Arrange
            var sendMessageHelper = new SendMessageHelper()
            {
                SenderId = 1,
                ReceiverUsername = "Test",
                MessageContent = "TestContent"
            };

            _mockUserServices.Setup(x => x.GetUserByUsername(sendMessageHelper.ReceiverUsername)).Returns(new User());
            _mockBlockServices.Setup(x => x.CheckIfUserBlocked(It.IsAny<int>(), sendMessageHelper.SenderId)).Returns(false);
            _mockUserServices.Setup(x => x.GetUserById(1)).Returns(new User());
            _mockMessageRepository.Setup(x => x.CreateMessage(It.IsAny<Message>())).Returns(false);

            //Act
            var result = _messageServicesUnderTest.Send(sendMessageHelper);
            var actualMessage = "Mesaj gönderilemedi, lütfen tekrar deneyiniz.";
            //Assert
            Assert.False(result.Success);
            Assert.Equal(actualMessage, result.Message);
        }

        [Fact]
        public void Send_ShouldSendMessage_AsExpected()
        {
            //Arrange
            var sendMessageHelper = new SendMessageHelper()
            {
                SenderId = 1,
                ReceiverUsername = "Test",
                MessageContent = "TestContent"
            };
            int possibleBlockerId = new int();

            _mockUserServices.Setup(x => x.GetUserByUsername(sendMessageHelper.ReceiverUsername)).Returns(new User());
            _mockBlockServices.Setup(x => x.CheckIfUserBlocked(possibleBlockerId, sendMessageHelper.SenderId)).Returns(false);
            _mockUserServices.Setup(x => x.GetUserById(1)).Returns(new User());
            _mockMessageRepository.Setup(x => x.CreateMessage(It.IsAny<Message>())).Returns(true);

            //Act
            var result = _messageServicesUnderTest.Send(sendMessageHelper);
            var actualMessage = "Mesajýnýz baþarýyla gönderilmiþtir.";
            //Assert
            Assert.True(result.Success);
            Assert.Equal(actualMessage, result.Message);
        }
    }
}
