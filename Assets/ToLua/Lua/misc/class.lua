local _class = {}

function class(super)
	local class_type = {}
	local mt
	class_type.ctor = false
	class_type.super = super
	class_type.new = function(...)
		local inst = {}
		setmetatable(inst, mt)
		local ctor = rawget(class_type, "ctor")
		if ctor then
			ctor(inst, ...)
		end
		return inst
	end

	local vtbl = {}
	_class[class_type] = vtbl
	setmetatable(class_type, {__newindex = function(t, k, v)
		vtbl[k] = v
	end})
	if super then
		setmetatable(vtbl, {__index = function(t, k)
			local ret = _class[super][k]
			vtbl[k] = ret
			return ret
		end})
	end
	mt = {__index = vtbl}
 
	return class_type
end