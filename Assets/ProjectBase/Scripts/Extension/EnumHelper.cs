using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ProjectBase.EnumExtension
{
	public static class EnumHelper
	{
		/// <summary>
		/// 类型，对应的枚举值，枚举描述
		/// </summary>
		static Dictionary<Type, Dictionary<Enum, KeyValuePair<int, string>>> _enumDic = new Dictionary<Type, Dictionary<Enum, KeyValuePair<int, string>>>();

		static Type descriptionType = typeof(System.ComponentModel.DescriptionAttribute);
		static Type flagType = typeof(System.FlagsAttribute);


		static bool CheckDescription(Type type)
		{
			return type.GetCustomAttribute(descriptionType) == null;
		}

		static bool CheckFlag(Type type)
		{
			return type.GetCustomAttribute(flagType) == null;
		}

		static void AddToDic<T>(Type type) where T : Enum
		{
			if (_enumDic.ContainsKey(type))
				return;

			Array arr1 = type.GetEnumValues();
			Dictionary<Enum, KeyValuePair<int, string>> newDic = new Dictionary<Enum, KeyValuePair<int, string>>();

			for (int i = 0; i < arr1.Length; i++)
			{
				object obj = arr1.GetValue(i);
				int value = obj.GetHashCode();
				string strArr = obj.ToString();
				FieldInfo fieldInfo = type.GetField(strArr);
				DescriptionAttribute descriptionAttribute = fieldInfo.GetCustomAttribute(descriptionType) as DescriptionAttribute;
				if (descriptionAttribute != null)
				{
					string description = descriptionAttribute.Description;
					newDic.Add((T)Enum.ToObject(type, value), new KeyValuePair<int, string>(value, description));
				}
			}
			_enumDic.Add(type, newDic);
		}

		public static IEnumerable<string> Descriptions<T>(this IEnumerable<T> enumValues) where T : Enum
		{
			Type type = typeof(T);
			AddToDic<T>(type);

			Dictionary<Enum, KeyValuePair<int, string>> dic = _enumDic[type];
			IEnumerable<string> descrpiptions = enumValues.Select(enumValue => dic[enumValue].Value);

			return descrpiptions;
		}

		public static string Description<T>(this T enumValue) where T : Enum
		{
			Type type = typeof(T);
			AddToDic<T>(type);

			return _enumDic[type][enumValue].Value;
		}

		public static IEnumerable<T> Split<T>(this T enumValue) where T : Enum
		{
			Type type = typeof(T);
			CheckFlag(type);
			AddToDic<T>(type);

			List<Enum> result = new List<Enum>();
			foreach (var item in _enumDic[type].Keys)
			{
				if (enumValue.HasFlag(item))
					result.Add(item);
			}
			if (result[0].GetHashCode() == 0)
				result.RemoveAt(0);

			return result.Cast<T>();
		}

		public static int Max<T>() where T : Enum
		{
			Type type = typeof(T);
			AddToDic<T>(type);

			return _enumDic[type].Keys.Last().GetHashCode();
		}

		public static int Min<T>() where T : Enum
		{
			Type type = typeof(T);
			AddToDic<T>(type);

			return _enumDic[type].Keys.First().GetHashCode();
		}

		public static T Random<T>() where T : Enum
		{
			Type type = typeof(T);
			AddToDic<T>(type);

			int randomValue = UnityEngine.Random.Range(0, _enumDic[type].Keys.Count);
			return (T)_enumDic[type].ElementAt(randomValue).Key;
		}

		public static bool Compare<T>(this T e1, T e2) where T : Enum
		{
			Type type = typeof(T);
			AddToDic<T>(type);

			int i1 = e1.GetHashCode();
			int i2 = e2.GetHashCode();
			if (i1 < 0)
				i1 = _enumDic[type].Values.Sum(value => value.Key);
			if (i2 < 0)
				i2 = _enumDic[type].Values.Sum(value => value.Key);
			return i1 == i2;
		}
	}
}