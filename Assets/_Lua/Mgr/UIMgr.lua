local class = class
local clone = clone
local UIBase = UIBase

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
		ui.__inst = LU:NewUI("UI/" .. name)
		ui:onInit()
	end

	ui.__active = true
	LU:SetActive(ui.__inst, ui.__active)
	ui:onShow()
end

function Cls_UIMgr:hide(name)
	local ui = self.m_uiList[name]
	ui.__active = false
	LU:SetActive(ui.__inst, ui.__active)
	ui:onHide()
end

-- function Cls_UIMgr:addClick(btn, func, param)
-- 	GameProxy:AddClick(btn, func, param)
-- end

uiMgr = Cls_UIMgr.new()