select temp.salesorderid,temp.totaldue from
 (select rank() over  (order by totaldue desc) as 'rank', * from sales.salesorderheader) temp
where temp.rank = 3



select temp.salesorderid,temp.totaldue from
 (select rank() over (order by totaldue desc) as 'rank', * from sales.salesorderheader) temp
where temp.rank = 3

/*
temporary tables
*/
select top 1 with ties rank() over (order by totaldue desc) as 'rank', * from sales.salesorderheader
order by totaldue

select top 10 with ties rank() over (order by eid ) as 'rank', * from address
order by eid





create table #employee
(
id int,
name varchar(10)
)

insert into #employee values (1,'a'),(2,'b'),(3,'c')

select * from #employee

--scope: bound to a session of the user who created it

--- creating global temporary table


create table 
id int,
name varchar(10)
)

insert into ##employee values (1,'a'),(2,'b'),(3,'c')

create view v_sheme with schemabinding as
(select id,name from dbo.v_test)

select * from v_sheme

alter table v_test
drop column id

drop view v_sheme
drop table v_test


use adventureworks;
go
select i.productid, p.name, i.locationid, i.quantity
    ,rank() over 
    (partition by i.locationid order by i.quantity desc) as 'rank'
from production.productinventory i 
    inner join production.product p 
        on i.productid = p.productid
order by p.name;
go


--select *,rank() over(partition by  state order by eid) as 'rank' from address

--select *,rank() over( order by eid) as 'rank' from address