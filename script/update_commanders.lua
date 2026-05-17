--[[
Описание: скрипт для обновления commanders_ids.xml
Требования:
- установленный клиент общего теста
- распакованный файл локализации global.po
Автор: Charsi (charsi2011@gmail.com)
Версия: 0.9.1
]]
--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==
-- НАСТРОЙКИ:
-- путь к клиенту общего теста
local mk_pt_path = [[D:\Program Files\Korabli_PT\]]
-- путь до global_dump.po
local dump = [[D:\mk_local_patch\global_dump.po]]
--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==--==
local script_path = debug.getinfo(1).source:sub(2):gsub("(.+\\).*", "%1")
if not package.path:find("xml2lua") then
	package.path = package.path .. ';' .. script_path .. 'xml2lua\\?.lua'
end

local xml2lua = require('xml2lua')
local handler = require('xmlhandler.tree')
local parser = xml2lua.parser(handler)

local function get_game_ver()
	local content = xml2lua.loadFile(mk_pt_path .. [[\game_info.xml]])
	parser:parse(content)
	local t = handler.root['protocol']['game']['part_versions']['version'][1]['_attr']['installed']
	return t:match('%d+$')
end
local game_ver = get_game_ver()
if not game_ver then
	print('cant read game_version')
	return
end
print('game_verios = '.. game_ver)
local fmods = mk_pt_path .. "bin\\" .. game_ver .. [[\res\banks\mod.xml]]
local content = xml2lua.loadFile(fmods)
parser:parse(content)

local t = handler.root['mod.xml']['states'].state[3].values.value -- "CrewName"
local cnt = 0
local outs = {}
for k, v in ipairs(t) do
	local name = v._attr.name
	outs['IDS_' .. name:upper()] = name
	cnt = cnt + 1
	-- print(cnt, 'IDS_' .. name:upper(), name)
end
-- print(cnt, 'ids found')

local sdump = {}
local fdump = assert(io.open(dump))
if fdump then
	local cnt = 0
	local key = nil
	for line in fdump:lines() do
		if not key then
			local name = line:match("^msgid \"(IDS_%S+)\"")
			if name then
				if outs[name] then
					cnt = cnt + 1
					key = outs[name] -- print('dbg:', key)
				else
					-- if name then print('dbg:', name) end
				end
			end
		elseif key then
			local ru = line:match("^msgstr \"(%S.+)\"")
			if ru then
				table.insert(sdump, {id = key, ru = ru})
			else
				-- print("ru:", key, line)
			end
			key = nil
		end
	end
	-- print(cnt, 'ru found')
	fdump:close()
end
table.sort(sdump, function(a, b) return a.id < b.id end)
-- for k, v in ipairs(sdump) do print(v.id,'=>', v.ru) end
-- print(#sdump)
local xml = (script_path .. [[\\commanders_ids.xml]]):from_utf8(0)
local fxml = io.open(xml, "wb")
local txml = {"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>", "<content>"}
if fxml then
	for k, v in ipairs(sdump) do
		-- print(v.id,'=>', v.ru)
		-- fxml:write(string.format('\t<item id="%s" ru="%s"/>\n', v.id, v.ru:gsub('%\\%"', '&quot;')))
		txml[#txml + 1] = string.format('\t<item id="%s" ru="%s"/>', v.id, v.ru:gsub('%\\%"', '&quot;'))
	end
	table.insert(txml, "</content>")
	fxml:write(table.concat(txml, '\r\n'));
	fxml:close()
end

print('done.')
