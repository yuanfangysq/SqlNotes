alter PROCEDURE NAME
@bsite varchar(20),
@esite varchar(20),
@t1 datetime,
@t2  datetime
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT

dccb=ROUND(CASE WHEN isnull(e.acctotal_in,0) = 0 THEN 0
        else  (e.accchauffertotal/ e.acctotal_in)*a.acctrans
		end, 2)
		,
	accleft=case acctype when 'œ÷∏∂'     then  case isnull(accnowstate,0) when 1 then accnowleft when 0 then accnow + ISNULL(accqf,0) end
				when null then accback end,	
FROM b_tyd a left join b_fcd b on(a.unit=b.unit and b.sendtimes=1) left join b_tyd_trace_tpl c on (a.unit=c.unit)
outer apply
(

select  
		accchauffertotal=isnull(h.accfirst,0)+isnull(h.accarrived,0)+isnull(h.accback,0)+isnull(h.accreward,0) + isnull(h.accyouka,0),		
		acctotal_in=sum((d.accnow  + d.accarrived  + d.accback  + d.accmonth + d.acchuokuankou)* c.qty / d.qty)
		from   B_VEHICLE_HT as h join b_fcd as  c  on (h.inonevehicleflag=c.inonevehicleflag)  join b_tyd as d on c.unit=d.unit 
where b.inonevehicleflag=c.inonevehicleflag
		group by h.accfirst,h.accarrived,h.accback,h.accreward,h.accyouka
) as e

WHERE 
(a.billdate BETWEEN @t1 AND @t2)
AND a.bsite like @bsite
AND ((a.esite like @esite) or (a.middlesite like @esite))
ORDER BY b.senddate  asc


sp_MShelpcolumns 'tablename'