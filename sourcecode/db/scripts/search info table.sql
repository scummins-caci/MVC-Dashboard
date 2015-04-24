create table highview_owner.search_info
(
    search_operator varchar2(200), 
	search_category varchar2(200), 
	search_filter 	varchar2(200),
	search_operand	varchar2(4000),
	user_name		varchar2(4000) not null,
	tstamp			timestamp(6) not null
);
grant select, update, insert, delete on highview_owner.search_info to highview_appl;
create or replace synonym highview_appl.search_info for highview_owner.search_info;

