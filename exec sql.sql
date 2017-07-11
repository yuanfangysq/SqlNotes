
---1---
create proc p1 (@string varchar(100))
as
begin
declare @qt varchar(200)
set @qt =
'select c.companyname, o.orderid
from  orders o
inner join customers c
on c.customerid = o.customerid
where c.companyname like'+ ' ''' +@string + '%' + ''''

print(@qt)
exec (@qt)
end

exec p1 'fr'

results 24 rows. 
companyname                              orderid
---------------------------------------- -----------
france restauration                      10671
france restauration                      10860
france restauration                      10971


---2---

--return order details of product by with orderid, product id or both. 

create proc p2(@orderid int = null, @productid int = null)
as
declare @sql1 varchar(max)
declare @sql2 varchar(max)
declare @sql  varchar(max)

set @sql =
'select p.productname [product name],od.quantity [quantity] ,o.orderdate 
from [order details] od 
inner join products p
on od.productid = p.productid
inner join orders o
on od.orderid= o.orderid  '

if @orderid is not null and @productid is null
set @sql2 = 'where o.orderid = ' + convert(varchar(20),@orderid)
else if @orderid is null and @productid is not null
set @sql2 = 'where p.productid = ' + cast(@productid as varchar(20))
else if @orderid is not null and @productid is not null
set @sql2 = ' where o.orderid = '+ convert(varchar(20),@orderid) + 
            ' and ' + 'p.productid = ' + convert(varchar(20),@productid)
else 
print 'give at least one integer input'

set @sql1 = @sql + @sql2 

print(@sql1)
exec (@sql1)
return 0

exec p2 10248,null


