---date: nov, 7, 2013

---join simulation 

create table employee
(
eid int, 
name varchar(10)
)

create table address
(
eid int,
city varchar (10),
state char(2)
)

insert into employee values (1,'abc'),(2,'def'),(3,'xyz'),(4,'pqr'),(5,'wxy')
insert into address (eid,city,state) values (1,'poway','ca'),(2,'san diego','ca'),(3,'pomerado','pa'),(6,'lajolla','tx'),(7,'civic','la')

select * from employee
select * from address


--inner join

select * from employee e 
inner join address a
on a.eid= e.eid

--full outer join
select * from employee e 
full outer join address a
on a.eid= e.eid


select *  from employee e left outer join address a on a.eid= e.eid
-- inner join from full outer join
select * from employee e 
full outer join address a
on a.eid= e.eid
where a.eid is not null and e.eid is not null


--- loj from foj

select * from employee e 
full outer join address a
on a.eid= e.eid
where e.eid is not null
---loj
select * from employee e 
left outer join address a
on a.eid= e.eid

--restricted loj from foj --找出不相同的事故隐患

select * from employee e 
full outer join address a
on a.eid= e.eid
where a.eid is null and e.eid is not null 


--roj from full outer join

select * from employee e 
full outer join address a
on a.eid= e.eid
where a.eid is not null




--- restricted roj from full outer join

select * from employee e 
full outer join address a
on a.eid= e.eid
where a.eid is not null and e.eid is null

--restricted full outer join

select * from employee e 
full outer join address a
on a.eid= e.eid
where a.eid is  null or e.eid is null

-- we can simulate all types of joins using foj excetp cross join --
-- cross join--
/*
cross join is the cartisian product of all the row from the left table with all the rows from the right table.
*/

select * from 
employee,address

--or--

select * from employee
cross join address

select * from address



use adventureworks

select count(*),hre2.managerid,hre2.title,pc.firstname,pc.lastname 
from humanresources.employee hre
 inner join humanresources.employee hre2 
 on hre2.employeeid = hre.managerid
 inner join person.contact pc
 on pc.contactid = hre2.contactid
group by hre2.managerid,hre2.title,pc.firstname,pc.lastname
having count(*) > =6
order by 1 desc


select distinct hre1.employeeid, hre2.managerid, hre1.title,pc.firstname,pc.lastname,
(pa.addressline1 + pa.addressline2) 'address' from humanresources.employee hre1
inner join humanresources.employee hre2
on hre1.employeeid = hre2.managerid
inner join person.contact pc
on hre1.contactid = pc.contactid
inner join humanresources.employeeaddress hea
on hea.employeeid = hre1.employeeid
inner join person.address pa
on pa.addressid = hea.addressid
--47: managers
--290: employee
--243 - not mangers


-- get the information of all employees who are not managers


select  hre1.employeeid, hre1.managerid, hre1.title,pc.firstname,pc.lastname,
(pa.addressline1 + pa.addressline2) 'address' from humanresources.employee hre1
left outer join humanresources.employee hre2
on hre1.employeeid = hre2.managerid
inner join person.contact pc
on hre1.contactid = pc.contactid
left join humanresources.employeeaddress hea
on hea.employeeid = hre1.employeeid
inner join person.address pa
on pa.addressid = hea.addressid
where hre2.managerid is null

select  distinct hre1.employeeid, hre1.managerid, hre1.title,pc.firstname,pc.lastname,
(pa.addressline1 + pa.addressline2) 'address' from humanresources.employee hre1
left outer join humanresources.employee hre2
on hre1.employeeid = hre2.managerid
inner join person.contact pc
on hre1.contactid = pc.contactid
left join humanresources.employeeaddress hea
on hea.employeeid = hre1.employeeid
inner join person.address pa
on pa.addressid = hea.addressid
where hre1.managerid is null

--sbuqueries
/*retrieve employeeid, firstname, lastname of all employees. 
*/

---using joins
select hre.employeeid ,pc.firstname,pc.lastname from humanresources.employee hre
inner join person.contact pc
on pc.contactid = hre.contactid


select employeeid from humanresources.employee
where contactid in (select contactid from person.contact)




/* 1. what are the differences between joins and sub-queries
   2. what are the limitations of sub queries
   3 what are the selections quidelines for sub-queries
   4. what are sub-queries
   
   */
   
   select employeeid from humanresources.employee
   where employeeid not in (select managerid from humanresources.employee where managerid is not null)

   select employeeid from 
   humanresources.employee hre
   left outer join sales.salesperson ss
   on hre.employeeid = ss.salespersonid
   where ss.salespersonid is null
   
   
   select employeeid from humanresources.employee 
   where employeeid not in (select salespersonid from sales.salesperson)
   
   
   --- set operators
   
   /*
   union,union all, except, intersect
   syntax: 
   select col1,col2,col3 from t1
   union/union all/except/intersect
   rules: 
   1. the no. of columns in first select statement must be same as no. of columns in second select statement. 
   2. the metadata of all the columns in first select statement must e exactly same as the metadata of all coumns in second 
   select statement accordingly.
   3. dont use * operator in either of the query
   4. order by clause do not work first select statement 
   */
   
   /* hierachy --
   1. joins (joins 2 or more tables horizontally)
   2. sub-queries
   3. set operators (joins 2 or more tables vertically)
   */
   
   
   /*-- in class assigment --
   1. get me the informatoin of all employees who are not managesr and ahre being managed by atleast 3 employees who are managess
   2. get the information of all intermediate level managers( hint all managesr except ceo)
   3. get the info of all managers who manage two or more managers which intern manges at least 2 employees who are not managers (leaf-level employee)
   4.get me a dataset that shows all employees that make more than the averate rate
   5.return all the employ
   */
   
   /*
   solution
   */
   
   --1
   
   
   
   --2. 
select managerid , count(employeeid) [number of employees] from humanresources.employee 
where managerid not in (
(select managerid from humanresources.employee
where employeeid not in 
(select managerid from humanresources.employee
where managerid is not null)
)
)
group by managerid


----3-----
select managerid , count(employeeid) [number of employees] from humanresources.employee 
where employeeid in (
(select managerid from humanresources.employee
where employeeid not in 
(select managerid from humanresources.employee
where managerid is not null)
group by managerid
having count(employeeid)>=2))
group by managerid
having count(employeeid) >=2 

select employeeid from humanresources.employee where 
managerid = 109
---- count(employeeid) [number of employees],
select distinct managerid,title from humanresources.employee
where employeeid not in 
(select managerid from humanresources.employee
where managerid is not null)



--managers of more than 2. 

select managerid 
from humanresources.employee
group by managerid
having count(employeeid) > =2 



select employeeid  from humanresources.employee where employeeid not in ( 
select employeeid from humanresources.employee
where employeeid not in 
(select managerid from humanresources.employee
where managerid is not null))


select  from humanresources.employee
where managerid in (

