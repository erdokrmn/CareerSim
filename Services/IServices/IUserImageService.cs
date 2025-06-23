namespace CareerSim.Services.IServices
{
    public interface IUserImageService
    {
        Task<string> GetProfileImageUrlAsync();
    }

}
