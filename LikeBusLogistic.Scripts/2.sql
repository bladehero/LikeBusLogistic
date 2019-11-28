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
    @driverId as int = null
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
    ) as bi
    where 1=1
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