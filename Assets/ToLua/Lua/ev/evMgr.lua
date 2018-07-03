local class = class
local logerr = logerr
local talbeType = "table"

local Cls_EvMgr = class()

function Cls_EvMgr:ctor()
	self.m_map = {}
end

function Cls_EvMgr:reg(name, func)
	local ev = self.m_map[name]
	if not ev then
		ev = {}
		self.m_map[name] = ev
	end
	ev[func] = true
end

function Cls_EvMgr:cancel(name, func)
	if func then
		local ev = self.m_map[name]
		if ev and ev[func] then
			ev[func] = nil
		end
	else
		self.m_map[name] = nil
	end
end

function Cls_EvMgr:trig(name, ...)
	local ev = self.m_map[name]
	if ev then
		for func, _ in pairs(ev) do
			func(...)
		end
	end
end

evMgr = Cls_EvMgr.new()

function trigForCS(name)
	evMgr:trig(name)
end