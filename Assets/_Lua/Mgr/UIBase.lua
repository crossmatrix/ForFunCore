UIBase = {}

function UIBase:onInit()
	self.vars = {}
end

function UIBase:onShow()

end

function UIBase:onHide()

end

function UIBase:onUpdate(t)

end

function UIBase:GetItemWdgs(ptr, ctName)
	local name = ctName .. "_Item"
	local tb = self.vars[name]
	if not tb then
		tb = {}
		self.vars[name] = tb
	end
	local rs = tb[ptr]
	local empty = false
	if not rs then
		rs = {}
		tb[ptr] = rs
		empty = true
	end
	return empty, rs
end
