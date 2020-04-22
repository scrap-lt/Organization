USE master;
GO

IF DB_ID ('Organization') IS NOT NULL
	DROP DATABASE [Organization];
GO

CREATE DATABASE [Organization]
ON
(
	NAME = [Organization],
	FILENAME = 'D:\Organization.mdf',
	SIZE = 512MB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1024KB 
)
LOG ON
(
	NAME = [Organization_log],
	FILENAME = 'D:\Organization_log.ldf',
	SIZE = 256MB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 10%
);
GO

USE [Organization];
GO

IF OBJECT_ID ('Positions', 'U') IS NOT NULL
	DROP TABLE [Positions];
GO

CREATE TABLE [Positions]
(
	[Id] INT IDENTITY (1, 1) PRIMARY KEY,
	[Name] NVARCHAR (150) NOT NULL
);
GO

INSERT INTO [Positions] ([Name]) VALUES ('Директор');
INSERT INTO [Positions] ([Name]) VALUES ('Бухгалтер');
INSERT INTO [Positions] ([Name]) VALUES ('Разработчик');
INSERT INTO [Positions] ([Name]) VALUES ('Тестировщик');
INSERT INTO [Positions] ([Name]) VALUES ('Администратор');

IF OBJECT_ID ('Employees', 'U') IS NOT NULL
	DROP TABLE [Employees];
GO

CREATE TABLE [Employees]
(
	[Id] INT IDENTITY (1, 1) PRIMARY KEY,
	[Name] NVARCHAR (150) NOT NULL,
	[Patronymic] NVARCHAR (150) NULL,
	[Surname] NVARCHAR (150) NOT NULL,
	[Gender] TINYINT NOT NULL,
	[PositionId] INT NOT NULL,
	[Birthdate] DATETIME NOT NULL,
	[Phone] NVARCHAR (20) NULL,
	CONSTRAINT [FK_Employees_Positions] FOREIGN KEY ([PositionId]) REFERENCES [Positions] ([Id]),
);
GO

INSERT INTO [Employees] ([Name], [Patronymic], [Surname], [Gender], [PositionId], [Birthdate])
VALUES ('Иван', 'Иванович', 'Иванов', 1, 5, DATEFROMPARTS (1990, 1, 1)),
	('Николай', 'Николаевич', 'Николаев', 1, 1, DATEFROMPARTS (1968, 7, 18)),
	('Светлана', 'Ивановна', 'Петрова', 2, 2, DATEFROMPARTS (1975, 5, 10)),
	('Семен', 'Семенович', 'Семенов', 1, 4, DATEFROMPARTS (1999, 11, 20)),
	('Максим', 'Олегович', 'Сидоров', 1, 3, DATEFROMPARTS (1985, 11, 11)),
	('Ирина', 'Александровна', 'Смирнова', 2, 3, DATEFROMPARTS (2000, 2, 4));

IF OBJECT_ID ('Roles', 'U') IS NOT NULL
	DROP TABLE [Roles];
GO

CREATE TABLE [Roles]
(
	[Id] INT IDENTITY (1, 1) PRIMARY KEY,
	[Name] NVARCHAR (50) NOT NULL
);
GO

INSERT INTO [Roles] ([Name]) VALUES ('Администратор');
INSERT INTO [Roles] ([Name]) VALUES ('Пользователь');

IF OBJECT_ID ('Users', 'U') IS NOT NULL
	DROP TABLE [Users];
GO

CREATE TABLE [Users]
(
	[Id] INT IDENTITY (1, 1) PRIMARY KEY,
	[Login] NVARCHAR (50) NOT NULL UNIQUE,
	[Password] NVARCHAR (150) NOT NULL,
	[RoleId] INT NOT NULL,
	[EmployeeId] INT NOT NULL,
	CONSTRAINT [FK_Employees_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]),
	CONSTRAINT [FK_Users_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [Users] ([Login], [Password], [RoleId], [EmployeeId])
VALUES ('adm1', 'qwerty', 1, 1),
	('max', 'max', 2, 5);
GO
