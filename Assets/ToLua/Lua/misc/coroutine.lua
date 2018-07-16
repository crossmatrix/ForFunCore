local class = class
local logerr = logerr
local evMgr = evMgr
local Util = Util
local create = coroutine.create
local running = coroutine.running
local resume = coroutine.resume
local yield = coroutine.yield
local status = coroutine.status
local dead = "dead"
local CoState = {Active = 1, DeActive = 2}

local Cls_CoMgr = class()

function Cls_CoMgr:ctor()
	self.m_comap = {}
end

function Cls_CoMgr:update(t)
	for co, state in pairs(self.m_comap) do
		if state == CoState.Active then
			local flag, msg = resume(co, t)
			self:check(co, flag, msg)
		end
		-- log("...")
	end
end

function Cls_CoMgr:start(f, ...)
	local co = create(f)
	self.m_comap[co] = CoState.Active
	local flag, msg = resume(co, ...)
	self:check(co, flag, msg)
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
	self:_setState(nil, co)
end

function Cls_CoMgr:pause(co)
	self:_setState(CoState.DeActive, co)
end

function Cls_CoMgr:continue(co)
	self:_setState(CoState.Active, co)
end

-- 1:active 2:deactive nil:stop
function Cls_CoMgr:_setState(state, co)
	local envCo = running()
	co = co or envCo
	if self.m_comap[co] then
		self.m_comap[co] = state
		-- 处理紧接着非await的代码
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

-- 等待一个协程结束
function Cls_CoMgr:awaitDoCo(f, ...)
	local co = self:start(f, ...)
	while self:isRunning(co) do
		yield()
	end
end

-- 等待一个协程结束
function Cls_CoMgr:awaitCo(co)
	co = co or running()
	if self.m_comap[co] then
		while self:isRunning(co) do
			yield()
		end
	else
		logerr("the co not exist")
	end
end

-- 等待多个协程都结束
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
	local loop = true
	local co = running()
	evMgr:reg(scenePath, function()
		loop = false
		-- 避免帧尾判断导致多等待一帧
		local flag, msg = resume(co)
		self:check(co, flag, msg)
	end)
	Util.LoadScene(scenePath)
	while loop do
		yield()
	end
	evMgr:cancel(scenePath)
end

coMgr = Cls_CoMgr.new()