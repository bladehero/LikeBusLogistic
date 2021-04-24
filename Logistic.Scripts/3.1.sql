use LogisticDatabase;
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
    values ('Administrator', '+1234567890')
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
    select top 1 Id from [User] where FirstName = N'Administrator' and Phone = '+1234567890'
  ) as u
) as src
on trg.Login = src.Login
  when not matched then
    insert (RoleId, UserId, Login, Password)
    values (src.RoleId, src.UserId, src.Login, src.Password)
  ;
go

create table #driverInfo
(
  FirstName	 nvarchar (200),
  LastName	 nvarchar (200),
  MiddleName nvarchar (200),
  Contact    nvarchar (100)
);
go

insert into #driverInfo
select s.*
    from
    ( 
      values (N'Jack'   , N'Rodriguez' , N'', N'+1234567891')
           , (N'William', N'Jones'     , N'', N'+1234567892')
           , (N'Daniel' , N'Brown'     , N'', N'+1234567893')
           , (N'James'  , N'Williams'  , N'', N'+1234567894')
           , (N'Joshua' , N'Johnson'   , N'', N'+1234567895')
           , (N'Joseph' , N'Davis'     , N'', N'+1234567896')
           , (N'Samuel' , N'Smith'     , N'', N'+1234567897')
    ) as s(FirstName, LastName, MiddleName, Contact)
;
go

merge into Driver as trg
using
(
  select di.FirstName
       , di.LastName
       , di.MiddleName
    from #driverInfo di
) src
on src.FirstName = trg.FirstName and src.LastName = trg.LastName and src.MiddleName = trg.MiddleName
  when not matched then
    insert(FirstName, LastName, MiddleName)
    values(src.FirstName, src.LastName, src.MiddleName)
    ;
go

merge into DriverContact as trg
using
(
  select di.Contact,
         d.Id as DriverId
    from #driverInfo di
    join Driver d 
      on di.FirstName = d.FirstName 
      and di.LastName = d.LastName 
      and di.MiddleName = d.MiddleName
) src
on src.DriverId = trg.DriverId and src.Contact = trg.Contact
  when not matched then
    insert(DriverId, Contact)
    values(src.DriverId, src.Contact)
    ;
go

create table #busInfo
(
  Producer	        nvarchar(200),
  Model	            nvarchar(200),
  PassengerCapacity int          ,
  Description       nvarchar(100),
  Number            nvarchar(8)  ,
  CrewCapacity      int
);
go

insert into #busInfo
select s.*
    from
    ( 
      values (N'Hyundai'      , N'H350'            , 14, N'', N'11234', 2)
           , (N'MAN'          , N'Lion’s Intercity', 32, N'', N'25432', 2)
           , (N'Mercedes-Benz', N'Sprinter II'     , 16, N'', N'35674', 2)
           , (N'Mercedes-Benz', N'Classic'         , 12, N'', N'48683', 2)
           , (N'MAN'          , N'Lion’s Coach'    , 44, N'', N'50959', 2)
           , (N'Hyundai'      , N'H350'            , 14, N'', N'68567', 2)
           , (N'MAN'          , N'Lion’s Intercity', 32, N'', N'79393', 2)
           , (N'Mercedes-Benz', N'Sprinter II'     , 16, N'', N'81238', 2)
           , (N'Mercedes-Benz', N'Classic'         , 12, N'', N'91200', 2)
           , (N'MAN'          , N'Lion’s Coach'    , 44, N'', N'00321', 2)
           , (N'Hyundai'      , N'H350'            , 14, N'', N'13543', 2)
           , (N'MAN'          , N'Lion’s Intercity', 32, N'', N'24334', 2)
           , (N'Mercedes-Benz', N'Sprinter II'     , 16, N'', N'30324', 2)
           , (N'Mercedes-Benz', N'Classic'         , 12, N'', N'48653', 2)
    ) as s(Producer, Model, PassengerCapacity, Description, Number, CrewCapacity)
;
go

merge into Vehicle as trg
using
(
  select distinct bi.Producer
                , bi.Model
                , bi.PassengerCapacity
                , bi.Description
    from #busInfo bi
) src
on trg.Producer = src.Producer and trg.Model = src.Model
  when not matched then
    insert(Producer, Model, PassengerCapacity, Description)
    values(src.Producer, src.Model, src.PassengerCapacity, src.Description)
    ;
go

merge into Bus as trg
using
(
  select v.Id as VehicleId
       , bi.Number
       , bi.CrewCapacity
    from #busInfo bi
    join Vehicle v
      on bi.Producer = v.Producer and bi.Model = v.Model
) src
on trg.Number = src.Number
  when not matched then
    insert(VehicleId, Number, CrewCapacity)
    values(src.VehicleId, src.Number, src.CrewCapacity)
    ;
go

drop table #busInfo;
go

drop table #driverInfo;
go

declare @BusCount int; 
select @BusCount = count(1) from Bus;
declare @DriverCount int; 
select @DriverCount = count(1) from Driver;

merge into BusDriver as trg
using
(
  select b.Id as BusId
       , d.Id as DriverId
    from Bus b
    cross join Driver d
      where b.Id % @BusCount = d.Id % @DriverCount
) src
on trg.BusId = src.BusId and trg.DriverId = src.DriverId
  when not matched then
    insert(BusId, DriverId)
    values(src.BusId, src.DriverId)
    ;
go

create table #locationInfo
(
  City nvarchar(100),
  District nvarchar(100),
  Country nvarchar(100),
  ShortCountryName char(2),
  Latitude float,
  Longitude float
);
go    
      
insert into #locationInfo
select rtrim(ltrim(s.City)) as City
     , null as District
     , rtrim(ltrim(s.Country)) as County
     , s.ShortCountryName as ShortCountryName
     , s.Latitude as Latitude
     , s.Longitude as Longitude
  from
  (
    values  ('LV', N'Latvia', N'Riga      ', 57.143620, 24.099277)
          , ('LV', N'Latvia', N'Daugavpils', 55.960565, 26.531722)
          , ('LV', N'Latvia', N'Jurmala   ', 57.132767, 23.632446)
          , ('LV', N'Latvia', N'Ventspils ', 57.394176, 21.556030)
          , ('LV', N'Latvia', N'Liepaja   ', 56.525670, 20.995727)
          , ('LV', N'Latvia', N'Jelgava   ', 56.652709, 23.720336)
                   
          , ('LT', N'Lithuania', N'Vilnius ', 54.689186, 25.269408)
          , ('LT', N'Lithuania', N'Klaipeda', 55.711129, 21.116576)
          , ('LT', N'Lithuania', N'Kaunas  ', 54.904519, 23.896117)
          , ('LT', N'Lithuania', N'Alytus  ', 54.402418, 24.027953)
                   
          , ('BY', N'Belarus', N'Minsk  ', 53.926452, 27.543578)
          , ('BY', N'Belarus', N'Gomel  ', 52.465498, 30.949340)
          , ('BY', N'Belarus', N'Zhlobin', 52.905042, 29.960571)
          , ('BY', N'Belarus', N'Vitebsk', 55.200301, 30.158324)
          , ('BY', N'Belarus', N'Brest  ', 52.116069, 23.764281)
                   
          , ('CZ', N'Czech', N'Prague      ', 50.068723, 14.406474)
          , ('CZ', N'Czech', N'Brno        ', 49.200676, 16.592753)
          , ('CZ', N'Czech', N'Ostrava     ', 49.824819, 18.246196)
          , ('CZ', N'Czech', N'Karlovy Vary', 50.241180, 12.862895)
          , ('CZ', N'Czech', N'Pardubice   ', 50.042271, 15.771525)
                   
          , ('DE', N'Germany', N'Munich     ', 48.131858, 11.517697)
          , ('DE', N'Germany', N'Nuremberg  ', 49.477622, 11.034298)
          , ('DE', N'Germany', N'Stuttgart  ', 48.802019, 9.144650 )
          , ('DE', N'Germany', N'Frankfurt  ', 50.158112, 8.683224 )
          , ('DE', N'Germany', N'Koln       ', 50.939950, 6.947384 )
          , ('DE', N'Germany', N'Dusseldorf ', 51.257312, 6.749630 )
          , ('DE', N'Germany', N'Leipzig    ', 51.339744, 12.330685)
                   
          , ('PO', N'Poland', N'Warsaw ', 52.230023, 21.020870)
          , ('PO', N'Poland', N'Krakow ', 50.066522, 19.944210)
          , ('PO', N'Poland', N'Lublin ', 51.264187, 22.580929)
          , ('PO', N'Poland', N'Gdansk ', 54.357071, 18.625851)
                   
          , ('HU', N'Hungary', N'Budapest', 47.512233, 19.032345)
                   
          , ('AT', N'Austria', N'Vein     ', 48.212452, 16.340695)
          , ('AT', N'Austria', N'Graz     ', 47.080077, 15.428829)
          , ('AT', N'Austria', N'Salzburg ', 47.808215, 13.044796)
                   
          , ('NL', N'Netherlands', N'Amsterdam', 52.377816, 4.881954)
          , ('NL', N'Netherlands', N'Rotterdam', 51.946503, 4.475460)
                   
          , ('BE', N'Belgium', N'Brussels ', 50.849865, 4.343624)
          , ('BE', N'Belgium', N'Antwerp  ', 51.222921, 4.376583)
          , ('BE', N'Belgium', N'Ghent    ', 51.064394, 3.684445)
          , ('BE', N'Belgium', N'Charleroi', 50.407331, 4.437008)
                   
          , ('CH', N'Switzerland', N'Zurich ', 47.365705, 8.522735)
          , ('CH', N'Switzerland', N'Berne  ', 46.958599, 7.429596)
          , ('CH', N'Switzerland', N'Lucerne', 47.063474, 8.270050)
          , ('CH', N'Switzerland', N'Geneva ', 46.211099, 6.138702)
                   
          , ('FR', N'France', N'Paris      ', 48.855007, 2.317929 )
          , ('FR', N'France', N'Lyon       ', 45.772557, 4.800839 )
          , ('FR', N'France', N'Nantes     ', 47.231930, -1.571231)
          , ('FR', N'France', N'Rouen      ', 49.451392, 1.076474 )
          , ('FR', N'France', N'Bourges    ', 47.082518, 2.372861 )
          , ('FR', N'France', N'Toulouse   ', 43.601532, 1.417050 )
          , ('FR', N'France', N'Grenoble   ', 45.187123, 5.679745 )
          , ('FR', N'France', N'Marseilles ', 43.330427, 5.328183 )
          , ('FR', N'France', N'Montpellier', 43.633347, 3.856015 )
                   
          , ('ES', N'Spain', N'Barcelona', 41.388420, 2.173207 )
          , ('ES', N'Spain', N'Madrid   ', 40.433641, -3.734691)
          , ('ES', N'Spain', N'Valencia ', 39.465109, -0.394847)
          , ('ES', N'Spain', N'Seville  ', 37.410464, -6.045607)
          , ('ES', N'Spain', N'Zaragoza ', 41.668059, -0.922191)
          , ('ES', N'Spain', N'Murcia   ', 38.008357, -1.152904)
          , ('ES', N'Spain', N'Malaga   ', 36.744322, -4.485548)
          , ('ES', N'Spain', N'Bilbao   ', 43.266148, -2.947462)
          , ('ES', N'Spain', N'Alicante ', 38.359902, -0.497511)
          , ('ES', N'Spain', N'Benidorm ', 38.553472, -0.129469)
          , ('ES', N'Spain', N'Tarragona', 41.113443, 1.232835 )
          , ('ES', N'Spain', N'Almeria  ', 36.841099, -2.486037)
          , ('ES', N'Spain', N'Marbella ', 36.515090, -4.886549)
                   
          , ('PT', N'Portugal', N'Lisbon ', 38.725650, -9.150795)
          , ('PT', N'Portugal', N'Porto  ', 41.163619, -8.650917)
          , ('PT', N'Portugal', N'Coimbra', 40.230745, -8.420204)
                   
          , ('DK', N'Denmark', N'Copenhagen', 55.675614, 12.552697)
          , ('DK', N'Denmark', N'Aalborg   ', 57.056757, 9.894005 )

          , ('SE', N'Sweden', N'Stockholm ', 59.338428, 18.051912)
          , ('SE', N'Sweden', N'Gothenburg', 57.733103, 11.943514)
          , ('SE', N'Sweden', N'Jönköping ', 57.872203, 14.155923)
                   
          , ('FI', N'Finland', N'Helsinki', 60.193915, 24.918123)
          , ('FI', N'Finland', N'Turku   ', 60.465828, 22.215486)
                   
          , ('NO', N'Norway', N'Oslo      ', 59.932920, 10.708903)
          , ('NO', N'Norway', N'Bergen    ', 60.384723, 5.332098 )
          , ('NO', N'Norway', N'Stravanger', 58.966722, 5.734212 )
                   
          , ('IT', N'Italy', N'Rome     ', 41.895208, 12.442955)
          , ('IT', N'Italy', N'Florence ', 43.795964, 11.190513)
          , ('IT', N'Italy', N'Milan    ', 45.468880, 9.125083 )
          , ('IT', N'Italy', N'Venice   ', 45.438475, 12.322449)
          , ('IT', N'Italy', N'Palermo  ', 38.112222, 13.355383)
          , ('IT', N'Italy', N'Genoa    ', 44.425465, 8.930420 )
          , ('IT', N'Italy', N'Naples   ', 40.850719, 14.264282)
          , ('IT', N'Italy', N'Bologna  ', 44.499955, 11.336426)
          , ('IT', N'Italy', N'Turin    ', 45.080815, 7.656006 )
          , ('IT', N'Italy', N'Padua    ', 45.409559, 11.863769)
          , ('IT', N'Italy', N'Bari     ', 41.124389, 16.862549)
          , ('IT', N'Italy', N'Verona   ', 45.444256, 10.979370)
          , ('IT', N'Italy', N'Lecce    ', 40.350561, 18.166181)
          , ('IT', N'Italy', N'Sorrento ', 40.654720, 14.383185)
                   
          , ('GR', N'Greece', N'Athens      ', 37.980574, 23.698418)
          , ('GR', N'Greece', N'Thessaloniki', 40.665637, 22.929375)
                   
          , ('SK', N'Slovakia', N'Kosice    ', 48.734091, 21.251214)
          , ('SK', N'Slovakia', N'Bratislava', 48.151059, 17.098382)
                   
          , ('SI', N'Slovenia', N'Ljubljana', 46.057601, 14.500115)
          , ('SI', N'Slovenia', N'Maribor  ', 46.560523, 15.656177)
                                
          , ('HR', N'Croatia', N'Zagreb   ', 45.810481, 15.985473)
          , ('HR', N'Croatia', N'Dubrovnik', 42.659071, 18.078369)
          , ('HR', N'Croatia', N'Split    ', 43.513562, 16.435913)
          , ('HR', N'Croatia', N'Zadar    ', 44.127876, 15.221924)
                   
          , ('ME', N'Montenegro', N'Podgorica', 42.447562, 19.247324)
                   
          , ('RO', N'Romania', N'Bucharest ', 44.449745, 26.100046)
          , ('RO', N'Romania', N'Cluj     ', 46.785282, 23.562204)
          , ('RO', N'Romania', N'Constanta', 44.158842, 28.593943)
                   
          , ('BG', N'Bulgaria', N'Sofia         ', 42.706945, 23.309519)
          , ('BG', N'Bulgaria', N'Varna         ', 43.229478, 27.879831)
          , ('BG', N'Bulgaria', N'Plovdiv       ', 42.122961, 24.715769)
          , ('BG', N'Bulgaria', N'Veliko Tarnovo', 43.077197, 25.594675)
  ) as s(ShortCountryName, Country, City, Latitude, Longitude)
;      
go

merge into Country trg
using
(
  select distinct Country as Name
                , ShortCountryName as ShortName
    from #locationInfo
) src
on src.Name = trg.Name
  when not matched then
    insert (Name, ShortName)
    values (src.Name, src.ShortName)
;
go

merge into District trg
using
(
  select distinct li.District as Name
                , c.Id as CountryId
    from #locationInfo li
    join Country c on li.Country = c.Name
    where li.District is not null
) src
on src.Name = trg.Name and src.CountryId = trg.CountryId
  when not matched then
    insert (Name, CountryId)
    values (src.Name, src.CountryId)
;
go

merge into City trg
using
(
  select li.City as Name
       , d.Id as DistrictId
    from #locationInfo li
    join District d on d.Name = li.District
    where li.District is not null
) src
on src.Name = trg.Name and src.DistrictId = trg.DistrictId
  when not matched then
    insert (Name, DistrictId)
    values (src.Name, src.DistrictId)
;
go

merge into Location trg
using
(
  select c.Id as CountryId
       , d.Id as DistrictId
       , ci.Id as CityId
       , li.City as Name
       , li.Latitude as Latitude
       , li.Longitude as Longitude
       , crypt_gen_random(1) % 2 as IsParking -- random value 
    from #locationInfo li
    left join Country c 
      on li.Country = c.Name
    left join District d
      on li.District = d.Name and c.Id = d.CountryId
    left join City ci 
      on li.City = ci.Name and d.Id = ci.DistrictId
) src
on src.CountryId = trg.CountryId and src.DistrictId = trg.DistrictId and src.CityId = trg.CityId and src.Name = trg.Name 
  when not matched then
    insert (CountryId, DistrictId, CityId, Name, Latitude, Longitude, IsParking)
    values (src.CountryId, src.DistrictId, src.CityId, src.Name, src.Latitude, src.Longitude, src.IsParking)
;
go

drop table #locationInfo;
go