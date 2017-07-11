---triggers

alter trigger deny_creatting on database
for create_table as
print 'create statement issued'
declare @tabname varchar(100), @sql_query varchar(max)

select @tabname= st.name from sys.tables st
where st.create_date = ( select max(create_date) from sys.tables)


set @sql_query = 'drop table ' + @tabname
print @sql_query
execute (@sql_query)

drop trigger deny_creatting on database

--dml triggers

create table sales_trig
(
salesid int,
pid int,
qty int,
name varchar(100)
)

create table stocks_trig
(
pid int,
pname varchar(50),
qty int
)

insert into stocks_trig values (100,'iphone',45),(102,'laptops',45),(101,'camera',25),(102,'desktop',70)

--case1: using instead of trigger

alter trigger sales1_trig on sales_trig
instead of insert as
begin 
   declare @reg_qty int, @reg_pid int, @ava_qty int
   select @reg_qty= qty, @reg_pid = pid from inserted
   select @ava_qty = qty from stocks_trig where pid = @reg_pid
   
    if @reg_qty <= @ava_qty
    begin
       insert into sales_trig
         select * from inserted
         
       update stocks_trig set qty = qty - @reg_qty where pid = @reg_pid
       
       end
       
       else print 'out of stock'
       
       end
       
       
   insert into sales_trig values(1,100,5,'iphone')
       select * from stocks_trig
       
       
       
       /*
       create a single instead of trigger on stock table for insert update and delete and notify dba which statements has caused
       the trigger to get fired
       
       
       */
insert into stocks_trig values(...)
--your output should be : 
---trigger got fired on insert statement---


create trigger test_me on stocks_trig 
	after insert,update,delete as
	declare @insertrows int,@deleterows int 
	
	select * from inserted
	select * from deleted
	
	select @insertrows = count(*) from inserted
	select @deleterows = count(*) from deleted
	
	if @insertrows <> 0 and @deleterows = 0
	print 'trigger got fired on insert statement'
	
	else if @insertrows = 0 and @deleterows <> 0
	print 'trigger got fired on delete statemet'
	
	else if @insertrows <> 0 and @deleterows<> 0
	print 'trigger got fired on update statement'
	
	else
	print 'something is wrong '
	drop trigger test_me
	
	insert into stocks_trig values (1,'abc',30)
	delete from stocks_trig where pid = 100
	
	update stocks_trig
	set pid = 100
	where pid = 102
	
	
	/*---assumming that  there are two stand alone tables parent and child, simlulate pk and fk relationships between these two tables with t1
	as parent and t2 s child table for inserting data*/
	
	create table parent1( id int)
	create table child1(id int)
	
	
	create trigger fk_sim on parent1
	after insert,update, delete as
	
    declare @id_value int
    select * from deleted
    
    select @id_value = id from deleted
    select * from deleted
    select @id_value = id from deleted 
    delete from child1 where id = @id_value
    
   
   delete from parent1 where id=1
    
    insert into parent1 values(2)
    select * from child1
    insert into child1 values(1),(2)
    go 3 

 drop trigger fk_sim
   
/* create a trigger that will populate an archive table which would hold all the historical records .data from the base table. 
 
 base table structure 
 btable(id,fname,lname,salary)
 archived table structure
 atable(id,fname,lname,salary,flag,ttime,user) */
 
 create table  btable(id int,fname varchar,lname int,salary int)
 create table  atable(id int,fname varchar,lname int,salary int,flag varchar,ttime date,user1 varchar)
 
 create trigger upd on btable after insert,update,delete
  
  as
  
  declare @flag varchar
  declare @user varchar
  declare @tt date
  declare @insertrows int,@deleterows int 
  
  select @user = system_user , @tt = getdate()
  
  select @insertrows = count(*) from inserted
  select @deleterows = count(*) from deleted
  
  if @insertrows <> 0 and @deleterows = 0
	insert into atable
	select id,fname,lname,salary,'i',getdate(),system_user from inserted
	
	else if @insertrows = 0 and @deleterows <> 0
	insert into atable
	select id,fname,lname,salary,'d',getdate(),system_user from deleted
	
	else if @insertrows <> 0 and @deleterows<> 0
	begin
	insert into atable
	select id,fname,lname,salary,'u_old',getdate(),system_user from deleted
	
	insert into atable
	select id,fname,lname,salary,'u_new',getdate(),system_user from inserted
	
	end
	else
	print 'no action performed on atable (vurn was there)'
	
	insert into btable values (1,'g1','j1',100)
	insert into btable values (1,'g2','j2',200)
	insert into btable values (1,'g3','j3',300)
	
	delete from btable
	where id = 1 
	
	update btable
	set id = 22
	where id = 2
	
	
	begin transaction t1
	
	update cur_test 
	