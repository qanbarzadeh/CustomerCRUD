# Customer CRUD Application

This is a Customer CRUD (Create, Read, Update, Delete) application built using Clean Architecture and following the TDD (Test-Driven Development) approach. It also uses CQRS (Command Query Responsibility Segregation) pattern and validation for bank account number.

## Getting Started

To get started with the project, clone the repository and open the solution file in Visual Studio 2019 or later. 

### Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2019 or later

### Installing

1. Clone the repository
2. Open the solution file in Visual Studio
3. Build the solution to restore NuGet packages

## Running the Tests

The application comes with a test project, `Mc2.CrudTest.Application.Tests`, that contains unit tests for the business logic. To run the tests, simply run the test project.

## Example Bank Account Number

The bank account number format used for validation is International Bank Account Number (IBAN) format .Here are some examples to use in Swagger fo testing :
 - GB29NWBK60161331926819
 - GB29NWBK60161331926819
 - FR1420041010050500013M02606
 - DE89370400440532013000


Here's an example JSON object that can be used as a request for Swagger:

```json
{  
  "firstname": "Ali",
  "lastname": "Qanbarzadeh",
  "dateOfBirth": "2023-04-30T17:54:39.708Z",
  "phoneNumber": "+60173771596",
  "email": "ghxalireza@gmail.com",
  "bankAccountNumber": "DE89370400440532013000"
}
