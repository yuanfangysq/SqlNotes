--�˻�����
alter PROCEDURE [dbo].[USP_UPDATE_TH]
@unit int
AS

IF((SELECT state FROM b_tyd WHERE unit=@unit)>4)
begin
	raiserror('�õ��Ѿ����ն�Ӫҵ�����⣬�����˻�!',16,1)
	return
end

IF((SELECT count(*) FROM b_account_plus WHERE unit=@unit AND xmtype='�Ḷ' AND account>0)>0)
begin
	raiserror('�õ��Ḷ�Ѻ����������˻�!',16,1)
	return
end

IF((SELECT count(*) FROM b_account_plus WHERE unit=@unit AND xmtype like '����%' AND account>0)>0)
begin
	raiserror('�õ��Ѿ��ɻ���������˻�!',16,1)
	return
end

IF((SELECT count(*) FROM b_tyd WHERE unit=@unit and isOne_tuihuo=1)>0)
begin
	raiserror('ֻ����һ���˻��������˻�!',16,1)
	return
end

begin tran tx
DECLARE @bsite VARCHAR(20), @esite VARCHAR(20),@middlesite VARCHAR(20),@oldremark VARCHAR(500),@newremark VARCHAR(500),@acctype   VARCHAR(20)
DECLARE @oldacctrans FLOAT, @newacctrans FLOAT, @acchuikou FLOAT --�˷�
DECLARE @oldaccnow FLOAT,@newaccnow FLOAT	--�ָ�
DECLARE @oldaccmonth FLOAT,@newaccmonth FLOAT	--�½�
DECLARE @oldaccarrived FLOAT,@newaccarrived FLOAT	--�Ḷ
DECLARE @oldacchuokuankou FLOAT,@newacchuokuankou FLOAT	--�����
DECLARE @oldaccback FLOAT,@newaccback FLOAT              	--accback
DECLARE @state INT
IF NOT EXISTS(SELECT * FROM B_THCL WHERE unit=@unit)
	IF EXISTS(SELECT * FROM B_TYD WHERE unit=@unit)
	BEGIN
		SELECT @bsite=bsite,@state=[state],@esite=esite,@oldacctrans=acctrans,@acchuikou=acchuikou,@oldaccnow=accnow,
		@oldaccmonth=accmonth,@acctype=acctype,@oldaccarrived=accarrived,@oldacchuokuankou=acchuokuankou,
		@oldaccback=accback,@oldremark=remark,@middlesite=middlesite
		FROM B_TYD WHERE unit=@unit

		--SET @newacctrans=(@oldacctrans-@acchuikou)*2
		SET @newacctrans=@oldacctrans*2
		set @newremark=@oldremark+'('+@bsite+'����'+@esite+')�˻�'
		IF(@acctype='�ָ�')
		SET @newaccnow=@oldaccnow+@oldacctrans
		ELSE IF(@acctype='�Ḷ')
		SET @newaccarrived=@oldaccarrived+@oldacctrans
		ELSE IF(@acctype='�ص���')
		SET @newaccback=@oldaccback+@oldacctrans
		ELSE IF(@acctype='�½�')
		SET @newaccmonth=@oldaccmonth+@oldacctrans
		ELSE IF(@acctype='�����')
		SET @newacchuokuankou=@oldacchuokuankou+@oldacctrans
		ELSE IF(@acctype='���ʸ�')
		SET @newaccarrived=@oldaccarrived+@oldacctrans


		INSERT INTO B_THCL(
		unit,esite,	acctrans,[state],remark,accnow,accmonth,accarrived,acchuokuankou,accback,middlesite)
		VALUES (@unit,@esite,@oldacctrans,@state,@oldremark,@oldaccnow,@oldaccmonth,@oldaccarrived,@oldacchuokuankou,@oldaccback,@middlesite)

		UPDATE b_tyd SET esite=@bsite,middlesite = @bsite,[state] = 11,acctrans=@newacctrans,remark = @newremark,accnow=isnull(@newaccnow,0),accarrived=isnull(@newaccarrived,0),
		accback=isnull(@newaccback,0),accmonth=isnull(@newaccmonth,0),acchuokuankou=isnull(@newacchuokuankou,0), isOne_tuihuo=1
		WHERE unit=@unit
	END

  IF @@error <>0
   	rollback tran tx
 ELSE
 	commit tran tx