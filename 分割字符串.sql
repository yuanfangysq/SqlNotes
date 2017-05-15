alter  PROCEDURE  [USP_SET_ITEMS_OK_ID] 
    @id varchar(8000),
    @appby varchar(10)
 as
begin tran tx 

declare @date datetime
set @date = getdate()

   UPDATE b_account set appdate=@date,appby=@appby  WHERE id in(SELECT unit FROM L_SplitStr_ONE(@id,'@'))
   insert into b_account(password) select*from L_SplitStr_ONE('11@22@+33@','@')

SELECT @date

commit tran tx
IF @@error <>  0 
  rollback tran tx

--------------------下面是方法--------------------------------------------
alter function [dbo].[L_SplitStr_ONE]
(
@SourceSql1 varchar(8000),
@StrSeprate char(1))     
returns @temp table(unit varchar(30))     
as       
begin     
declare @unit varchar(30)
  
 
  
   while(@SourceSql1 <> '')     
  begin     
    set @unit=left(@SourceSql1,charindex(@StrSeprate,@SourceSql1,1)-1)   
    set @SourceSql1=stuff(@SourceSql1,1,charindex(@StrSeprate,@SourceSql1,1),'')

   
 
      insert @temp values(@unit)     
  end      
return     
end





