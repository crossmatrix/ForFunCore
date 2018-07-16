local class = class
local Time = Time
local UpdateBeat = UpdateBeat

local Cls_LuaDrive = class()

function Cls_LuaDrive:ctor()
	self.m_handle = UpdateBeat:CreateListener(self.update, self)
	UpdateBeat:AddListener(self.m_handle)
	self.m_listeners = {}
	
	-- local files = self:requireInfos()
	-- for _, v in ipairs(files) do
	-- 	require(v)
	-- end

	-- self:register(pool)
	self:register(coMgr)
	
	-- self:register(uiMgr)
end

function Cls_LuaDrive:update()
	local t = Time.unscaledDeltaTime
	for _, listener in ipairs(self.m_listeners) do
		listener:update(t)
	end
end

-- function Cls_LuaDrive:stop()
-- 	UpdateBeat:RemoveListener(self.m_handle)
-- end

function Cls_LuaDrive:register(listener)
	table.insert(self.m_listeners, listener)
end

function Cls_LuaDrive:requireInfos()
	return 
	{
		"Mgr.UIBase",
		"Mgr.UIMgr",
		"UI.UILogin",
	}
end

LuaDrive = Cls_LuaDrive.new()
