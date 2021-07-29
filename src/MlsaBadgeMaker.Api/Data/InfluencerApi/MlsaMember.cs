using System.ComponentModel.DataAnnotations;

namespace MlsaBadgeMaker.Api.Data.InfluencerApi
{
    public class MlsaMember
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StudentPartnerEmail { get; set; }

        public MlsaLevel LevelStatus { get; set; }

        public string ProfilePictureUrl { get; set; }
    }
}