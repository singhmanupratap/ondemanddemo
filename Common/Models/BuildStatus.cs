namespace Common.Models
{
    public enum BuildStatus
    {
        BuildQueued = 1,
        BuildCompleted = 2,
        DeploymentQueued = 3,
        DeploymentCompleted = 4,
        Error = 0,
        InProcess = 5
    }
}