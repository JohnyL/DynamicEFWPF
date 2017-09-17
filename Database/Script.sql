use master;
go

drop database if exists Test;
go

create database Test;
go

use Test;
go

create table dbo.Strategy
(
	Id     int identity not null primary key,
	[Name] varchar(500)
);

create table dbo.Operator
(
	Id     int identity not null primary key,
	[Name] varchar(500)
);

create table dbo.OrderState
(
	Id     int identity not null primary key,
	[Name] varchar(500)
);

-- Insert dummy values
insert into dbo.Strategy values ('Strategy1'), ('Strategy2'), ('Strategy3');
insert into dbo.Operator values ('Operator1'), ('Operator2'), ('Operator3');
insert into dbo.OrderState values ('OrderState1'), ('OrderState2'), ('OrderState3');