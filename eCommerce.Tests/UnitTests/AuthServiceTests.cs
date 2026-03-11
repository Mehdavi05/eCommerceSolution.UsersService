using AutoMapper;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.Services;
using FluentAssertions;
using Moq;

namespace eCommerce.Tests.UnitTests;

public class AuthServiceTests 
{ 
	[Fact] 
	public async Task Login_ReturnsAuthenticationResponse_WhenUserExists() 
	{ 
		// Arrange
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 

		
		var user = new ApplicationUser() 
		{ 
			UserId = Guid.NewGuid(), 
			Email = "test@test.com", 
			PersonName = "John", 
			Gender = "Male", 
			Password = "123456" 
		}; 
		
		repoMock.Setup(r => r.GetUserByEmailAndPassword("test@test.com", "123456"))
					.ReturnsAsync(user);
		
		var service = new UsersService(repoMock.Object, mapperMock.Object);
		
		var loginRequest = new LoginRequest("test@test.com", "123456"); 
		
		// Act
		var result = await service.Login(loginRequest);

		// Assert
		result.Should().NotBeNull(); 
		result!.Email.Should().Be("test@test.com"); 
		result.PersonName.Should().Be("John"); 
		result.Success.Should().BeTrue(); 
	}
	
	[Fact] 
	public async Task Login_ReturnsNull_WhenUserDoesNotExist() 
	{ 
		// Arrange
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 

		
		repoMock.Setup(r => r.GetUserByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
					.ReturnsAsync((ApplicationUser?)null);
		
		var service = new UsersService(repoMock.Object, mapperMock.Object);
		
		var loginRequest = new LoginRequest("wrong@test.com","wrongpass"); 
		
		// Act
		var result = await service.Login(loginRequest); 
		
		// Assert
		result.Should().BeNull(); 
	}
	
	[Fact] 
	public async Task Register_ReturnsAuthenticationResponse_WhenUserCreated() 
	{ 
		// Arrange
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 

		var user = new ApplicationUser() 
		{ 
			UserId = Guid.NewGuid(), 
			Email = "test@test.com", 
			PersonName = "John",
			Gender = "Male", 
			Password = "123456" 
		};
		
		
		//Repository mock
		repoMock.Setup(r => r.AddUser(It.IsAny<ApplicationUser>()))
					.ReturnsAsync(user);
		
		// Mapper mock
		mapperMock
			.Setup(m => m.Map<AuthenticationResponse>(It.IsAny<ApplicationUser>()))
			.Returns((ApplicationUser u) => new AuthenticationResponse(
				u.UserId,
				u.Email,
				u.PersonName,
				u.Gender,
				Token: "token",
				Success: true
			));
		
		var service = new UsersService(repoMock.Object, mapperMock.Object);
		
		var request = new RegisterRequest("test@test.com", "John", "123456", GenderOptions.Male); 
		
		// Act
		var result = await service.Register(request);

		// Assert
		result.Should().NotBeNull(); 
		result!.Email.Should().Be("test@test.com"); 
		result.PersonName.Should().Be("John"); 
		result.Success.Should().BeTrue(); 
	}
	
	[Fact] 
	public async Task Register_ReturnsNull_WhenRepositoryFails() 
	{ 
		// Arrange
		var repoMock = new Mock<IUsersRepository>(); 
		var mapperMock = new Mock<IMapper>(); 

		
		repoMock.Setup(r => r.AddUser(It.IsAny<ApplicationUser>()))
					.ReturnsAsync((ApplicationUser?)null); 
		
		var service = new UsersService(repoMock.Object, mapperMock.Object); 
		
		var request = new RegisterRequest("test@test.com","John","123456",GenderOptions.Male); 
		
		// Act
		var result = await service.Register(request); 
		
		// Assert
		result.Should().BeNull(); 
	} 
	
	//1️⃣ Setup() – Define Mock Behavior
	//2️⃣ Returns() / ReturnsAsync() – Return Values
	[Fact] 
	public async Task Register_SetupMoqPatternSimulation_WhenUserCreated() 
	{ 
		// Arrange
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 
		
		var user = new ApplicationUser() 
		{ 
			UserId = Guid.NewGuid(), 
			Email = "test@test.com", 
			PersonName = "John",
			Gender = "Male", 
			Password = "123456" 
		};
		
		repoMock.Setup(r => r.AddUser(It.IsAny<ApplicationUser>()))
			.ReturnsAsync(user);
		
		// Mapper mock
		mapperMock
			.Setup(m => m.Map<AuthenticationResponse>(It.IsAny<ApplicationUser>()))
			.Returns((ApplicationUser u) => new AuthenticationResponse(
				u.UserId,
				u.Email,
				u.PersonName,
				u.Gender,
				Token: "token",
				Success: true
			));
		
		var service = new UsersService(repoMock.Object, mapperMock.Object);
		
		var request = new RegisterRequest("test@test.com", "John", "123456", GenderOptions.Male); 
		
		// Act
		var result = await service.Register(request);

		// Assert
		result.Should().NotBeNull(); 
		result!.Email.Should().Be("test@test.com"); 
		result.PersonName.Should().Be("John"); 
		result.Success.Should().BeTrue(); 
	}
	
	//3️⃣ Verify() – Check That a Method Was Called
	[Fact]
	public async Task Register_AddUserShouldGetCalledOnce_WhenCreatingUser()
	{
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 
		
		var user = new ApplicationUser() 
		{ 
			UserId = Guid.NewGuid(), 
			Email = "test@test.com", 
			PersonName = "John",
			Gender = "Male", 
			Password = "123456" 
		};

		repoMock.Setup(r => r.AddUser(It.IsAny<ApplicationUser>()))
			.ReturnsAsync(user);
		
		// Mapper mock
		mapperMock
			.Setup(m => m.Map<AuthenticationResponse>(It.IsAny<ApplicationUser>()))
			.Returns((ApplicationUser u) => new AuthenticationResponse(
				u.UserId,
				u.Email,
				u.PersonName,
				u.Gender,
				Token: "token",
				Success: true
			));

		var service = new UsersService(repoMock.Object,  mapperMock.Object);

		var request = new RegisterRequest("test@test.com", "John", "123456", GenderOptions.Male); 

		var result = await service.Register(request);

		result.Should().NotBeNull();

		/*
		 Times.Once
		 Times.Never
		 Times.Exactly(3)
		 Times.AtLeastOnce
		 Times.AtMost(2)
		 */
		repoMock.Verify(r => r.AddUser(It.IsAny<ApplicationUser>()), Times.Once);
	}
	
	//4️⃣ It.Is<T>() – Match Parameters with Conditions Pattern
	// Sometimes you want to validate the parameters.
	[Fact]
	public async Task Register_MatchParametersWithConditionPattern_WhenCreatingUser()
	{
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 
		
		var user = new ApplicationUser() 
		{ 
			UserId = Guid.NewGuid(), 
			Email = "test@test.com", 
			PersonName = "John",
			Gender = "Male", 
			Password = "123456" 
		};

		//Only match if Email == admin@test.com
		repoMock.Setup(r => r.AddUser(It.Is<ApplicationUser>(u => u.Email == "admin@test.com")))
			.ReturnsAsync(user);

		var service = new UsersService(repoMock.Object, mapperMock.Object);

		var request = new RegisterRequest("admin@test.com", "John", "123456", GenderOptions.Male); 

		var result = await service.Register(request);

		result.Should().NotBeNull();

		/*
		 Times.Once
		 Times.Never
		 Times.Exactly(3)
		 Times.AtLeastOnce
		 Times.AtMost(2)
		 */
		repoMock.Verify(r => r.AddUser(
			It.Is<ApplicationUser>(u => u.Email == "admin@test.com")));
	}
	
	//5️⃣ Throws() / ThrowsAsync() – Simulate Errors
	[Fact]
	public async Task Register_ThrowExceptionPattern_WhenCreatingUser()
	{
		// Arrange
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 

		repoMock.Setup(r => r.AddUser(It.IsAny<ApplicationUser>()))
			.ThrowsAsync(new Exception("Database error"));

		var service = new UsersService(repoMock.Object, mapperMock.Object);

		var request = new RegisterRequest("admin@test.com", "John", "123456", GenderOptions.Male);

		// Act
		Func<Task> act = async () => await service.Register(request);

		// Assert
		await act.Should().ThrowAsync<Exception>()
			.WithMessage("Database error");
	}
	
	//6️⃣ Callback() – Capture Parameters
	[Fact]
	public async Task Register_CallbackPattern_WhenCreatingUser()
	{
		// Arrange
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 

		ApplicationUser? capturedUser = null;

		repoMock.Setup(r => r.AddUser(It.IsAny<ApplicationUser>()))
			.Callback<ApplicationUser>(u => capturedUser = u)
			.ReturnsAsync(new ApplicationUser());

		var service = new UsersService(repoMock.Object, mapperMock.Object);

		var request = new RegisterRequest("admin@test.com", "123456", "John", GenderOptions.Male);

		// Act
		await service.Register(request);

		// Assert
		capturedUser.Should().NotBeNull();
		capturedUser!.Email.Should().Be("admin@test.com");
		capturedUser.PersonName.Should().Be("John");
		capturedUser.Password.Should().Be("123456");
		capturedUser.Gender.Should().Be("Male");
	}
	
	//Having many of the above six patterns
	[Fact] 
	public async Task Register_ShouldCreateUser() 
	{ 
		var repoMock = new Mock<IUsersRepository>();
		var mapperMock = new Mock<IMapper>(); 
		
		ApplicationUser? capturedUser = null;
		
		repoMock.Setup(r => r.AddUser(It.IsAny<ApplicationUser>()))
        .Callback<ApplicationUser>(u => capturedUser = u)
        .ReturnsAsync(new ApplicationUser 
        { 
	        UserId = Guid.NewGuid(),
            Email = "john@test.com",
            PersonName = "John",
            Gender = "Male"
        });
		
		var service = new UsersService(repoMock.Object, mapperMock.Object);
		
		var request = new RegisterRequest(
        "john@test.com",
        "123456",
        "John",
        GenderOptions.Male);
		
		var result = await service.Register(request);
		
		result.Should().NotBeNull(); 
		result!.Email.Should().Be("john@test.com");
		
		repoMock.Verify(r => r.AddUser(It.IsAny<ApplicationUser>()), Times.Once);
		
		capturedUser!.Email.Should().Be("john@test.com"); 
	}
}