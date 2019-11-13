use LikeBusLogisticDatabase;
go

merge dbo.[Role] as trg
using
(
  select r.Name
  from
  (
    values ('Administrator')
         , ('Moderator')
         , ('Operator')
  ) as r(Name)
) as src
on trg.Name = src.Name
  when not matched then
    insert (Name)
    values (src.Name)
  ;
go

merge dbo.[User] as trg
using
(
  select u.FirstName
       , u.Phone
  from
  (
    values ('Администратор', '+380949466705')
  ) as u(FirstName, Phone)
) as src
on trg.FirstName = src.FirstName and trg.Phone = src.Phone
  when not matched then
    insert (FirstName, Phone)
    values (src.FirstName, src.Phone)
  ;
go

merge dbo.Account as trg
using
(
  select r.Id as RoleId
       , u.Id as UserId
       , a.Login as Login
       , dbo.MD5HashPassword(a.Password) as Password
  from
  (
    values ('admin', 'p@ssw0rd321')
  ) as a(Login, Password)
  cross apply
  (
    select Id from [Role] where Name = 'Administrator'
  ) as r
  cross apply
  (
    select top 1 Id from [User] where FirstName = 'Администратор' and Phone = '+380949466705'
  ) as u
) as src
on trg.RoleId = src.RoleId and trg.UserId = src.UserId and trg.Login = src.Login
  when not matched then
    insert (RoleId, UserId, Login, Password)
    values (src.RoleId, src.UserId, src.Login, src.Password)
  ;
go