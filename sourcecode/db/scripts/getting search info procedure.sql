set serveroutput on format wrapped;
declare
  obj 		json;
  filters 	json_list;
  operands 	json_list;
  json_data clob;
  op varchar2(200);
  sCategory varchar2(200);
  filter2 varchar2(200);
  operand json_value;
  
begin
  
  FOR search_rec
  IN  (select user_name, tstamp, message
		from highview_owner.hv_audit b
		join highview_owner.hv_audit_configs c
		on c.action_id = b.action_id
		where c.action_name = 'WB_SEARCH'
		order by tstamp desc)
  LOOP
		obj := json(search_rec.message);
		  filters := json_list(obj.get('filters'));

		  FOR i IN 1..filters.count LOOP
			op 			:=  json_ext.get_string(json(filters.get(i)), 'op');
			sCategory 	:=	json_ext.get_string(json(filters.get(i)), 'cat');
			filter2 		:=	json_ext.get_string(json(filters.get(i)), 'filter');
			operands 		:=	json_ext.get_json_list(json(filters.get(i)), 'operand');
			
			FOR z IN 1..operands.count LOOP
			  operand := operands.get(z);
			  insert into highview_owner.search_info values(op, sCategory, filter2, operand.get_string,search_rec.user_name, search_rec.tstamp);
				
			END LOOP;
		  END LOOP;
  END LOOP;

end;
/

