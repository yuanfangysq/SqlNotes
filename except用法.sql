use northwind

--1-----
select o.orderid 
from orders o
inner join customers c
on c.customerid = o.customerid
where c.companyname = 'island trading'

--results--row=10


  --2---
select orderid 
from orders
where customerid in
(
 select customerid
 from customers
 where companyname = 'island trading'
 )
 
 --results--row=10
 
 ---3---
 select o.productid,p.productname,sum(o.quantity) [total sold]
 from products p
 inner join [order details] o
 on o.productid = p.productid
 group by o.productid,p.productname
 having sum(o.quantity) < 100
 
 ---results--row = 1 
 
 ---4---
 select distinct e.lastname,e.firstname
 from employees e
 inner join orders o
 on e.employeeid = o.employeeid
 inner join customers c
 on c.customerid = o.customerid
 where c.companyname = 'island trading'
 
----results -- row = 7. 

---5 ----
select lastname,firstname
from employees 
where employeeid in 
 (select employeeid 
  from orders
  where customerid in
     (select customerid 
      from customers 
      where companyname = 'island trading'
      )
      )
---results--- row = 7

---6----
select distinct e.lastname, e.firstname
from employees e
left outer join orders
on e.employeeid = orders.employeeid
and
orderdate   
between 'march 1,1997' and 'march 7,1997'
where orders.employeeid is null
--- results -- row 3

---7---
select lastname,firstname
from employees
where employeeid not in 
 (select  employeeid 
  from orders
  where orderdate between 'march 1,1997' and 'march 7,1997')
---results row 3

---8---

select distinct companyname,contactname,city,region,postalcode,country,phone
from customers c
inner join orders o
on c.customerid = o.customerid
where o.orderid in (
select o.orderid  from [order details] od
inner join orders o
on od.orderid = o.orderid
where o.orderid in
 (
select orderid from [order details] od
 inner join products p 
 on od.productid = p.productid
 where p.productname = 'tofu'))
 union  
select companyname,contactname,city,region,postalcode,country,phone
from suppliers s
inner join products p
on s.supplierid = p.supplierid
where p.productname = 'tofu'

--results row = 19
 
 ---9----

use adventureworks
select hre.employeeid, hre.nationalidnumber,hre.title,pc.firstname,pc.middlename,pc.lastname
from humanresources.employee hre
inner join person.contact pc
on hre.contactid = pc.contactid
where pc.title is not null

---results rows = 8

---10----

select hre.employeeid, hre.nationalidnumber,hre.title,pc.firstname,pc.middlename,pc.lastname
from humanresources.employee hre
inner join person.contact pc
on hre.contactid = pc.contactid
where hre.employeeid in
(select managerid from humanresources.employee where managerid is not null)
--results rows = 47 

---11---

select  p.city , p.postalcode 
from person.address p
inner join humanresources.employeeaddress hre
on hre.addressid = p.addressid

intersect 

select p.city, p.postalcode
from person.address p
inner join sales.customeraddress s
on s.addressid = p.addressid

---results rows 23

---12---

select top 1 with ties ssd.salesorderid, ssd.productid,pp.name,ssh.orderdate
from sales.salesorderdetail ssd	
inner join sales.salesorderheader ssh
on ssd.salesorderid = ssh.salesorderid
inner join production.product pp
on ssd.productid = pp.productid
order by ssh.orderdate desc

---results-- rows: 96

---13----

select   temp.productid,temp.name 
from 
(select top 1 with ties pp.productid,pp.name,ssh.orderdate
from sales.salesorderdetail ssd	
inner join sales.salesorderheader ssh
on ssd.salesorderid = ssh.salesorderid
inner join production.product pp
on ssd.productid = pp.productid
order by year (ssh.orderdate) desc  ) temp
except
select pp.productid,pp.name 
from production.product pp
inner join production.productreview ppr
on ppr.productid = pp.productid

---results rows:181 

---14---
select salesorderid ,customerid from 
(select  top 1 with ties sso.salesorderid,sso.billtoaddressid,sca.customerid,sca.addressid,sso.orderdate
from sales.salesorderheader sso
inner join sales.customeraddress sca
on sso.customerid = sca.customerid
order by year(sso.orderdate) desc
) temp
where temp.billtoaddressid != temp.addressid
select top 1 * from sales.salesorderheader
--results : rows: 25. 

---15---

select p.name [product name],v.name [vendor name]  
from  purchasing.productvendor ppv
inner join purchasing.vendor v
on ppv.vendorid = v.vendorid
inner join production.product p
on p.productid = ppv.productid
where p.productsubcategoryid 
in (select  productsubcategoryid 
    from production.productsubcategory 
    where name ='shorts')

----results : rows 7

---16---

select employeeid, firstname,lastname
from person.contact pc
inner join humanresources.employee hre
on pc.contactid = hre.contactid
where hre.employeeid in (
select salespersonid from  sales.salesperson
intersect
select employeeid from purchasing.purchaseorderheader
)
--results: rows 0