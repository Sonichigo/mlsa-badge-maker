namespace MlsaBadgeMaker.Api.Data.InfluencerApi
{
    public class MlsaMember
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StudentPartnerEmail { get; set; }

        public MlsaLevel LevelStatus { get; set; }

        public string ProfilePictureUrl { get; set; }
    }

    public class MlsaLevel
    {
        public string LevelName { get; set; }
    }
}