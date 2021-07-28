using System;
using System.Linq;
using System.Threading.Tasks;
using MlsaBadgeMaker.Api.Services;
using Xunit;

namespace MlsaBadgeMaker.Tests
{
    public class MlsaDirectoryServiceTests
    {
        [Fact]
        public async Task MlsaDirectoryService_GetAllMembers_Successfully()
        {
            // Arrange
            var service = new MlsaDirectoryService();

            // Act
            var members = (await service.GetAllMembersAsync()).ToList();

            // Assert
            Assert.NotEmpty(members);
            Assert.True(members.All(x => !string.IsNullOrEmpty(x.FirstName)));
            Assert.True(members.All(x => !string.IsNullOrEmpty(x.LastName)));
            Assert.True(members.All(x => !string.IsNullOrEmpty(x.LevelStatus.LevelName)));
            Assert.True(members.All(x => !string.IsNullOrEmpty(x.StudentPartnerEmail)));
            Assert.Contains(members, x => !string.IsNullOrEmpty(x.ProfilePictureUrl));
        }
    }
}
