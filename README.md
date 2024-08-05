This is a Blog Post Application developed in ASP.NET Core MVC with postgreSQL database.
The database connection string used here is: -
Host=localhost;Database=efcore;Username=postgres;Password=api123,
Host=localhost;Database=blogappAuthDb;Username=postgres;Password=api123
Two databases are used here to maintain separation of Users and Roles Tables from Blog Tables.

The Application has 3 types of users - superadmin,admin and user. 
The superadmin can create, update, delete BlogPosts, Tags as well as Delete and Create Users. 
The admin can create users and add/edit blogposts, tags. The user can only like and comment the BlogPosts.
