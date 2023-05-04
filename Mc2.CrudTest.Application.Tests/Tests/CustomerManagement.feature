Feature: Customer Management

  As a user
  I want to be able to create customers with valid information
  So that I can manage my customer records

  Scenario: Create a customer with valid information
    Given I have entered valid customer information
    When I request to create a new customer
    Then the new customer should be created and returned
