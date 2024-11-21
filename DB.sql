USE [master]
GO
/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'QuizPractice')
BEGIN
	ALTER DATABASE QuizPractice SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE QuizPractice
END

GO
 
CREATE  DATABASE QuizPractice
GO

USE QuizPractice

CREATE TABLE [Users] (
	[id] INTEGER NOT NULL IDENTITY UNIQUE,
	[username] VARCHAR(20),
	[password] VARCHAR(16) NOT NULL,
	[status] INTEGER NOT NULL,
	[createdAt] DATETIME NOT NULL,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [UserDetails] (
	[userId] INTEGER FOREIGN KEY REFERENCES Users(id),
	[firstName] NVARCHAR(16) NOT NULL,
	[lastName] NVARCHAR(16) NOT NULL,
	[email] VARCHAR(50) NOT NULL,
	[gender] VARCHAR(10) NOT NULL,
	[dob] DATE NOT NULL,
	[avatar] VARCHAR(MAX),
	PRIMARY KEY (userId)
);
GO
