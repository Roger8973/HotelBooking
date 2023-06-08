using Domain.Entities;
using Domain.Ports;
using Moq;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Application;

namespace ApplicationTests
{
    public  class GuestManagerTests
    {
        GuestManager _guestManager;

        [Fact]
        public async Task HappyPathCreatedGuest()
        {
            var guestRepositoryFake = new Mock<IGuestRepository>();

            guestRepositoryFake.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(333));

            _guestManager = new GuestManager(guestRepositoryFake.Object);


            var guestDto = new GuestDTO
            {
                Name = "Joao",
                Surname = "Alfredo",
                Email = "joao.alfredo@mail.com",
                IdNumber = "12334",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto,
            };

            var res = await _guestManager.CreateGuest(request);

            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.Equal(333, res.Data.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        public async Task Should_Return_InvalidPersonDocumentIdException_WhenDocsAreInvalid(string docNumber)
        {
            var guestRepositoryFake = new Mock<IGuestRepository>();

            guestRepositoryFake.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(333));

            _guestManager = new GuestManager(guestRepositoryFake.Object);


            var guestDto = new GuestDTO
            {
                Name = "Joao",
                Surname = "Alfredo",
                Email = "joao.alfredo@mail.com",
                IdNumber = docNumber,
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto,
            };

            var res = await _guestManager.CreateGuest(request);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.Equal(ErrosCodes.INVALID_PERSON_ID, res.ErrorCode);
            Assert.Equal("The ID passed is not valid.", res.Message);
        }

        [Theory]
        [InlineData("", "surnametest", "abcd@mail.com")]
        [InlineData(null, "surnametest", "abcd@mail.com")]
        [InlineData("Fulano", "", "abcd@mail.com")]
        [InlineData("Fulano", null, "abcd@mail.com")]
        [InlineData("Fulano", "surnametest", "")]
        [InlineData("Fulano", "surnametest", null)]
        public async Task Should_Return_MissingRequiredInformation_WhenDocsAreInvalid(string name, string surname, string email)
        {
            var guestRepositoryFake = new Mock<IGuestRepository>();

            guestRepositoryFake.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(333));

            _guestManager = new GuestManager(guestRepositoryFake.Object);


            var guestDto = new GuestDTO
            {
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "12345",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto,
            };

            var res = await _guestManager.CreateGuest(request);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.Equal(ErrosCodes.MISSING_REQUIRED_INFORMATION, res.ErrorCode);
            Assert.Equal("Missing required information passed.", res.Message);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        [InlineData("_@mail.com")]
        [InlineData("@mail.com")]
        [InlineData("abc@gmail.c")]
        public async Task Should_Return_InvalidEmailException_WhenDocsAreInvalid(string email)
        {
            var guestRepositoryFake = new Mock<IGuestRepository>();

            guestRepositoryFake.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(333));

            _guestManager = new GuestManager(guestRepositoryFake.Object);

            var guestDto = new GuestDTO
            {
                Name = "Joao",
                Surname = "Alfredo",
                Email = email,
                IdNumber = "12345",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto,
            };

            var res = await _guestManager.CreateGuest(request);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.Equal(ErrosCodes.INVALID_EMAIL, res.ErrorCode);
            Assert.Equal("The given email is not valid.", res.Message);
        }

        [Fact]
        public async Task HappyPathGetGuest()
        {
            var guestRepositoryFake = new Mock<IGuestRepository>();

            var guest = new Guest
            {
              Id = 333,
              Name = "teste",
              DocumentId = new Domain.ValueObjects.PersonId
              {
                  DocumentType = Domain.Enums.DocumentType.DriveLicence,
                  IdNumber = "123"
              }
            };

            guestRepositoryFake.Setup(x => x.Get(333)).Returns(Task.FromResult(guest));

            _guestManager = new GuestManager(guestRepositoryFake.Object);


            var res = await _guestManager.GetGuest(333);

            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.Equal(333, res.Data.Id);
        }

        [Fact]
        public async Task Should_Return_GuestNotFound_When_GuestDoesnotExist()
        {
            var guestRepositoryFake = new Mock<IGuestRepository>();

            guestRepositoryFake.Setup(x => x.Get(333)).Returns(Task.FromResult<Guest>(null));

            _guestManager = new GuestManager(guestRepositoryFake.Object);

            var res = await _guestManager.GetGuest(333);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.Equal(ErrosCodes.GUEST_NOT_FOUND, res.ErrorCode);
            Assert.Equal("No Guest record was found with the given Id.", res.Message);
        }
    }
}
