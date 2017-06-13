--取收货人，供客户中心使用
alter proc QSP_GET_CONSIGNEE_Ex
@bsite varchar(20) 	 
AS


SELECT isprice=case e.num when 0 then 0 else 1 end,--是否有运价
    iscanbedelete=1,
	iscanbeedit=1,
	parentid,cname,man,cgroup,tel,mb,bsite,address,QTYPRICE,wprice,
vprice,yewuyuan,product,package,acctype,billdate,cansms,remark,
j.id,webid,email,bankcode,bankname,bankman,field,modifydate,cid
	FROM B_CONSIGNEE as j 
	--left join (select max(id) ,man,mb,webid from B_CONSIGNEE group by  man,mb,webid) as c
	outer apply   (select count(*) as num  from b_consignee_price as k WHERE j.cname=k.consignee ) e
	WHERE j.bsite like @bsite
	AND j.id  not in (select max(id)  from B_CONSIGNEE group by  man,mb,webid )

union all
	SELECT isprice=case e.num when 0 then 0 else 1 end,--是否有运价
    iscanbedelete=0,
	iscanbeedit=0,
	parentid,j.cname,j.man,cgroup,tel,j.mb,bsite,address,QTYPRICE,wprice,
vprice,yewuyuan,product,package,acctype,billdate,cansms,remark,
j.id,j.webid,email,bankcode,bankname,bankman,field,modifydate,cid
	FROM B_CONSIGNEE as j 
	inner join  (select max(id) as id,man,mb,webid from B_CONSIGNEE group by  man,mb,webid) as c on j.id = c.id
	outer apply (select count(*) as num  from b_consignee_price as k WHERE j.cname=k.consignee ) e
	WHERE j.bsite like @bsite

order by man



--SELECT isprice=case (SELECT count(*) FROM b_consignee_price as k WHERE j.cname=k.consignee) when 0 then 0 else 1 end,--是否有运价
--    iscanbedelete=1,
--	iscanbeedit=1,*
--	FROM B_CONSIGNEE as j WHERE j.bsite like @bsite
--	AND j.id in (SELECT b.id  FROM B_CONSIGNEE a left join B_CONSIGNEE b on a.man=b.man AND isnull(b.mb,'0')=isnull(a.mb,'0') AND b.webid =a.webid
--WHERE b.id not in(SELECT max(id) FROM B_CONSIGNEE WHERE man=a.man AND isnull(mb,'0')=isnull(a.mb,'0') AND webid =a.webid)
--group by a.man,b.id
--having  count(a.id)>1)

--union all
--	SELECT isprice=case (SELECT count(*) FROM b_consignee_price as k WHERE j.cname=k.consignee) when 0 then 0 else 1 end,--是否有运价
--    iscanbedelete=0,
--	iscanbeedit=0,*
--	FROM B_CONSIGNEE as j WHERE j.bsite like @bsite
--	AND j.id not in (SELECT b.id  FROM B_CONSIGNEE a left join B_CONSIGNEE b on a.man=b.man AND isnull(b.mb,'0')=isnull(a.mb,'0') AND b.webid =a.webid
--WHERE b.id not in(SELECT max(id) FROM B_CONSIGNEE WHERE man=a.man AND isnull(mb,'0')=isnull(a.mb,'0') AND webid =a.webid)
--group by a.man,b.id
--having  count(a.id)>1)

--order by man
