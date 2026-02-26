namespace PayerPortalAutomation.Infrastructure.Storage;

public class FileArtifactStorage : IArtifactStorage
{
    private readonly string _basePath = "artifacts";

    public string CreateRunFolder(string runId)
    {
        var path = Path.Combine(_basePath, runId);

        Directory.CreateDirectory(path);

        return path;
    }

    public string GetEobFolder(string runId)
    {
        var path = Path.Combine(_basePath, runId, "eobs");

        Directory.CreateDirectory(path);

        return path;
    }
}