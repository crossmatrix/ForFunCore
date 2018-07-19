local uiMgr = uiMgr
UILogin = uiMgr:reg("UILogin")

local testData

function UILogin:onInit()
	UIBase.onInit(self)

	-- text1
	Util.SetTxt(self.__inst, "BtnLogin/Label", "haha1")
	-- text2
	self.vars["text"] = Util.GetPtr(self.__inst, "TestShadow/Text", 2)
	Util.SetTxt(self.vars["text"], "abc")

	-- image1
	Util.SetImg(self.__inst, "BtnLogin", "Common/hero_alc_acidterror_u")
	-- image2
	self.vars["img"] = Util.GetPtr(self.__inst, "TestShadow", 3)
	Util.SetImg(self.vars["img"], "Common/4")

	-- ev1
	Util.SetUIEv(self.__inst, "Img1", 13, function() self:Img1_OnBeginDrag() end)
	Util.SetUIEv(self.__inst, "Img1", 14, function() self:Img1_OnEndDrag() end)
	-- ev2
	local temp = Util.GetPtr(self.__inst, "Img1", 0)
	Util.SetUIEv(temp, 5, function() self:Img1_OnDrag() end)

	-- btn1
	Util.AddClick(self.__inst, "Img2", function() print("img2") end)
	-- btn2
	local temp2 = Util.GetPtr(self.__inst, "BtnLogin", 0)
	Util.AddClick(temp2, function() print("BtnLogin") end)

	-- toggle1
	Util.CopyChild(self.__inst, "TG2", 3)
	for i = 1, 3 do
		Util.InitTog(self.__inst, "TG2/Tog" .. i, "Tog" .. i, function() log("this is tog" .. i) end)
	end
	Util.ResetTog(self.__inst, "TG2/Tog1")
	-- toggle2
	for i = 1, 3 do
		Util.InitTog(self.__inst, "TG1/Tog" .. i, "Tog" .. i, function() log("that is tog" .. i) end)
	end
	Util.ResetTog(self.__inst, "TG1/Tog1")

	-- Progress SetPrg
	-- coMgr:awaitTime(1)
	local prgPtr = Util.GetPtr(self.__inst, "Progress", 4)
	Util.SetPrg(prgPtr, 0)
	-- Util.SetPrg(prgPtr, 1.99999)
	-- Util.SetPrg(prgPtr, 2.00001)
	-- Util.SetPrg(prgPtr, 2)
	-- Util.SetPrg(prgPtr, 1)
	Util.SetPrg(prgPtr, 1.4)
	Util.AddClick(self.vars["img"], function()
		-- Util.SetPrg(prgPtr, 0.8, true)
		-- Util.SetPrg(prgPtr, 1, true)
		-- Util.SetPrg(prgPtr, 1.0001, true)
		-- Util.SetPrg(prgPtr, 1.9999, true)
		Util.SetPrg(prgPtr, 2.5, true)
	end)

	-- Progress AddPrg
	-- coMgr:awaitTime(1)
	local prgPtr2 = Util.GetPtr(self.__inst, "GradProg", 4)
	Util.SetPrg(prgPtr2, 0.3)
	Util.AddClick(self.vars["img"], function()
		-- Util.AddPrg(prgPtr2, 0.7)
		-- Util.AddPrg(prgPtr2, 1.7)
		-- Util.AddPrg(prgPtr2, 1.6999)
		-- Util.AddPrg(prgPtr2, 1.7001)
			
		-- Util.AddPrg(prgPtr2, 0.5, true)
		-- Util.AddPrg(prgPtr2, 1.7, true)
		-- Util.AddPrg(prgPtr2, 1.6999, true)
		Util.AddPrg(prgPtr2, 1.7001, true)
	end)

	-- inputfield
	Util.SetInputCont(self.__inst, "InputField", "hahaha...")

	-- sv
	self:GenData(102)
	self.vars["sv1"] = Util.InitSR(self.__inst, "VertSR/View/VertContainer", self.OnWrapItem1)
	self.vars["sv2"] = Util.InitSR(self.__inst, "VertSR1/View/VertContainer", self.OnWrapItem2)
	self.vars["sv3"] = Util.InitSR(self.__inst, "HorzSR/View/HorzContainer", self.OnWrapItem3)
	self.vars["sv4"] = Util.InitSR(self.__inst, "GridSR/View/GridContainer", self.OnWrapItem4)
end

local test = 1
function UILogin:onShow()
	log("uilogin show")

	coMgr:start(function()
		coMgr:awaitTime(1)
		Util.RefreshSR(self.vars["sv1"], #testData, test)
		Util.RefreshSR(self.vars["sv2"], #testData, test, true)
		Util.RefreshSR(self.vars["sv3"], #testData, test, true)
		Util.RefreshSR(self.vars["sv4"], #testData, test, true)
	end)
end

function UILogin:onHide()
	log("uilogin hide")
end

function UILogin:onUpdate(t)
	-- log("update", t)
	if UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.A) then
		Util.ResetTog(self.__inst, "TG2/Tog1")
	elseif UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.D) then
		log(Util.GetInputCont(self.__inst, "InputField"))
	elseif UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Q) then
		-- self:GenData(3)
		test = test + 1
		-- Util.RefreshSR(self.vars["sv1"], #testData, test)
		Util.RefreshSR(self.vars["sv3"], #testData, test)
	elseif UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.W) then
		self:GenData(3)
		test = test + 1
		Util.RefreshSR(self.vars["sv2"], #testData, test, true)
	elseif UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.E) then
		self:GenData(12)
		test = test + 1
		Util.RefreshSR(self.vars["sv4"], #testData, test, true)
	end
end

function UILogin:GenData(t)
	testData = {}
	for i = 1, t do
		testData[i] = i
	end
end

function UILogin:Img1_OnBeginDrag()
	log("begin drag")
end

function UILogin:Img1_OnDrag()
	log("drag...")
end

function UILogin:Img1_OnEndDrag()
	log("end drag")
end

function UILogin.OnWrapItem1(ptr, realIndex)
	local data = testData[realIndex]
	local empty, wdgs = UILogin:GetSRItem(ptr, "sv1")
	if empty then
		wdgs["Text"] = Util.GetPtr(ptr, "Text", 2)
		Util.AddClick(ptr, "Btn", function() UILogin:ClickItem(ptr) end)
	end
	Util.SetTxt(wdgs["Text"], data)
end

function UILogin.OnWrapItem2(ptr, realIndex)
	local data = testData[realIndex]
	local empty, wdgs = UILogin:GetSRItem(ptr, "sv2")
	if empty then
		wdgs["Text"] = Util.GetPtr(ptr, "Text", 2)
		Util.AddClick(ptr, "Btn", function() UILogin:ClickItem2(ptr) end)
	end
	Util.SetTxt(wdgs["Text"], data)
end

function UILogin.OnWrapItem3(ptr, realIndex)
	local data = testData[realIndex]
	local empty, wdgs = UILogin:GetSRItem(ptr, "sv3")
	if empty then
		wdgs["Text"] = Util.GetPtr(ptr, "Text", 2)
		Util.AddClick(wdgs["Text"], function() UILogin:ClickItem3(ptr) end)
	end
	Util.SetTxt(wdgs["Text"], data)
end

function UILogin.OnWrapItem4(ptr, realIndex)
	local data = testData[realIndex]
	local empty, wdgs = UILogin:GetSRItem(ptr, "sv4")
	if empty then
		wdgs["Text"] = Util.GetPtr(ptr, "Text", 2)
		Util.AddClick(wdgs["Text"], function() UILogin:ClickItem4(ptr) end)
	end
	if data then
		Util.SetObject(ptr, 1)
		Util.SetTxt(wdgs["Text"], data)
	else
		Util.SetObject(ptr, 2)
	end
end

function UILogin:ClickItem(ptr)
	log(Util.SRSelect(self.vars["sv1"], ptr))
end

function UILogin:ClickItem2(ptr)
	log(Util.SRSelect(self.vars["sv2"], ptr, true))
end

function UILogin:ClickItem3(ptr)
	log(Util.SRSelect(self.vars["sv3"], ptr, true))
end

function UILogin:ClickItem4(ptr)
	log(Util.SRSelect(self.vars["sv4"], ptr, true))
end

function UILogin:Open()
	uiMgr:show("UILogin")
end

function UILogin:Close()
	uiMgr:hide("UILogin")
end
