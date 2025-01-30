using Application.Transactions.Interfaces;
using NHibernate;

namespace Infra.Transactions
{
    public class UnitOfWorks(ISession session) : IUnitOfWorks
    {
        private ITransaction transaction;

        public void Begintransaction()
        {
            transaction = session.BeginTransaction();
        }

        public void Commit()
        {
            if (transaction.IsActive)
                transaction.Commit();
        }

        public void RollBack()
        {
            if (transaction.IsActive)
                transaction.Rollback();
        }
    }
}
