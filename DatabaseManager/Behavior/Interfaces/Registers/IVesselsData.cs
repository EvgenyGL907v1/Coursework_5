using System;
using System.Collections.Generic;

namespace DatabaseManager
{
	public struct VesselData
	{ 
		public int VesselId { get; set; }
		public string RegNumber { get; set; }
		public string Name { get; set; }
		public int TypeId { get; set; }
		public string TypeName { get; set; }
		public int ClassId { get; set; }
		public string ClassName { get; set; }
		public string CreateDate { get; set; }
		public string CaptainName { get; set; }

		public byte[] Image { get; set; }
	}

	public interface IVesselsData
	{
		void Add(VesselData vesselData);
		void Edit(VesselData vesselData);
		void Delete(VesselData vesselData);
		List<VesselData> GetVesselsList();
	}
}
