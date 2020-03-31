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
    outer apply
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

if (object_ID('dbo.GetLocationInfo') is not null)
   drop function dbo.GetLocationInfo
go

-- ============================================================================
-- Example    : select * from dbo.GetLocationInfo(1)
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

create function dbo.GetLocationInfo(@locationId int = null)
returns @result table
(
  Id           int not null,
  FullName     nvarchar(max) null,
  Name         nvarchar(500) null,
  Latitude     float null,
  Longitude   float null,
  IsCarRepair  bit null,
  IsParking    bit null,
  CityId       int null,
  CityName     nvarchar(500) null,
  DistrictId   int null,
  DistrictName nvarchar(500) null,
  CountryId    int null,
  CountryName  nvarchar(500) null,
  IsDeleted    bit not null
)
as 
begin 

  insert @result
  select l.Id
       , concat(l.Name, ' ('+concat( c.Name, ', '+d.Name, ', '+ctr.Name)+')') as FullName
       , l.Name
       , l.Latitude
       , l.Longitude
       , case
           when exists
           (
             select 1
               from RepairSpecialist rs
               where LocationId = l.Id
           )
             then 1
             else 0
         end as IsCarRepair
       , l.IsParking
       , c.Id
       , c.Name
       , d.Id
       , d.Name
       , ctr.Id
       , ctr.Name
       , l.IsDeleted
   from Location l
   left join City c on l.CityId = c.Id and c.IsDeleted = 0
   left join District d on l.DistrictId = d.Id and d.IsDeleted = 0
   left join Country ctr on l.CountryId = ctr.Id and ctr.IsDeleted = 0
   where 1=1
     and l.Id = @locationId

   return;

end
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
      , concat(l.Name, ' ('+concat(ctr.Name, ', '+d.Name, ', '+c.Name)+')') as FullName
      , l.Name as Name
      , l.Latitude as Latitude
      , l.Longitude as Longitude
      , case
          when exists
          (
            select 1
              from RepairSpecialist rs
              where LocationId = l.Id
          )
            then 1
            else 0
        end as IsCarRepair
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

if object_id(N'dbo.GetRoute') is null
  exec('create procedure dbo.GetRoute as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetRoute 1
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetRoute
(  
    @routeId as int = null,
    @withDeleted as bit = 0
)  
as  
begin  
  
  select r.Id as Id
       , r.Name as Name
       , r.EstimatedDurationInHours as EstimatedDurationInHours
       , r.DepartureId as DepartureId
       , concat(d.Name, ' ('+concat( d.CountryName+', ', d.DistrictName+', ', d.CityName)+')') as DepartureLocationName
       , r.ArrivalId as ArrivalId
       , concat(a.Name, ' ('+concat( a.CountryName+', ', a.DistrictName+', ', a.CityName)+')') as ArrivalLocationName
       , r.IsDeleted as IsDeleted
    from [Route] r
    cross apply
    (
      select * from dbo.GetLocationInfo
        (r.DepartureId) l
    ) d
    cross apply
    (
      select *
        from dbo.GetLocationInfo(r.ArrivalId) l
    ) a
    where 1=1
      and r.Id = isnull(@routeId, r.Id)
      and (r.IsDeleted = 0 or @withDeleted = 1)

end;
go

if object_id(N'dbo.GetRouteLocation') is null
  exec('create procedure dbo.GetRouteLocation as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetRouteLocation 2, 5
-- Author     : Nikita Dermenzhi
-- Date       : 13/12/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetRouteLocation
(  
    @routeId as int = null,
    @locationId as int = null
)  
as  
begin  
  
  with LinkedList (RouteId
                 , RouteLocationId
                 , RouteName
                 , Distance
                 , CurrentLocationId
                 , PreviousLocationId
                 , [Level])
  as
  (
    select r1.Id as RouteId
         , rl1.Id as RouteLocationId
         , r1.[Name] as RouteName
         , rl1.Distance as Distance
         , rl1.CurrentLocationId as CurrentLocationId
         , rl1.PreviousLocationId as PreviousLocationId
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
         , rl2.Id as RouteLocationId
         , r2.[Name] as RouteName
         , rl2.Distance as Distance
         , rl2.CurrentLocationId as CurrentLocationId
         , rl2.PreviousLocationId as PreviousLocationId
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
       , ll.RouteLocationId as RouteLocationId
       , ll.RouteName as RouteName
       , ll.Distance as Distance
       , ll.CurrentLocationId as CurrentLocationId
       , ll.PreviousLocationId as PreviousLocationId

       -- Current Location
       , c.FullName as CurrentFullName
       , c.[Name] as CurrentName
       , c.CityId as CurrentCityId
       , c.CityName as CurrentCityName
       , c.CountryId as CurrentCountryId
       , c.CountryName as CurrentCountryName
       , c.DistrictId as CurrentDistrictId
       , c.DistrictName as CurrentDistrictName
       , c.IsCarRepair as CurrentIsCarRepair
       , c.IsParking as CurrentIsParking
       , c.Latitude as CurrentLatitude
       , c.Longitude as CurrentLongitude

       -- Previous Location
       , p.FullName as PreviousFullName
       , p.[Name] as PreviousName
       , p.CityId as PreviousCityId
       , p.CityName as PreviousCityName
       , p.CountryId as PreviousCountryId
       , p.CountryName as PreviousCountryName
       , p.DistrictId as PreviousDistrictId
       , p.DistrictName as PreviousDistrictName
       , p.IsCarRepair as PreviousIsCarRepair
       , p.IsParking as PreviousIsParking
       , p.Latitude as PreviousLatitude
       , p.Longitude as PreviousLongitude

       , (
            select top 1 TomTomInfo 
              from Distance d 
              where 1=1 
                and d.Location1 = PreviousLocationId 
                and d.Location2 = CurrentLocationId
              order by d.DateModified, d.DateCreated desc
         ) as TomTomInfo
    from LinkedList ll
    -- Current location
    cross apply
    (
      select *
        from dbo.GetLocationInfo(ll.CurrentLocationId)
    ) c
    -- Previous Location
    outer apply
    (
      select *
        from dbo.GetLocationInfo(ll.PreviousLocationId)
    ) p
    where 1=1 
      and ll.CurrentLocationId = isnull(@locationId, ll.CurrentLocationId)
    order by ll.Level

end;
go

if object_id(N'dbo.GetSchedule') is null
  exec('create procedure dbo.GetSchedule as set nocount on;');
go
   
-- ============================================================================  
-- Example    : exec dbo.GetSchedule  
-- Author     : Nikita Dermenzhi  
-- Date       : 01/03/2020  
-- Description: Ч  
-- ============================================================================  
  
alter procedure dbo.GetSchedule  
(    
   @scheduleId as int = null,
   @routeId as int = null, 
   @withDeleted as bit = 0  
)    
as    
begin    
    
  select s.Id        as Id  
       , s.Name      as Name  
       , s.RouteId   as RouteId  
       , iif(dbo.IsScheduleMatchRoute(s.Id) = 0, 1, 0) as NeedsSync  
       , r.Name      as RouteName
       , case 
           when exists
           (
              select 1 
                from Trip t
                where 1=1
                  and t.ScheduleId = s.Id
                  and (t.Status in('S', 'D')
                       or getdate() > dateadd(hour, -12, (cast(t.Departure as datetime) + cast(dt.DepartureTime as datetime)))
                      )
           ) then 1
             else 0 
         end as HasConfirmedTrips
       , s.IsDeleted as IsDeleted  
    from Schedule s  
    join [Route] r on s.RouteId = r.Id
    outer apply
    (
      select top 1 sl.DepartureTime
        from ScheduleLocation sl
        where 1=1
          and sl.ScheduleId = s.Id
          and sl.PreviousLocationId is null
          and sl.IsDeleted = 0
    ) as dt
    where 1=1  
     and (s.IsDeleted = 0 or @withDeleted = 1)  
     and (r.IsDeleted = 0 or @withDeleted = 1)  
     and s.Id         = isnull(@scheduleId, s.Id)
     and s.RouteId    = isnull(@routeId, s.RouteId)
  
end;  
go

if object_id(N'dbo.GetScheduleInfo') is null
  exec('create procedure dbo.GetScheduleInfo as set nocount on;');
go

-- ============================================================================  
-- Example    : exec dbo.GetScheduleInfo 8
-- Author     : Nikita Dermenzhi  
-- Date       : 01/03/2020  
-- Description: Ч  
-- ============================================================================  

alter procedure dbo.GetScheduleInfo
(  
    @scheduleId as int = null
)  
as  
begin  

  with LinkedList (ScheduleId
                 , ScheduleName
                 , ScheduleRouteId
                 , SchedulePreviousLocationId
                 , ScheduleCurrentLocationId
                 , ScheduleLocationArrivalTime
                 , ScheduleLocationDepartureTime
                 , ScheduleLocationDistance
                 , Level)
  as
  (
    select parents.Id                  as ScheduleId
         , parents.Name                as ScheduleName
         , parents.RouteId             as ScheduleRouteId
         , parentsl.PreviousLocationId as SchedulePreviousLocationId
         , parentsl.CurrentLocationId  as ScheduleCurrentLocationId
         , parentsl.ArrivalTime        as ScheduleLocationArrivalTime
         , parentsl.DepartureTime      as ScheduleLocationDepartureTime
         , parentsl.Distance           as ScheduleLocationDistance
         , 0                           as Level
      from Schedule parents
      join ScheduleLocation parentsl 
        on parents.Id = parentsl.ScheduleId
      where 1=1
        and parents.Id = @scheduleId
        and parentsl.PreviousLocationId is null
        and parents.IsDeleted = 0
        and parentsl.IsDeleted = 0

    union all

    select childs.Id                  as ScheduleId
         , childs.Name                as ScheduleName
         , childs.RouteId             as ScheduleRouteId
         , childsl.PreviousLocationId as SchedulePreviousLocationId
         , childsl.CurrentLocationId  as ScheduleCurrentLocationId
         , childsl.ArrivalTime        as ScheduleLocationArrivalTime
         , childsl.DepartureTime      as ScheduleLocationDepartureTime
         , childsl.Distance           as ScheduleLocationDistance
         , parentlist.Level + 1       as Level
      from Schedule childs
      join ScheduleLocation childsl 
        on childs.Id = childsl.ScheduleId
      join LinkedList parentlist
        on childsl.PreviousLocationId = parentlist.ScheduleCurrentLocationId
      where 1=1
        and childs.Id = @scheduleId
        and childs.IsDeleted = 0
        and childsl.IsDeleted = 0
  )
  select l.ScheduleId                                       as Id
       , l.ScheduleName                                     as Name
       , l.ScheduleRouteId                                  as RouteId
       , l.SchedulePreviousLocationId                       as SchedulePreviousLocationId
       , l.ScheduleCurrentLocationId                        as ScheduleCurrentLocationId
       , l.ScheduleLocationArrivalTime                      as ScheduleLocationArrivalTime
       , l.ScheduleLocationDepartureTime                    as ScheduleLocationDepartureTime
       , l.ScheduleLocationDistance                         as ScheduleLocationDistance

       -- Current Location
       , c.FullName                                         as ScheduleLocationCurrentFullName
       , c.[Name]                                           as ScheduleLocationCurrentName
       , c.CityId                                           as ScheduleLocationCurrentCityId
       , c.CityName                                         as ScheduleLocationCurrentCityName
       , c.CountryId                                        as ScheduleLocationCurrentCountryId
       , c.CountryName                                      as ScheduleLocationCurrentCountryName
       , c.DistrictId                                       as ScheduleLocationCurrentDistrictId
       , c.DistrictName                                     as ScheduleLocationCurrentDistrictName
       , c.IsCarRepair                                      as ScheduleLocationCurrentIsCarRepair
       , c.IsParking                                        as ScheduleLocationCurrentIsParking
       , c.Latitude                                         as ScheduleLocationCurrentLatitude
       , c.Longitude                                        as ScheduleLocationCurrentLongitude

       -- Previous Location
       , p.FullName                                         as ScheduleLocationPreviousFullName
       , p.[Name]                                           as ScheduleLocationPreviousName
       , p.CityId                                           as ScheduleLocationPreviousCityId
       , p.CityName                                         as ScheduleLocationPreviousCityName
       , p.CountryId                                        as ScheduleLocationPreviousCountryId
       , p.CountryName                                      as ScheduleLocationPreviousCountryName
       , p.DistrictId                                       as ScheduleLocationPreviousDistrictId
       , p.DistrictName                                     as ScheduleLocationPreviousDistrictName
       , p.IsCarRepair                                      as ScheduleLocationPreviousIsCarRepair
       , p.IsParking                                        as ScheduleLocationPreviousIsParking
       , p.Latitude                                         as ScheduleLocationPreviousLatitude
       , p.Longitude                                        as ScheduleLocationPreviousLongitude
       
       , cast(iif(c.CountryId <> isnull(p.CountryId, c.CountryId), 1, 0) as bit) as ScheduleLocationIsBoundary

    from LinkedList l 
    -- Current location
    cross apply
    (
      select *
        from dbo.GetLocationInfo(l.ScheduleCurrentLocationId)
    ) c
    -- Previous Location
    outer apply
    (
      select *
        from dbo.GetLocationInfo(l.SchedulePreviousLocationId)
    ) p
    where 1=1 
    order by l.Level

end;
go

if (object_ID('dbo.GetScheduleRouteLocation') is not null)
   drop function dbo.GetScheduleRouteLocation
go

-- ============================================================================
-- Example    : select * from dbo.GetScheduleRouteLocation(8)
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

create function dbo.GetScheduleRouteLocation(@scheduleId int)
returns table
as 
  return 
  (
    with LinkedList (
                       ScheduleId
                     , ScheduleName
                     , ScheduleRouteId
                     , SchedulePreviousLocationId
                     , ScheduleCurrentLocationId
                     , ScheduleLocationArrivalTime
                     , ScheduleLocationDepartureTime
                     , ScheduleLocationDistance
                     , Level
                    )
    as
    (
      select parents.Id                  as ScheduleId
           , parents.Name                as ScheduleName
           , parents.RouteId             as ScheduleRouteId
           , parentsl.PreviousLocationId as SchedulePreviousLocationId
           , parentsl.CurrentLocationId  as ScheduleCurrentLocationId
           , parentsl.ArrivalTime        as ScheduleLocationArrivalTime
           , parentsl.DepartureTime      as ScheduleLocationDepartureTime
           , parentsl.Distance           as ScheduleLocationDistance
           , 0                           as Level
        from Schedule parents
        join ScheduleLocation parentsl 
          on parents.Id = parentsl.ScheduleId
        where 1=1
          and parents.Id = @scheduleId
          and parentsl.PreviousLocationId is null
          and parents.IsDeleted = 0
          and parentsl.IsDeleted = 0
  
      union all
  
      select childs.Id                  as ScheduleId
           , childs.Name                as ScheduleName
           , childs.RouteId             as ScheduleRouteId
           , childsl.PreviousLocationId as SchedulePreviousLocationId
           , childsl.CurrentLocationId  as ScheduleCurrentLocationId
           , childsl.ArrivalTime        as ScheduleLocationArrivalTime
           , childsl.DepartureTime      as ScheduleLocationDepartureTime
           , childsl.Distance           as ScheduleLocationDistance
           , parentlist.Level + 1       as Level
        from Schedule childs
        join ScheduleLocation childsl 
          on childs.Id = childsl.ScheduleId
        join LinkedList parentlist
          on childsl.PreviousLocationId = parentlist.ScheduleCurrentLocationId
        where 1=1
          and childs.Id = @scheduleId
          and childs.IsDeleted = 0
          and childsl.IsDeleted = 0
    )  
    select l.ScheduleId                                       as Id
         , l.ScheduleRouteId                                  as RouteId
         , l.SchedulePreviousLocationId                       as SchedulePreviousLocationId
         , l.ScheduleCurrentLocationId                        as ScheduleCurrentLocationId
         , l.ScheduleLocationArrivalTime                      as ScheduleLocationArrivalTime
         , l.ScheduleLocationDepartureTime                    as ScheduleLocationDepartureTime
         , l.ScheduleLocationDistance                         as ScheduleLocationDistance
         , l.Level                                            as Level
      from LinkedList l 
      -- Current location
      cross apply
      (
        select *
          from dbo.GetLocationInfo(l.ScheduleCurrentLocationId)
      ) c
      -- Previous Location
      outer apply
      (
        select *
          from dbo.GetLocationInfo(l.SchedulePreviousLocationId)
      ) p
      where 1=1 
  )
go

if object_id(N'dbo.GetTrips') is null
  exec('create procedure dbo.GetTrips as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetTrips
-- Author     : Nikita Dermenzhi
-- Date       : 13/03/2020
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetTrips
(  
   @tripId as int = null,
   @status as char(1) = null,
   @withDeleted as bit = 0   
)  
as  
begin  

  select t.Id                       as Id
       , cast(t.Departure as datetime) + 
         cast(dt.DepartureTime as datetime) as Departure
       , t.Status                   as Status
       , t.Color                    as Color
       , s.Id                       as ScheduleId
       , s.Name                     as ScheduleName
       , r.Id                       as RouteId
       , r.Name                     as RouteName
       , b.BusId                    as BusId
       , b.BusCrewCapacity          as BusCrewCapacity
       , b.BusNumber                as BusNumber
       , b.VehicleId                as VehicleId
       , b.VehicleModel             as VehicleModel
       , b.VehiclePassengerCapacity as VehiclePassengerCapacity
       , b.VehicleProducer          as VehicleProducer

       , td.TotalDistance           as TotalDistance
    from Trip t
    join Schedule s on t.ScheduleId = s.Id
    join Route r on r.Id = s.RouteId
    cross apply
    (
      select top 1 tb.Id               as TripBusId
                 , b.Id                as BusId
                 , b.CrewCapacity      as BusCrewCapacity
                 , b.Number            as BusNumber
                 , v.Id                as VehicleId
                 , v.Model             as VehicleModel
                 , v.PassengerCapacity as VehiclePassengerCapacity
                 , v.Producer          as VehicleProducer
        from TripBus tb
        join Bus b on tb.BusId = b.Id
        join Vehicle v on b.VehicleId = v.Id
        where 1=1
          and tb.TripId = t.Id
          and tb.IsDeleted = 0
        order by tb.DateModified, tb.DateCreated desc
    ) as b
    outer apply
    (
      select top 1 sl.DepartureTime
        from ScheduleLocation sl
        where 1=1
          and sl.ScheduleId = s.Id
          and sl.PreviousLocationId is null
          and sl.IsDeleted = 0
    ) as dt
    cross apply
    (
      select sum(srl.ScheduleLocationDistance) as TotalDistance
        from dbo.GetScheduleRouteLocation(s.Id) srl
    ) as td
    where 1=1
      and t.Id = isnull(@tripId, t.Id)
      and t.Status = isnull(@status, t.Status)
      and
      (
        @withDeleted = 1
        or
        (
              t.IsDeleted = 0
          and s.IsDeleted = 0
          and r.IsDeleted = 0
        )
      )

end;
go

if object_id(N'dbo.GetDistance') is null
  exec('create procedure dbo.GetDistance as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetDistance 1, 2
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

alter procedure dbo.GetDistance
(  
    @location1 as int 
  , @location2 as int
)  
as  
begin  
  
  select d.Id           as Id
       , d.TomTomInfo   as TomTomInfo
       , IsDeleted      as IsDeleted
       , l1.Id          as Location1         
       , l1.FullName    as FirstLocationFullName   
       , l1.Name        as FirstLocationName       
       , l1.Latitude    as FirstLocationLatitude   
       , l1.Longitude   as FirstLocationLongitude  
       , l1.IsCarRepair as FirstLocationIsCarRepair
       , l1.IsParking   as FirstLocationIsParking
       , l2.Id          as Location2         
       , l2.FullName    as SecondLocationFullName   
       , l2.Name        as SecondLocationName       
       , l2.Latitude    as SecondLocationLatitude   
       , l2.Longitude   as SecondLocationLongitude  
       , l2.IsCarRepair as SecondLocationIsCarRepair
       , l2.IsParking   as SecondLocationIsParking
    from Distance d
    cross apply
    (
      select Id          
           , FullName    
           , Name        
           , Latitude    
           , Longitude   
           , IsCarRepair 
           , IsParking
        from dbo.GetLocationInfo(d.Location1)
    ) l1
    cross apply
    (
      select Id          
           , FullName    
           , Name        
           , Latitude    
           , Longitude   
           , IsCarRepair 
           , IsParking  
        from dbo.GetLocationInfo(d.Location2)
    ) l2
    where 1=1
      and d.IsDeleted = 0
      and d.Location1 = @location1
      and d.Location2 = @location2
    order by d.DateModified, d.DateCreated, d.Id

end;
go

if (object_ID('dbo.HasConfirmedTripsByRouteId') is not null)
   drop function dbo.HasConfirmedTripsByRouteId
go

-- ============================================================================
-- Example    : select dbo.HasConfirmedTripsByRouteId(2)
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

create function dbo.HasConfirmedTripsByRouteId(@routeId int)
returns bit
as 
begin 

  if exists
  (
    select 1
      from Route r
      join Schedule s 
        on r.Id = s.RouteId
      join Trip t
        on s.Id = t.ScheduleId
      outer apply
      (
        select top 1 sl.DepartureTime
          from ScheduleLocation sl
          where 1=1
            and sl.ScheduleId = s.Id
            and sl.PreviousLocationId is null
            and sl.IsDeleted = 0
      ) as dt
      where 1=1
        and @routeId = r.Id
        and (
              t.Status in('S', 'D')
              or getdate() > dateadd(hour, -12, (cast(t.Departure as datetime) + cast(dt.DepartureTime as datetime)))
            )
  ) return 1;
  return 0;

end
go

if (object_ID('dbo.IsScheduleMatchRoute') is not null)
   drop function dbo.IsScheduleMatchRoute
go

-- ============================================================================
-- Example    : select dbo.IsScheduleMatchRoute(14)
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: Ч
-- ============================================================================

create function dbo.IsScheduleMatchRoute(@scheduleId int)
returns bit
as 
begin 
  if exists
  (
    select 1 
      from ScheduleLocation s
      full join RouteLocation r
        on s.CurrentLocationId = r.CurrentLocationId
      where s.ScheduleId=@scheduleId
        and s.IsDeleted = 0
        and isnull(r.IsDeleted, 0) = 0
        and (s.Id is null or r.Id is null)
  ) return 0;
  return 1;
end
go