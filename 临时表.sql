alter    PROCEDURE 专线分理处统计
  @t1   datetime,
  @t2   datetime,
  @webid  varchar(20),

@LoginSite varchar(50),
@LoginWeb varchar(50),
@LoginUserID varchar(50),
@LoginUserName varchar(50) ,

@LocalIP varchar(50),
@PublicIP varchar(50),
@mac varchar(100)
 AS
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
 

 IF object_id('tempdb..#t_zxcfl') is not null
 begin
    drop table #t_zxcfl
 end

 
create table #t_zxcfl
(
	esite varchar(30),
	acchuikou decimal(10,2),
	acczz  decimal(10,2),
	outacc_c decimal(10,2),
	accsend_c decimal(10,2),
	webid_num int,
	qty int,
	accnow decimal(10,2),
	accarrived decimal(10,2),
	accmonth decimal(10,2),
	acckoufu decimal(10,2),
	kz_acctrans  decimal(10,2),--运费合计
	kz_accdaishou  decimal(10,2),--代收货款
	yb_acctrans decimal(10,2),
	yb_accdaishou decimal(10,2)
)


insert into #t_zxcfl
SELECT
	--都是扣掉回扣、垫付中转费、结算中转费、结算送货费之后的数值
	esite =case when isnull(middlesite,'')='' then esite
	     else middlesite end,
	acchuikou=sum(isnull(acchuikou,0)),
	acczz=sum(isnull(acczz,0)),
	outacc_c=sum(isnull(outacc_c,0)),
	accsend_c=sum(isnull(accsend_c,0)),
	webid_num=COUNT(isnull(esite,0)),
	qty=sum(qty),
	accnow= sum(isnull(accnow,0)) ,
	accarrived=sum(isnull(accarrived,0)) ,
	accmonth=sum(isnull(accmonth,0)),
	acckoufu=sum(isnull(acchuokuankou,0)) ,
	kz_acctrans=sum(isnull(acctrans,0))-sum(isnull(acchuikou,0))-sum(isnull(acczz,0))-sum(isnull (outacc_c,0))-sum(isnull(accsend_c,0))  ,--运费合计
	kz_accdaishou=sum(isnull(accdaishou,0))-sum(isnull(acchuikou,0))-sum(isnull(acczz,0))-sum(isnull (outacc_c,0))-sum(isnull(accsend_c,0))  ,--代收货款
	yb_acctrans=sum(isnull(acctrans,0)),
	yb_accdaishou=sum(isnull(accdaishou,0))
    FROM B_TYD  
 
WHERE(billdate    BETWEEN @t1 AND @t2)  AND (webid like  @webid)  --and isnull(middlesite,'')=''临汾
group by   esite,middlesite

select 	esite,
        acchuikou=sum(isnull(acchuikou,0)) ,
		acczz=sum(isnull(acczz,0))  ,
		outacc_c=sum(isnull(outacc_c,0)) ,
		accsend_c=sum(isnull(accsend_c,0)) ,
		webid_num=sum(isnull(webid_num,0)),
		qty=sum(isnull(qty,0)),
		accnow=sum(isnull(accnow,0)) ,
		accarrived=sum(isnull(accarrived,0)) ,
		accmonth=sum(isnull(accmonth,0)) ,
		acckoufu=sum(isnull(acckoufu,0)) ,
		kz_acctrans=sum(isnull(kz_acctrans,0))  ,--运费合计
		kz_accdaishou=sum(isnull(kz_accdaishou,0))  ,--代收货款
		yb_acctranssum=sum(isnull(yb_acctrans,0)) ,
	    yb_accdaishousum=sum(isnull( yb_accdaishou,0))
	    from  #t_zxcfl group by esite

drop table #t_zxcfl