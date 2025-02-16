namespace AccreditSolutions.Models.Factories
{
    public interface IFactory<T> 
    {
        T Create();
    }
}