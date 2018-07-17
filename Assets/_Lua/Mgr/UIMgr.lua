local class = class
local clone = clone
local UIBase = UIBase
local Util = Util

--[[
	1.动静UI元素分离放置在不同canvas下
	2.运动频率不一致的元素也分开在不同canvas
	3.RectMask2D替换Mask
	4.纹理分辨率，格式等检查，mipmap，read/write
--]]

local Cls_UIMgr = class()

function Cls_UIMgr:ctor()
	self.m_uiList = {}
end

function Cls_UIMgr:update(t)
	for _, ui in pairs(self.m_uiList) do
		if ui.__active then
			ui:onUpdate(t)
		end
	end
end

function Cls_UIMgr:reg(name)
	local ui = clone(UIBase)
	ui.__inst = nil
	ui.__active = false
	self.m_uiList[name] = ui
	return ui
end

function Cls_UIMgr:show(name)
	local ui = self.m_uiList[name]
	if not ui.__inst then
		ui.__inst = Util.NewUI("UI/" .. name)
		ui:onInit()
	end
	ui.__active = true
	Util.SetObject(ui.__inst, 1)
	ui:onShow()
end

function Cls_UIMgr:hide(name)
	local ui = self.m_uiList[name]
	ui.__active = false
	Util.SetObject(ui.__inst, 2)
	ui:onHide()
end

uiMgr = Cls_UIMgr.new()