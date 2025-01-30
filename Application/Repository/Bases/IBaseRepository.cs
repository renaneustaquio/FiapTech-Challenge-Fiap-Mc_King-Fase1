namespace Application.Repository.Bases
{
    public interface IBaseRepository<T>
    {
        void Inserir(T entidade);
        void Alterar(T entidade);
        void Excluir(T entidade);
        void Refresh(T entidade);
        T RetornarPorCodigo(int Codigo);
        IList<T> Consultar();
        IQueryable<T> Recuperar();


    }
}
