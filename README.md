# Medical Clinic Project Documentation

## Overview
The Medical Clinic project is a web application developed to manage patient records for a medical clinic. It provides functionalities for creating, updating, and removing patient records, as well as viewing patient details.

### Project Structure
The project follows the MVC (Model-View-Controller) architectural pattern and is structured as follows:


#### Models
- **Patient:** Represents a patient record with properties such as Id, Firstname, Lastname, PersonalNumber, Phone, Email, and Address.
- **Address:** Represents the address of a patient with properties such as Id, Street, City, and Zip.


#### Controllers
- **PatientController:** Handles HTTP requests related to patient management including actions for listing patients, viewing patient details, creating new patients, updating patient information, and removing patients.

#### Views
- **Patient:** Contains Razor views for rendering patient-related pages including index (list of patients), detail (patient details with possibility to remove or update), create (form for creating new patients).

#### Data
- **IPatientRepository:** Defines the interface for interacting with patient data, including methods for getting all patients, getting a patient by id, adding a new patient, updating an existing patient, and deleting a patient.
- **PatientRepository:** Implements the IPatientRepository interface using Entity Framework Core to interact with the database.

### Features
- **Listing Patients:** The Index action in the PatientController lists all patients with options for **searching**, **sorting**, and **pagination**.
- **Viewing Patient Details:** The Detail action in the PatientController displays detailed information about a specific patient.
- **Creating Patients:** The Create action in the PatientController provides a form for creating new patient records.
- **Updating Patients:** The Update action in the PatientController allows users to modify existing patient information.
- **Removing Patients:** The Remove action in the PatientController enables users to delete patient records from the system.

### Testing
Unit tests have been implemented to ensure the functionality and reliability of the PatientController actions. These tests cover scenarios such as listing patients, viewing patient details, creating new patients, updating patient information, and removing patients.

## Technology Stack

### ASP.NET Core MVC

ASP.NET Core MVC was selected as the primary framework for building the Medical Clinic web application. MVC (Model-View-Controller) architecture is well-suited for this project as it provides a structured approach to organizing code, separating concerns, and handling HTTP requests.

### PostgreSQL

PostgreSQL is an advanced open-source relational database management system known for its reliability, scalability, and support for SQL queries.

### Entity Framework Core

Entity Framework Core is an Object-Relational Mapping (ORM) framework that simplifies data access by enabling developers to work with databases using .NET objects. It ensures consistency between the object model and the database schema, reducing the amount of boilerplate code required.

### xUnit

xUnit is a popular unit testing framework for .NET applications. It provides a simple and intuitive syntax for writing and executing unit tests, helping ensure the stability and reliability of the codebase.

## Setting up and Running the Application

1. **Clone the Repository:**
   Clone the Medical Clinic repository from the source control.

2. **Install Dependencies:**
   Navigate to the project directory and restore the dependencies by running the following command:
`dotnet restore`

1. **Database Configuration:**
Configure the database connection string in the `appsettings.json` file located in the project directory. Ensure that the database server is running and accessible.

1. **Apply Migrations:**
Apply any pending migrations to the database by running the following command:
`dotnet ef database update`

1. **Run the Application:**
Start the application by running the following command:
`dotnet run`

This command will build the application and start a web server. You can access the application by navigating to the specified URL in your web browser.

1. **Access the Application:**
Once the application is running, you can access it through your web browser by navigating to the URL specified during the startup process.

1. **Interact with the Application:**
You can now interact with the Medical Clinic application by performing actions such as listing patients, viewing patient details, creating new patients, updating patient information, and removing patients.

1. **Run Unit Tests:**
To run the unit tests for the application, execute the following command:
`dotnet test`

This command will run all the unit tests in the project and display the test results.

By following these steps, you should be able to set up and run the application successfully.