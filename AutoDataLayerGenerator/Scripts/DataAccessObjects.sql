declare @modelsNamespace varchar(max)= '{{Namespace}}';
declare @daosUsing varchar(max)= '{{Using}}';
declare @daosNamespace varchar(max)= '{{ModelsNamespace}}';
declare @baseDaoClass varchar(max)= '{{BaseClass}}';
declare @lineBreak varchar(2) = char(13)+char(10);
declare @tabSpace varchar(1) = char(9);

select concat(tab.name, 'Dao', '.cs') as [File]
     , concat('using ', @modelsNamespace, ';', @lineBreak, @daosUsing, @lineBreak, 'namespace ', @daosNamespace, 
              @lineBreak, @lineBreak, '{', @lineBreak, @tabSpace, 'public class ', tab.name, 'Dao',
              iif(@baseDaoClass is not null, ' : ' + @baseDaoClass + '<' + tab.name + '>', ''), @lineBreak, 
              @tabSpace, '{', @lineBreak, @tabSpace, @tabSpace, 'public ', tab.name, 
              'Dao(IDbConnection connection) : base("', sch.name, '.', tab.name,'", connection) { }',
              @lineBreak, @tabSpace, '}',@lineBreak, '}') as Content
    from sys.tables tab
    join sys.schemas sch
    on tab.schema_id = sch.schema_id
    order by tab.name