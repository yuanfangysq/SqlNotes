
if OBJECT_ID('Demo_AllProducts') is not null 
drop table Demo_AllProducts 
go 
Create table Demo_AllProducts 
(PKID int not null identity(1,1) primary key 
,DName Nvarchar(20) null 
,DCode NVarchar(30) null 
,DDate datetime null 
) 
go 

--this SQL is only for SQL Server 2008 
Insert into Demo_AllProducts 
(DName,DCode,DDate) 
values 
('DemoA','AAA',GETDATE()), 
('DemoB','BBB',GETDATE()), 
('DemoC','CCC',GETDATE()), 
('DemoD','DDD',GETDATE()), 
('DemoE','EEE',GETDATE()) 

select * from Demo_AllProducts 

--PKID DName DCode DDate 
--1 DemoA AAA 2010-10-12 20:33:54.417 
--2 DemoB BBB 2010-10-12 20:33:54.417 
--3 DemoC CCC 2010-10-12 20:33:54.417 
--4 DemoD DDD 2010-10-12 20:33:54.417 
--5 DemoE EEE 2010-10-12 20:33:54.417 

if OBJECT_ID('Demo_Shop1_Product') is not null 
drop table Demo_Shop1_Product 
go 

Create table Demo_Shop1_Product 
(PKID int not null identity(1,1) primary key 
,DName Nvarchar(20) null 
,DCode NVarchar(30) null 
,DDate datetime null 
) 
go 

--this SQL is only for SQL Server 2008 
Insert into Demo_Shop1_Product 
(DName,DCode,DDate) 
values 
('DemoA','AAA',GETDATE()), 
('DemoB','CCC',GETDATE()), 
('DemoF','FFF',GETDATE()) 





--PKID DName DCode DDate 
--1 DemoA AAA 2010-10-17 20:19:32.767 
--2 DemoB CCC 2010-10-17 20:19:32.767 
--3 DemoF FFF 2010-10-17 20:19:32.767 


select * from Demo_Shop1_Product 

select * from Demo_AllProducts 

--确定目标表 
Merge Into Demo_AllProducts p 
--从数据源查找编码相同的产品 
using Demo_Shop1_Product s on p.DCode=s.DCode 
--如果编码相同，则更新目标表的名称 
When Matched and P.DName<>s.DName Then Update set P.DName=s.DName 
--如果目标表中不存在，则从数据源插入目标表 
When Not Matched By Target Then Insert (DName,DCode,DDate) values (s.DName,s.DCode,s.DDate) 
--如果数据源的行在源表中不存在，则删除源表行 
When Not Matched By Source Then Delete; 




