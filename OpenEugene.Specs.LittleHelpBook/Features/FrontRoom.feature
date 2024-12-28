Feature: Front Room Specs Scenarios and Behaviors
Access Rights and Information Management

Scenario: As a front room staff member, I need to update information in the system.
Given I am logged into the system,
When I navigate to the resource listing,
Then I should be able to comment on the listing.

Scenario: As an administrator, I need to edit information in the system.
Given I am logged into the system as an administrator,
When I navigate to the resource listing,
Then I should be able to edit the listing.

Scenario: As an administrator, I need to be notified of comments on resource listings.
Given a front room staff member comments on a resource listing,
When the comment is submitted,
Then I should receive an email notification.

Scenario: As a front room staff member, I need to provide clients with resource information.
Given I am assisting a client,
When I search for resources in the system,
Then I should be able to compile and print a personalized list of resources for the client.

Scenario: As a front room staff member, I need to report inaccuracies in resource information.
Given I find inaccurate information in the resource listing,
When I comment on the listing,
Then the administrator should be notified to review and correct the information.

Scenario: As a project team member, I need to understand my role and responsibilities.
Given I am part of the project team,
When I review the project plan,
Then I should see clear roles and responsibilities assigned to each team member.

Scenario: As a project team member, I need to communicate effectively with other team members.

Given I am working on the project,
When I need to discuss project details,
Then I should be able to collaborate and communicate with other team members effectively.
