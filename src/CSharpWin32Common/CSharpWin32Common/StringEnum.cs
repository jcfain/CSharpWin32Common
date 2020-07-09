using System;
using System.Collections;
using System.Reflection;

namespace Automation.Common
{
	/// <summary>
	/// Creates a string value conversion for an enum
	/// </summary>
	internal class StringValueAttribute : Attribute
	{
		private string _value;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value"></param>
		public StringValueAttribute(string value)
		{
			_value = value;
		}

		/// <summary>
		/// Get the enum values string
		/// </summary>
		public string Value
		{
			get { return _value; }
		}
	}

	/// <summary>
	/// Used to get a string value from an enum with the StringValue attribute
	/// </summary>
	internal static class StringEnum
	{
		private static Hashtable _stringValues = new Hashtable();

		/// <summary>
		/// Used to get a string value from an enum with the StringValue attribute
		/// </summary>
		public static string GetStringValue(Enum value)
		{
			string output = null;
			Type type = value.GetType();

			//Check first in our cached results...
			if (_stringValues.ContainsKey(value))
			{
				output = (_stringValues[value] as StringValueAttribute).Value;
			}
			else
			{
				//Look for our 'StringValueAttribute' 
				//in the field's custom attributes
				FieldInfo fi = type.GetField(value.ToString());
				StringValueAttribute[] attrs =
					fi.GetCustomAttributes(typeof (StringValueAttribute),
						false) as StringValueAttribute[];
				if (attrs.Length > 0)
				{
					_stringValues.Add(value, attrs[0]);
					output = attrs[0].Value;
				}
			}

			return output;
		}
	}
}
