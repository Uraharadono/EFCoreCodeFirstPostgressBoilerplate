# Entity Framework Core 3.1 - Code First - Postgress Boilerplate
Boilerplate project for quick-start of EntityFrameworkCore CodeFirst approach with  Postgress database


## Solution contents:

Solution contains 2 projects in it:
- 1st one contains only EFCore and functionalities to get quickstart with the setting up database.
- 2nd one contains contents of 1st project regarding database setup along with Unit of Work and Repository patterns.

There is big TODO regarding 2nd project, regarding me not knowing and Microsoft refusing to put resolve logic for properties contained in BaseController. So for now, you have to inject in every controller you are using Unit of Work.

### Setup:
1. Clone/download project
2. Restore (or just build) npm packages
3. Change connection string in "appsettings.json" to your creds
4. Open PM (Package Manager) Console:
- Run command "Add-Migration InitialMigration" 
- Run command "Update-Database"
5. Database should be created
6. Run one of the projects depending on your choosing and start making requests. 

** If you are new to development, and just want to see what this is all about, for methods of type POST (that post some data) you should use something like Postman to make requests
