
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IGCConsWrapper
{		
	[Serializable()]
	public struct ConsSystem
	{
		[System.Xml.Serialization.XmlAttribute("ShortName")]
		public string shortName;
		[System.Xml.Serialization.XmlAttribute("FullName")]
		public string fullName;
		[XmlArray("BaseList")]
    	[XmlArrayItem("Base", typeof(ConsBase))]
		public List<ConsBase> baselist;
		
		public static ConsSystem Empty = new ConsSystem()
		{
			shortName = string.Empty,
			fullName = string.Empty,
			baselist = new List<ConsBase>()
		};
	}
	
	[Serializable()]
	[System.Xml.Serialization.XmlRoot("SystemList")]
	public class SystemList
	{
		[XmlArray("Systems")]
    	[XmlArrayItem("System", typeof(ConsSystem))]
		public List<ConsSystem> systems;
		
		public SystemList()
		{
		}
		
		public ConsSystem GetSystemByName(string name)
		{
			ConsSystem system = ConsSystem.Empty;
			foreach (ConsSystem element in this.systems)
			{
				if ((element.shortName.ToUpper() == name.ToUpper())
				    ||(element.fullName.ToUpper() == name.ToUpper()))
				{
					system = element;
				}
			}
			return system;
		}
		
		public bool HasSystem(string name)
		{
			foreach (ConsSystem consSystem in systems)
			{
				if ((consSystem.shortName.ToUpper() == name.ToUpper())
				    ||(consSystem.fullName.ToUpper() == name.ToUpper()))
					return true;
			}
			return false;
		}
	}
}
