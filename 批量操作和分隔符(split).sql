

drop function split2
create function [dbo].[split2](@list nvarchar(2000),@list2 nvarchar(2000),@spliton nvarchar(5))  
returns @rtnvalue table 
(id int identity(1,1),value nvarchar(100),value2 nvarchar(50)
) 
as  
begin
while (charindex(@spliton,@list)>0)
begin
insert into @rtnvalue (value,value2)
select 
    value = ltrim(rtrim(substring(@list,1,charindex(@spliton,@list)-1))),
	value2 = ltrim(rtrim(substring(@list2,1,charindex(@spliton,@list2)-1)))

set @list = substring(@list,charindex(@spliton,@list)+len(@spliton),len(@list))
set @list2 = substring(@list2,charindex(@spliton,@list2)+len(@spliton),len(@list2))
end

--ysq ������滹д���ָ��� ���������Բ��� ����(selectvalue,value2 from  split2('1@2@310@','2@3@4������ǲ���@','@'))
--ysq ������治д�ָ����Ļ�������ȡ��ע�;���(selectvalue,value2 from  split2('1@2@310','2@3@4������ǲ���','@')) ������䶼�ǵȼ۵�
/*insert into @rtnvalue (value,value2) --������һ��
    select value = ltrim(rtrim(@list)),
	       value2 = ltrim(rtrim(@list2))*/

    return
end

select * from split2('1@2@3@','2@3@4@','@')

select * from t1

-- insert
insert into  t1(id,mark)  
   select
   value,value2
from split2('1@2@310@','2@3@4�������ʧ��@','@')

-- update

update t1 set   mark=a.value from
   (
     select value
     from  split2('1@2@3','2@3@4','@')  
   ) a

where a.value=id

-- delete 

delete t1  from 
   (
     select value
     from  split2('1@2@3','2@3@4','@')  
   ) a

where id=a.value



