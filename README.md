
**Cafe Employee Management API**

This project is a RESTful API for managing cafes and employees using .NET 6.x and MySQL. It includes endpoints for CRUD operations on cafes and employees.

**Technologies Used**:

.NET 6.x
MySQL
Entity Framework Core
Swagger for API documentation
Installation

**Clone the repository:**

git clone <repository-url>
cd CafeEmployeeManagement


**Restore the NuGet packages:**

dotnet restore

**Features**

- Cafe Management: Add and view cafes.
- Employee Management: Manage employees tied to specific cafes.
- Database Migrations: Automatic migration and seeding.
- Swagger: Interactive API documentation.


**Configuration:**
Database Connection: Update the appsettings.json file with your MySQL connection string:

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=cafeemployee;user=sds;password=Qaz1234*"
  }
}



**Back end Database Setup [MySql] :**

-- Create the database
CREATE DATABASE cafeemployee;

-- Use the created database
USE cafeemployee;

-- Create the Cafe table
CREATE TABLE Cafes (
    id BINARY(16) NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    logo VARCHAR(255),
    location VARCHAR(100) NOT NULL
);

-- Create the Employees table
CREATE TABLE Employees (
    id VARCHAR(10) NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email_address VARCHAR(100) NOT NULL,
    phone_number VARCHAR(10) NOT NULL,
    gender ENUM('Male', 'Female') NOT NULL,
    cafe_id BINARY(16),
    start_date DATE,
    FOREIGN KEY (cafe_id) REFERENCES Cafes(id) ON DELETE CASCADE,
    UNIQUE (email_address),
    CHECK (phone_number REGEXP '^[89][0-9]{7}$'),  -- Phone number starts with 8 or 9 and has 8 digits
    CHECK (id REGEXP '^UI[0-9A-Z]{7}$')          -- Employee id starts with 'UI' followed by 7 alphanumeric characters
);


--  Create a sample cafe for seeding data 

INSERT INTO Cafes (id, name, description, logo, location) 
VALUES (UUID_TO_BIN(UUID()), 'Cafe A', 'A cozy place to relax', NULL, 'Downtown');

--  Create a sample employee for seeding data


INSERT INTO Employees (id, name, email_address, phone_number, gender, cafe_id, start_date) 
VALUES ('UI1234567', 'John Doe', 'john@example.com', '91234567', 'Male', (SELECT id FROM Cafes LIMIT 1), CURDATE());

**Usage :**
Run the Application:
dotnet run

**Access Swagger UI:**

Open your browser and navigate to https://localhost:7074/   or  

https://localhost:7074/swagger/  

to access the Swagger UI for testing the API.



**Endpoints**
Cafes

GET /cafes?location=<location>: Get a list of cafes, filtered by location.

POST /cafe: Create a new cafe.

PUT /cafe: Update an existing cafe.

DELETE /cafe: Delete a cafe.

Employees
GET /employees?cafe=<café>: Get a list of employees, filtered by cafe.

POST /employee: Create a new employee.

PUT /employee: Update an existing employee.

DELETE /employee: Delete an employee.

**Seeding the Database:**

Upon running the application for the first time, the database will be seeded with sample cafes and employees if they do not already exist. Modification can be done for seed data in the SeedDatabase method in Program.cs.
