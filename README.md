// README.md
# WebTextForum
### Introduction
WebTextForum is an API  with capabilities to add, retrieve and like posts.
### Project Support Features
* Users can add, retrieve and like posts.  Moderators can flag posts.
* The API uses JWT authentication, all endpoints are authenticated except retrieving posts.  
* All users can view posts, only authenticated users can post, comment like and flag posts.
### Installation Guide
* Clone this repository [here](https://github.com/PieterCoetze/WebTextForum.git).
* The master branch is the most stable branch at any given time, ensure you're working from it.
* Run dotnet restore to install all dependencies.
* The API is pointing to a SQL Server Database hosted on azure.
### Usage
* Start the application.
* Connect to the API using Postman https://localhost:7125.
* To authenticate a user, the /User/AuthenticateUser must be called.  A jwt token will be returned.  Set the token value in the GLOBALS in postman.
### API Endpoints
Run the application and use swagger https://localhost:7125/swagger/v1/swagger.json
### Technologies Used
* .NET CORE WEB API
* SLQ SERVER
### Authors
* Pieter Koortzen-Coetzee
