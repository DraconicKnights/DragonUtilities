namespace DragonUtilities.Interfaces;

public interface IDatabaseTransaction : IDisposable
{
    void Commit();
    void Rollback();
}