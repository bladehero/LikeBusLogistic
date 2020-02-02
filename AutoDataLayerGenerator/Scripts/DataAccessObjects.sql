declare @modelsNamespace varchar(max)= '{{Namespace}}';
declare @daosUsing varchar(max)= '{{Using}}';
declare @daosNamespace varchar(max)= '{{ModelsNamespace}}';
declare @baseDaoClass varchar(max)= '{{BaseClass}}';
declare @lineBreak varchar(2) = char(13)+char(10);

select concat(tab.name, 'Dao', '.cs') as [File]
     , concat('using ', @modelsNamespace, ';
              ', @daosUsing, @lineBreak, '
              namespace ', @daosNamespace, '
              {', 
              '
              public class ', tab.name, 'Dao', iif(@baseDaoClass is not null, ' : ' + @baseDaoClass + '<' + tab.name + '>', ''), '
              ', 
              '{
              ',
              'public ', tab.name, 'Dao(IDbConnection connection) : base("', sch.name, '.', tab.name,'", connection) { }
              }', '
              }') as Content
from sys.tables tab
join sys.schemas sch
on tab.schema_id = sch.schema_id
order by tab.name