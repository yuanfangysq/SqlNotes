/* create a data mart that should be capable of providing efficient

    support in decision making in terms of production and sales in the

    company. data mart will be responsible for tracking trends and

    historical patterns of salesamount, netprofit

    by employee, product, productsubcategory, productcategory taking

    place in different countries. users would be interested in generating

    reports from dm and would like to see information atleast on monthly

    basis. identify required dimension tables, fact tables and

    grain involved in creation of dm. also, perform etl on newly created

    dm for inital load. (use adventureworks database as oltp source for dm)

    also create etl mapping documents that would map source coulmns from

    oltp tables to corresponding olap tables during the process of designing)

 

*/


 /*
oltp tables: 
-------------
1. humanresources.employee
2. production.product
3. productsubcategory
4. productcategory
5. sales.salesterritory
6. sales.salesorderdetail
7. sales.salesorderheader
8. person.contact
 */
 
 
/*
prestaging & staging area with it's dimensions 
*/

----creating the prestaging & staging area database. 
create database project_staging

use project_staging
                   --==dim employee==
--tracking table
create table trackemp (
                        employeeid nvarchar(15) null,
                        datacheck int
                       )
--prestaging                     

--staging
select * into dimemp from dimemployee where 1=2	
		
				  
					--==dimproduct==

--tracking
create table trackpdt (
                        productid nvarchar(15) null,
                        datacheck int
                       )
--prestaging                      					  				   
create table   dimproduct (
						   productkey int identity(1,1) not null primary key,
						   productalternatekey nvarchar(25) not null unique ,
						   standardcost money null,
						   productcategoryname nvarchar(50) not null,
						   productsubcategoryname nvarchar(50) not null,
						   effectivedate datetime not null,
						   expireddate   datetime null,
						   status nvarchar(50) null
                          )	
--staging                          
 select * into dimpdt from dimproduct where 1=2 
 
 
                         				   
                    --==dimsalescountry==
--tracking                
create table tracksc (
                        salesterritoryid int not null,
                        datacheck int
                       )                            
 --prestaging                                     
create table   dimsalescountry  (
                                 salescountrykey int identity(1,1) not null primary key,
                                 salescountryalternatekey int not null unique,
                                 countryname nvarchar(50) not null
                                )
 ---staging                              
 select * into dimsc from dimsalescountry where 1=2    
 
 
                            
--dimtime
create table   dimtime    (
						   timekey int identity(1,1) not null primary key,
						   fulldatealternatekey datetime not null,
						   monthnumberofyear tinyint null,
						   englishmonthname nvarchar(25) null,
						   calenderquater tinyint null,
						   calenderyear char(4) null,
						   fiscalquater tinyint null,
						   fiscalyear char(4) null
					      )
select * into dimtm from dimtime where 1=2
alter table dimtm
alter column fulldatealternatekey nvarchar(20) not null --month/year
--factinternetsales
create table   factinternetsales(
								 productkey int foreign key references dimproduct(productkey) ,
								 salescountrykey int foreign key references dimsalescountry(salescountrykey),
								 orderdatekey  int foreign key references  dimtime(timekey),
								 duedatekey    int foreign key references  dimtime(timekey),
								 shipdatekey   int foreign key references  dimtime(timekey),
								 orderquantity smallint null,
							-- salesordernumber nvarchar(20) not null,
								 productstandardcost money null,
								 totalproductstandardcost money null,
								 salesamount money null,
								 netprofitamount money null
                               )
 select * into factis from factinternetsales where 1=2

--prestaging










create table   dimemployee(
						   employeekey int identity(1,1) not null primary key,
						   employeenationalidalternatekey nvarchar(15) not null unique,
						   fristname nvarchar(50) not null,
						   lastname  nvarchar(50) not null,
						   middlename nvarchar(50) not null,
						   effectivedate datetime not null,
						   expireddate   datetime null,
						   status nvarchar(50)
					       )

--factresellersales
create table   factresellersales(
                                 employeekey int foreign key references dimemployee(employeekey),
								 productkey int foreign key references dimproduct(productkey),
								 salescountrykey int foreign key references dimsalescountry(salescountrykey),
								 orderdatekey  int foreign key references  dimtime(timekey),
								 duedatekey    int foreign key references  dimtime(timekey),
								 shipdatekey   int foreign key references  dimtime(timekey),
								 orderquantity smallint null,
							  --salesordernumber nvarchar(20) not null,
								 productstandardcost money null,
								 totalproductstandardcost money null,
								 salesamount money null,
								 netprofitamount money null
                                )
--staging
 select * into factrs from factresellersales where 1=2






--========================================================================================================================================






















 select purchasing.purchaseorderdetail
 select top 1 * from production.productcosthistory
 select ssod.productid, ssod.unitprice,ssod.linetotal,pp.standardcost from sales.salesorderdetail ssod
 inner join production.product pp
 on pp.productid = ssod.productid where pp.standardcost < ssod.unitprice
 select top 1 * from sales.salesorderheader 
 select top 1 * from sales.salesorderdetail
 select top 1 * from production.product
 select *  from production.productlistpricehistory where productid = 709


 --Ctrl+Shift+U 转为大写
--Ctrl+Shift+L 转为小写