-- log("---------------------------------")

--[[
	~ mem fps
	~ Editor: create lua

	~ snapshot
	~ luaProfiler: ctrl+, ctrl+.

	~ misc.functions
		split
		toStr/toStrIndent
		log/logerr
		clone

	~ list(queue/stack)

	~ strict

	~ error
	~ Print/PrintN
	~ Editor: locate code pos
	~ 增强 错误堆栈，定位

	~ setting in lua(GameGlobal)

	~ class

	~ statemachine
		状态不允许从'自己'到'自己'
		允许只有to的状态

	~ coroutine
		base: start stop await
		考虑unity内置协程解决awaitLoadScene

	~ event
		
	~ pool: path

	~ Editor: atlas
		美术定义图和border文件，从工程外部创建atlas和碎图数据（保证unity内无多余资源）
		unity内切割出sprite
	
	~ bundletool
	~ resourceCtrl	

	~ 不用tolua的Translator，1.id不方便自己管理 2.很多不需要对象被缓存 3.非静态调用，lock，慢
		Util
		ObjectCache
	~ 目标1：清空translator里的objectpool和map，Instantiate尚未处理
	~ 目标2：清理ptr（Obj类和资源类）
	目标3：枚举


	uiMgr
	uiCtrl
	ui
		~ uibase

		~ 绑定事件(SetUIEv/SetUIEvD)

		~ unlimit-list(v/h/grid)
			~ 缓存滚动列表item的变量
			~ 给item添加点击事件并且不影响到滚动
			~ 点击高亮
			jumpTo: 返回对象(0:原地, 1~n:指定)
			refresh(some pos, this pos, init pos)(1.jumpTo  2.clear select)

		dyn-progress
		层级（包括夹杂粒子） -> uimgr
		自动关闭之前ui		 -> uimgr

	dotween/uitween

	Editor: ui动静/动频率检查

	ui-effect
	frame-anim(ui/not ui)
	hud/hud层级

	db

	pb
	net
	event

	hot fix

	some 3rd plugin

	--lua call c# 性能
		1.int比Transform等高级对象快
    	2.参数越少越快
    	3.变长参数慢，数组更慢，但能接受（100万次2个Transform参数试验比较：0.45[两个Trans]，0.73[变长Trans]，1.15[Trans数组]）
    	4.luatable比数组更更更慢，而且luatable过多会锁死unity！每次ToLua.CheckLuaTable可能会新建一个缓存table，然后锁住gclist并删除这个table的ref（瓶颈）
    	5.重载无影响
    	6.继承对象略微慢
    	7.委托传递比luafunction慢太多，但调用略快于luafunction（beginpcall, push, endpcall）！
    --lua call c# 调用堆栈
    	ToLua.ToObject -> LuaDll.rawnetobj -> udata(int)
    	Translator.GetObject(udata) -> obj
    	obj.xxx
    	ToLua.PushUserObj(get metaMap ref by obj.GetType)
		return ToLua.PushUserData(translator.getUdata(xxx))
		if not exist -> Translator.AddObject(xxx), Translator.BackMap.Add(udata)
--]]



--[[
	snapshot测试
	缓存的old表不作为比较对象
]]
-- local snapshot = require "snapshot"
-- local old
-- function TestSnap()
-- 	local cur = snapshot.snapshot()
-- 	local ret = {}
-- 	local count = 0

-- 	if old then
-- 		for k, v in pairs(cur) do
-- 			if old[k] == nil then
-- 				count = count + 1
-- 				ret[k] = v
-- 			end
-- 		end
-- 	end

-- 	for k, v in pairs(ret) do
-- 		print(k, v)
-- 	end
-- 	print("diff: ", count)
-- 	old = cur
-- end

-- TestSnap()
-- local abcd = {}
-- TestSnap()



--[[
	string.split
]]
-- local a = "a b cd"
-- local b = "a   b  c d   "
-- local c = "a;b;;d"
-- local rs = string.split(a, " ")
-- local rs = string.split(b, " ")
-- local rs = string.split(b, "  ")
-- local rs = string.split(c, ";")
-- for k, v in ipairs(rs) do
-- 	print(k, "-" .. v .. "-")
-- end



--[[
	toStr/log
]]
-- local num = 1000
-- local str = "this is str"
-- local b = true
-- local ni = nil
-- local func = function() end
-- local tb = {}
-- local co = coroutine.create(function()
-- 	coroutine.yield()
-- end)
-- local tb2 = {
-- 	a = 1,
-- 	12,
-- 	"good",
-- 	b = function() end,
-- 	p = {
-- 		p1 = 1,
-- 		15,
-- 		o2 = "a str",
-- 		ccc = {
-- 			iit = true
-- 		}
-- 	},
-- 	31,
-- 	n = "hai",
-- 	ggg = nil
-- }

-- log(toStr(num, str, b, ni, func, tb, co))
-- log(toStr(tb2))
-- log(nil, type(nil), UnityEngine.GameObject)
-- log(10, "abc", true, nil, func, tb, co, tb2)
-- log(toStr(tb2, 1000, tb2, "abc", true))
-- log(toStr(10, "abc", true, nil, func, tb), co, nil)
-- log(nil, toStr(nil))
-- log(toStr())
-- logerr("haha")
-- log("haha", toStr("haha"))

-- local a = {10, 20}
-- local b = {}
-- b[a] = {1, 2, 3}
-- log(toStr(b))



--[[
	list
]]
-- local l = list.new()
-- l:push(10)
-- l:push(20)
-- l:push(30)
-- log(l:pop())
-- log("-> 1", toStr(l))	
-- l:pop()
-- l:pop()
-- log(l:pop())
-- log("-> 2", toStr(l))
-- l:push(10)
-- l:push(20)
-- l:unshift(30)
-- log("-> 3", toStr(l))
-- log(l:shift())
-- log("-> 4", toStr(l))



--[[
	clone
]]
-- local p1 = 10
-- local p2 = "a str"
-- local p3 = true
-- local p4 = nil

-- local p5 = function() end
-- local p6 = coroutine.create(function() end)
-- local p7 = {10, 20}

-- local function getClose(p)
-- 	local rs = {}
-- 	return function(i)
-- 		p = p + i
-- 		rs[i] = i
-- 		log(p)
-- 	end
-- end
-- local f1 = getClose(10)
-- f1(1)
-- f1(2)

-- local container = {b = 20}
-- local mt = {
-- 	__index = function(t, k)
-- 		local rs = container[k]
-- 		if not rs then
-- 			container[k] = k
-- 		end
-- 		return rs
-- 	end
-- }
-- local p8 = setmetatable({a = 10, f = f1, {"aaa", "bbb"}}, mt)
-- local np = clone(p8)
-- p8.f(1)
-- log(p8.a, p8.b, p8.c, toStr(container, p8, np))
-- local a = np.d
-- log(toStr(container, p8, np))
-- np.f(2)



--[[
	err/locate
	lua: logerr assert error 直接错误
	c#: LogError/LogExc Debugger.LogError/LogExc throw 直接错误
	in co
]]
-- function Test1()
--     logerr("test1 logerr")
--     log("test1 log")
-- end

-- function Test2(p)
--     assert(p)
--     log("test2 go on")
-- end

-- function Test3()
--     error("test3..")
--     log("test3 go on")
-- end

-- function Test4()
--     abcd()
-- end

-- public void Test5() {
--     UnityEngine.Debug.LogError("test5");
-- }

-- public void Test6() {
--     Debugger.LogError("test6");
-- }

-- public void Test7() {
--     Debug.LogException(new System.Exception("test7"));
-- }

-- public void Test8() {
--     Debugger.LogException(new System.Exception("test8"));
-- }

-- public void Test9() {
--     throw new System.Exception("test9");
-- }

-- public void Test10() {
--     int[] a = new int[3];
--     int b = a[4];
-- }

-- coMgr:start(function()
-- 	log(1)

	-- Test1()
	-- Test2()
	-- Test3()
	-- Test4()

	-- Util.Test5()
	-- Util.Test6()
	-- Util.Test7()
	-- Util.Test8()

	-- 类型：LuaDLL.toluaL_exception(L, e)
	-- Util.Test9()
	-- Util.Test10()

	-- 类型：LuaDLL.toluaL_exception(L, e, o, "attempt to index transform on a nil value");
	-- local obj = UnityEngine.GameObject.New()
	-- UnityEngine.GameObject.Destroy(obj)
	-- local trans = obj.transform

	-- 类型：LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityEngine.GameObject.New")
	-- local com = obj:GetComponent(123)

-- 	log(2)
-- end)



--[[
	class
]]
-- local Veicle = class()
-- Veicle.speed = 10
-- function Veicle:ctor(p1, p2)
-- 	log("Base ctor", p1, p2)
-- 	self.p1, self.p2 = p1, p2
-- end
-- function Veicle:Run(p1, p2)
-- 	log("V run", self.speed, self.p1, self.p2)
-- end
-- function Veicle:Stop()
-- 	log("V stop", self.speed, self.p1, self.p2)
-- end

-- local Car = class(Veicle)
-- function Car:Move(speed)
-- 	self.speed = speed
-- 	log(speed, "Car Move", self.speed, self.p1, self.p2)
-- 	self:Run()
-- end

-- local AdCar = class(Car)
-- function AdCar:ctor(speed)
-- 	self.speed = speed
-- 	log("AdCar ctor")
-- 	Veicle.ctor(self, "c", "d")
-- end
-- function AdCar:AdRun(speed)
-- 	self.speed = speed
-- 	log("AdRun", self.speed, self.p1, self.p2)
-- end

-- local Ship1 = class(Veicle)
-- function Ship1:Float()
-- 	self:Run()
-- end

-- local Ship2 = class(Veicle)
-- function Ship2:Run()
-- 	log("ship2 run", self.p1, self.p2)
-- end
-- function Ship2:Float()
-- 	log("----")
-- 	self:Run()
-- end

-- local Bus = class(Car)
-- function Bus:ctor()
-- 	self:Move(9)
-- end
-- function Bus:Run()
-- 	log("bus run")
-- end

-- local v1 = Veicle.new("a", "b")
-- v1:Run()
-- v1:Stop()

-- local v2 = Car.new()
-- v2:Run()
-- v2:Move(100)
-- v2:Stop()
-- log("--1 step--")

-- local v3 = AdCar.new(33)
-- v3:Run()
-- v3:AdRun(20)
-- v3:Stop()
-- log("--2 step--")

-- local v4 = Ship1.new()
-- v4:Float()
-- v4:Stop()
-- log("--3 step--")

-- local v5 = Ship2.new()
-- v5:Run()
-- v5:Float()
-- log("--4 step--")

-- local v6 = Bus.new()



--[[
	statemachine
]]
-- local fsm = newFSM({
-- 	initial = "hungry",
-- 	events = {
-- 		{name = "eat", from = "hungry", to = "satisfied"},
-- 		{name = "eat", from = "satisfied", to = "full"},
-- 		{name = "eat", from = "full", to = "sick"},
-- 		{name = "rest", from = {"hungry", "satisfied", "full", "sick"}, to = "hungry"},
-- 		{name = "play", to = "happy"}
-- 	},
-- 	callbacks = {
-- 		onenterhungry = function(arg) log("enter hung", arg[1], arg[2]) end,
-- 		onleavehungry = function(arg) log("leave hung", arg[1], arg[2]) end,
-- 		onentersick = function() log("enter sick") end,
-- 		onleavesick = function() log("leave sick") end,
-- 		onenterhappy = function(arg) log(">>>> this is happy", arg[1]) end,
-- 	}
-- })

-- fsm:eat(1, "fish")
-- fsm:eat()
-- fsm:eat()
-- log("look-->", fsm.current)
-- fsm:eat()	--内部can为false，抛异常

-- log(fsm.current)
-- log(fsm:is("hungry"))
-- log(fsm:can("eat"))

-- fsm:rest()
-- log(fsm.current, fsm:can("eat"))

-- -- fsm:eat()
-- -- fsm:eat()
-- -- fsm:eat()
-- fsm:play(10)
-- log(fsm.current)

-- log(fsm:can("eat"))
-- log(fsm:can("rest"))
-- log(fsm:can("play"))

-- fsm:play()



--[[
	co
	coMgr:start(func, ...)
	awaitFrame/awaitTime/awaitCo/awaitAll
]]
-- 闭包
-- function testCallCo1(p1, p2)
-- 	log(1)
-- 	coMgr:start(function()
-- 		-- coMgr:awaitFrame()
-- 		log(2)
-- 		-- local a = 10 .. nil
-- 		coMgr:awaitTime(2)
-- 		log(3, p1 + p2)
-- 		coMgr:awaitTime(2)
-- 		log(4)
-- 	end)
-- 	log(5)
-- end
-- testCallCo1(10, 20)

--非闭包
-- function abc(p1, p2)
-- 	log(p1)
-- 	coMgr:awaitTime(2)
-- 	log(p2)
-- end
-- function testCallCo2()
-- 	log(1)
-- 	coMgr:start(abc, 10, 20)
-- 	log(2)
-- end
-- testCallCo2()

-- 自定义调用（不推荐）
-- 检测重复调用
-- function testCallCo3()
-- 	local co
-- 	co = coMgr:newCo(function(p1, p2, p3)
-- 		log(p1)
-- 		coMgr:awaitFrame(80)
-- 		log(p2)
-- 		coMgr:awaitTime(2)
-- 		log(p3)

-- 		-- coMgr:proc(co, p1, p2, p3)
-- 	end)

-- 	log(1)
-- 	coMgr:proc(co, 10, 20, 30)
-- 	log(2)
-- 	-- coMgr:proc(co, 10, 20, 30)

-- 	coMgr:start(function()
-- 		coMgr:awaitTime(6)
-- 		coMgr:proc(co, 10, 20, 30)
-- 	end)
-- end
-- testCallCo3()

--等其他协程
-- function testCallCo4()
-- 	local f1 = function(a, b)
-- 		log("co 1", a, b)
-- 		coMgr:awaitTime(1.5)
-- 		log("co 1 finished")
-- 	end
-- 	local f2 = function()
-- 		log("co 2")
-- 		coMgr:awaitTime(1.5)
-- 		-- local a = 10 .. nil
-- 		log("co 2 finished")
-- 	end
-- 	local f3 = function()
-- 		log("co 3")
-- 		coMgr:awaitTime(1.5)
-- 		log("co 3 finished")
-- 	end

-- 	log(1)
-- 	coMgr:start(function()
-- 		log(2)
-- 		coMgr:awaitCo(f1, 10, 20)
-- 		log(3)
-- 		coMgr:awaitCo(f2)
-- 		log(4)
-- 		coMgr:awaitCo(f3)
-- 		log(5)
-- 	end)
-- 	log(6)
-- end
-- testCallCo4()

--等待多个协程完成
-- function testCallCo6()
-- 	local f1 = function()
-- 		coMgr:awaitTime(2)
-- 		log("this is 1")
-- 	end
-- 	local f2 = function()
-- 		-- local a = 10 .. nil
-- 		coMgr:awaitTime(3)
-- 		log("this is 2")
-- 	end
-- 	coMgr:start(function()
-- 		coMgr:awaitAll({f1, f2})
-- 		log(4)
-- 	end)
-- end
-- testCallCo6()

--内部中断，外部中断
-- function testCallCo5()
	-- local co
	-- co = coMgr:start(function()
	-- 	log(1)
	-- 	-- coMgr:stop()
	-- 	-- coMgr:stop(co)
	-- 	coMgr:awaitTime(2)
	-- 	log(2)

	-- 	-- coMgr:stop(co)
	-- 	coMgr:awaitFrame()
	-- 	-- coMgr:stop()
	-- 	log(3)
	-- end)
	-- -- coMgr:stop(co)

	-- local co = coMgr:newCo(function()
	-- 	log(2)
	-- 	coMgr:awaitFrame()
	-- 	log(3)
	-- end)
	-- coMgr:start(function()
	-- 	log(1)
	-- 	coMgr:proc(co)
	-- 	coMgr:stop(co)
	-- 	-- coMgr:stop()
	-- 	log(4)
	-- end)
-- end
-- testCallCo5()

--awaitUntil
-- local a = 0
-- local testTb = {}
-- function testTb:update(t)
-- 	a = a + t
-- end
-- LuaDrive:register(testTb)

-- function testCallCo7()
-- 	coMgr:start(function()
-- 		log(1)
-- 		coMgr:awaitUntil(function()
-- 			log("be called")
-- 			return a > 5
-- 		end)
-- 		log(2)
-- 	end)
-- end
-- testCallCo7()

--awaitLoadScene
-- Util.InitResourceCtrl()
-- coMgr:start(function()
-- 	coMgr:awaitLoadScene("Scene/Main")
-- 	UnityEngine.GameObject.Find("Cube"):SetActive(false)
-- 	log("load Suc", toStr(evMgr))
-- end)



--[[
	event
]]
-- local f1 = function(p1, p2)
-- 	log("aaa", p1, p2)
-- end
-- local f2 = function()
-- 	log("bbb")
-- end
-- evMgr:reg("a", f1)
-- evMgr:reg("a", f2)
-- evMgr:trig("a", 10, 20)

-- log("--------")
-- evMgr:cancel("a", f1)
-- -- evMgr:cancel("a")
-- evMgr:trig("a", 10, 20)



--[[
	pool
]]
-- function TestPool()
-- 	log("test pool")
-- 	local arg1 = "Model/Eff1"
-- 	local destroyTime = 3	--回收时间+1

	-- pool:spawn()
	-- local inst1 = pool:spawn(arg1)
	-- log(inst1.abc)

	-- log(1, toStr(pool))
	-- local inst1 = pool:spawn(arg1)
	-- log(2, toStr(pool))
	-- coMgr:awaitTime(2)
	-- Util.SetActive(inst1.ptr, false)
	-- Util.SetParent(inst1.ptr, GameDefine.ProxyObj)
	-- pool:despawn(inst1)
	-- log(3, toStr(pool))
	-- coMgr:awaitTime(destroyTime)
	-- log(4, toStr(pool))

	-- log("->inst", inst1, toStr(inst1))
	-- log("->pool", pool, toStr(pool))
	-- local inst2 = pool:spawn(arg1)
	-- log("->pool", toStr(pool))
	-- local inst3 = pool:spawn(arg1)
	-- log("->pool", toStr(pool))

	-- pool:despawnAll(arg1)
	-- log("->pool", toStr(pool))

	-- local inst1 = pool:spawn(arg1)
	-- log("->1", toStr(pool))
	-- coMgr:awaitTime(destroyTime)	
	-- log("->1-1", toStr(pool))
	-- pool:spawn(arg1)
	-- log(">1-2", toStr(pool))

	-- pool:despawn(inst1)
	-- log("->2", toStr(pool))
	-- pool:despawn(inst1)
	-- log("->3", toStr(pool))

	
	-- local inst1 = pool:spawn(arg1)
	-- coMgr:awaitTime(2)
	-- -- public static void TestDestroy(int ptr) {
	-- -- 	//测试1.删除go
 -- --        GameObject obj = objCache.GetGameObject(ptr);
 -- --        Object.Destroy(obj);
 -- --        //测试2.删除ptr
 -- --        objCache.ClearPtr(ptr);
 -- --        //测试3.删除go和ptr
 -- --        objCache.DestroyPtr(ptr);
 -- --    }
	-- Util.TestDestroy(inst1.ptr)
	-- log(toStr(inst1), toStr(pool))
	-- -- coMgr:awaitFrame()
	-- pool:despawn(inst1)

	-- local i1 = pool:spawn(arg1)
	-- local i2 = pool:spawn(arg1)
	-- local i3 = pool:spawn(arg1)
	-- pool:despawnAll(arg1)
	-- log(1, toStr(pool))
	-- -- coMgr:awaitTime(destroyTime)
	-- -- log(2, toStr(pool))
	-- i1 = pool:spawn(arg1)
	-- log(toStr(i1, i3))
	-- pool:despawn(i3)
	-- log(3, toStr(pool))
	-- pool:despawn(i1)
	-- log(4, toStr(pool))
	-- pool:despawn(i2)
	-- log(5, toStr(pool))
-- end

-- Util.InitResourceCtrl()
-- coMgr:start(function()
-- 	coMgr:awaitLoadScene("Scene/Main")
-- 	UnityEngine.GameObject.Find("Cube"):SetActive(false)
-- 	log("load Suc")
-- 	TestPool()
-- end)

do return end

function TestUI()
	UILogin:Open()
	-- coMgr:awaitTime(2)
	-- UILogin:Close()
	-- coMgr:awaitTime(2)
	-- UILogin:Open()
end
