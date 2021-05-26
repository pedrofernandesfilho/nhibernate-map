using ConsoleApp1.Domain;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.Data
{
    public class ProductRepositoryTests : BaseTest<Product>
    {
        [Test]
        public async Task ShouldInsertProductWithoutFields()
        {
            const string productName = "PlayStation";
            var productToPersist = new Product(productName);
            await Repository.InsertAsync(productToPersist);
            Session.Clear();

            var persistedProduct = await Repository.GetByIdAsync(productToPersist.Id);

            persistedProduct.Should().BeEquivalentTo(productToPersist);
        }

        [Test]
        public async Task ShouldInsertProductWithFields()
        {
            var fieldsOnDatabase = new[] {
                new Field("Field1", "F1"),
                new Field("Field2", "F2")
            };
            var fieldRepository = ServiceProvider.GetRequiredService<IBaseRepository<Field>>();
            await fieldRepository.InsertAsync(fieldsOnDatabase);
            Session.Clear();
            const string productName = "XBox";
            var productToPersist = new Product(productName);
            productToPersist.AddField(fieldsOnDatabase[0]);
            productToPersist.AddField(fieldsOnDatabase[1]);
            await Repository.InsertAsync(productToPersist);
            Session.Clear();

            var persistedProduct = await Repository.GetByIdAsync(productToPersist.Id);

            persistedProduct.Should().BeEquivalentTo(productToPersist);
        }
    }
}
