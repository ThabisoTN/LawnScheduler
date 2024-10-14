Lawn Scheduler
Overview
Lawn Scheduler is a web application designed for managing bookings of lawn machines. The application allows customers to register, book machines, and view their bookings. Operators can view assigned bookings and conflict manager resolve conflicts when necessary.

Features
User Registration: Only users assigned the role of "Customer" can register.
Machine Booking: Customers can book available machines for scheduled dates.
Conflict Management: If a booking conflicts with another, it will be marked, and operators can resolve these conflicts.
Role-based Access: Different views and functionalities are available based on the user's role (Customer, Machine Operator, and Conflict Manager).

Technologies Used
Asp.net MVC
Ef Core
SQL
Identity Framework
GitHub
CSS
JavaScript
HTML
C#

You can use the following sample data to test the application:

Conflict Manager:

Email: conflictmanager@gmail.com
Role: Conflict Manager
Password: @Thabiso111


Machine Operators:
Email: operator1@gmail.com, Password: @Thabiso111
Email: operator2@gmail.com, Password: @Thabiso111
Email: operator3@gmail.com, Password: @Thabiso111
Email: operator4@gmail.com, Password: @Thabiso111
Email: operator5@gmail.com, Password: @Thabiso111
Email: operator6@gmail.com, Password: @Thabiso111

Customers: or you can regidter as a customer
Email: customer1@gmail.com, Password: @Thabiso111
Email: customer2@gmail.com, Password: @Thabiso111


How to Test
Register a Customer: Use one of the customer emails to register and log in.

Book a Machine: Navigate to the booking page and select a machine to book for a scheduled date.

Test Conflict Management: Try to book the same machine for the same date with another customer to trigger conflict handling.

Operator Actions: Log in as an operator and check the bookings assigned to them, ensuring that conflicts are not visible in their view.

Conflict Manager Actions: Log in as the conflict manager to resolve any conflicts that may arise.

Conclusion
Lawn Scheduler provides a user-friendly interface for managing lawn care equipment bookings. By following the above steps and using the provided credentials, you can test the application's core functionalities and ensure that it meets your needs.
