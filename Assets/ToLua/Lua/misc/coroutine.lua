local class = class
local logerr = logerr
local Util = Util
local create = coroutine.create
local running = coroutine.running
local resume = coroutine.resume
local yield = coroutine.yield
local status = coroutine.status
local dead = "dead"

local Cls_CoMgr = class()

function Cls_CoMgr:ctor()
	self.m_comap = {}
end

function Cls_CoMgr:update(t)
	for co, _ in pairs(self.m_comap) do
		local flag, msg = resume(co, t)
		self:check(co, flag, msg)
		-- log("co update")
	end
end

function Cls_CoMgr:newCo(f)
	return create(f)
end

function Cls_CoMgr:proc(co, ...)
	if not self.m_comap[co] then
		self.m_comap[co] = true
		local flag, msg = resume(co, ...)
		self:check(co, flag, msg)
	else
		logerr("the co is runing")
	end
end

function Cls_CoMgr:start(f, ...)
	local co = self:newCo(f)
	self:proc(co, ...)
	return co
end

function Cls_CoMgr:check(co, flag, msg)
	if status(co) == dead then
		self.m_comap[co] = nil
	end
	if not flag then
		logerr(msg)
	end
end

function Cls_CoMgr:stop(co)
	local envCo = running()
	co = co or envCo
	if self.m_comap[co] then
		self.m_comap[co] = nil
		-- 处理协程内部在最后结束‘自己’的情况
		if envCo and envCo == co then
			yield()
		end
	else
		logerr("the co not exist")
	end
end

function Cls_CoMgr:isRunning(co)
	return self.m_comap[co] ~= nil
end

-- function Cls_CoMgr:awaitDoCo(co, ...)
-- 	self:proc(co, ...)
-- 	while status(co) ~= dead do
-- 		yield()
-- 	end
-- end

-- 等待一个协程结束
function Cls_CoMgr:awaitCo(f, ...)
	local co = self:newCo(f)
	self:proc(co, ...)
	while status(co) ~= dead do
		yield()
	end
end

-- 等待多个协程结束（每个都结束了才算）
function Cls_CoMgr:awaitAll(funcs)
	local coTb = {}
	for i = 1, #funcs do
		table.insert(coTb, self:start(funcs[i]))
	end

	while true do
		local state = false
		for i = 1, #coTb do
			if coMgr:isRunning(coTb[i]) then
				state = true
				break
			end
		end

		if not state then
			break
		end
		yield()
	end
end

function Cls_CoMgr:awaitTime(time)
	local count = 0
	while count < time do
		count = count + yield()
	end
end

function Cls_CoMgr:awaitFrame(num)
	num = num or 1
	while num > 0 do
		yield()
		num = num - 1
	end
end

function Cls_CoMgr:awaitUntil(cond)
	while not cond() do
		yield()
	end
end

function Cls_CoMgr:awaitWWW()
	--todo
end

function Cls_CoMgr:awaitLoadScene(scenePath)
	local ptr = Util.LoadScene(scenePath)
	while Util.AOIsDone(ptr) == 0 do
		yield()
	end
	Util.ClearPtr(ptr)
end

coMgr = Cls_CoMgr.new()