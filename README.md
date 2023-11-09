<p align="center">
   <img src="https://github.com/MarcinSzkurlat/BazArt/assets/94744112/8af2975e-2fea-47ef-ab02-f63cdce37b59" alt="BazArt_logo_Theme_Light"/>
</p>

# About project
BazArt is an app that combines the concept of a bazaar with art. In this app, artists will be able to showcase and sell their work, create events, and more.

The application will be developed using C# and ASP.NET Core, employing Clean Architecture and CQRS with MediatR. Microsoft SQL Server will be used as the database, while the frontend will be developed in React with TypeScript.

## Web app features
### Done
- Logged users can perform CRUD operations on its own products and events
- App have two different roles - Admin and User
- Integrated database
- Seeder with fake data
- Login and registration system with JWT
- Dockerized app

### In progress
- User favorite products and items
- User cart
- Searching products, events and users
- Picture upload for products, events and users avatars

## Used technologies

| Backend | Frontend | Version control | IDE / Tools |
| ------------- | ------------- | ------------- | ------------- |
| ![C#](https://img.shields.io/badge/C%20Sharp-239120.svg?style=for-the-badge&logo=C-Sharp&logoColor=white) | ![React](https://img.shields.io/badge/React-61DAFB.svg?style=for-the-badge&logo=React&logoColor=black) | ![Git](https://img.shields.io/badge/Git-F05032.svg?style=for-the-badge&logo=Git&logoColor=white) | ![VS](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=Visual-Studio&logoColor=white) |
| ![.Net](https://img.shields.io/badge/.NET-512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white) | ![TypeScript](https://img.shields.io/badge/TypeScript-3178C6.svg?style=for-the-badge&logo=TypeScript&logoColor=white) | ![GitHub](https://img.shields.io/badge/GitHub-181717.svg?style=for-the-badge&logo=GitHub&logoColor=white) | ![Postman](https://img.shields.io/badge/Postman-FF6C37.svg?style=for-the-badge&logo=Postman&logoColor=white) |
| ![MSSQL](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927.svg?style=for-the-badge&logo=Microsoft-SQL-Server&logoColor=white) | ![MOBX](https://img.shields.io/badge/MobX-FF9955.svg?style=for-the-badge&logo=MobX&logoColor=white) | 
| ![JWT](https://img.shields.io/badge/JSON%20Web%20Tokens-000000.svg?style=for-the-badge&logo=JSON-Web-Tokens&logoColor=white) | ![AXIOS](https://img.shields.io/badge/Axios-5A29E4.svg?style=for-the-badge&logo=Axios&logoColor=white) |
|   | ![SemanticUIReact](https://img.shields.io/badge/Semantic%20UI%20React-35BDB2.svg?style=for-the-badge&logo=Semantic-UI-React&logoColor=white) |

## Screenshots

![BazArt_Home_page](https://github.com/MarcinSzkurlat/BazArt/assets/94744112/8004e420-58ce-4c90-b734-8960a04cd4e5)

![BazArt_Registration](https://github.com/MarcinSzkurlat/BazArt/assets/94744112/cd84397d-bb4b-413c-aba0-7f6dc3884387)

![BazArt_Create_event_modal](https://github.com/MarcinSzkurlat/BazArt/assets/94744112/f765faa3-62c9-46c4-8e86-74fff33f7bcc)

![BazArt_Event](https://github.com/MarcinSzkurlat/BazArt/assets/94744112/58610402-b65c-4081-bd9e-c7bf4cb2482d)

# Getting Started

Clone this repository.
```
git clone https://github.com/MarcinSzkurlat/BazArt.git
```

Make sure you have installed Docker on your computer. After that, you can run the below command from the `/src/` directory and get started with the `BazArt` immediately.
```gitbash
docker compose up
```

You should be able to browse the application by using the below URL:
```
http://localhost:3000
```

You can test the application as `user` or `admin`.
| | USER | ADMIN |
| -------- | -------- | -------- |
| Login | ```user@test.com``` | ```admin@test.com``` |
| Password | ```Test123?``` | ```BA_Admin123``` |
