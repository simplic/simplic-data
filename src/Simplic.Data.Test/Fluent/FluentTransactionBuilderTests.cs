using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Simplic.Data.Test
{
    public class FluentTransactionBuilderTests
    {
        private readonly IFluentTransactionBuilder fluentTransactionBuilder;
        private readonly Mock<ITransaction> transactionMock = new Mock<ITransaction>();
        private readonly Mock<ITransactionService> transactionServiceMock = new Mock<ITransactionService>();
        private readonly Mock<ITransactionRepository<object, int>> objectTransactionServiceMock = new Mock<ITransactionRepository<object, int>>();

        public FluentTransactionBuilderTests()
        {
            transactionServiceMock.Setup(s => s.CreateAsync()).ReturnsAsync(() => transactionMock.Object);

            fluentTransactionBuilder = new FluentTransactionBuilder(transactionServiceMock.Object);
        }

        [Fact]
        public async Task ShouldCreateAndCommitTransaction_CreateObject()
        {
            var testObject = new { Name = "Test", Id = 11 };

            await fluentTransactionBuilder
                .AddService<ITransactionRepository<object, int>, object, int>(objectTransactionServiceMock.Object)
                .Create<ITransactionRepository<object, int>, object, int>(service => testObject)
                .CommitAsync();

            transactionServiceMock.Verify(s => s.CreateAsync(), Times.Once);
            transactionServiceMock.Verify(s => s.CommitAsync(It.IsAny<ITransaction>()), Times.Once);
            transactionServiceMock.Verify(s => s.AbortAsync(It.IsAny<ITransaction>()), Times.Never);

            objectTransactionServiceMock.Verify(s => s.CreateAsync(testObject, It.IsAny<ITransaction>()), Times.Once);
            objectTransactionServiceMock.Verify(s => s.UpdateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.DeleteAsync(testObject.Id, It.IsAny<ITransaction>()), Times.Never);
        }

        [Fact]
        public async Task ShouldCreateAndAbortTransaction_CreateObject()
        {
            var testObject = new { Name = "Test", Id = 12 };

            await fluentTransactionBuilder
                .AddService<ITransactionRepository<object, int>, object, int>(objectTransactionServiceMock.Object)
                .Create<ITransactionRepository<object, int>, object, int>(service => testObject)
                .AbortAsync();

            transactionServiceMock.Verify(s => s.CreateAsync(), Times.Once);
            transactionServiceMock.Verify(s => s.CommitAsync(It.IsAny<ITransaction>()), Times.Never);
            transactionServiceMock.Verify(s => s.AbortAsync(It.IsAny<ITransaction>()), Times.Once);

            objectTransactionServiceMock.Verify(s => s.CreateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.UpdateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.DeleteAsync(testObject.Id, It.IsAny<ITransaction>()), Times.Never);
        }

        [Fact]
        public async Task ShouldCreateAndCommitTransaction_UpdateObject()
        {
            var testObject = new { Name = "Test", Id = 13 };

            await fluentTransactionBuilder
                .AddService<ITransactionRepository<object, int>, object, int>(objectTransactionServiceMock.Object)
                .Update<ITransactionRepository<object, int>, object, int>(service => testObject)
                .CommitAsync();

            transactionServiceMock.Verify(s => s.CreateAsync(), Times.Once);
            transactionServiceMock.Verify(s => s.CommitAsync(It.IsAny<ITransaction>()), Times.Once);
            transactionServiceMock.Verify(s => s.AbortAsync(It.IsAny<ITransaction>()), Times.Never);

            objectTransactionServiceMock.Verify(s => s.CreateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.UpdateAsync(testObject, It.IsAny<ITransaction>()), Times.Once);
            objectTransactionServiceMock.Verify(s => s.DeleteAsync(testObject.Id, It.IsAny<ITransaction>()), Times.Never);
        }

        [Fact]
        public async Task ShouldCreateAndAbortTransaction_UpdateObject()
        {
            var testObject = new { Name = "Test", Id = 14 };

            await fluentTransactionBuilder
                .AddService<ITransactionRepository<object, int>, object, int>(objectTransactionServiceMock.Object)
                .Update<ITransactionRepository<object, int>, object, int>(service => testObject)
                .AbortAsync();

            transactionServiceMock.Verify(s => s.CreateAsync(), Times.Once);
            transactionServiceMock.Verify(s => s.CommitAsync(It.IsAny<ITransaction>()), Times.Never);
            transactionServiceMock.Verify(s => s.AbortAsync(It.IsAny<ITransaction>()), Times.Once);

            objectTransactionServiceMock.Verify(s => s.CreateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.UpdateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.DeleteAsync(testObject.Id, It.IsAny<ITransaction>()), Times.Never);
        }

        [Fact]
        public async Task ShouldCreateAndCommitTransaction_DeleteObject()
        {
            var testObject = new { Name = "Test", Id = 15 };

            await fluentTransactionBuilder
                .AddService<ITransactionRepository<object, int>, object, int>(objectTransactionServiceMock.Object)
                .Delete<ITransactionRepository<object, int>, object, int>(service => testObject.Id)
                .CommitAsync();

            transactionServiceMock.Verify(s => s.CreateAsync(), Times.Once);
            transactionServiceMock.Verify(s => s.CommitAsync(It.IsAny<ITransaction>()), Times.Once);
            transactionServiceMock.Verify(s => s.AbortAsync(It.IsAny<ITransaction>()), Times.Never);

            objectTransactionServiceMock.Verify(s => s.CreateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.UpdateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.DeleteAsync(testObject.Id, It.IsAny<ITransaction>()), Times.Once);
        }

        [Fact]
        public async Task ShouldCreateAndAbortTransaction_DeleteObject()
        {
            var testObject = new { Name = "Test", Id = 16 };

            await fluentTransactionBuilder
                .AddService<ITransactionRepository<object, int>, object, int>(objectTransactionServiceMock.Object)
                .Delete<ITransactionRepository<object, int>, object, int>(service => testObject.Id)
                .AbortAsync();

            transactionServiceMock.Verify(s => s.CreateAsync(), Times.Once);
            transactionServiceMock.Verify(s => s.CommitAsync(It.IsAny<ITransaction>()), Times.Never);
            transactionServiceMock.Verify(s => s.AbortAsync(It.IsAny<ITransaction>()), Times.Once);

            objectTransactionServiceMock.Verify(s => s.CreateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.UpdateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.DeleteAsync(testObject.Id, It.IsAny<ITransaction>()), Times.Never);
        }

        [Fact]
        public async Task ShouldCreateAndCommitTransaction_MultipleOpetrations()
        {
            var testObject = new { Name = "Test", Id = 17 };

            await fluentTransactionBuilder
                .AddService<ITransactionRepository<object, int>, object, int>(objectTransactionServiceMock.Object)
                .Create<ITransactionRepository<object, int>, object, int>(service => testObject)
                .Update<ITransactionRepository<object, int>, object, int>(service => testObject)
                .Delete<ITransactionRepository<object, int>, object, int>(service => testObject.Id)
                .CommitAsync();

            transactionServiceMock.Verify(s => s.CreateAsync(), Times.Once);
            transactionServiceMock.Verify(s => s.CommitAsync(It.IsAny<ITransaction>()), Times.Once);
            transactionServiceMock.Verify(s => s.AbortAsync(It.IsAny<ITransaction>()), Times.Never);

            objectTransactionServiceMock.Verify(s => s.CreateAsync(testObject, It.IsAny<ITransaction>()), Times.Once);
            objectTransactionServiceMock.Verify(s => s.UpdateAsync(testObject, It.IsAny<ITransaction>()), Times.Once);
            objectTransactionServiceMock.Verify(s => s.DeleteAsync(testObject.Id, It.IsAny<ITransaction>()), Times.Once);
        }

        [Fact]
        public async Task ShouldCreateAndAbortTransaction_MultipleOpetrations()
        {
            var testObject = new { Name = "Test", Id = 18 };

            await fluentTransactionBuilder
                .AddService<ITransactionRepository<object, int>, object, int>(objectTransactionServiceMock.Object)
                .Create<ITransactionRepository<object, int>, object, int>(service => testObject)
                .Update<ITransactionRepository<object, int>, object, int>(service => testObject)
                .Delete<ITransactionRepository<object, int>, object, int>(service => testObject.Id)
                .AbortAsync();

            transactionServiceMock.Verify(s => s.CreateAsync(), Times.Once);
            transactionServiceMock.Verify(s => s.CommitAsync(It.IsAny<ITransaction>()), Times.Never);
            transactionServiceMock.Verify(s => s.AbortAsync(It.IsAny<ITransaction>()), Times.Once);

            objectTransactionServiceMock.Verify(s => s.CreateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.UpdateAsync(testObject, It.IsAny<ITransaction>()), Times.Never);
            objectTransactionServiceMock.Verify(s => s.DeleteAsync(testObject.Id, It.IsAny<ITransaction>()), Times.Never);
        }
    }
}
