using WorkFlow.Domain.Enuns;
using WorkFlow.Domain.Exceptions;
using WorkFlow.Domain.Models;

namespace WorkFlow.UnitTests.Domain.Models
{
    public class RequestModelTests
    {
        private const string ValidTitle = "Solicitação de Software";
        private const string ValidDescription = "Licença para IDE.";
        private const string RequesterId = "user-123";
        private const string ManagerId = "manager-456";

        [Fact]
        public void CriarSolicitacao_ComDadosValidos()
        {
            //Act
            var request = RequestModel.CreateRequest(ValidTitle, ValidDescription, RequestCategoryEnum.IT, RequestPriorityEnum.High, RequesterId);

            //Assert
            Assert.NotNull(request);
            Assert.NotEqual(Guid.Empty, request.Id);
            Assert.Equal(RequestStatusEnum.Pending, request.Status);
            Assert.Equal(ValidTitle, request.Title);

            // Verifica histórico: deve ter 1 item
            Assert.Single(request.History);
            Assert.Equal(RequestStatusEnum.Pending, request.History.First().ToStatus);
        }

        [Fact]
        public void Aprovar_QuandoSolicitacaoEstaPendente_DeveAlterarStatusParaAprovado()
        {
            // Arrange
            var request = RequestModel.CreateRequest(ValidTitle, ValidDescription, RequestCategoryEnum.IT, RequestPriorityEnum.High, RequesterId);
            string comment = "Aprovado.";

            // Act
            request.Approve(ManagerId, comment);

            // Assert
            Assert.Equal(RequestStatusEnum.Approved, request.Status);
            Assert.NotNull(request.UpdatedAt);

            Assert.Equal(2, request.History.Count);

            var lastEntry = request.History.Last();
            Assert.Equal(RequestStatusEnum.Approved, lastEntry.ToStatus);
            Assert.Equal(ManagerId, lastEntry.ChangedBy);
            Assert.Equal(comment, lastEntry.Comment);
        }

        [Fact]
        public void Aprovar_QuandoSolicitacaoNaoEstaPendente_DeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var request = RequestModel.CreateRequest(ValidTitle, ValidDescription, RequestCategoryEnum.IT, RequestPriorityEnum.High, RequesterId);
            request.Approve(ManagerId, "Ok");

            // Act & Assert
            var exception = Assert.Throws<BusinessException>(() =>
                request.Approve(ManagerId, "Tentativa Duplicada")
            );

            Assert.Equal("Only pending requests can be approved.", exception.Message);
        }
    }
}