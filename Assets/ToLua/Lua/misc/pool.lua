local class = class
local logerr = logerr
local Util = Util
local list = list
local ilist = ilist
local CheckInterval = 1
local DestroyInstItv = 2

local Cls_Pool = class()

function Cls_Pool:ctor()
	self.m_list = {}
	self.m_checkCount = 0
	self.m_rootPtr = Util.NewObj("PrefPool", GameDefine.ProxyObj)
end

function Cls_Pool:update(t)
	local count = self.m_checkCount + t
	if count >= CheckInterval then
		self:checkIdle(count)
		self.m_checkCount = 0
	else
		self.m_checkCount = count
	end
end

function Cls_Pool:checkIdle(t)
	for flag, container in pairs(self.m_list) do
		local idle = container.idle
		for itr, inst in ilist(idle) do
			local count = inst._count + t
			if count >= DestroyInstItv then
				idle:remove(itr)
				Util.DestroyPtr(inst.ptr)
			else
				inst._count = count
			end
		end
	end
	-- log(toStr(self))
end

function Cls_Pool:searchFromCache(arg)
	local targ = nil
	local container = self.m_list[arg]
	if container then
		targ = container.idle:shift()
	else
		container = {busy = {}, idle = list:new(), busyLength = 0, box = nil}
		self.m_list[arg] = container
		container.box = Util.NewObj("[rs]" .. arg, self.m_rootPtr)
	end
	return targ
end

function Cls_Pool:spawn(arg)
	if type(arg) == "string" then
		return self:_pathSpawn(arg)
	end
end

function Cls_Pool:_pathSpawn(name)
	local targ = self:searchFromCache(name)
	if not targ then
		targ = {ptr = Util.NewPref(name, 0), _count = 0, _flag = name}
	else
		targ = {ptr = targ.ptr, _count = 0, _flag = name}
	end
	self:_pushToBusy(targ)
	return targ
end

function Cls_Pool:_pushToBusy(targ)
	Util.SetActive(targ.ptr, true)
	local container = self.m_list[targ._flag]
	container.busy[targ] = true
	container.busyLength = container.busyLength + 1
end

function Cls_Pool:despawn(inst, container)
	container = container or self.m_list[inst._flag]
	if container and container.busy[inst] then
		if Util.CheckPtr(inst.ptr) then
			container.busy[inst] = nil
			container.busyLength = container.busyLength - 1
			container.idle:push(inst)
			Util.SetActive(inst.ptr, false)
			Util.SetParent(inst.ptr, container.box)
		end
	end
end

function Cls_Pool:despawnAll(...)
	local len = select('#', ...)
	for i = 1, len do
		local arg = select(i, ...)
		local container = self.m_list[arg]
		if container then
			for inst, _ in pairs(container.busy) do
				self:despawn(inst, container)
			end
		end
	end
end

pool = Cls_Pool.new()