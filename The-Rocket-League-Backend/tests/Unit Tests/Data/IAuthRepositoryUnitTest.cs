using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using The_Rocket_League_Backend.Data;
using The_Rocket_League_Backend.Models;
using Xunit;

namespace Tests.Unit_Tests.Data{
    public class IAuthRepositoryUnitTest{
        /* TODO: Test IAuthRepository, must mock extension methods */
        private Mock<DataContext> context = new Mock<DataContext>();


//        [Fact]
//        public async Task UserExists_ReturnsFalse_WhenUserDoesNotExist(){
//            string mockUsername = "John";
//
//            context.Setup(context => context.Users.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
//                .ReturnsAsync(false);
//            var repo = new AuthRepository(context.Object);
//
//            var result = await repo.UserExists(It.IsAny<string>());
//
//            Assert.False(result);
//        }
    }
}