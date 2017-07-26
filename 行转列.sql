ALTER proc [dbo].[hzl]
as
IF object_id('tempdb..#t_hzl') is not null
 begin
    drop table #t_hzl
 end

 
create table #t_hzl
(
  id int,mark varchar(20),
  id1 int,mark1 varchar(20),
  id2 int,mark2 varchar(20),
)

declare @count int
declare @i int=1

set @count=(select count(id) from t2)

while @count>@i
begin
insert #t_hzl  

--declare @i int=1
select *  from 
(
	select a.id,a.mark from   (select id ,mark ,row_number() over( order by id desc) as num from t2   )as a 
	where a.num=@i 
) as a
outer apply
(
	select a.id,a.mark from   (select id ,mark  ,row_number() over( order by id desc) as num from t2   )as a 
	where	a.num=@i+1
) as b 
outer apply
(
	select a.id,a.mark from   (select id ,mark  ,row_number() over( order by id desc) as num from t2   )as a 
	where a.num=@i+2
) as c
set @i=@i+3

end

select * from #t_hzl

drop table #t_hzl
