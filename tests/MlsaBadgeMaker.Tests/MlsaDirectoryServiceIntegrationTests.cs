using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MlsaBadgeMaker.Api.Data.InfluencerApi;
using MlsaBadgeMaker.Api.Services;
using Moq;
using Xunit;

namespace MlsaBadgeMaker.Tests
{
    public class MlsaDirectoryServiceIntegrationTests
    {
        [Fact]
        public async Task MlsaDirectoryService_GetAllMembers_Successfully()
        {
            // Arrange
            var service = new MlsaDirectoryService(new HttpClient());

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
