using System;
using System.Security.Authentication;
using Xunit;
using ToggleAPI.Services;
using ToggleAPI.Models;


namespace ToggleAPI.Tests
{
    public class UserServiceTests
    {
        private IUserService _userService;

        public UserServiceTests(IUserService userService)
        {
            _userService = userService;
        }

        //Testing the Authentication Method
        [Fact]
        public void Auth_WithInvalidCredentials_AuthenticationException()
        {
           Exception ex = Assert.Throws<AuthenticationException>(() => _userService.Auth("name","wrongPassword"));

           Assert.Equal("Exception of type 'System.Exception' was thrown.", ex.Message);
        }
    }
}
