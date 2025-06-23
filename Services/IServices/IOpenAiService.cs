namespace CareerSim.Services.IServices
{
    public interface IOpenAiService
    {
        Task<string> EvaluateAsync(string prompt);
    }
}
