
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

--ȷ��Ŀ��� 
Merge Into Demo_AllProducts p 
--������Դ���ұ�����ͬ�Ĳ�Ʒ 
using Demo_Shop1_Product s on p.DCode=s.DCode 
--���������ͬ�������Ŀ�������� 
When Matched and P.DName<>s.DName Then Update set P.DName=s.DName 
--���Ŀ����в����ڣ��������Դ����Ŀ��� 
When Not Matched By Target Then Insert (DName,DCode,DDate) values (s.DName,s.DCode,s.DDate) 
--�������Դ������Դ���в����ڣ���ɾ��Դ���� 
When Not Matched By Source Then Delete; 




