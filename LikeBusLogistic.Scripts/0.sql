if not exists (select name from master.dbo.sysdatabases where name = 'LikeBusLogisticDatabase')
  create database LikeBusLogisticDatabase;
go
