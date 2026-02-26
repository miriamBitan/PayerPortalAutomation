namespace PayerPortalAutomation.Infrastructure.Storage;

public interface IArtifactStorage
{
    string CreateRunFolder(string runId);

    string GetEobFolder(string runId);
}