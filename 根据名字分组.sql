 ---------------- sql��� -----------
select * into #222
from (
select '����' as name,'�ӱ�' as [address],'10:00' as arrive_time
union all 
select '����' as name,'�ӱ�2' as [address],'12:00' as arrive_time
union all 
select '����' as name,'����1' as [address],'12:00' as arrive_time
union all 
select '����' as name,'����2' as [address],'12:00' as arrive_time
) as a

select * from #222 

select name, 
      stuff((select','+ convert(varchar(200),address) from #222
            where name=v.name for xml   path('')),1,1,'') as newaddress,       
      max(arrive_time) as maxtime       from #222 v         group by name