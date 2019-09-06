CREATE DATABASE DemoExpenses;
GO
USE DemoExpenses;
GO
CREATE TABLE ExpenseItems(
  Name varchar(255) PRIMARY KEY NOT NULL,
  TripId varchar(255) NULL,
  Cost money NULL,
  Currency varchar(255) NULL,
  Date date NULL
);
GO