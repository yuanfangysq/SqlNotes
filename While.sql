USE [text]
GO
/****** Object:  UserDefinedFunction [dbo].[L_SplitStr_HT_DF]    Script Date: 2017-1-3 9:24:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER function [dbo].[L_SplitStr_HT_DF]
(
@SourceSql1 varchar(8000),
@SourceSql2 varchar(8000),

@StrSeprate char(1))     
returns @temp table(esite varchar(20),df float )     
as       
begin   
  
declare @esite varchar(20)
declare @df float 

  
   while(@SourceSql1 <> '')     
  begin     
    set @esite=left(@SourceSql1,charindex(@StrSeprate,@SourceSql1,1)-1)   
    set @SourceSql1=stuff(@SourceSql1,1,charindex(@StrSeprate,@SourceSql1,1),'')

    set @df=left(@SourceSql2,charindex(@StrSeprate,@SourceSql2,1)-1)   
    set @SourceSql2=stuff(@SourceSql2,1,charindex(@StrSeprate,@SourceSql2,1),'')

      insert @temp values(@esite,@df)     
  end      
return     
end

declare @g varchar(max)
set @g='select * from '+QUOTENAME('person')

exec(@g )

   delete  from person   


--if object_id(person) is null

IF OBJECT_ID (N'dbo.person', N'U') IS not  NULL
print(1)

alter  PROCEDURE  [USP_SET_ITEMS_OK_ID] 
    @id varchar(8000),
    @appby varchar(10)
 as
begin tran tx 

declare @date datetime
set @date = getdate()

   UPDATE b_account set appdate=@date,appby=@appby  WHERE id in(SELECT unit FROM L_SplitStr_ONE(@id,'@'))
   insert into b_account(password) select*from L_SplitStr_ONE('11@22@+33@','@')
   insert into person(password,age) select*  from L_SplitStr_HT_DF('11@22@+33@','44@55@33@','@')
   UPDATE b_account set appdate=@date,appby=@appby  WHERE id in(SELECT unit FROM L_SplitStr_ONE(@id,'@'))
   insert into b_account(password) select*from L_SplitStr_ONE('11@22@+33@','@')


SELECT @date

commit tran tx
IF @@error <>  0 
  rollback tran tx


alter  proc yd_QSP_GET_FETCH_FOR_PRINT_PL_FOR_RUILANG
  @unit varchar(2000)
as

declare @sql varchar(5000)

set @sql=
	   'SELECT  unit=a.billno,' 
        +' accdaishou= accdaishou+isnull(acchkchangenumber,0) ,'
        +' accarrived=convert(varchar,(accnow  + accarrived  + accback  + accmonth + acchuokuankou )) +isnull(acctype,0),'
        +' a.middlesite,a.esite,fetchdate,fetchmadeby,acctf=accarrived,accqt=(accback+accnow+accmonth)'
	+' FROM b_tyd as a,b_fcd as b'
	+' WHERE a.unit=b.unit AND a.unit in ('+@unit+') AND b.sendtimes=1 '

exec(@sql)



