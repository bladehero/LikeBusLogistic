declare @using varchar(max)= 'using System;';
declare @namespace varchar(max)= 'LikeBusLogistic.DAL.Models';
declare @ignorableColumns table (ColumnName varchar(max))

insert into @ignorableColumns 
values ('Id'),('DateCreated'),('DateModified'),('IsDeleted'),('CreatedBy'),('ModifiedBy');

select concat(tab.name, '.cs') as FileName
     , concat(@using, '
     namespace ', @namespace, '
     {', 
     '
     public class ', tab.name, '
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
char(13)+char(10), '}
',
char(13)+char(10), '}
')
from sys.tables tab
order by tab.name
go