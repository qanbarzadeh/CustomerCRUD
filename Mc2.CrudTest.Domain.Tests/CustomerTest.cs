using Mc2.CrudTest.Domain.Entities;

namespace Mc2.CrudTest.Domain.Tests
{

    public class CustomerTest
    {

        [Fact]
        public void CreateCustomer_ShouldSucceed()
        {
            // Arrange
            var firstname = "John";
            var lastname = "Doe";
            var dateOfBirth = new DateTime(1985, 1, 1);
            var phoneNumber = "1234567890";
            var email = "johndoe@example.com";
            var bankAccountNumber = "123-456-789";

            // Act
            var customer = new Customer(firstname, lastname, dateOfBirth, phoneNumber, email, bankAccountNumber);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(firstname, customer.Firstname);
            Assert.Equal(lastname, customer.Lastname);
            Assert.Equal(dateOfBirth, customer.DateOfBirth);
            Assert.Equal(phoneNumber, customer.PhoneNumber);
            Assert.Equal(email, customer.Email);
            Assert.Equal(bankAccountNumber, customer.BankAccountNumber);
        }


    }
}
