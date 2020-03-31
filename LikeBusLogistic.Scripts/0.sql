if not exists (select name from master.dbo.sysdatabases where name = 'LikeBusLogisticDatabase')
  create database LikeBusLogisticDatabase;
go

use LikeBusLogisticDatabase; 
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Role' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Role
  (
    Id int not null primary key identity,
    Name nvarchar(20) not null unique,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint UQ_dbo_Role_Name unique (Name)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='User' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.[User]
  (
    Id int not null primary key identity,
    FirstName nvarchar(100) not null,
    LastName nvarchar(100) null,
    MiddleName nvarchar(100) null,
    Email varchar(100) null,
    Phone char(13) null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Account' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Account
  (
    Id int not null primary key identity,
    RoleId int not null, 
    UserId int not null,
	  Login varchar(100) not null,
	  Password char(32) not null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_Account_RoleId_dbo_Role_Id foreign key (RoleId) references Role(Id),
    constraint FK_dbo_Account_UserId_dbo_User_Id foreign key (UserId) references [User](Id),
    constraint UQ_dbo_Account_RoleId_UserId unique (RoleId, UserId),
    constraint UQ_dbo_Account_Login unique (Login)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Driver' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Driver
  (
    Id int not null primary key identity,
    FirstName nvarchar(100) not null,
    LastName nvarchar(100) null,
    MiddleName nvarchar(100) null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='DriverContact' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.DriverContact
  (
    Id int not null primary key identity,
    DriverId int not null,
    Contact varchar(100) not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_DriverContact_DriverId_dbo_Driver_Id foreign key (DriverId) references Driver(Id),
    constraint UQ_dbo_DriverContact_Contact unique (Contact)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Country' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Country
  (
    Id int not null primary key identity,
    Name nvarchar(100) not null,
    ShortName char(2) not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='District' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.District
  (
    Id int not null primary key identity,
    Name nvarchar(200) not null,
    CountryId int not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_District_CountryId_dbo_Country_Id foreign key (CountryId) references Country(Id)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='City' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.City
  (
    Id int not null primary key identity,
    Name nvarchar(200) not null,
    DistrictId int null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_City_DistrictId_dbo_District_Id foreign key (DistrictId) references District(Id)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Location' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.[Location]
  (
    Id int not null primary key identity,
    CountryId int null,
    DistrictId int null,
    CityId int null,
    Name nvarchar(200) null,
    Latitude float not null,
    Longitude float not null,
    IsParking bit not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_Location_CountryId_dbo_Country_Id foreign key (CountryId) references Country(Id),
    constraint FK_dbo_Location_DistrictId_dbo_District_Id foreign key (DistrictId) references District(Id),
    constraint FK_dbo_Location_CityId_dbo_City_Id foreign key (CityId) references City(Id)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Vehicle' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Vehicle
  (
    Id int not null primary key identity,
    Producer nvarchar(100) not null,
    Model nvarchar(100) not null,
    PassengerCapacity int not null,
    Description nvarchar(1000) null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Bus' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Bus
  (
    Id int not null primary key identity,
    VehicleId int null,
    Number nvarchar(10) not null,
    CrewCapacity int not null constraint DF_dbo_Bus_CrewCapacity default 2,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_Bus_VehicleId_dbo_Vehicle_Id foreign key (VehicleId) references Vehicle(Id),
    constraint UQ_dbo_Bus_Number unique (Number),
    constraint CK_dbo_Bus_CrewCapacity check (CrewCapacity > 0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='BusDriver' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.BusDriver
  (
    Id int not null primary key identity,
    BusId int not null,
    DriverId int not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_BusDriver_BusId_dbo_Bus_Id foreign key (BusId) references Bus(Id),
    constraint FK_dbo_BusDriver_DriverId_dbo_Driver_Id foreign key (DriverId) references Driver(Id)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='RepairSpecialist' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.RepairSpecialist
  (
    Id int not null primary key identity,
    [Name] nvarchar(200) null,
    LocationId int not null,
    Contact nvarchar(500) null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_RepairSpecialist_LocationId_dbo_Location_Id foreign key (LocationId) references [Location](Id)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Route' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.[Route]
  (
    Id int not null primary key identity,
    DepartureId int null,
    ArrivalId int null,
    Name nvarchar(100) not null,
    EstimatedDurationInHours float null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_Route_DepartureId_dbo_Locaiton_Id foreign key (DepartureId) references [Location](Id),
    constraint FK_dbo_Route_ArrivalId_dbo_Location_Id foreign key (ArrivalId) references [Location](Id),
    constraint CK_dbo_Route_EstimatedDurationInHours check (EstimatedDurationInHours > 0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='RouteLocation' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.RouteLocation
  (
    Id int not null primary key identity,
    RouteId int not null,
    CurrentLocationId int not null,
    PreviousLocationId int null,
    Distance float not null default(0),
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_RouteLocation_CurrentLocationId_dbo_Locaiton_Id foreign key (CurrentLocationId) references [Location](Id),
    constraint FK_dbo_RouteLocation_PreviousLocationId_dbo_Location_Id foreign key (PreviousLocationId) references [Location](Id),
    constraint CK_dbo_RouteLocation_CurrentLocationId_PreviousLocationId check (CurrentLocationId <> PreviousLocationId),
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Schedule' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Schedule
  (
    Id int not null primary key identity,
    Name nvarchar(100) not null unique,
    RouteId int not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_Schedule_RouteId_dbo_Route_Id foreign key (RouteId) references [Route](Id)
  );
go
 
if not exists (select 1 
               from sys.tables t 
               where t.name='ScheduleLocation' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.ScheduleLocation
  (
    Id int not null primary key identity,
    ScheduleId int not null,
    PreviousLocationId int null,
    CurrentLocationId int not null,
    Distance float not null default(0),
    ArrivalTime time null,
    DepartureTime time null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_ScheduleRouteLocation_ScheduleId_dbo_Schedule_Id 
    foreign key (ScheduleId) references Schedule(Id),
    
    constraint FK_dbo_ScheduleRouteLocation_PreviousLocationId_dbo_Location_Id 
    foreign key (PreviousLocationId) references Location(Id),
    constraint FK_dbo_ScheduleRouteLocation_CurrentLocationId_dbo_Location_Id 
    foreign key (CurrentLocationId) references Location(Id)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Trip' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Trip
  (
    Id int not null primary key identity,
    ScheduleId int not null,
    Departure date not null,
    Status char(1) not null default('P'),
    Color char(7) null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    constraint FK_dbo_Trip_ScheduleId_dbo_Schedule_Id foreign key (ScheduleId) references Schedule(Id),
    constraint CK_dbo_Trip_Status check(Status in ('P','S','D','F')) -- P(pending), S(started), D(delayed), F(finished)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='TripBus' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.TripBus
  (
    Id int not null primary key identity,
    BusId int not null,
    TripId int not null,
    LocationId int not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
    
    constraint FK_dbo_TripBus_BusId_dbo_Bus_Id foreign key (BusId) references Bus(Id),
    constraint FK_dbo_TripBus_TripId_dbo_Trip_Id foreign key (TripId) references Trip(Id),
    constraint FK_dbo_TripBus_LocationId_dbo_Location_Id foreign key (LocationId) references [Location](Id)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='TripBusDriver' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.TripBusDriver
  (
    Id int not null primary key identity,
    TripBusId int not null,
    DriverId int not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
    
    constraint FK_dbo_TripBusDriver_DriverId_dbo_Driver_Id foreign key (DriverId) references Driver(Id),
    constraint FK_dbo_TripBusDriver_TripBusId_dbo_TripBus_Id foreign key (TripBusId) references TripBus(Id),
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='BusCoordinate' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.BusCoordinate
  (
    Id int not null primary key identity,
    BusId int not null,
    LocationId int null,
    Latitude float null,
    Longitude float null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
    
    constraint FK_dbo_BusLocation_BusId_dbo_Bus_Id foreign key (BusId) references Bus(Id),
    constraint FK_dbo_BusLocation_LocationId_dbo_Bus_Id foreign key (LocationId) references Location(Id),
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Distance' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Distance
  (
    Id int not null primary key identity,
    Location1 int not null,
    Location2 int not null,
    TomTomInfo nvarchar(max) null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
    
    constraint FK_dbo_Distance_Location1_dbo_Location_Id foreign key (Location1) references Location(Id),
    constraint FK_dbo_Distance_Location2_dbo_Location_Id foreign key (Location2) references Location(Id)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Lookups' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.Lookups
  (
    Id int not null primary key identity,
    Description	varchar not null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='LookupValues' 
               and t.schema_id = schema_id('dbo'))
  create table dbo.LookupValues
  (
    Id int not null primary key identity,
    LookupId int	not null,
    Value	varchar	null,
    DisplayText	varchar null,
    IsEditable	bit null,
    SortOrder	int null,
    CreatedBy int null foreign key references Account(Id),
    ModifiedBy int null foreign key references Account(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
    
    constraint FK_dbo_LookupValues_LookupId_dbo_Lookups_Id foreign key (LookupID) references Lookups(Id)
  );
go