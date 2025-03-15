Feature: ApiTesting

Scenario: Validate API GET Response
    Given I have a valid API endpoint
    When I send a GET request to /posts/1
    Then the response status code should be 200

Scenario: Validate API POST Request
    Given I have a valid API endpoint
    When I send a POST request to /posts with data
        | title | body | userId |
        | foo   | bar  | 1      |
    Then the response status code should be 201
    And the response data should match the sent data

Scenario: Validate API PUT Request
    Given I have a valid API endpoint
    When I send a PUT request to /posts/1 with data
        | title | body         | userId |
        | Updated Post | Updated Body | 1      |
    Then the response status code should be 200
    And the response data should match the sent data

Scenario: Validate API DELETE Request
    Given I have a valid API endpoint
    When I send a DELETE request to /posts/1
    Then the response status code should be 200