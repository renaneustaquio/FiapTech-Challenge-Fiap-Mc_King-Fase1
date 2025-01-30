using Infra.Repository.Bases;
using NHibernate;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Infra.Tests.Repository.Bases
{
    public class BaseRepositoryTestes
    {
        private readonly ISession session;
        private readonly BaseRepository<Entidade> sut;

        public BaseRepositoryTestes()
        {
            session = Substitute.For<ISession>();
            sut = new BaseRepository<Entidade>(session);
        }

        [Fact]
        public void Inserir_DeveChamarSaveNoSession()
        {
            var entidade = new Entidade();

            sut.Inserir(entidade);

            session.Received(1).Save(entidade);
        }

        [Fact]
        public void Alterar_DeveChamarUpdateNoSession()
        {
            var entidade = new Entidade();

            sut.Alterar(entidade);

            session.Received(1).Update(entidade);
        }

        [Fact]
        public void Excluir_DeveChamarDeleteNoSession()
        {
            var entidade = new Entidade();

            sut.Excluir(entidade);

            session.Received(1).Delete(entidade);
        }

        [Fact]
        public void Refresh_DeveChamarRefreshNoSession()
        {
            var entidade = new Entidade();

            sut.Refresh(entidade);

            session.Received(1).Refresh(entidade);
        }

        [Fact]
        public void RetornarPorCodigo_DeveChamarGetNoSession()
        {
            var entidade = new Entidade();

            var entidadeCodigo = 1;

            session.Get<Entidade>(entidadeCodigo).Returns(entidade);

            var result = sut.RetornarPorCodigo(entidadeCodigo);

            session.Received(1).Get<Entidade>(entidadeCodigo);

            Assert.Equal(entidade, result);
        }

        [Fact]
        public void Consultar_DeveRetornarListaDeEntidades()
        {
            var entidades = new List<Entidade>
            {
                new Entidade(),
                new Entidade()
            };

            session.Query<Entidade>().Returns(entidades.AsQueryable());

            var result = sut.Consultar();

            Assert.Equal(2, result.Count);
        }
    }

    public class Entidade
    {
        public int Codigo { get; set; }
    }
}
