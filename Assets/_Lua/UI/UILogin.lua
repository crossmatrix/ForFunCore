local uiMgr = uiMgr
UILogin = uiMgr:reg("UILogin")

local testData

function UILogin:onInit()
	UIBase.onInit(self)

	testData = {}
	for i = 1, 102 do
		testData[i] = i
	end

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
	-- Util.SetPrg(prgPtr, 0)
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

	-- self.vars["sv1"] = LU:InitSR(self.__inst, "VertSR/View/VertContainer", self.OnWrapItem1)
	-- self.vars["sv2"] = LU:InitSR(self.__inst, "HorzSR/View/HorzContainer", self.OnWrapItem2)
	-- self.vars["sv3"] = LU:InitSR(self.__inst, "GridSR/View/GridContainer", self.OnWrapItem3)

	-- self.vars["Test1"] = GameProxy.GetPtr(self.__inst, "Test1", 0)
end

function UILogin:onShow()
	log("uilogin show")

	-- coMgr:start(function()
	-- 	coMgr:awaitTime(1)
	-- 	LU:RefreshSR(self.vars["sv1"], #testData)
	-- 	LU:RefreshSR(self.vars["sv2"], #testData)
	-- 	LU:RefreshSR(self.vars["sv3"], #testData)
	-- end)
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
	local empty, wdgs = UILogin:GetItemWdgs(ptr, "sv1")
	if empty then
		wdgs["Text"] = GameProxy.GetPtr(ptr, "Text", 2)
		LU:AddClick(wdgs["Text"], function() UILogin:ClickItem(ptr) end)
	end
	LU:SetText(wdgs["Text"], data)
end

function UILogin.OnWrapItem2(ptr, realIndex)
	local data = testData[realIndex]
	local empty, wdgs = UILogin:GetItemWdgs(ptr, "sv2")
	if empty then
		wdgs["Text"] = GameProxy.GetPtr(ptr, "Text", 2)
		LU:AddClickD(ptr, "Text", function() UILogin:ClickItem2(ptr) end)
	end
	LU:SetText(wdgs["Text"], data)
end

function UILogin.OnWrapItem3(ptr, realIndex)
	local data = testData[realIndex]
	local empty, wdgs = UILogin:GetItemWdgs(ptr, "sv3")
	if empty then
		wdgs["Text"] = GameProxy.GetPtr(ptr, "Text", 2)
		LU:AddClick(wdgs["Text"], function() UILogin:ClickItem3(ptr) end)
	end
	if data then
		LU:SetActive(ptr, true)
		LU:SetText(wdgs["Text"], data)
	else
		LU:SetActive(ptr, false)
	end
end

function UILogin:ClickItem(ptr)
	log(LU:SRSelect(self.vars["sv1"], ptr, true))
end

function UILogin:ClickItem2(ptr)
	log(LU:SRSelect(self.vars["sv2"], ptr))
end

function UILogin:ClickItem3(ptr)
	log(LU:SRSelect(self.vars["sv3"], ptr, true))
end

function UILogin:Open()
	uiMgr:show("UILogin")
end

function UILogin:Close()
	uiMgr:hide("UILogin")
end
