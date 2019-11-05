-- Stored Procedure's example:
-- if object_id(N'dbo.StoreProcedureName') is null
--   exec('create procedure dbo.StoreProcedureName as set nocount on;');
-- go
-- 
-- -- ============================================================================
-- -- Example    : exec dbo.StoreProcedureName
-- -- Author     : Nikita Dermenzhi
-- -- Date       : 25/07/2019
-- -- Description: —
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
-- -- Description: —
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
-- Description: —
-- ============================================================================

create function dbo.MD5HashPassword(@password nvarchar(100))
returns char(32)
as 
begin 
  return convert(varchar(32), hashbytes('MD5', @password + 'L1k3Bu$L0gi$tiC'), 2) 
end
go

if object_id(N'dbo.GetUserAccount') is null
  exec('create procedure dbo.GetUserAccount as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetUserAccount 'a', 'a'
-- Author     : Nikita Dermenzhi
-- Date       : 25/07/2019
-- Description: —
-- ============================================================================

alter procedure dbo.GetUserAccount
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

end;
go