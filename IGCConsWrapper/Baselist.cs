
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IGCConsWrapper
{
	[Serializable()]
	public struct ConsBase
	{
		[System.Xml.Serialization.XmlAttribute("ShortName")]
		public string shortName {get; set;}
		[System.Xml.Serialization.XmlAttribute("FullName")]
		public string fullName {get; set;}
		
		public static ConsBase Empty = new ConsBase()
		{
			shortName = string.Empty,
			fullName = string.Empty
		};
	}
	
	[Serializable()]
	[System.Xml.Serialization.XmlRoot("BaseList")]
	public class BaseList
	{
		[XmlArray("Bases")]
    	[XmlArrayItem("Base", typeof(ConsBase))]
		public List<ConsBase> bases;
		
		public BaseList()
		{
		}
		
		public bool HasBase(string name)
		{
			foreach (ConsBase consBase in bases)
			{
				if ((consBase.shortName.ToUpper() == name.ToUpper())
				    ||(consBase.fullName.ToUpper() == name.ToUpper()))
					return true;
			}
			return false;
		}
		
		public ConsBase GetBaseByName(string name)
		{
			foreach (ConsBase consBase in bases)
			{
				if ((consBase.shortName.ToUpper() == name.ToUpper())
				    ||(consBase.fullName.ToUpper() == name.ToUpper()))
					return consBase;
			}
			return ConsBase.Empty;
		}
	}
}
