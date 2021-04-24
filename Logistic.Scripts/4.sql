use LogisticDatabase; 
go


-- sp_configure 'show advanced options', 1
-- EXEC master.dbo.sp_configure 'xp_cmdshell', 1
-- RECONFIGURE WITH OVERRIDE
-- GO


-- DECLARE @Text AS VARCHAR(100)
-- DECLARE @Cmd AS VARCHAR(100)
-- SET @Text = 'text'
-- SET @Cmd ='echo ' +  @Text + ' > AppTextFile.txt'
-- EXECUTE Master.dbo.xp_CmdShell  @Cmd


declare @modelsUsing varchar(max)= 'using System;';
declare @modelsNamespace varchar(max)= 'Logistic.DAL.Models';
declare @baseModelClass varchar(max)= 'BaseEntity';
declare @ignorableColumns table (ColumnName varchar(max));

declare @daosUsing varchar(max)= 'using System.Data;';
declare @daosNamespace varchar(max)= 'Logistic.DAL.Dao';
declare @baseDaoClass varchar(max)= 'BaseDao';

insert into @ignorableColumns 
values ('Id'),('DateCreated'),('DateModified'),('IsDeleted'),('CreatedBy'),('ModifiedBy');

select concat(tab.name, '.cs') as EntityFileName
     , concat(@modelsUsing, '
              namespace ', @modelsNamespace, '
              {', 
              '
              public class ', tab.name, iif(@baseModelClass is not null, ' : ' + @baseModelClass, ''), '
              ', 
              '{
              ',
              (
                  select top 1 string_agg(concat('public ', t.ColumnType, t.NullableSign, ' ', t.ColumnName, ' { get; set; }'), char(13))
                  from
                  (
                      select REPLACE(col.name, ' ', '_') ColumnName, 
                             column_id ColumnId,
                             case typ.name
                                 when'bigint'then'long'when'binary'then'byte[]'when'bit'then'bool'when'char'then'string'when'date'then'DateTime'
                                 when'datetime'then'DateTime'when'datetime2'then'DateTime'when'datetimeoffset'then'DateTimeOffset'when'decimal'
                                 then 'decimal'when'float'then'float'when'image'then'byte[]'when'int'then'int'when'money'then'decimal'when'nchar'
                                 then 'string'when'ntext'then'string'when'numeric'then'decimal'when'nvarchar'then'string'when'real'then'double'
                                 when 'smalldatetime'then'DateTime'when'smallint'then'short'when'smallmoney'then'decimal'when'text'then'string'
                                 when 'time'then'TimeSpan'when'timestamp'then'DateTime'when'tinyint'then'byte'when'uniqueidentifier'then'Guid'
                                 when 'varbinary'then'byte[]'when'varchar'then'string'else'UNKNOWN_'+typ.name
                             end ColumnType,
                             case when col.is_nullable = 1 and typ.name in('bigint','bit','date','datetime','datetime2','datetimeoffset','decimal',
                             'float','int','money','numeric','real','smalldatetime','smallint','smallmoney','time','tinyint','uniqueidentifier')
                                 then '?'
                                 else ''
                             end NullableSign
                      from sys.columns col
                           join sys.types typ on col.system_type_id = typ.system_type_id
                                                 and col.user_type_id = typ.user_type_id
                      where tab.object_id = col.object_id
                            and not exists (select 1 from @ignorableColumns where ColumnName = col.name)
                  ) t
              ),
              char(13), 
              '}
              ',
              char(13), 
              '}
              ') as EntityContent
     , concat(tab.name, 'Dao', '.cs') as DaoFileName
     , concat('using ', @modelsNamespace, ';
              ', @daosUsing, '
              namespace ', @daosNamespace, '
              {', 
              '
              public class ', tab.name, 'Dao', iif(@baseDaoClass is not null, ' : ' + @baseDaoClass + '<' + tab.name + '>', ''), '
              ', 
              '{
              ',
              'public ', tab.name, 'Dao(IDbConnection connection) : base("', sch.name, '.', tab.name,'", connection) { }
              }', '
              }') as DaoContent
from sys.tables tab
join sys.schemas sch
on tab.schema_id = sch.schema_id
order by tab.name
go