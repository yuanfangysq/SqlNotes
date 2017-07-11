----2

create function returnlast (@companyname varchar(200))
returns table
return  (select e.firstname,e.lastname 
from dbo.orders o
inner join dbo.employees e
on o.employeeid =o.employeeid
inner join dbo.customers c
on c.customerid = o.customerid
where c.companyname = @companyname)

select * from  returnlast('quick-stop')

----3
alter function next_class(@month varchar(15),@year varchar(10))
returns @table table (employeeid int, hiredate date, lastname varchar(100), firstname varchar(100),emailaddress varchar(200))
as
begin
if  (@month) = 2 or (@month) = 8
insert into @table
select hre.employeeid,hre.hiredate,pc.lastname, pc.firstname, pc.emailaddress
from humanresources.employee hre
inner join person.contact pc
on hre.contactid = pc.contactid
where @year-year(hre.hiredate) < = 2
return
end

select * from next_class(5,2003)

alter function dbo.distance(@slat decimal(10,1),@slon decimal(10,1), @plat decimal(10,1) ,@plon decimal(10,1))
returns decimal(10,1) 
as
begin
declare @distance decimal(10,1)
set @distance = 
 sqrt( ((69.17 * (@slat - @plat) )

* (69.17 * (@slat - @plat))

+ ( 57.56 * (@slon - @plon) )

* ( 57.56 * (@slon - @plon)))) 
return @distance
end
--34.0664 and longitude 118.3103 and p at latitude 33.6784 and longitude 118.0054, 
select dbo.distance(34.0664,118.3103,33.6784,111.0054) distance