﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UtilWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Util), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("CheckPtr", CheckPtr);
		L.RegFunction("ClearPtr", ClearPtr);
		L.RegFunction("DestroyPtr", DestroyPtr);
		L.RegFunction("GetPtr", GetPtr);
		L.RegFunction("InitResourceCtrl", InitResourceCtrl);
		L.RegFunction("NewObj", NewObj);
		L.RegFunction("NewPref", NewPref);
		L.RegFunction("SetObject", SetObject);
		L.RegFunction("LoadScene", LoadScene);
		L.RegFunction("NewUI", NewUI);
		L.RegFunction("SetTxt", SetTxt);
		L.RegFunction("SetImg", SetImg);
		L.RegFunction("InitTog", InitTog);
		L.RegFunction("ResetTog", ResetTog);
		L.RegFunction("SetPrg", SetPrg);
		L.RegFunction("AddPrg", AddPrg);
		L.RegFunction("GetInputCont", GetInputCont);
		L.RegFunction("SetInputCont", SetInputCont);
		L.RegFunction("CopyChild", CopyChild);
		L.RegFunction("InitSR", InitSR);
		L.RegFunction("RefreshSR", RefreshSR);
		L.RegFunction("SRSelect", SRSelect);
		L.RegFunction("SetUIEv", SetUIEv);
		L.RegFunction("AddClick", AddClick);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckPtr(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			bool o = Util.CheckPtr(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearPtr(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			Util.ClearPtr(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyPtr(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			Util.DestroyPtr(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPtr(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
			uint o = Util.GetPtr(arg0, arg1, arg2);
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitResourceCtrl(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			Util.InitResourceCtrl();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NewObj(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = ToLua.CheckString(L, 1);
				uint o = Util.NewObj(arg0);
				LuaDLL.lua_pushnumber(L, o);
				return 1;
			}
			else if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				uint arg1 = (uint)LuaDLL.luaL_checknumber(L, 2);
				uint o = Util.NewObj(arg0, arg1);
				LuaDLL.lua_pushnumber(L, o);
				return 1;
			}
			else if (count == 3)
			{
				string arg0 = ToLua.CheckString(L, 1);
				uint arg1 = (uint)LuaDLL.luaL_checknumber(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				uint o = Util.NewObj(arg0, arg1, arg2);
				LuaDLL.lua_pushnumber(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.NewObj");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NewPref(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = ToLua.CheckString(L, 1);
				uint o = Util.NewPref(arg0);
				LuaDLL.lua_pushnumber(L, o);
				return 1;
			}
			else if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				uint arg1 = (uint)LuaDLL.luaL_checknumber(L, 2);
				uint o = Util.NewPref(arg0, arg1);
				LuaDLL.lua_pushnumber(L, o);
				return 1;
			}
			else if (count == 3)
			{
				string arg0 = ToLua.CheckString(L, 1);
				uint arg1 = (uint)LuaDLL.luaL_checknumber(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				uint o = Util.NewPref(arg0, arg1, arg2);
				LuaDLL.lua_pushnumber(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.NewPref");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetObject(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				Util.SetObject(arg0);
				return 0;
			}
			else if (count == 2)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
				Util.SetObject(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
				uint arg2 = (uint)LuaDLL.luaL_checknumber(L, 3);
				Util.SetObject(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.SetObject");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadScene(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			Util.LoadScene(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NewUI(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			uint o = Util.NewUI(arg0);
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTxt(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				Util.SetTxt(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				string arg2 = ToLua.CheckString(L, 3);
				Util.SetTxt(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.SetTxt");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetImg(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				Util.SetImg(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				string arg2 = ToLua.CheckString(L, 3);
				Util.SetImg(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.SetImg");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitTog(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			string arg2 = ToLua.CheckString(L, 3);
			LuaFunction arg3 = ToLua.CheckLuaFunction(L, 4);
			Util.InitTog(arg0, arg1, arg2, arg3);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetTog(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			Util.ResetTog(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPrg(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
				Util.SetPrg(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 3);
				Util.SetPrg(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.SetPrg");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddPrg(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
				Util.AddPrg(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 3);
				Util.AddPrg(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.AddPrg");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInputCont(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			string o = Util.GetInputCont(arg0, arg1);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetInputCont(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			string arg2 = ToLua.CheckString(L, 3);
			Util.SetInputCont(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CopyChild(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
			Util.CopyChild(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitSR(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			SRContainer.DlgWrapItem arg2 = (SRContainer.DlgWrapItem)ToLua.CheckDelegate<SRContainer.DlgWrapItem>(L, 3);
			uint o = Util.InitSR(arg0, arg1, arg2);
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshSR(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				Util.RefreshSR(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 4)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				bool arg3 = LuaDLL.luaL_checkboolean(L, 4);
				Util.RefreshSR(arg0, arg1, arg2, arg3);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.RefreshSR");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SRSelect(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				uint arg1 = (uint)LuaDLL.luaL_checknumber(L, 2);
				int o = Util.SRSelect(arg0, arg1);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				uint arg1 = (uint)LuaDLL.luaL_checknumber(L, 2);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 3);
				int o = Util.SRSelect(arg0, arg1, arg2);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.SRSelect");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetUIEv(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
				LuaFunction arg2 = ToLua.CheckLuaFunction(L, 3);
				Util.SetUIEv(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 4)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				LuaFunction arg3 = ToLua.CheckLuaFunction(L, 4);
				Util.SetUIEv(arg0, arg1, arg2, arg3);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.SetUIEv");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddClick(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				LuaFunction arg1 = ToLua.CheckLuaFunction(L, 2);
				Util.AddClick(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				uint arg0 = (uint)LuaDLL.luaL_checknumber(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				LuaFunction arg2 = ToLua.CheckLuaFunction(L, 3);
				Util.AddClick(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: Util.AddClick");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

