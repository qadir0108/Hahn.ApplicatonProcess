namespace Hahn.ApplicatonProcess.July2021.Domain.Interfaces
{
    public interface ICache 
    {
        T Get<T>(string key) where T : class, new();
    }
}