using Application.Repository.Bases;
using NHibernate;

namespace Infra.Repository.Bases
{
    public class BaseRepository<T>(ISession session) : IBaseRepository<T> where T : class
    {
        public void Alterar(T entidade)
        {
            session.Update(entidade);
        }

        public void Excluir(T entidade)
        {
            session.Delete(entidade);
        }

        public void Inserir(T entidade)
        {
            session.Save(entidade);
        }

        public void Refresh(T entidade)
        {
            session.Refresh(entidade);
        }

        public IList<T> Consultar()
        {
            return [.. (from c in session.Query<T>() select c)];
        }

        public T RetornarPorCodigo(int Codigo)
        {
            return session.Get<T>(Codigo);
        }

        public IQueryable<T> Recuperar()
        {
            return session.Query<T>();
        }
    }
}
