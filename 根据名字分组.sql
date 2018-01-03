 ---------------- sql语句 -----------
select * into #222
from (
select '张三' as name,'河北' as [address],'10:00' as arrive_time
union all 
select '张三' as name,'河北2' as [address],'12:00' as arrive_time
union all 
select '李四' as name,'江西1' as [address],'12:00' as arrive_time
union all 
select '李四' as name,'江西2' as [address],'12:00' as arrive_time
) as a

select * from #222 

select name, 
      stuff((select','+ convert(varchar(200),address) from #222
            where name=v.name for xml   path('')),1,1,'') as newaddress,       
      max(arrive_time) as maxtime       from #222 v         group by name