-- Stored Procedure's example:
-- if object_id(N'dbo.StoreProcedureName') is null
--   exec('create procedure dbo.StoreProcedureName as set nocount on;');
-- go
-- 
-- -- ============================================================================
-- -- Example    : exec dbo.StoreProcedureName
-- -- Author     : Nikita Dermenzhi
-- -- Date       : 25/07/2019
-- -- Description: Ч
-- -- ============================================================================
-- 
-- alter procedure dbo.StoreProcedureName
-- (  
--     @Param1 as int = null  
--   , @Param2 as varchar(100) = null  
-- )  
-- as  
-- begin  
--   
-- 
-- 
-- end;
-- go
--
-- Function's example:
-- if (object_ID('dbo.FunctionName') is not null)
--    drop function dbo.FunctionName
-- go
-- 
-- -- ============================================================================
-- -- Example    : select dbo.FunctionName('qwe')
-- -- Author     : Nikita Dermenzhi
-- -- Date       : 25/07/2019
-- -- Description: Ч
-- -- ============================================================================
-- 
-- create function dbo.FunctionName(@Param1 nvarchar(100))
-- returns char(32)
-- as 
-- begin 
-- 
-- end
-- go


use LikeBusLogisticDatabase;
go

if (object_ID('dbo.MD5HashPassword') is not null)
   drop function dbo.MD5HashPassword
go

-- ============================================================================
-- Example    : select dbo.MD5HashPassword('qwe')
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

create function dbo.MD5HashPassword(@password nvarchar(100))
returns char(32)
as 
begin 
  return convert(varchar(32), hashbytes('MD5', @password + 'L1k3Bu$L0gi$tiC'), 2) 
end
go

if object_id(N'dbo.GetUserAccountById') is null
  exec('create procedure dbo.GetUserAccountById as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetUserAccountById 'a', 'a'
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetUserAccountById
(  
    @id as int
)  
as  
begin  
  
  select a.Id         as AccountId
       , r.Id         as RoleId
       , r.Name       as RoleName
       , u.Id         as UserId
       , u.FirstName  as FirstName
       , u.LastName   as LastName
       , u.MiddleName as MiddleName
       , u.Phone      as Phone
       , u.Email      as Email
    from Account a
    join [Role] r on a.RoleId = r.Id
    join [User] u on a.UserId = u.Id
    where 1=1
      and a.Id = @id
      and a.isDeleted = 0
      and r.isDeleted = 0
      and u.isDeleted = 0

end;
go

if object_id(N'dbo.GetUserAccountByCredentials') is null
  exec('create procedure dbo.GetUserAccountByCredentials as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetUserAccountByCredentials 'a', 'a'
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetUserAccountByCredentials
(  
    @login as varchar(100) = null  
  , @password as varchar(100) = null  
)  
as  
begin  
  
  select a.Id         as AccountId
       , r.Id         as RoleId
       , r.Name       as RoleName
       , u.Id         as UserId
       , u.FirstName  as FirstName
       , u.LastName   as LastName
       , u.MiddleName as MiddleName
       , u.Phone      as Phone
       , u.Email      as Email
    from Account a
    join [Role] r on a.RoleId = r.Id
    join [User] u on a.UserId = u.Id
    where 1=1
      and a.Login = @login
      and a.Password = dbo.MD5HashPassword(@password)
      and a.isDeleted = 0
      and r.isDeleted = 0
      and u.isDeleted = 0

end;
go

if object_id(N'dbo.GetDriverInfo') is null
  exec('create procedure dbo.GetDriverInfo as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetDriverInfo 1
-- Author     : Nikita Dermenzhi
-- Date       : 20/11/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetDriverInfo
(  
    @driverId as int = null,
    @withDeleted as bit = 0
)  
as  
begin  
  
  select d.Id as DriverId
       , bi.BusId
       , bi.BusInfo
       , d.FirstName
       , d.LastName
       , d.MiddleName
       , d.IsDeleted
    from Driver d
    cross apply
    (
      select top 1 bs.BusId
                 --, b.Number as BusInfo
                 , concat(b.Number, ' (', v.Producer, ' ', v.Model, ')') as BusInfo
        from BusDriver bs
        join Bus b on bs.BusId = b.Id
        join Vehicle v on b.VehicleId = v.Id
        where bs.DriverId = d.Id
          and bs.IsDeleted = 0
          and b.IsDeleted = 0
          and v.IsDeleted = 0
    ) as bi
    where 1=1
      and (d.IsDeleted = 0 or @withDeleted = 1)
      and d.Id = isnull(@driverId, d.Id)

end;
go

if object_id(N'dbo.MergeDriver') is null
  exec('create procedure dbo.MergeDriver as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.MergeDriver @driverId=3,@busId=1,@firstName='Ќиколай',@lastName='—тепаненко',@middleName='¬асильевич'
-- Author     : Nikita Dermenzhi
-- Date       : 20/11/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.MergeDriver
(  
    @driverId as int = null,
    @busId as int,
    @firstName as nvarchar(100),
    @lastName as nvarchar(100),
    @middleName as nvarchar(100)
)  
as  
begin  
  
   begin try
    begin transaction

      merge dbo.Driver as trg
      using
      (
        select @driverId as Id
             , @firstName as FirstName
             , @lastName as LastName
             , @middleName as MiddleName
      ) as src
      on trg.Id = src.Id
        when not matched then
          insert (FirstName, LastName, MiddleName)
          values (src.FirstName, src.LastName, src.MiddleName)
        when matched then
          update set FirstName = src.FirstName, LastName = src.LastName, MiddleName = src.MiddleName
        ;
      
      set @driverId = isnull(@driverId, ident_current('dbo.Driver'));

      merge dbo.BusDriver as trg
      using
      (
        select @driverId as DriverId
             , @busId as BusId
      ) as src
      on trg.DriverId = src.DriverId
        when not matched then
          insert (DriverId, BusId)
          values (src.DriverId, src.BusId)
        when matched then
          update set DriverId = src.DriverId, BusId = src.BusId, IsDeleted = 0
        ;

    commit
  end try
  begin catch
   rollback
  end catch

end;
go

if object_id(N'dbo.GetDistrict') is null
  exec('create procedure dbo.GetDistrict as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetDistrict
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetDistrict
(  
    @districtId as int = null,
    @withDeleted as bit = 0
)  
as  
begin  
  
 select d.Id as Id
      , d.Name as Name
      , c.Id as CountryId
      , c.Name as CountryName
      , d.IsDeleted as IsDeleted
   from District d
   join Country c on d.CountryId = c.Id
   where 1=1
     and (c.IsDeleted = 0 or @withDeleted = 1)
     and (d.IsDeleted = 0 or @withDeleted = 1)
     and d.Id  = isnull(@districtId, d.Id)

end;
go

if object_id(N'dbo.GetCity') is null
  exec('create procedure dbo.GetCity as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetCity
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetCity
(  
    @cityId as int = null,
    @withDeleted as bit = 0
)  
as  
begin  
  
 select c.Id as Id
      , c.Name as Name
      , d.Id as DistrictId
      , d.Name as DistrictName
      , ctr.Id as CountryId
      , ctr.Name as CountryName
      , c.IsDeleted as IsDeleted
   from City c
   join District d on c.DistrictId = d.Id
   join Country ctr on d.CountryId = ctr.Id
   where 1=1
     and (c.IsDeleted = 0 or @withDeleted = 1)
     and (d.IsDeleted = 0 or @withDeleted = 1)
     and (ctr.IsDeleted = 0 or @withDeleted = 1)
     and c.Id  = isnull(@cityId, c.Id)

end;
go

if object_id(N'dbo.GetLocation') is null
  exec('create procedure dbo.GetLocation as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetLocation
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetLocation
(  
    @locationId as int = null,
    @withDeleted as bit = 0
)  
as  
begin  
  
 select l.Id as Id
      , l.Name as Name
      , l.Latitude as Latitude
      , l.Longtitude as Longtitude
      , l.IsCarRepair as IsCarRepair
      , l.IsParking as IsParking
      , c.Id as CityId
      , c.Name as CityName
      , d.Id as DistrictId
      , d.Name as DistrictName
      , ctr.Id as CountryId
      , ctr.Name as CountryName
      , l.IsDeleted as IsDeleted
   from Location l
   left join City c on l.CityId = c.Id and c.IsDeleted = 0
   left join District d on l.DistrictId = d.Id and d.IsDeleted = 0
   left join Country ctr on l.CountryId = ctr.Id and ctr.IsDeleted = 0
   where 1=1
     and (l.IsDeleted = 0 or @withDeleted = 1)
     and l.Id  = isnull(@locationId, l.Id)

end;
go

if object_id(N'dbo.GetRouteLocation') is null
  exec('create procedure dbo.GetRouteLocation as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetRouteLocation 1
-- Author     : Nikita Dermenzhi
-- Date       : 13/12/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetRouteLocation
(  
    @routeId as int = null
)  
as  
begin  
  
  with LinkedList (RouteId
                 , RouteName
                 , EstimatedDurationInHours
                 , CurrentLocationId
                 , PreviousLocationId
                 , StopDurationInHours
                 , [Level])
  as
  (
    select r1.Id as RouteId
         , r1.[Name] as RouteName
         , rl1.EstimatedDurationInHours as EstimatedDurationInHours
         , rl1.CurrentLocationId as CurrentLocationId
         , rl1.PreviousLocationId as PreviousLocationId
         , rl1.StopDurationInHours as StopDurationInHours
         , 0 as [Level]
      from [Route] r1
      join RouteLocation rl1 
        on r1.Id = rl1.RouteId
      where 1=1
        and r1.Id = @routeId
        and r1.IsDeleted = 0
        and rl1.IsDeleted = 0
        and rl1.PreviousLocationId is null
  
      union all
  
    select r2.Id as RouteId
         , r2.[Name] as RouteName
         , rl2.EstimatedDurationInHours as EstimatedDurationInHours
         , rl2.CurrentLocationId as CurrentLocationId
         , rl2.PreviousLocationId as PreviousLocationId
         , rl2.StopDurationInHours as StopDurationInHours
         , [Level] + 1 as [Level]
      from [Route] r2
      join RouteLocation rl2 
        on r2.Id = rl2.RouteId
      join LinkedList as l
        on rl2.PreviousLocationId = l.CurrentLocationId
      where 1=1
        and r2.Id = @routeId
        and r2.IsDeleted = 0
        and rl2.IsDeleted = 0
  )
  select ll.RouteId as RouteId
       , ll.RouteName as RouteName
       , ll.EstimatedDurationInHours as EstimatedDurationInHours
       , ll.StopDurationInHours as StopDurationInHours
       , ll.CurrentLocationId as CurrentLocationId
       , ll.PreviousLocationId as PreviousLocationId

       -- Current Location
       , cl.[Name] as CurrentName
       , cl.CityId as CurrentCityId
       , c1.[Name] as CurrentCityName
       , cl.CountryId as CurrentCountryId
       , co1.[Name] as CurrentCountryName
       , cl.DistrictId as CurrentDistrictId
       , d1.[Name] as CurrentDistrictName
       , cl.IsCarRepair as CurrentIsCarRepair
       , cl.IsParking as CurrentIsParking
       , cl.Latitude as CurrentLatitude
       , cl.Longtitude as CurrentLongtitude

       -- Previous Location
       , pl.[Name] as PreviousName
       , pl.CityId as PreviousCityId
       , c2.[Name] as PreviousCityName
       , pl.CountryId as PreviousCountryId
       , co2.[Name] as PreviousCountryName
       , pl.DistrictId as PreviousDistrictId
       , d2.[Name] as PreviousDistrictName
       , pl.IsCarRepair as PreviousIsCarRepair
       , pl.IsParking as PreviousIsParking
       , pl.Latitude as PreviousLatitude
       , pl.Longtitude as PreviousLongtitude

    from LinkedList ll

    -- Current location
    left join [Location] cl 
      on ll.CurrentLocationId = cl.Id and cl.IsDeleted = 0
    left join City c1
      on cl.CityId = c1.Id
    left join Country co1
      on cl.CountryId = co1.Id
    left join District d1
      on cl.DistrictId = d1.Id
      
    -- Previous Location
    left join [Location] pl
      on ll.PreviousLocationId = pl.Id and pl.IsDeleted = 0
    left join City c2
      on pl.CityId = c2.Id
    left join Country co2
      on pl.CountryId = co2.Id
    left join District d2
      on pl.DistrictId = d2.Id

end;
go
