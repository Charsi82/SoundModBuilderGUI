--[[
Описание: Скрипт для обновления commanders_ids.xml
Версия: 0.9.1
Автор: Charsi (charsi2011@gmail.com)

Требования:
1.Установленная игра
]]

-------------------------------------------------
-- путь к игре
local game_dir = [[D:\Program Files\Korabli_PT\]]
-------------------------------------------------

local curr_path = debug.getinfo(1).source:sub(2):match(".+[/\\]")
local libs = ';' .. curr_path .. 'xml2lua\\?.lua' .. ';' .. curr_path .. '?.lua'
package.path = package.path .. libs

local xml2lua = assert(require('xml2lua'))
local handler = assert(require('xmlhandler.tree'))
local parser = xml2lua.parser(handler)
local function printt(t, tabs)
	if not tabs then tabs = '' end
	for k, v in pairs(t) do
		if type(v)=='table' then
			print(tabs.."["..k.."] = {")
			printt(v, tabs..'\t')
			print(tabs.."}")
		else
			print(tabs.."["..k.."] = ".. tostring(v))
		end
	end
end

local game_ver = ''
local function get_game_ver()
	local content = xml2lua.loadFile(game_dir.."game_info.xml")
	parser:parse(content)
	local t = handler.root['protocol']['game']['part_versions']['version'][1]['_attr']['installed']
	return t:match('%d+$')
end
local status, result = pcall(get_game_ver)
if not status then print("can't get game_version:", result) return end

game_ver = result
print('game version =', game_ver)

local fn_mods = game_dir.."bin\\"..game_ver..[[\res\banks\mod.xml]]
local function can_read_file(fn)
	local f = io.open(fn)
	if f then f:close() return true end
	print("can't open ", fn)
	return false
end
if not can_read_file(fn_mods) then return end

local content = xml2lua.loadFile(fn_mods)
parser:parse(content)

local t = handler.root['mod.xml']['states'].state[3].values.value
local cnt = 0
local outs = {}
for k, v in ipairs(t) do
	local name = v._attr.name
	if name ~= 'Unknown' then
		outs['IDS_' .. name:upper()] = name
		cnt = cnt + 1
	end
	-- print(cnt, 'IDS_' .. name:upper(), name)
end
-- print(cnt, 'ids found')

local global_mo = game_dir.."bin\\"..game_ver..[[\res\texts\ru\LC_MESSAGES\global.mo]]
local mo = require"mo"
local get_name = mo(global_mo)

local sdump = {}
for IDS, name in pairs(outs) do
	table.insert(sdump, {id = name, ru = get_name(IDS):gsub('"', '&quot;')})
end
table.sort(sdump, function(a, b) return a.id < b.id end)

local xml = curr_path..[[commanders_ids.xml]]
print('save to', xml:to_utf8(0))
local fxml = io.open(xml, "wb")
if fxml then
	local txml = {"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>", "<content>"}
	for k, v in ipairs(sdump) do
		-- print(v.id,'=>', v.ru)
		txml[#txml + 1] = string.format('\t<item id="%s" ru="%s"/>', v.id, v.ru)
	end
	txml[#txml + 1] = "</content>"
	fxml:write(table.concat(txml, '\n'));
	fxml:close()
end
print('done.')
-- "CrewName"