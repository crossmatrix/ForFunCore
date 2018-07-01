local require = require
local string = string
local table = table
local GameSettings = GameSettings
local type = type
local print_n = print_n
local _strVar
local _space = "      "
int64.zero = int64.new(0,0)
uint64.zero = uint64.new(0,0)

function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter == '') then return nil end

    local rs = {}
    while true do
        local pos1, pos2 = string.find(input, delimiter)
        if pos1 then
            table.insert(rs, string.sub(input, 1, pos1 - 1))
            input = string.sub(input, pos2 + 1)
        else
            table.insert(rs, input)
            break
        end
    end
    return rs
end

-- function import(moduleName, currentModuleName)
--     local currentModuleNameParts
--     local moduleFullName = moduleName
--     local offset = 1

--     while true do
--         if string.byte(moduleName, offset) ~= 46 then -- .
--             moduleFullName = string.sub(moduleName, offset)
--             if currentModuleNameParts and #currentModuleNameParts > 0 then
--                 moduleFullName = table.concat(currentModuleNameParts, ".") .. "." .. moduleFullName
--             end
--             break
--         end
--         offset = offset + 1

--         if not currentModuleNameParts then
--             if not currentModuleName then
--                 local n,v = debug.getlocal(3, 1)
--                 currentModuleName = v
--             end

--             currentModuleNameParts = string.split(currentModuleName, ".")
--         end
--         table.remove(currentModuleNameParts, #currentModuleNameParts)
--     end

--     return require(moduleFullName)
-- end

function reimport(name)
    local package = package
    package.loaded[name] = nil
    package.preload[name] = nil
    return require(name)    
end

function log(...)
    if GameSettings.ShowLog then
        print_n(1, debug.traceback("", 2), ...)
    end
end

function logerr(...)
    if GameSettings.ShowLog then
        print_n(2, debug.traceback("", 2), ...)
    end
end

_strVar = function(var, indent)
    local rs = ""
    if type(var) == "table" then
        if rawget(var, "_type") == "list" then
            rs = var:tostring(indent)
        else
            rs = "{\n"
            local index = 1
            for k, v in pairs(var) do
                if index ~= 1 then
                    rs = rs .. "\n"
                end
                index = index + 1
                rs = rs .. string.rep(_space, indent + 1) .. "[" .. _strVar(k, indent + 1) .. "] = " .. _strVar(v, indent + 1) .. ","
            end
            rs = rs .. "\n" .. string.rep(_space, indent) .. "}"
        end
    else
        rs = tostring(var)
        --tostring(invalid userdata) is nil, not a string
        if not rs then
            rs = "nil-userdata"
        end
    end
    return rs
end

function toStr(...)
    local rs = ""
    local len = select('#', ...)
    for i = 1, len do
        local var = select(i, ...)
        rs = rs .. _strVar(var, 0)
        if i ~= len then
            rs = rs .. ", "
        end
    end
    return rs
end

function toStrIndent(indent, ...)
    local rs = ""
    local len = select('#', ...)
    for i = 1, len do
        local var = select(i, ...)
        rs = rs .. _strVar(var, indent)
        if i ~= len then
            rs = rs .. ", "
        end
    end
    return rs
end

function clone(obj)
    local _type = type(obj)
    if _type == "table" then
        local newTb = {}
        for k, v in pairs(obj) do
            newTb[clone(k)] = clone(v)
        end
        local mt = getmetatable(obj)
        if mt then
            local newMt = clone(mt)
            setmetatable(newTb, newMt)
        end
        return newTb
    else
        return obj
    end
end
