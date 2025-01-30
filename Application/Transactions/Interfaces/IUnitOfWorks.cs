namespace Application.Transactions.Interfaces
{
    public interface IUnitOfWorks
    {
        void Begintransaction();
        void RollBack();
        void Commit();
    }
}
